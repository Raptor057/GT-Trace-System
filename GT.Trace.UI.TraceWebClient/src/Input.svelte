<script>
import Sfx from "./utils/Sfx";
import { onMount } from "svelte";
import {ProcessHistoryApi} from "./utils/HttpRequests";
import MessageLog from "./MessageLog.svelte";
let addMessage;
  

export let lineCode = null;

let inputValue = "";

  const transmissionRegex  = /\[\)>06SWB(?<transmissionID>\d+)P(?<clientPartNo>.+)Z.+1T(?<partNo>.+)2T(?<partRev>.+)3T(?<julianDay>\d+)$/;

  const motorRegex = /^(?<Volt>[0-9\.]+[A-Z])\|(?<RPM>[0-9]+)\|(?<datetime>\d{6}-\d{6})\|(?<SerialNumber>[0-9A-Z]+)$/;

  const etis = /^(E|SA)\d{4,9}(-T\d{4,9})?$/;
  //----------------
  function Etis(EtiNo){
    ProcessHistoryApi.UpdateEtis(EtiNo)
    .then((response)=>{
      Sfx.playSuccessSoundAsync();
    })
    .catch((error) => {
        Sfx.playFailureSoundAsync();
        addMessage(error);
      });
  }

  function SaveMotors(ProductionID,LineCode){
    ProcessHistoryApi.SaveElectricalMotors(ProductionID,LineCode)
    .then((response)=>{
      Sfx.playSuccessSoundAsync();
    })
    .catch((error) => {
        Sfx.playFailureSoundAsync();
        addMessage(error);
      });
  }



  function RecordProcess(UnitID,LineCode){
    ProcessHistoryApi.RecordProcessNo(UnitID,LineCode)
    .then((response)=>{
    Sfx.playSuccessSoundAsync();
    })
    .catch((error) => {
        Sfx.playFailureSoundAsync();
        addMessage(error);
      });
  }
  //----------------
  const handleSubmit = (event) => {
    event.preventDefault();
    const isEti = etis.test(inputValue);
    const isTransmissionInput = transmissionRegex.test(inputValue);
    const isMotor = motorRegex.test(inputValue);

    if (isEti) {  
    Etis(inputValue);
    addMessage(`La Eti \n ${inputValue} \n se registro correctamente para la linea: ${lineCode}.`);
  } else if (isTransmissionInput) {
    RecordProcess(inputValue,lineCode);
    addMessage(`El valor \n ${inputValue} \n se registro en: ${lineCode}.`);
  } else if(isMotor){
    SaveMotors(inputValue,lineCode);
    addMessage(`El valor \n ${inputValue} \n se registro en: ${lineCode}.`);
  }
  else{
    Sfx.playFailureSoundAsync();
      //alert(`El valor \n ${inputValue} \n no es válido para la linea ${lineCode}.`);
      addMessage(`El valor \n ${inputValue} \n no es válido para la linea ${lineCode}.`);
      
  }
    inputValue = "";
  };
  
</script>

<div class="app-child">

  <form on:submit={handleSubmit}>
    <input
      type="text"
      bind:value={inputValue}
      placeholder="Favor de escanear una eti."
    />
  </form>
  <MessageLog bind:addMessage />
</div>

<style>
  input {
    width: 100%;
  }
  div.app-child {
    padding: 2.3rem;
  }
</style>
