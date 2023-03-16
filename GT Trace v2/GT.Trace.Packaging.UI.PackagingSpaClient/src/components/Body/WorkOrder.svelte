<script>
  import { isInt } from "../../utils/Math";
  import { PackagingApi } from "../../utils/HttpRequests";

  export let workOrderCode = "";
  export let workOrderSize = 0;
  export let workOrderQuantity = 0;
  export let lineCode = "";
  export let headcount = null;
  export let addMessage = null;

  $: progress = (
    workOrderSize ? (100 * workOrderQuantity) / workOrderSize : 0
  ).toFixed(2);

  const handleHeadcountClick = (e) => {
    e.target.disabled = true;

    const input = prompt(
      "Introduzca la cantidad de personal en la línea:",
      headcount
    );

    if (input && isInt(input)) {
      const newHeadcount = Number(input);
      PackagingApi.setHeadcount(lineCode, workOrderCode, newHeadcount)
        .then(() => {
          headcount = newHeadcount;
          e.target.disabled = false;
        })
        .catch((error) => {
          addMessage(error);
          e.target.disabled = false;
        });
    }
  };
</script>

<div>
  <table>
    <caption>
      <b><u>{workOrderCode}</u></b>
      <br />
      <small>orden de fabricación</small>
    </caption>
    <tbody>
      <tr><th>Requerimiento</th><td>{workOrderSize}</td></tr>
      <tr>
        <th>Cumplimiento</th><td>{workOrderQuantity}</td>
      </tr>
      <tr>
        <th>Pendiente</th>
        <td>{Math.max(0, workOrderSize - workOrderQuantity)}</td>
      </tr>
      <tr>
        <th>Avance</th>
        <td class="progress">
          <span>{progress} %</span>
          <div
            class="progress-bar"
            style={`width: ${Math.min(100, progress)}%`}
          />
        </td>
      </tr>
      <tr>
        <th>Personal</th>
        <td>
          {#if headcount !== null}
            <button on:click={handleHeadcountClick}>{headcount}</button>
          {:else}
            <button disabled>0</button>
          {/if}
        </td>
      </tr>
    </tbody>
  </table>
</div>

<style lang="scss">
  .progress {
    position: relative;
    > .progress-bar {
      border: 2px solid green;
      position: absolute;
    }
  }

  button {
    border: 0;
    border-radius: 0.25rem;
    padding: 0.25em 0.5em;
    color: blue;
    cursor: pointer;
    &:disabled {
      color: gray;
    }
  }
</style>
