<script>
  import HourlyProduction from "./HourlyProduction/HourlyProduction.svelte";
  import Body from "./Body/Body.svelte";
  import Footer from "./Footer.svelte";
  import Header from "./Header.svelte";
  import { PackagingApi } from "../utils/HttpRequests.js";
  import { onMount } from "svelte";
  import Input from "./Input.svelte";
  import MessageLog from "./MessageLog.svelte";
  import SignalR from "../utils/SignalR.svelte";
  import Sfx from "../utils/Sfx";

  export let hostname = null;

  let addMessage;
  let lineState = {
    loadedPart: { number: null, revision: null },
    headcount: null,
    workOrder: {
      packType: { isBox: false },
      code: null,
      masterType: { isAteq: false },
      client: { code: "", name: "", description: "", partNo: "" },
      po: null,
    },
    bomLabel: { description: "", number: "" },
    pallet: {
      packagingImageBase64Data: "",
      sampleImageBase64Data: "",
      size: 0,
      quantity: 0,
      container: {
        size: 0,
        quantity: 0,
      },
    },
  };
  let hourlyProduction = [];

  const update = () =>
    PackagingApi.getStationState(hostname, null, null, null)
      .then((state) => {
        console.debug(state);
        return state;
      })
      .then((state) => {
        lineState = { ...state };
        fetchHourlyProduction();
      })
      .catch(addMessage);

  onMount(update);

  const fetchHourlyProduction = () => {
    PackagingApi.fetchHourlyProduction(lineState.lineCode)
      .then((data) => (hourlyProduction = data.hourlyProduction))
      .catch(addMessage);
  };

  const handleUnitPacked = (e) => {
    addMessage(`Unidad #${e.detail.unitID} empacada.`);
    lineState = {
      ...lineState,
      workOrder: {
        ...lineState.workOrder,
        quantity: lineState.workOrder.quantity + 1,
      },
      pallet: {
        ...lineState.pallet,
        isQcApproved:
          !lineState.pallet.isApproved &&
          lineState.approval.stopLevel <= lineState.workOrder.quantity,
        quantity: lineState.pallet.quantity + 1,
        container: {
          ...lineState.pallet.container,
          quantity: lineState.pallet.container.quantity + 1,
        },
      },
      picking: {
        ...lineState.picking,
        countdown: lineState.picking.countdown - 1,
      },
    };

    fetchHourlyProduction();
  };

  const handleContainerComplete = (e) => {
    addMessage("Contenedor completo.");
    update();
  };

  const handlePalletComplete = (e) => {
    const data = JSON.parse(e.detail.jsonData);
    addMessage(
      `Tarima completa. ${
        lineState.workOrder.masterType.isAteq ? "ATEQ" : "MASTER"
      }#${data.MasterID}`
    );
    update();
  };

  const handleUnitPicked = (e) => {
    addMessage(
      `${lineState.picking.sequence + 1}a. pieza tomada para pruebas (unidad #${
        e.detail.unitID
      })`
    );
    Sfx.playPickingSoundAsync();
    if (lineState.picking.sequence < lineState.picking.totalSamples - 1) {
      lineState = {
        ...lineState,
        picking: {
          ...lineState.picking,
          sequence: lineState.picking.sequence + 1,
        },
      };
    } else {
      lineState = {
        ...lineState,
        picking: {
          ...lineState.picking,
          sequence: 0,
          countdown: lineState.picking.interval,
        },
      };
    }
  };
</script>

<SignalR
  lineCode={lineState.lineCode}
  workOrderCode={lineState.workOrder.code}
  on:unitPacked={handleUnitPacked}
  on:containerComplete={handleContainerComplete}
  on:palletComplete={handlePalletComplete}
  on:unitPicked={handleUnitPicked}
/>
<div id="app">
  <Header
    lineName={lineState.name}
    lineCode={lineState.lineCode}
    workOrderCode={lineState.workOrder.code}
    partNo={lineState.loadedPart.number}
    revision={lineState.loadedPart.revision}
    masterType={lineState.workOrder.masterType.isAteq ? "ATEQ" : "MASTER"}
    family={lineState.family}
    {hostname}
    containerQuantity={lineState.pallet.container.quantity}
    {addMessage}
  />
  <Input {hostname} {addMessage} workOrderCode={lineState.workOrder.code} />
  <Body
    packType={lineState.workOrder.packType.isBox ? "Caja" : "Colapsable"}
    palletSize={lineState.pallet.size}
    palletQuantity={lineState.pallet.quantity}
    containerSize={lineState.pallet.container.size}
    containerQuantity={lineState.pallet.container.quantity}
    workOrderCode={lineState.workOrder.code}
    workOrderSize={lineState.workOrder.size}
    workOrderQuantity={lineState.workOrder.quantity}
    approval={{
      ...lineState.approval,
      isApproved: lineState.pallet.isQcApproved,
    }}
    picking={lineState.picking || {
      countdown: 0,
      sequence: 0,
      totalSamples: 0,
    }}
    customerNo={lineState.workOrder.client.code}
    customerName={lineState.workOrder.client.name}
    customerDescription={lineState.workOrder.client.description}
    customerPartNo={lineState.workOrder.client.partNo}
    poNumber={lineState.workOrder.po}
    bomLabel={lineState.bomLabel}
    packagingImageBase64Data={lineState.pallet.packagingImageBase64Data}
    sampleImageBase64Data={lineState.pallet.sampleImageBase64Data}
    lineCode={lineState.lineCode}
    headcount={lineState.headcount}
    {addMessage}
  />
  <HourlyProduction
    lineCode={lineState.lineCode}
    {addMessage}
    {hourlyProduction}
  />
  <MessageLog bind:addMessage />
  <Footer {...lineState.station} />
</div>

<style lang="scss">
  :global(*) {
    box-sizing: border-box;
  }

  :global(body, html) {
    background-color: whitesmoke;
    height: 100%;
    margin: 0;
    padding: 0;
  }

  :global(body, input, button) {
    font-family: "Calibri", "Segoe UI";
    font-size: 1.5vw;
  }

  :global(table tbody tr th) {
    text-align: right;
    vertical-align: top;
  }

  :global(#app) {
    display: flex;
    flex-direction: column;
    height: 100%;
  }

  :global(#app > .app-child) {
    padding: 0.5rem;
  }

  :global(#app > #app-body) {
    flex-grow: 0;
    display: flex;
    justify-content: space-evenly;
    gap: 5px;
  }

  :global(#app > #app-body > div) {
    background: #00000011;
    border-radius: 5px;
    padding: 8px;
    flex: 1;
  }

  :global(#app table) {
    width: 100%;
  }

  :global(#app table > tbody > tr > th) {
    width: 25%;
  }
</style>
