<script>
  import { onMount } from "svelte";
  import Sfx from "./utils/Sfx";
  import { PackagingApi } from "./utils/HttpRequests";

export let addMessage = null;
let input = null;
let qrMotor = null;
export let lineCode;
export let partNo;
 const handleSubmit = (event) => 
 {
    event.preventDefault();
    if(input.value != "")
    {
      input.disabled = true;
      qrMotor= prompt("Escanea el QR del motor:");
        if(qrMotor != "")
        {
      
            PackagingApi.JoinFramelessMotors(
            input.value,
            qrMotor,
            lineCode,
            partNo)
            .then((data) => 
            {
              Sfx.playSuccessSoundAsync();
              addMessage(data);
              input.disabled = false;
              input.focus();
              input.value = "";
            })
              .catch((error) => 
              {
                Sfx.playFailureSoundAsync();
                addMessage(error);
              })
                .then(() => 
                {
                  input.disabled = false;
                  input.focus();
                  input.value = "";
                });
                return false;
        }
            else
            {
            addMessage('No se acepta el campo del motor vacio');
            Sfx.playFailureSoundAsync();
            input.disabled = false;
            input.focus();
            input.value = "";
            }
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
