<script>
  export let lineName = null;
  export let partNo = null;
  export let revision = null;
  export let workOrderCode = null;


</script>

<div class="header">
  <b>Join Motors EZ</b>
  <strong>{lineName || "?"}</strong>
  <span>Modelo</span>
  <strong>{partNo || "?"} {revision || ""}</strong>
  <span>&Oacute;rden</span>
  <strong>{workOrderCode || "?"}</strong>
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
  }
</style>
