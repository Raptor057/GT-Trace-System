<script>
    export let lineName = null;
    export let partNo = null;
    export let revision = null;
    export let workOrderCode = null;
    export let btnDel;
  
  </script>
  
  <header class="app-child">
    <img src="gt-logo.png" alt="General Transmissions" />
    <b>GT Join Frameless Motors</b>
    <strong>{lineName || "?"}</strong>
    <span>Modelo</span>
    <strong>{partNo || "?"} {revision || ""}</strong>
    <span>&Oacute;rden</span>
    <strong>{workOrderCode || "?"}</strong>
    <button id="btnDel" on:click={btnDel}>Desenlazar</button>

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
      > button{
        background-color: #ffffff33;
        color: #ff9500;
        border-radius: 0.25em;
        float: right;
        height: 1.5em;
        margin: 0 0.5em;
        min-width: 3rem;
        padding: 0 0.5em;
      }

    }
  </style>