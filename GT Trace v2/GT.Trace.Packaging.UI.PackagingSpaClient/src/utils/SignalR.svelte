<script>
  import { createEventDispatcher } from "svelte";
  import { onMount } from "svelte";

  export let lineCode = "";

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

  const packaging = getSignalRClient(
    "Packaging SignalR Client",
    "http://mxsrvapps.gt.local/gtt/services/packaging/hubs/events"
  )
    //const SignalRPackagingClient = getSignalRClient('Packaging SignalR Client', 'http://localhost:5183/hubs/events')
    .on("UnitPacked", (_lineCode, unitID, workOrderCode) => {
      if (lineCode != _lineCode) return;
      console.debug("UnitPacked", { lineCode: lineCode, unitID: unitID });
      dispatch("unitPacked", { lineCode: lineCode, unitID: unitID });
    })
    .on("UnitUnpacked", (_lineCode, unitID) => {
      /**
       * ! Picking counter not being updated when a unit is packed in another line.
       * TODO: Update picking counter using the family (add extra info if needed).
       */
      if (lineCode != _lineCode) return;
      console.debug("UnitUnpacked", { lineCode: lineCode, unitID: unitID });

      dispatch("unitUnpacked", { lineCode: lineCode, unitID: unitID });
    })
    .on("UnitPicked", (_lineCode, unitID) => {
      if (lineCode != _lineCode) return;
      console.debug("UnitPicked", { lineCode: lineCode, unitID: unitID });
      dispatch("unitPicked", { lineCode: lineCode, unitID: unitID });
    })
    .on("PalletComplete", (_lineCode, jsonData) => {
      if (lineCode != _lineCode) return;
      console.debug("PalletComplete", {
        lineCode: lineCode,
        jsonData: jsonData,
      });
      dispatch("palletComplete", {
        lineCode: lineCode,
        jsonData: jsonData,
      });
    })
    .on("ContainerComplete", (_lineCode, jsonData) => {
      if (lineCode != _lineCode) return;
      console.debug("ContainerComplete", {
        lineCode: lineCode,
        jsonData: jsonData,
      });
      dispatch("containerComplete", {
        lineCode: lineCode,
        jsonData: jsonData,
      });
    });

  onMount(async () => {
    packaging.start();
  });
</script>
