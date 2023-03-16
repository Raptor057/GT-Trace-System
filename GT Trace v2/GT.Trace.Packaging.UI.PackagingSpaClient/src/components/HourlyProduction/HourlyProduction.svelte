<script>
  import TotalsRow from "./TotalsRow.svelte";
  import HeadRow from "./HeadRow.svelte";

  export let hourlyProduction;

  let intervals = [];
  let totals = {};
  let partNos = [];

  $: postProcessHourlyProductionData(hourlyProduction);

  const postProcessHourlyProductionData = (data) => {
    console.log("data", data);
    intervals = data
      //.filter((item) => item.partNo)
      .map((item) => ({
        name: item.intervalName,
        requirement: data
          .filter((i) => i.intervalName === item.intervalName)
          .reduce((p, c) => p + c.targetQuantity, 0),
        pph: data
          .filter((i) => i.intervalName === item.intervalName)
          .reduce((p, c) => p + c.pph, 0),
        status: item.isCurrent ? "is-active" : item.isPastDue ? "is-due" : "",
      }))
      .filter((v, i, a) => a.findIndex((e) => e.name === v.name) === i);

    partNos = data
      .map((item) => item.partNo)
      .filter((v, i, a) => v && a.indexOf(v) === i)
      .sort();

    hourlyProduction = data;

    totals = intervals.reduce((prev, curr) => {
      const records = hourlyProduction.filter(
        (item) => item.intervalName === curr.name
      );

      const quantity = records.reduce((p, c) => p + c.quantity || 0, 0);
      prev[curr.name] = {
        status:
          curr.status === "is-due"
            ? curr.requirement <= quantity
              ? "good"
              : "warn"
            : "",
        quantity: quantity,
      };

      return prev;
    }, {});
  };
</script>

<table class="app-child">
  <HeadRow {intervals} {totals} />
  <tbody>
    {#each partNos as partNo, i}
      <tr>
        <th>{partNo}</th>
        {#each intervals as interval, i}
          <td class={`${interval.status} ${totals[interval.name].status}`}>
            {(
              hourlyProduction.find(
                (item) =>
                  item.intervalName === interval.name && item.partNo === partNo
              ) || { quantity: "" }
            ).quantity ||
              (interval.status === "is-due" || interval.status === "is-active"
                ? "0"
                : "")}
          </td>
        {/each}
        <th>
          {hourlyProduction
            .filter((item) => item.partNo === partNo)
            .reduce((p, c) => p + c.quantity, 0)}
        </th>
      </tr>
    {/each}
    <tr class="pph">
      <th><span>PPH</span></th>
      {#each intervals as interval, i}
        <td class={`${interval.status} ${totals[interval.name].status}`}>
          <span>{(interval.pph || 0).toFixed(1)}</span>
        </td>
      {/each}
      <th>
        <span>
          <small>avg</small>
          {intervals
            .map((item) => item.pph)
            .filter((item) => item)
            .reduce((p, c, i, a) => p + c / a.length, 0)
            .toFixed(1)}
        </span>
      </th>
    </tr>
  </tbody>
  {#if partNos.length > 1}
    <TotalsRow {totals} {intervals} />
  {/if}
</table>

<style lang="scss">
  table {
    color: gray;
    border-spacing: 0;
    table-layout: fixed;
    text-align: center;
    width: 100%;
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

  :global(.good) {
    background-color: #effaf5;
    border-right: 1px #ffffff99 solid;
    color: #257953;
  }

  :global(.warn) {
    background-color: #fffaeb;
    border-right: 1px #ffffff99 solid;
    color: #946c00;
  }

  :global(.is-active) {
    background-color: #eff5fb;
    border-left: 1px #296fa8 solid;
    border-right: 1px #296fa8 solid;
    color: #296fa8;
    font-weight: bold;
  }

  :global(.is-due) {
    color: black;
  }

  tr.pph > th,
  tr.pph > td {
    padding: 0;
  }

  tr.pph > th > span,
  tr.pph > td > span {
    background-color: #0000000f;
    display: block;
    padding: 0.25em;
  }
</style>
