<script>
  import { onMount } from "svelte";

  export let showToKeep;
  export let showToTake;
  export let showToPut;

  let loaded = false;

  $: if (loaded) localStorage["showToKeep"] = showToKeep;
  $: if (loaded) localStorage["showToPut"] = showToPut;
  $: if (loaded) localStorage["showToTake"] = showToTake;

  onMount(() => {
    showToKeep = localStorage["showToKeep"] == "true";
    showToPut = localStorage["showToPut"] == "true";
    showToTake = localStorage["showToTake"] == "true";
    loaded = true;
  });
</script>

<footer>
  <label class="keep nohigh">
    <input type="checkbox" bind:checked={showToKeep} />
    <b>COM&Uacute;N</b>
  </label>
  <label class="take nohigh">
    <input type="checkbox" bind:checked={showToTake} />
    <b>RETORNAR</b>
  </label>
  <label class="put nohigh">
    <input type="checkbox" bind:checked={showToPut} />
    <b>CARGAR</b>
  </label>
</footer>

<style>
  footer {
    background-color: #061933;
    bottom: 0;
    color: white;
    height: 2em;
    line-height: 2em;
    padding-left: 1em;
    position: absolute;
    width: 100%;
  }

  footer > label {
    border: none;
    display: inline-block;
    padding: 0 0.25em;
  }

  .nohigh {
    user-select: none;
    -webkit-user-select: none;
    -moz-user-select: none;
    -ms-user-select: none;
  }
  .nohigh::selection {
    background: transparent;
  }

  b {
    padding: 0 0.5em;
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
</style>
