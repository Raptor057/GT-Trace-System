<script>
  import { createEventDispatcher } from "svelte";
  import { onMount } from "svelte";

  export let lineCode = null;
  export let workOrderCode = null;

  const dispatch = createEventDispatcher();

  const getSignalRClient = (name, url) => {
    let client = {};

    const connection = new signalR.HubConnectionBuilder()
      .withUrl(url)
      .configureLogging(signalR.LogLevel.Information)
      .build();

    client.start = async function start() {
      try {
        await connection.start({ withCredentials: false });
        console.log(`${name} Connected!`, url);
      } catch (err) {
        console.error(name, err);
        setTimeout(start, 3000);
      }
    };

    connection.onclose(async () => await client.start());

    client.on = (messageName, callback) => {
      connection.on(messageName, callback);
      return client;
    };

    return client;
  };

  const etiMovements = getSignalRClient(
    "ETI Movements SignalR Client",
    "http://mxsrvapps.gt.local/gtt/services/etimovements/hubs/etimovements"
  )
    .on("EtiLoaded", (_lineCode, etiNo, componentNo, pointOfUseCode) => {
      if (_lineCode !== lineCode) return;
      dispatch("etiLoaded", {
        etiNo: etiNo,
        componentNo: componentNo,
        pointOfUseCode: pointOfUseCode,
      });
    })
    .on("EtiUsed", (_lineCode, etiNo, componentNo, pointOfUseCode) => {
      if (_lineCode !== lineCode) return;
      dispatch("etiUsed", {
        etiNo: etiNo,
        componentNo: componentNo,
        pointOfUseCode: pointOfUseCode,
      });
    })
    .on("EtiUnloaded", (_lineCode, etiNo, componentNo, pointOfUseCode) => {
      if (_lineCode !== lineCode) return;
      dispatch("etiUnloaded", {
        etiNo: etiNo,
        componentNo: componentNo,
        pointOfUseCode: pointOfUseCode,
      });
    })
    .on(
      "EtiReturned",
      (
        _lineCode,
        etiNo,
        partNo,
        componentNo,
        pointOfUseCode,
        operatorNo,
        utcTimeStamp
      ) => {
        if (_lineCode !== lineCode) return;
        dispatch("etiReturned", {
          etiNo: etiNo,
          componentNo: componentNo,
          pointOfUseCode: pointOfUseCode,
        });
      }
    );

  const materialLoading = getSignalRClient(
    "Material Loading SignalR Client",
    "http://mxsrvapps.gt.local/gtt/services/materialloading/hubs/pointsofuse"
  ).on(
    "EtiCreated",
    (
      lineCodex,
      etiID,
      componentNo,
      revision,
      compDescription,
      quantity,
      utcTimeStamp
    ) => {
      if (lineCodex === lineCode) {
        dispatch("etiCreated", {
          quantity: quantity,
        });
      }
    }
  );

  const packaging = getSignalRClient(
    "Packaging SignalR Client",
    "http://mxsrvapps.gt.local/gtt/services/packaging/hubs/events"
  ).on("UnitPacked", (_lineCode, unidID, _workOrderCode) => {
    console.log("Unit Packed", {
      lineCode: _lineCode,
      workOrderCode: workOrderCode,
    });
    if (_workOrderCode === workOrderCode) {
      dispatch("unitPacked");
    }
  });

  onMount(async () => {
    etiMovements.start();
    materialLoading.start();
    packaging.start();
  });
</script>
