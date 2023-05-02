<script>
  export let showToKeep;
  export let showToPut;
  export let showToTake;
  export let gamma;
  console.log("gamma", gamma);
</script>

<div class="flex-container">
  {#each gamma as item, i}
    {#if (item.incoming && item.outgoing && showToKeep) || (item.incoming && !item.outgoing && showToPut) || (!item.incoming && item.outgoing && showToTake)}
      <div
        class={item.incoming && item.outgoing
          ? "keep"
          : item.incoming
          ? "put"
          : "take"}
      >
        <b>
          {item.incoming
            ? item.incoming.pointOfUseCode
            : item.outgoing.pointOfUseCode}
        </b>
        <strong>
          {item.incoming ? item.incoming.compNo : item.outgoing.compNo}
          {item.incoming ? item.incoming.compRev : item.outgoing.compRev}
        </strong>
        <small>
          {item.incoming ? item.incoming.compDesc : item.outgoing.compDesc}
        </small>
      </div>
    {/if}
  {/each}
</div>

<style>
  .flex-container {
    bottom: 2em;
    position: absolute;
    left: 0;
    top: 2em;
    width: 100%;
    overflow-x: auto;

    display: flex;

    align-items: stretch;
    /* flex-flow: column wrap; */
    flex-direction: column;
    flex-wrap: wrap;
    align-content: center;
    background-color: whitesmoke;
    padding: 15px;
    gap: 0.5em;
  }

  .flex-container > div {
    border-radius: 0.25rem;
    padding: 0.25rem;
    padding-left: 2.75em;
    position: relative;
  }

  div > b {
    border-radius: 0.25rem 0 0 0.25rem;
    bottom: 0;
    left: 0;
    padding-top: 5%;
    position: absolute;
    text-align: center;
    top: 0;
    width: 2.5em;
  }

  .keep {
    background-color: #eff5fb;
    border: 1px solid #296fa8;
    color: #296fa8;
  }

  .keep > b {
    background-color: #3e8ed0;
    color: #fff;
  }

  .put {
    background-color: #feecf0;
    border: 1px solid #cc0f35;
    color: #cc0f35;
  }

  .put > b {
    background-color: #f14668;
    color: #fff;
  }

  .take {
    background-color: #fffaeb;
    border: 1px solid #946c00;
    color: #946c00;
  }

  .take > b {
    background-color: #ffe08a;
    color: rgba(0, 0, 0, 0.7);
  }

  .flex-container > div > small {
    display: block;
  }
</style>
