<script>
  import { isInt } from "../../utils/Math";

  // Capacity of the container
  export let containerSize = 0;
  // Actual pieces in the container
  export let containerQuantity = 0;
  // Capacity of the pallet
  export let palletSize = 0;
  // Actual pieces in the pallet
  export let palletQuantity = 0;
  // ATEQ or Master
  export let packType = "";
  // Work order code
  export let workOrderCode = null;

  /**
   * Container's capacity to be shown in the UI
   * * It takes the information from either the work order
   * * or the local storage, if overrided.
   */
  let uiContainerSize = 0;
  /**
   * Pallet's capacity to be shown in the UI
   * * It takes the information from either the work order
   * * or the local storage, if overrided.
   */
  let uiPalletSize = 0;

  /**
   * Update the UI container and pallet sizes with the modified value. If
   * there's no value stored the value from the work order is used.
   * * It will be executed every time the work order code changes its value,
   * * ignoring empty ones.
   */
  $: if (workOrderCode) {
    uiContainerSize =
      localStorage[`containerSize_${workOrderCode}`] || containerSize;
    uiPalletSize = localStorage[`palletSize_${workOrderCode}`] || palletSize;
  }

  /**
   * Try to change the total pieces by container according to the user input.
   * * Fallsback to the work order quantity in case the input is not valid.
   * @param e Click event details.
   */
  const handleContainerSizeClick = (e) => {
    const input = prompt(
      "Ingrese la cantidad por contenedor:",
      uiContainerSize
    );
    const key = `containerSize_${workOrderCode}`;
    if (isInt(input)) {
      localStorage[key] = uiContainerSize = Number(input);
    } else {
      localStorage.removeItem(key);
      uiContainerSize = containerSize;
    }
  };

  /**
   * Try to change the total pieces by pallet according to the user input.
   * * Fallsback to the work order quantity in case the input is not valid.
   * @param e Click event details.
   */
  const handlePalletSizeClick = (e) => {
    const input = prompt("Ingrese la cantidad por tarima:", uiPalletSize);
    const key = `palletSize_${workOrderCode}`;
    if (isInt(input)) {
      localStorage[key] = uiPalletSize = Number(input);
    } else {
      localStorage.removeItem(key);
      uiPalletSize = palletSize;
    }
  };
</script>

<div>
  <table>
    <caption>
      <b><u>{packType}</u></b>
      <br />
      <small>piezas en curso</small>
    </caption>
    <tbody>
      <tr>
        <td>
          <sub>Contenedor</sub>
          <br />
          <b>{containerQuantity}</b>
          <br />
          <div class="size-wrapper">
            <button class="size" on:click={handleContainerSizeClick}>
              <small>/ {uiContainerSize}</small>
            </button>
          </div>
        </td>
        <td>
          <sub>Tarima</sub>
          <br />
          <b>{palletQuantity}</b>
          <br />
          <div class="size-wrapper">
            <button class="size" on:click={handlePalletSizeClick}>
              <small>/ {uiPalletSize}</small>
            </button>
          </div>
        </td>
      </tr>
    </tbody>
  </table>
</div>

<style lang="scss">
  table {
    width: 100%;
  }

  table > tbody > tr > td {
    text-align: center;
    width: 50%;
  }

  table > tbody > tr > td > b {
    color: blue;
    font-size: 3em;
  }

  .size-wrapper {
    position: relative;
    > .size {
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
  }
</style>
