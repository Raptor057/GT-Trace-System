<!-- <script>
  export let lineName = null;
  export let partNo = null;
  export let revision = null;
  export let workOrderCode = null;
  export let activeEtiCount = null;
  export let gammaSize = null;
  export let production = {};
</script>

<div class="header">
  <b>GT Trace Station</b>
  <strong>{lineName || "?"}</strong>
  <span>Modelo</span>
  <strong>{partNo || "?"} {revision || ""}</strong>
  <span>&Oacute;rden</span>
  <strong>{workOrderCode || "?"}</strong>
  <span>Trazabilidad</span>
  <strong>{activeEtiCount || "?"} / {gammaSize || "?"}</strong>
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
</style> -->
<script>
  export let lineName = "?";
  export let partNo = "?";
  export let revision = "";
  export let workOrderCode = "?";
  export let activeEtiCount = "?";
  export let gammaSize = "?";
  export let production = {};
</script>

<div class="header">
  <div class="header-info">
    <div class="header-item">
      <b>GT Trace Station</b>
    </div>
    <div class="header-item">
      <span>L&iacute;nea</span>
      <strong>{lineName}</strong>
    </div>
    <div class="header-item">
      <span>Modelo</span>
      <strong>{partNo} {revision}</strong>
    </div>
    <div class="header-item">
      <span>Orden</span>
      <strong>{workOrderCode}</strong>
    </div>
    <div class="header-item">
      <span>Trazabilidad</span>
      <strong>{activeEtiCount} / {gammaSize}</strong>
    </div>
  </div>
</div>

<style lang="scss">
  .header {
    background-color: #061933;
    //background-color: #000000;
    color: white;
    height: 2rem;
    line-height: 2rem;
    padding-left: 0.5em;
    position: relative;
  }

  .header-info {
    display: flex;
    justify-content: space-between;
    align-items: center;
    height: 100%;
    padding: 0 1rem;
  }
  
  strong {
    background-color: #ffffff33;
    border-radius: 0.25em;
    color: #ff9500;
    min-width: 3rem;
    padding: 0 0.5em;
  }

  // i {
  //   font-style: normal;
  //   &.good {
  //     color: lime;
  //     &::after {
  //       content: "✔";
  //     }
  //   }
  //   &.bad {
  //     color: red;
  //     &::after {
  //       content: "⚠";
  //     }
  //   }
  // }
</style>
