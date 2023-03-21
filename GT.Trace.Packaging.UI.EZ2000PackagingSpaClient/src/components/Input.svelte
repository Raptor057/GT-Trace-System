<script>
  import { onMount } from "svelte";
  import { PackagingApi } from "../utils/HttpRequests";
  import Sfx from "../utils/Sfx";

  export let hostname = null;
  export let addMessage = null;
  export let workOrderCode = "";

  let input = null;
  const handleSubmit = (e) => {
    e.preventDefault();
    input.disabled = true;
    PackagingApi.packUnit(
      hostname,
      input.value,
      localStorage[`palletSize_${workOrderCode}`],
      localStorage[`containerSize_${workOrderCode}`],
      localStorage[`purchaseOrder_${workOrderCode}`]
    )
      .then((data) => {
        Sfx.playSuccessSoundAsync();
      })
      .catch((error) => {
        Sfx.playFailureSoundAsync();
        addMessage(error);
      })
      .then(() => {
        input.disabled = false;
        input.focus();
        input.value = "";
      });
    return false;
  };
  onMount(() => input.focus());
</script>

<div class="app-child">
  <form on:submit={handleSubmit}>
    <input
      bind:this={input}
      type="text"
      placeholder="Favor de escanear la etiqueta individual."
    />
  </form>
</div>

<style>
  input {
    width: 100%;
  }
  div.app-child {
    padding: 0.5rem;
  }
</style>
