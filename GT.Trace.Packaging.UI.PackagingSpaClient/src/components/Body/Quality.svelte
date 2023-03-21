<script>
  export let palletQuantity = 0;
  export let sampleImageBase64Data = null;
  export let approval = null;
  export let picking = null;

  $: status = approval.isApproved
    ? "QC OK"
    : palletQuantity >= approval.stopLevel
    ? "QC NOK"
    : palletQuantity >= approval.warningLevel
    ? `QC NOK (${approval.stopLevel - palletQuantity})`
    : `QC NOK (${approval.warningLevel - palletQuantity})`;

  $: className = approval.isApproved
    ? "approved"
    : palletQuantity >= approval.stopLevel
    ? "locked"
    : palletQuantity >= approval.warningLevel
    ? "warning"
    : "";
</script>

<div>
  <table id="quality-table">
    <caption><b><u>Calidad</u></b></caption>
    <tbody>
      <tr>
        <td colspan="2">
          <span class="qc-approval-status {className}">{status}</span>
        </td>
      </tr>
      <tr>
        <td colspan="2">
          <img
            alt="Referencia."
            src={`data:image/png;base64,${sampleImageBase64Data}`}
          />
        </td>
      </tr>
      <tr>
        <td colspan="2">
          {picking.countdown > 0
            ? `Piezas para picking: ${picking.countdown}`
            : `${
                picking.totalSamples - picking.sequence
              } pieza(s) pendiente(s) para picking.`}
        </td>
      </tr>
    </tbody>
  </table>
</div>

<style lang="scss">
  .qc-approval-status {
    background-color: grey;
    color: white;
    display: inline-block;
    text-align: center;
    width: 100%;
    &.warning {
      background-color: yellow;
      color: #333;
    }
    &.locked {
      background-color: red;
      color: white;
    }
    &.approved {
      background-color: blue;
      color: white;
    }
  }
  img {
    border: 1px blue solid;
    max-width: 100%;
  }
</style>
