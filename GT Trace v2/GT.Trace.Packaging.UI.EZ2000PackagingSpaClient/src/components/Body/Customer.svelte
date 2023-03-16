<script>
  export let customerName = "";
  export let customerNo = "";
  export let customerDescription = "";
  export let customerPartNo = "";
  export let poNumber = null;
  export let workOrderCode = "";

  let uiPoNumber = "";

  $: if (workOrderCode) {
    uiPoNumber = localStorage[`purchaseOrder_${workOrderCode}`] || poNumber;
  }

  const handlePoClick = (e) => {
    const input = prompt("Ingrese el número de orden de compra:", uiPoNumber);
    const key = `purchaseOrder_${workOrderCode}`;
    if (input) {
      localStorage[key] = uiPoNumber = input;
    } else {
      localStorage.removeItem(key);
      uiPoNumber = poNumber;
    }
  };
</script>

<div>
  <table>
    <caption>
      <b><u>{customerName}</u></b><br />
      <small>modelo</small>
    </caption>
    <tbody>
      <tr><th>Código</th><td>{customerNo}</td></tr>
      <tr><th>Nombre</th><td>{customerDescription}</td></tr>
      <tr><th>Parte</th><td>{customerPartNo}</td></tr>
      <tr>
        <th>PO#</th>
        <td>
          <button type="button" on:click={handlePoClick}>
            {#if uiPoNumber}
              {uiPoNumber}
            {:else}
              <span style="color: red">N/A</span>
            {/if}
          </button>
        </td>
      </tr>
    </tbody>
  </table>
</div>

<style lang="scss">
  table {
    border: none;
  }
  button {
    background-color: white;
    border: 0 transparent none;
    border-radius: 0.25em;
    color: blue;
    cursor: pointer;
    display: inline-block;
    padding: 0.25em 1em;
    text-decoration: none;
    &:hover {
      background-color: blue;
      color: white;
    }
  }
</style>
