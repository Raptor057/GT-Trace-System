<!-- 
Description: 
This archive it the body of the interface of Assembly Web Client, 
here is coding functional (the colors boxes),
here it view the composition of the gamma or also named boom. 
-->
<script>
  import { onMount } from "svelte";
  import AppHeader from "./AppHeader.svelte";
  import Input from "./Input.svelte";
  import AppFooter from "./AppFooter.svelte";
  import {MaterialLoadingApi} from "./utils/HttpRequests";
  import MessageLog from "./MessageLog.svelte";
  let addMessage;

  export let lineCode = "";

  let state = {
    name: null,
    activePart: { number: null, revision: null },
    activeWorkOrderCode: null,
    pointsOfUse: [],
    workOrder: { size: null, quantity: null },
  };

  // Handle for the timeout used to update the screen info.
  let timeoutHandle = null;

  // Handle API errors.               //<----Aqui se reciben los errores!!!!!!
  let handleError = (message) => {alert(message);}

  /**
   * Update the local line data on page load.
   */
  onMount(async () => updateLineData(lineCode));

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

</script>

<AppHeader 
  lineName={state.name}
  partNo={state.activePart.number}
  revision={state.activePart.revision}
  workOrderCode={state.activeWorkOrderCode}/>
<Input {lineCode}/>
<AppFooter/>

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
