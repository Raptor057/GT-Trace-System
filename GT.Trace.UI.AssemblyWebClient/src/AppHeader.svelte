<script>
  export let lineName = null;
  export let partNo = null;
  export let revision = null;
  export let workOrderCode = null;
  export let activeEtiCount = null;
  export let gammaSize = null;
  export let production = {};
  export let btnUnlock;
  export let btnPrintSubAssembly;
  //$: console.log("xxxxx", production);

</script>

<div class="header">
  <b>GT Trace</b>
  <strong>{lineName || "?"}</strong>
  <span>Modelo</span>
  <strong>{partNo || "?"} {revision || ""}</strong>
  <span>&Oacute;rden</span>
  <strong>{workOrderCode || "?"}</strong>
  <span>Trazabilidad</span>
  <strong>{activeEtiCount || "?"} / {gammaSize || "?"}</strong>
  <span>Hora</span>
  <strong>{production.interval}</strong>
  <span>Cumplimiento</span>
  <strong>{production.actualQuantity} / {production.expectedQuantity}</strong>
  <span>Pzs &times; Min</span>
  <strong>
    {Number(production.actualRate).toFixed(2)}
    /
    {Number(production.expectedRate).toFixed(2)}
  </strong>
  <span>Pronóstico</span>
  <strong>
    {production.forecast} / {production.requirement}
    <i class={production.forecast < production.requirement ? "bad" : "good"} />
  </strong> 
  <!-- Aqui se habilita el boton de desbloqueo de linea -->
  <button id="btnUnlock" on:click={btnUnlock}>Desbloquear Linea Manualmente</button>

  <!-- Aqui se habilita el boton de impresion de sub ensambles en linea -->
  <button id="btnPrintSubAssembly" on:click={btnPrintSubAssembly}>Imprimir Etiqueta Sub Ensamble</button>
</div>

<style lang="scss">
  div.header {
    background-color: #061933;
    color: white;
    height: 2rem;
    left: 0;
    line-height: 2em;
    overflow: hidden;
    padding-left: 0.5em;
    position: absolute;
    right: 0;
    top: 0;
    > strong {
      background-color: #ffffff33;
      border-radius: 0.25em;
      color: #ff9500;
      margin: 0 0.5em;
      min-width: 3rem;
      padding: 0 0.5em;
    }
    // > button{
    //     background-color: #ffffff33;
    //     color: #ff9500;
    //     border-radius: 0.25em;
    //     float: right;
    //     height: 2em;
    //     margin: 0 0.5em;
    //     min-width: 3rem;
    //     padding: 0 0.5em;
    //     position: absolute;
    //   }
    #btnUnlock,
    #btnPrintSubAssembly {
      /* Estilos específicos para los botones por ID */
      background-color: #ffffff33;
      color: #ff9500;
      border-radius: 0.25em;
      display: inline-block; /* Hacer que los botones sean elementos en línea */
      height: 2em;
      margin: 0 0.25em; /* Ajustar el margen entre los botones */
      min-width: 3rem;
      padding: 0 0.5em;
      position: relative; /* Cambiar a relative para que position funcione correctamente */
    }
  }
  i {
    font-style: normal;
    &.good {
      color: lime;
      &::after {
        content: "✔";
      }
    }
    &.bad {
      color: red;
      &::after {
        content: "⚠";
      }
    }
  }
</style>
