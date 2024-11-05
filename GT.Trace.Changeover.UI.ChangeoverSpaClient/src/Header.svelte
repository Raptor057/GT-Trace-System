<script>
  import { ChangeoverApi,CommonApi } from "./utils/HttpRequests";

  export let line;
  export let workOrder;
  export let status;

  let disabled = false;

  const handleApplyChangeoverClick = () => {
    if (confirm("Se aplicarán los cambios. Presióne OK para continuar...")) {
      disabled = true;
      ChangeoverApi.applyChangeover(line.code)
        .then(() => window.location.reload(true))
        .catch((msg) => {
          disabled = false;
          window.alert(msg);
        });
    }
  };


  const handleUpdateGamaClick = () => {
  const confirmationMessage = `Se actualizaran las gamas de la línea ${line.code} Modelos [${line.partNo} y ${workOrder.partNo}]`;
  CommonApi.UpdateGama(line.partNo,workOrder.partNo,line.code,line.code)
  .then(() => window.location.reload(true));
  alert(confirmationMessage);};


  const handleDeletedGamaClick = () => {
    if (confirm("Seguro que vas a realizar un reempaque?")) {
        const confirmationMessage = `Se borraron las gamas de la línea ${line.code}. Modelos [${line.partNo} y ${workOrder.partNo}]`;
        CommonApi.DeleteGama(line.partNo, workOrder.partNo, line.code, line.code)
            .then(() => {
                alert(confirmationMessage);
                window.location.reload(true);
            });
    }
};

// const handleDeletedGamaClick = () => {
// if (confirm("Press a button!") == true)
// {
// const confirmationMessage = `Se borraran las gamas de la línea ${line.code} Modelos [${line.partNo} y ${workOrder.partNo}]`;
// CommonApi.DeleteGama(line.partNo,workOrder.partNo,line.code,line.code)
// .then(() => window.location.reload(true));
// alert(confirmationMessage);};
// }
// else {

// }


</script>

<header>
  {#if status === "Cambio de módelo requerido."}
    <button
      id="apply-changeover-button"
      on:click={handleApplyChangeoverClick}
      {disabled}
    >
      Aplicar
    </button>
  {/if}
  <img src="gt-logo.png" alt="General Transmissions" />
  <b>Cambio de Modelo</b>
  <strong>{line.code}</strong>
  <span>&Oacute;rden Cargada</span>
  <strong>
    {line.workOrderCode}
    &bull;
    {line.partNo}
    {line.revision}
  </strong>
  <span>&Oacute;rden Activa</span>
  <strong>
    {workOrder.code}
    &bull;
    {workOrder.partNo}
    {workOrder.revision}
  </strong>

  <button
  id="gama-button"
  on:click={handleUpdateGamaClick}
  {disabled}>
  Actualizar Gammas
</button>

<button
id="gama-button"
on:click={handleDeletedGamaClick}
{disabled}>
Borrar Gammas Para Reempaque
</button>

</header>

<style>
  header {
    background-color: #061933;
    color: white;
    height: 2em;
    line-height: 2em;
  }

  header > img {
    position: absolute;
    height: 2em;
  }

  header > b {
    margin-left: 3em;
  }

  header > strong {
    background-color: #ffffff33;
    border-radius: 0.25em;
    color: #ff9500;
    margin: 0 0.5em;
    min-width: 3em;
    padding: 0 0.5em;
  }

  button#apply-changeover-button {
    background-color: transparent;
    color: #48c78e;
    display: inline;
    float: right;
    padding: 0 0.5em;
    border: none;
    height: 2em;
    line-height: 2em;
    cursor: pointer;
    -webkit-transition: background-color 100ms linear;
    -moz-transition: background-color 100ms linear;
    -o-transition: background-color 100ms linear;
    -ms-transition: background-color 100ms linear;
    transition: background-color 100ms linear;
  }

  button#apply-changeover-button:hover {
    background-color: #48c78e;
    border-color: transparent;
    color: #fff;
    -webkit-transition: background-color 100ms linear;
    -moz-transition: background-color 100ms linear;
    -o-transition: background-color 100ms linear;
    -ms-transition: background-color 100ms linear;
    transition: background-color 100ms linear;
  }

  button#gama-button {
    background-color: transparent;
    color: #48c78e;
    display: inline;
    /* float: right; */
    padding: 0 0.5em;
    border: none;
    height: 2em;
    line-height: 2em;
    cursor: pointer;
    -webkit-transition: background-color 100ms linear;
    -moz-transition: background-color 100ms linear;
    -o-transition: background-color 100ms linear;
    -ms-transition: background-color 100ms linear;
    transition: background-color 100ms linear;
  }

  button#gama-button:hover {
    background-color: #48c78e;
    border-color: transparent;
    color: #fff;
    -webkit-transition: background-color 100ms linear;
    -moz-transition: background-color 100ms linear;
    -o-transition: background-color 100ms linear;
    -ms-transition: background-color 100ms linear;
    transition: background-color 100ms linear;
  }
</style>
