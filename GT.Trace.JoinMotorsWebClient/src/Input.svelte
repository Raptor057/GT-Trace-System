<script>
  import { onMount } from "svelte";
  import Sfx from "./utils/Sfx";
  import {JoinMotors} from "./utils/HttpRequests";
  import MessageLog from "./MessageLog.svelte";
let addMessage;
  
  
let inputValue = "";
let motor1 = "";
let motor2 = "";

const transmissionRegex  = /\[\)>06SWB(?<transmissionID>\d+)P(?<clientPartNo>.+)Z.+1T(?<partNo>.+)2T(?<partRev>.+)3T(?<julianDay>\d+)$/;
//const motorRegex = /^(?<website>.+)\s+(?<voltage>[0-9\.]+[A-Z])\s+(?<rpm>[0-9]+)\s+(?<date>\d{4}-\d{1,2}-\d{1,2})\s+(?<time>\d{1,2}:\d{2})\s+(?<id>[0-9]+)$/;
  const motorRegex = /^(?<website>.+)\s+(?<voltage>[0-9\.]+[A-Z])(?:\s+)?(?<rpm>[0-9]+)\s+(?<date>\d{4}-\d{1,2}-\d{1,2})\s+(?<time>\d{1,2}:\d{2})\s+(?<id>[0-9]+)$/;

  function FnJoinMotors(ezlabel,motor1,motor2){
    JoinMotors.Join(ezlabel,motor1,motor2)
    .then((response)=>{
    console.log(response);
    })
    .catch((err)=> addMessage ('Error al enviar los datos: ' +err));
  }
  

  const handleSubmit = (event) => {
    event.preventDefault();
    const isValid = transmissionRegex.test(inputValue);
    if (isValid) {
      motor1 = prompt("Ingresa el motor #1:");
      const isValidMotor1 = motorRegex.test(motor1);
      if (isValidMotor1) {
        
        motor2 = prompt("Ingresa el motor #2:");
        const isValidMotor2 = motorRegex.test(motor2);
        if (isValidMotor2) {
        if (motor1 === motor2) {
          addMessage("El motor #1 y el motor #2 no pueden ser iguales. Por favor, inténtalo nuevamente.");
          Sfx.playFailureSoundAsync();
        } else {
          const transmissionID = inputValue.match(transmissionRegex).groups.transmissionID;
          const motorID1 = motor1.match(motorRegex).groups.id;
          const motorID2 = motor2.match(motorRegex).groups.id;

          FnJoinMotors(inputValue,motor1,motor2);
          Sfx.playSuccessSoundAsync();
          addMessage(`La Transmision \n ${transmissionID} \n se registro correctamente con los motores ${motorID1} y motores ${motorID2}.`);
        }

      } else {
        addMessage("Error al ingresar el motor #2, por favor intenta nuevamente");
          Sfx.playFailureSoundAsync();
        }
      } else {
        addMessage("Error al ingresar el motor #1, por favor intenta nuevamente");
        Sfx.playFailureSoundAsync();
      }
    } else {
      addMessage("Error por favor escanea la etiqueta de transmision!");
      Sfx.playFailureSoundAsync();
    }
    inputValue = "";
  };
  

  let input;
onMount(() => {
  if (input) {
    input.focus();
  }
});
</script>

<div class="app-child">
  <form on:submit={handleSubmit}>
    <input
      type="text"
      bind:value={inputValue}
      placeholder="Favor de escanear la etiqueta de la transmision."
    />
    <MessageLog bind:addMessage />
  </form>
</div>

<style>
  input {
    width: 100%;
  }
  div.app-child {
    padding: 2.5rem;
  }
</style>
