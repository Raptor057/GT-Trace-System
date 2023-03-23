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
    EventsHistory,
  } from "./utils/HttpRequests";
  import { intros } from "svelte/internal";
  export let lineCode = "";
  let production = {};

  //#region 
  //*Poka Yoke implemented to block the lines requested as a corrective action for 8D ACIN-2223-005.


/*
The splitString function takes a string str as an argument and returns an object with two properties: first and second, which represent the two words separated by a space in the string.
Feature Details:
The text string is split into an array using the JavaScript split() method, which takes as its argument the separator to use to split the string into array elements. In this case, the separator is a space.
The resulting array contains two elements, corresponding to the two words separated by space in the original string.
These two elements map to the first and second properties of the object being returned.
*/
  const splitString = (str) => {
    const arr = str.split(" ");
    return { first: arr[0], second: arr[1] };
  };

  /*
This is a JavaScript function that uses an API called MaterialLoadingApi to get usage points from a specified ETI (Material Identification Tag).
The function takes a parameter called "etiInput", which is the ETI number to search for.
The function then declares four additional variables: "etiNo", "selectedPartNo", "selectedLineCode", and "pointsOfUse".
The variable "etiNo" is simply a copy of the input parameter "etiInput". "selectedPartNo" is set to the current active part number, which is found in the "number" property of the "state" object.
The "selectedLineCode" variable is set using a function called "splitString", which takes the "name" property of the "state" object, which is assumed to be a string in the "FirstSecond" format. The function splits this string into two parts, "first" and "second", and returns an object with both parts. The "selectedLineCode" variable is set to the "second" part of this object.
The "pointsOfUse" variable is simply an empty array that will be used to store the ETI points of use found by the API.
The function then calls the "getEtiPointsOfUse" method of the "MaterialLoadingApi" API, passing "etiNo", "selectedLineCode" and "selectedPartNo" as parameters. This method returns a promise, which the function uses to get the ETI usage points.
If the promise is resolved successfully, the function adds the usage points returned by the API to the "pointsOfUse" array using the spread operator. The function then checks if the length of the returned data is zero. If so, the function displays an alert that no usage points were found and calls a function called "unlockLinewhile".
If the promise is rejected (that is, if there is an error), the function sets the "pointsOfUse" array to an empty array and displays the error on the console.
In short, this function uses an API to look up the usage points of an ETI and handles the results and errors of the promise returned by the API.
  */
  const getEtiPointsOfUse = (etiInput) => {
  let etiNo = etiInput;
  let selectedPartNo = state.activePart.number;
  let { first, second } = splitString(state.name);
  let selectedLineCode = second;
  let pointsOfUse = [];
  // alert(etiNo);
  // alert(selectedLineCode);
  // alert(selectedPartNo);
  MaterialLoadingApi.getEtiPointsOfUse(etiNo, selectedLineCode, selectedPartNo)
    .then((data) => {
      pointsOfUse = [...pointsOfUse, ...data];
      if (data.length === 0) {
        const getEtimessage = (`- La ETI "${etiNo}" no corresponde con ningún punto de uso para la gama "${selectedPartNo} en la Linea ${selectedLineCode}".`);
        //alert(`La ETI "${etiNo}" no corresponde con ningún punto de uso para la gama "${selectedPartNo} en la Linea ${selectedLineCode}".`);
        alert(getEtimessage);
        unlockLinewhile();
        EventHistory(getEtimessage,lineCode);
    }})
    .catch((error) => {
      pointsOfUse = [];
      //console.log(error);
    });
};

//#endregion

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

  /*
  Envio de mensaje al host
  */
  function EventHistory(message, lineCode) {
    EventsHistory.recordHistory(message, lineCode)
      .then(() => {
        //alert('Mensaje enviado con éxito.');
      })
      .catch((err) => alert('Error al enviar el mensaje: ' + err));
  }

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

  // Indicates whether the corresponding checkbox is checked.
  let materialReturnModeIsEnabled = false;

  // Handle for the timeout used to update the screen info.
  let timeoutHandle = null;

  // Handle API errors.               //<----Aqui se reciben los errores!!!!!!
  let handleError = (message) => 
  { alert(message);
    EventHistory(message,lineCode);
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
            //Aqui 
            getEtiPointsOfUse(input); //<--Aqui se esta agregando este pedo
    if (match && match.groups && match.groups.unit) {
      if (!match.groups.process) {
        alert(
          "Se detectó una etiqueta individual pero no se encontró información del proceso."
        );
      } else {
        ProcessHistoryApi.recordProcess(
          lineCode,
          match.groups.process.trim(),
          parseInt(match.groups.unit)
        )
          .then(() => {
            Sfx.playEtiUsedAudio(); //Cambio: se agrego sonido en esta linea al escanear una etiqueta.
          })
          .catch((err) => alert(err));
      }
      return true;
    }

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

    if (materialReturnModeIsEnabled) {
      if (
        confirm(
          `ETI#${e.detail.input} será retornada. Presióne OK para continuar...`
        )
      ) {
        EtiMovementsApi.returnEti(lineCode, e.detail.input).catch(handleError);
      }
    } else {
      EtiMovementsApi.useEti(lineCode, e.detail.input).catch(handleError);
    }
  };

  /**
   * Get production info for the current hour.
   */
  const fetchCurrentHourProduction = () =>
    CommonApi.getCurrentHourProduction(lineCode)
      .then((data) => (production = { ...data.production }))
      .catch(handleError);

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

/**
 *Poka Yoke implemented to block the lines requested as a corrective action for 8D ACIN-2223-005.
2 endpoints were added in the Packaging api
1) set a line lock and unlock when the assembly UI scans for something wrong.
2) supervisor password validation.
*/
const unlockLinewhile = () => {SetBlockStatus()} //Constante que se ejecuta al momento de un alert() que a su vez manda allamar a otra constante.


let Isavailablepassword = null;

const getAuthorizedUserPassword = (AuthorizedUserPassword) => {
  return PackagingApi.getAuthorizedUserPassword(AuthorizedUserPassword)
      const data =  response.json()
      .then((response) => {
      Isavailablepassword = response.data.password;
      return isavailablepassword;
    })
    .catch(handleError);
};

const SetBlockStatus = () => {
  PackagingApi.SetStationBlocked(1,lineCode) //Bloqueamos toda la linea
  let imputpassword = prompt("Linea Bloqueada \n Ingresa la contraseña del supervisor / Auditor de calidad", ""); //Entrada de contraseña
  getAuthorizedUserPassword(imputpassword)
    .then(handleAuthorization);
};

export const ButtonUnlockLine = () => {
  let imputpassword = prompt("Ingresa la contraseña del supervisor / Auditor de calidad", ""); //Entrada de contraseña
  getAuthorizedUserPassword(imputpassword)
    .then(handleAuthorization);
};


const handleAuthorization = (passwordResponse) => {
  if (passwordResponse) {
    if (passwordResponse.password == true) {
      PackagingApi.SetStationBlocked(0, lineCode);
    } else {
      alert("Contraseña Incorrecta \n Ingresa la contraseña Correcta para desbloquear");
      unlockLinewhile();
    }
  }
}
//#region 
//Este codigo es el original para bloqueo de linea, pero arriba se separo en 2 funciones para poder desbloquear la linea mediante un boton.
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
//#endregion
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
  <!-- La linea btnUnlock={ButtonUnlockLine} en el AppHeader activa el metodo ButtonUnlockLine al precionar el boton desbloquear Linea  -->
<AppHeader
  {production}
  lineName={state.name}
  partNo={state.activePart.number}
  revision={state.activePart.revision}
  {activeEtiCount}
  {gammaSize}
  workOrderCode={state.activeWorkOrderCode}
  btnUnlock={ButtonUnlockLine}
/>

<Gamma {lineCode} items={state.pointsOfUse} {materialReturnModeIsEnabled} />

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
