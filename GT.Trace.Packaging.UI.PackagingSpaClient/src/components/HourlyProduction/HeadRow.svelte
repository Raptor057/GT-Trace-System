<script>
  export let intervals = [];
  export let totals = {};
</script>

<thead>
  <tr>
    <th>Hora</th>
    {#each intervals as interval, i}
      <th class={`${interval.status} ${totals[interval.name].status}`}
        >{interval.name}</th
      >
    {/each}
    <th>Totales</th>
  </tr>
  <tr>
    <th>Requerimiento</th>
    {#each intervals as interval, i}
      <td class={`${interval.status} ${totals[interval.name].status}`}>
        {interval.requirement}
      </td>
    {/each}
    <th>{intervals.reduce((p, c) => p + c.requirement, 0)}</th>
  </tr>
</thead>

<style lang="scss">
  thead > tr > td,
  thead > tr > th {
    border-bottom: 1px #33333366 solid;
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
