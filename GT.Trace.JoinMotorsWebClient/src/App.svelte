<script>
  import { onMount } from "svelte";
  import { MaterialLoadingApi } from "./utils/HttpRequests";
  import AppHeader from "./AppHeader.svelte";
  import Input from "./Input.svelte";
  import MessageLog from "./MessageLog.svelte";
  import AppFooter from "./AppFooter.svelte";

  import Sfx from "./utils/Sfx";
  import { PackagingApi } from "./utils/HttpRequests";

  export let lineCode = "";
  export let partNo = "";
  let unitID = null;
  let qrMotor = null;
  let qrMotor1 = null;
  let qrMotor2 = null;
  let addMessage;
  
  let state = {
    name: null,
    activePart: { number: null, revision: null },
    activeWorkOrderCode: null,
    pointsOfUse: [],
    workOrder: { size: null, quantity: null },
  };


  const btnDelTransmissions = () => 
 {
    unitID = prompt("Escanea el QR de la tranmision:");
    if(unitID != "")
    {
      qrMotor1= prompt("Escanea el QR del motor 1:");
        if(qrMotor1 != "")
        {
          qrMotor2= prompt("Escanea el QR del motor 2:");
          if(qrMotor2 != "")
          {
            PackagingApi.JoinEZMotors(
            unitID,
            qrMotor1,
            qrMotor2,
            0,)
            .then((data) => 
            {
              Sfx.playSuccessSoundAsync();
              addMessage(data);
            })
              .catch((error) => 
              {
                Sfx.playFailureSoundAsync();
                addMessage(error);
              })
                .then(() => 
                {});
                return false;
          }
          else
          {
            addMessage('No se acepta el campo del motor 1 vacio.');
          }
          
        }
          else
          {
            addMessage('No se acepta el campo del motor 1 vacio.');
          }
    }
    else
    {
    addMessage('No se acepta el campo de la transmision vacio.');
    }
  };

  // Handle for the timeout used to update the screen info.
  let timeoutHandle = null;

  // Handle API errors.               //<----Aqui se reciben los errores!!!!!!
  let handleError = (message) => {alert(message);}

  /**
   * Update the local line data on page load.
   */
  onMount(async () => updateLineData(lineCode));
  onMount(async () => partNo =state.activePart.number);

  /**
   * Fetch line data.
   * @param lineCode Two-char line code.
   */
  const updateLineData = async (lineCode) => {
    if (lineCode) {
      MaterialLoadingApi.getLine(lineCode)
        .then((data) => (state = data))
        .catch(handleError);
    }
  };

</script>

<div id="app">
  <AppHeader
  lineName={state.name}
  partNo={state.activePart.number}
  revision={state.activePart.revision}
  workOrderCode={state.activeWorkOrderCode}
  btnDel={btnDelTransmissions}
/>
<Input partNo={state.activePart.number} {lineCode}{addMessage}/>
<MessageLog bind:addMessage />
<AppFooter/>

</div>


<style lang="scss">
  :global(*) {
    box-sizing: border-box;
    user-select: none; /* For Chrome and Opera */
    -ms-user-select: none; /* For Internet Edge and Explorer*/
    -webkit-user-select: none; /* For Safari */
    -moz-user-select: none; /* For Firefox */
    -khtml-user-select: none; /* Konqueror HTML */
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

  // :global(#app > .app-child) {
  //   padding: 0.5rem;
  // }

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