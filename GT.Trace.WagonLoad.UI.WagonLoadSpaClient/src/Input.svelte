<script>
  import { onMount } from "svelte";
  import Sfx from "./utils/Sfx";
  export let addMessage = null;
let input = null;

 const handleSubmit = (event) => 
 {
    event.preventDefault();

    if(input.value != "")
    {
        Sfx.playSuccessSoundAsync();
        input.disabled = false;
        input.focus();
        input.value = "";
    }
      else
      {
      addMessage('No se acepta el campo de la transmision vacio');
      Sfx.playFailureSoundAsync();
      input.disabled = false;
      input.focus();
      input.value = "";
      }
  };

  onMount(() => input.focus());

</script>

<div class="app-child">
  <form on:submit={handleSubmit}>
    <input
      type="text"
      bind:this={input}
      placeholder="Favor de escanear la etiqueta de componente."
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
