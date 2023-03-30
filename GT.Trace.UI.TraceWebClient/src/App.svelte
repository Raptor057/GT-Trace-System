<!-- 
Description: 
This archive it the body of the interface of Assembly Web Client, 
here is coding functional (the colors boxes),
here it view the composition of the gamma or also named boom. 
-->
<script>
  import { onMount } from "svelte";
  import Gamma from "./Gamma/Gamma.svelte";
  //import MessagePane from "./MessagePane/MessagePane.svelte";
  import AppHeader from "./AppHeader.svelte";
  import AppFooter from "./AppFooter.svelte";
  import GlobalKeyboardInputListener from "./utils/GlobalKeyboardInputListener.svelte";
  import Sfx from "./utils/Sfx";
  import Animator from "./utils/Animator";
  import SignalR from "./utils/SignalR.svelte";
  import {
    MaterialLoadingApi,
    EtiMovementsApi,
    CommonApi,
    ProcessHistoryApi,
    PackagingApi,
  } from "./utils/HttpRequests";
  import { intros } from "svelte/internal";

  export let lineCode = "";

  let production = {};

  /**
   * Updates the state's point of use data.
   * @param pointOfUse The new point of use state.
   */
  const updatePointOfUseInLineData = (pointOfUse) => {
    state = {
      ...state,
      pointsOfUse: state.pointsOfUse.filter((item) =>
        item.code === pointOfUse.code &&
        item.component.number === pointOfUse.component.number
          ? pointOfUse
          : item
      ),
    };
  };

  /**
   * Find point of use data.
   * @param detail Point of use code and component number.
   */
  const findPointOfUseInLineData = (detail) => {
    return state.pointsOfUse.find(
      (item) =>
        item.code === detail.pointOfUseCode &&
        item.component.number === detail.componentNo
    );
  };

  let state = {
    name: null,
    activePart: { number: null, revision: null },
    activeWorkOrderCode: null,
    pointsOfUse: [],
    workOrder: { size: null, quantity: null },
  };

  //================ESTO SE COMENTO 3/10/2023====================
  // Indicates whether the corresponding checkbox is checked.
  let materialReturnModeIsEnabled = false;

  // Handle for the timeout used to update the screen info.
  //================ESTO SE COMENTO 3/10/2023====================
  //let timeoutHandle = null;

  // Handle API errors.               //<----Aqui se reciben los errores!!!!!!
  let handleError = (message) => 
  {
      if(message.indexOf('No se puede utilizar la ETI') !== -1) 
      {
        alert(message);
        //================ESTO SE COMENTO 3/10/2023====================
        //unlockLinewhile();
      }
        else
        {
          alert(message);
        }
  }
    
  
  // Get the total active ETI count.
  $: activeEtiCount = state.pointsOfUse.filter(
    (item) => item.activeEti && item.activeEti.number
  ).length;

  // Get the size of the gamma.
  $: gammaSize = state.pointsOfUse.length;

  /**
   * Update the local line data on page load.
   */
  onMount(async () => updateLineData(lineCode));

  /**
   * This is basically a hack to allow individual labels to be scanned in the traceability screen.
   * * This should be handled server side but I don't know how to identify the label without duplicating the code.
   *
   * @return True if and individual label was identified.
   */
  const tryProcessIndividualLabel = (input) => {
    const re = /(?<process>\d+)?\[\).+[A-Z]{2}(?<unit>.+?)P.+/;
    const match = re.exec(input);
    Sfx.playEtiUsedAudio(); //Cambio: se agrego sonido en esta linea al escanear una etiqueta.
    alert("Etiqueta individual escaneada");
    //================ESTO SE COMENTO 3/10/2023====================
    // if (match && match.groups && match.groups.unit) {
    //   if (!match.groups.process) {
    //     alert(
    //       "Se detectó una etiqueta individual pero no se encontró información del proceso."
    //     );
    //   } else {
    //     ProcessHistoryApi.recordProcess(
    //       lineCode,
    //       match.groups.process.trim(),
    //       parseInt(match.groups.unit)
    //     )
    //       .then(() => {
    //         Sfx.playEtiUsedAudio(); //Cambio: se agrego sonido en esta linea al escanear una etiqueta.
    //       })
    //       .catch((err) => alert(err));
    //   }
    //   return true;
    // }

    return false;
  };

  /**
   * Try to process the user input when the enter key is detected. The input can be an
   * individual label or an ETI label. The ETI will be returned if the Return checkbox
   * is checked.
   * @param e An event structure with the detail property containing the input attribute.
   */
  const handleTriggerPress = (e) => {
    // Try to handle the input as individual label.
    if (tryProcessIndividualLabel(e.detail.input)) {
      return;
    }

    //================ESTO SE COMENTO 3/10/2023====================
    // if (materialReturnModeIsEnabled) {
    //   if (
    //     confirm(
    //       `ETI#${e.detail.input} será retornada. Presióne OK para continuar...`
    //     )
    //   ) {
    //     EtiMovementsApi.returnEti(lineCode, e.detail.input).catch(handleError);
    //   }
    // } else {
    //   EtiMovementsApi.useEti(lineCode, e.detail.input).catch(handleError);
    // }
  };

  /**
   * Get production info for the current hour.
   */
//================ESTO SE COMENTO 3/10/2023====================
   // const fetchCurrentHourProduction = () =>
  //   CommonApi.getCurrentHourProduction(lineCode)
  //     .then((data) => (production = { ...data.production }))
  //     .catch(handleError);

  /**
   * Fetch line data.
   * @param lineCode Two-char line code.
   */
  const updateLineData = async (lineCode) => {
    if (lineCode) {
      MaterialLoadingApi.getLine(lineCode)
        .then((data) => (state = data))
        .catch(handleError);
      fetchCurrentHourProduction();
    }
  };

  /**
   * Prepare a timeout to update the line data after n seconds.
   * @param lineCode Two-char line code.
   * @param seconds Timeout seconds.
   */
  const scheduleLineDataUpdate = (lineCode, seconds = 15) => {
    if (timeoutHandle) window.clearTimeout(timeoutHandle);
    timeoutHandle = window.setTimeout(async () => {
      updateLineData(lineCode);
      timeoutHandle = null;
    }, seconds * 1000);
  };

  /**
   * Handle the SignalR event that occurrs every time an ETI is used by the operation.
   * The loaded ETI counter is reduced and active ETI is shown in the corresponding point of use.
   * @param e Event details.
   */
  const handleEtiUsed = (e) => {
    Sfx.playEtiUsedAudio();
    scheduleLineDataUpdate(lineCode);
    const pointOfUse = findPointOfUseInLineData(e.detail);
    pointOfUse.activeEti = { number: e.detail.etiNo };
    pointOfUse.load -= 1;
    window.setTimeout(() => updatePointOfUseInLineData(pointOfUse), 800);
    Animator.flipInX(pointOfUse);
  };

  /**
   * Handle the SignalR event that occurrs every time an ETI is loaded by the waterspider.
   * The loaded ETI counter is increased for the corresponding point of use.
   * @param e Event details.
   */
  const handleEtiLoaded = (e) => {
    Sfx.playEtiLoadedAudio();
    scheduleLineDataUpdate(lineCode);
    const pointOfUse = findPointOfUseInLineData(e.detail);
    pointOfUse.load += 1;
    window.setTimeout(() => updatePointOfUseInLineData(pointOfUse), 150);
    Animator.bounceIn(pointOfUse);
  };

  /**
   * Handle the SignalR event that occurrs every time an ETI is unloaded (not returned).
   * The loaded ETI counter is reduced for the corresponding point of use.
   * @param e Event details.
   */
  const handleEtiUnloaded = (e) => {
    Sfx.playEtiUnloadedAudio();
    scheduleLineDataUpdate(lineCode);
    const pointOfUse = findPointOfUseInLineData(e.detail);
    pointOfUse.load -= 1;
    window.setTimeout(() => updatePointOfUseInLineData(pointOfUse), 1000);
    Animator.bounceOutDown(pointOfUse);
  };

  /**
   * Handle the SignalR event that occurrs every time an ETI is returned.
   * The active ETI is cleared from the point of use.
   * @param e Event details.
   */
  const handleEtiReturned = (e) => {
    Sfx.playEtiReturnedAudio();
    scheduleLineDataUpdate(lineCode);
    const pointOfUse = findPointOfUseInLineData(e.detail);
    pointOfUse.activeEti.number = "";
    window.setTimeout(() => updatePointOfUseInLineData(pointOfUse), 1000);
    Animator.bounceOutDown(pointOfUse);
  };

  /**
   * Handle the SignalR event that occurrs every time an ETI is packed.
   * The work order status is updated.
   * @param e Event details.
   */
  const handleUnitPacked = (e) => {
    console.debug("Unit Packed");

    fetchCurrentHourProduction();
  };


  //================ESTO SE COMENTO 3/10/2023====================
// /**
//  *Poka Yoke implemented to block the lines requested as a corrective action for 8D ACIN-2223-005.
// 2 endpoints were added in the Packaging api
// 1) set a line lock and unlock when the assembly UI scans for something wrong.
// 2) supervisor password validation.
// */
// const unlockLinewhile = () => {SetBlockStatus()} //Constante que se ejecuta al momento de un alert() que a su vez manda allamar a otra constante.


// let Isavailablepassword = null;

// const getAuthorizedUserPassword = (AuthorizedUserPassword) => {
//   return PackagingApi.getAuthorizedUserPassword(AuthorizedUserPassword)
//       const data =  response.json()
//       .then((response) => {
//       Isavailablepassword = response.data.password;
//       return isavailablepassword;
//     })
//     .catch(handleError);
// };

// const SetBlockStatus = () => {
//             PackagingApi.SetStationBlocked(1,lineCode) //Bloqueamos toda la linea
//             let imputpassword = prompt("Linea Bloqueada \n Ingresa la contraseña del supervisor", ""); //Entrada de contraseña
//             //https://developer.mozilla.org/es/docs/Learn/JavaScript/First_steps/Useful_string_methods
//             // let imputpassword3 = imputpassword.slice(3,20);
//             // let imputpassword4 = imputpassword.slice(4,20);z
//             getAuthorizedUserPassword(imputpassword)
//             .then((response) => {
//               if(response)
//               {
//                   if(response.password == true){
//                   PackagingApi.SetStationBlocked(0, lineCode);  
//                   }
//                   else{
//                     alert("Contraseña Incorrecta \n Ingresa la contraseña Correcta para desbloquear");
//                     unlockLinewhile();
//                   }
//               } 
//             });
//           };
</script>

<SignalR
  {lineCode}
  workOrderCode={state.workOrder.code}
  on:etiUsed={handleEtiUsed}
  on:etiLoaded={handleEtiLoaded}
  on:etiUnloaded={handleEtiUnloaded}
  on:etiReturned={handleEtiReturned}
  on:unitPacked={handleUnitPacked}
/>

<GlobalKeyboardInputListener
  on:triggerpress={handleTriggerPress}
  triggerKey="Enter"
/>

<!-- <MessagePane bind:handleError /> -->

<AppHeader
  {production}
  lineName={state.name}
  partNo={state.activePart.number}
  revision={state.activePart.revision}
  {activeEtiCount}
  {gammaSize}
  workOrderCode={state.activeWorkOrderCode}
/>

  <!-- ================ESTO SE COMENTO 3/10/2023==================== -->
<!-- <Gamma {lineCode} items={state.pointsOfUse} {materialReturnModeIsEnabled} /> -->
<Gamma {lineCode} items={state.pointsOfUse} />


<AppFooter bind:materialReturnModeIsEnabled />

<!-- $color: #ff3e00; -->
<style lang="scss">
  :global(*) {
    box-sizing: border-box;
    user-select: none; /* For Chrome and Opera */
    -ms-user-select: none; /* For Internet Edge and Explorer*/
    -webkit-user-select: none; /* For Safari */
    -moz-user-select: none; /* For Firefox */
    -khtml-user-select: none; /* Konqueror HTML */
  }

  :global(body) {
    background-color: whitesmoke;
    font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto,
      Oxygen-Sans, Ubuntu, Cantarell, "Helvetica Neue", sans-serif;
    font-size: 16px;
    padding: 0;
  }
</style>
