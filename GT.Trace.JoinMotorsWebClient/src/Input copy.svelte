<script>
  import { onMount } from "svelte";
  import Sfx from "./utils/Sfx";
  import { PackagingApi } from "./utils/HttpRequests";

export let addMessage = null;
export let lineCode = null;
let input = null;
let qrMotor1 = null;
let qrMotor2 = null;
let PalletQR = null;

 const handleSubmit = (event) => 
 {
    //alert(lineCode);
    event.preventDefault();
    if(input.value != "")
    {
      input.disabled = true;

    PalletQR= prompt("Escanea el QR del Pallet:");
    if(PalletQR != "")
    {
      qrMotor1= prompt("Escanea el QR del motor 1:");
        if (qrMotor1 != "") 
        {
          qrMotor2= prompt("Escanea el QR del motor 2:");
            if(qrMotor2 != "")
            {
                  PackagingApi.JoinEZMotors(
                  input.value,
                  qrMotor1,
                  qrMotor2,
                  1)
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
                                    
                  PackagingApi.JoinPallet(
                  input.value,
                  PalletQR,
                  lineCode,
                  1)
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
                  addMessage('No se acepta el campo Pallet vacio');
                  Sfx.playFailureSoundAsync();
                  input.disabled = false;
                  input.focus();
                  input.value = "";
                }
        }
        else
        {
        addMessage('No se acepta el campo de motor 1 vacio');
        Sfx.playFailureSoundAsync();
        input.disabled = false;
        input.focus();
        input.value = "";
        }
      }
            else
            {
              addMessage('No se acepta el campo de motor 2 vacio');
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
