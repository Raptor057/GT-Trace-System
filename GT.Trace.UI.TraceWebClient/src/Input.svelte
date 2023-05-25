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
  //Etis
  function Etis(EtiNo){
    ProcessHistoryApi.UpdateEtis(EtiNo)
    .then((response)=>{
      Sfx.playSuccessSoundAsync();
    })
    .catch((error) => {
        Sfx.playFailureSoundAsync();
        addMessage(error);
        inputValue = "";
      });
  }

  //Frameless
  function SaveMotors(ProductionID,LineCode){
    ProcessHistoryApi.SaveElectricalMotors(ProductionID,LineCode)
    .then((response)=>{
      Sfx.playSuccessSoundAsync();
      //addMessage(response);
      const match = ProductionID.match(motorRegex);
      if (match) {
        const SerialNumber = match.groups.SerialNumber;
        addMessage(`La Unidad \n ${SerialNumber} \n se registró con exito.`);
      } else {
        addMessage(`No se pudo extraer el transmissionID de ${SerialNumber}.`);
      }
    })
    .catch((error) => {
        Sfx.playFailureSoundAsync();
        addMessage(error);
      });
      inputValue = "";
  }

  //EZ
  function RecordProcess(UnitID,LineCode){
    ProcessHistoryApi.RecordProcessNo(UnitID,LineCode)
    .then((response)=>{
    Sfx.playSuccessSoundAsync();
    //addMessage(response);
    const match = UnitID.match(transmissionRegex);
      if (match) {
        const transmissionID = match.groups.transmissionID;
        addMessage(`El valor \n ${transmissionID} \n se registró con exito.`);
      } else {
        addMessage(`No se pudo extraer el transmissionID de ${UnitID}.`);
      }
    })
    .catch((error) => {
        Sfx.playFailureSoundAsync();
        addMessage(error);
        //inputValue = "";
      });
      inputValue = "";
  }


  //----------------
  const handleSubmit = (event) => {
    event.preventDefault();
    const isEti = etis.test(inputValue);
    const isTransmissionInput = transmissionRegex.test(inputValue);
    const isMotor = motorRegex.test(inputValue);

    if (isEti) {  
    Etis(inputValue);
    //addMessage(`La Eti \n ${response} \n se registro correctamente para la linea: ${lineCode}.`);
  } else if (isTransmissionInput) {
    RecordProcess(inputValue,lineCode);
    //addMessage(`El valor \n ${inputValue} \n se registro en: ${lineCode}.`);
    addMessage (`${response}`);
  } else if(isMotor){
    SaveMotors(inputValue,lineCode);
    //addMessage(`El valor \n ${inputValue} \n se registro en: ${lineCode}.`);
  }
  else{
    Sfx.playFailureSoundAsync();
      //alert(`El valor \n ${inputValue} \n no es válido para la linea ${lineCode}.`);
      addMessage(`La ETI \n ${inputValue} \n no es válido para la linea ${lineCode}.`);
      
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
