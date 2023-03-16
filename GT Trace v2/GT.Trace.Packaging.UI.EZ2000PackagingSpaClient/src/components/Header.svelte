<script>
  import { PackagingApi } from "../utils/HttpRequests";

  export let hostname = null;
  export let lineName = null;
  export let partNo = null;
  export let revision = null;
  export let masterType = null;
  export let family = null;
  export let containerQuantity = 0;
  export let addMessage = null;

  const handleWipClick = (e) => {
    if (containerQuantity > 0) {
      PackagingApi.printWipLabel(hostname)
        .then((data) =>
          addMessage(`Etiqueta WIP por ${containerQuantity} piezas generada.`)
        )
        .catch(addMessage);
    } else {
      addMessage("No se puede generar etiqueta WIP con cero piezas empacadas.");
    }
  };
  const handlePartialClick = (e) => {
    if (containerQuantity > 0) {
      PackagingApi.printPartialLabel(hostname)
        .then((data) =>
          addMessage(
            `Etiqueta parcial por ${containerQuantity} piezas generada.`
          )
        )
        .catch(addMessage);
    } else {
      addMessage(
        "No se puede generar etiqueta parcial con cero piezas empacadas."
      );
    }
  };
  const handleUnpackClick = (e) => {};
</script>

<header class="app-child">
  <img src="gt-logo.png" alt="General Transmissions" />
  <b>Empaque</b>
  <strong>{lineName || "?"}</strong>
  <span>Parte</span>
  <strong>{partNo || ""} {revision || "?"}</strong>
  <span>Tipo</span>
  <strong>{masterType || "?"}</strong>
  <span>Familia</span>
  <strong>{family || "?"}</strong>
  <button class="wip-button" type="button" on:click={handleWipClick}>
    WIP
  </button>
  <button class="partial-button" type="button" on:click={handlePartialClick}>
    Partial
  </button>
  <button class="unpack-unit-button" type="button" on:click={handleUnpackClick}>
    Desempaque
  </button>
</header>

<style lang="scss">
  header {
    background-color: #061933;
    color: white;
    height: 1.5em;
    line-height: 1.5em;
    margin: 0;
    padding: 0 !important;
    padding-left: 0.5em;
    > img {
      position: absolute;
      height: 1.5em;
    }
    > b {
      margin-left: 2em;
    }
    > strong {
      background-color: #ffffff33;
      border-radius: 0.25em;
      color: #ff9500;
      margin: 0 0.5em;
      min-width: 3rem;
      padding: 0 0.5em;
    }
    > button {
      cursor: pointer;
      border-radius: 0.2em;
      float: right;
      font-family: "Calibri", "Segoe UI";
      font-size: 1.5vw;
      background-color: white;
      font-weight: bold;
      margin-left: 0.25em;
      padding: 0 0.25em;
      &:hover {
        background-color: whitesmoke;
      }
      button.wip-button {
        border: 2px solid green;
        color: green;
      }

      &.unpack-unit-button {
        border: 2px solid white;
        color: white;
        background-color: blue;
      }

      &.unpack-unit-button:hover {
        border: 2px solid blue;
        color: blue;
        background-color: white;
        -webkit-transition: background-color 100ms linear, color 100ms linear,
          border-color 100ms linear;
        -ms-transition: background-color 100ms linear, color 100ms linear,
          border-color 100ms linear;
        transition: background-color 200ms linear, color 200ms linear,
          border-color 200ms linear;
      }

      &.partial-button {
        border: 2px solid red;
        color: red;
      }
    }
  }
</style>
