<script>
  export let intervals = [];
  export let totals = {};
</script>

<tfoot>
  <tr>
    <th>Totales</th>
    {#each intervals as interval, i}
      <td class={`${interval.status} ${totals[interval.name].status}`}>
        {totals[interval.name].quantity}
      </td>
    {/each}
    <th>{Object.keys(totals).reduce((p, c) => p + totals[c].quantity, 0)}</th>
  </tr>
</tfoot>

<style lang="scss">
  tfoot > tr > td,
  tfoot > tr > th {
    border-top: 1px #33333366 solid;
  }
  th,
  td {
    padding: 0.25em;
  }

  th:first-child,
  th:last-child {
    color: black;
    overflow: hidden;
    white-space: nowrap;
    text-align: right;
    text-overflow: ellipsis;
  }
  th:first-child {
    border-right: 1px #33333366 solid;
  }
</style>
