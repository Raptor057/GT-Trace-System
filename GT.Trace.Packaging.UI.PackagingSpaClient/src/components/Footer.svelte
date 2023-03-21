<script>
  export let canCheckFunctionalTest = false;
  export let canCheckPartNo = false;
  export let canCheckRevision = false;
  export let canPick = false;
  export let canTrace = false;
</script>

<div id="app-footer" class="app-child">
  <ul class="station-flags">
    <li
      title="Validar prueba funcional en unidades."
      class={!canCheckFunctionalTest ? "flag-off" : ""}
    >
      Validar Prueba Funcional
    </li>
    <li
      title="Seleccionar piezas para pruebas de calidad."
      class={!canCheckPartNo ? "flag-off" : ""}
    >
      Picking de Pruebas Activo
    </li>
    <li
      title="Generar registro de trazabilidad."
      class={!canTrace ? "flag-off" : ""}
    >
      Trazabilidad Activa
    </li>
    <li
      title="Validar número de parte de cliente."
      class={!canPick ? "flag-off" : ""}
    >
      Validar Número de Parte
    </li>
    <li
      title="Validar revisión en etiqueta."
      class={!canCheckRevision ? "flag-off" : ""}
    >
      Validar Revisión
    </li>
  </ul>
</div>

<style>
  #app-footer {
    padding: 0;
  }

  ul.station-flags {
    list-style: none;
    margin: 0;
    padding: 0.2em 0 0.2em 0.2em;
  }

  .station-flags > li {
    background-color: steelblue;
    border-radius: 0.2em;
    color: white;
    display: inline-block;
    font-size: 1em;
    padding: 0.25em 0.5em;
  }

  .station-flags > li.flag-off {
    color: lightgrey;
    background-color: white;
  }
</style>
