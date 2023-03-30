<script>
  import { onMount } from "svelte";
  import MessageItem from "./MessageItem.svelte";
  export let handleError;
  export let handleMessage;
  export let isShown = false;

  let messages = [];

  handleError = (message) => handleMessage(message, "error");

  handleMessage = (message, type = "normal") => {
    const item = {
      time: new Date().toLocaleTimeString(),
      text: message,
      type: type,
    };
    messages = [item, ...messages];
  };

  onMount(async () => {
    console.log = (...data) => {
      if (data.length == 1) handleMessage(data[0], "normal");
      else if (data.length > 1)
        handleMessage(`${data[0]}\n${data[1]}`, "normal");
    };
    console.error = (...data) => {
      if (data.length == 1) handleMessage(data[0], "error");
      else if (data.length > 1)
        handleMessage(`${data[0]}\n${data[1]}`, "error");
    };
    console.info = (...data) => {
      if (data.length == 1) handleMessage(data[0], "info");
      else if (data.length > 1) handleMessage(`${data[0]}\n${data[1]}`, "info");
    };
    // console.debug = (...data) => {
    //   if (data.length == 1) handleMessage(data[0], "warning");
    //   else if (data.length > 1)
    //     handleMessage(`${data[0]}\n${data[1]}`, "warning");
    // };
    window.alert = (message) => handleMessage(message, "success");
  });
</script>

<div class="pane">
  <button style="color: white" on:click={(e) => (isShown = !isShown)}>
    {isShown ? "❱" : "❰"}
  </button>
  <ul class={isShown ? "is-shown" : ""}>
    {#each messages as { time, text, type }, i}
      <MessageItem {time} {text} {type} />
    {/each}
  </ul>
</div>

<style lang="scss">
  .pane {
    background-color: #061933cc;
    border-left: 1px #ffffff99 solid;
    bottom: 0;
    color: white;
    position: absolute;
    right: 0;
    top: 0;
    z-index: 9999;
    max-width: 35%;
    > ul {
      list-style: none;
      margin: 0;
      overflow: hidden;
      padding: 0;
      width: 0;
      &.is-shown {
        overflow: hidden scroll;
        width: 100%;
      }
    }
    > button {
      background-color: steelblue;
      left: -2em;
      padding: 0.2em 0.5em;
      position: absolute;
    }
  }
</style>
