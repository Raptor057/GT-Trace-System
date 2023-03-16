<script>
  import { onMount } from "svelte";

  export let materialReturnModeIsEnabled = false;
  export let curTime = new Date();

  onMount(async () => setInterval(async () => (curTime = new Date()), 500));
</script>

<div class="footer">
  <label class="material-return-checkbox">
    <input type="checkbox" bind:checked={materialReturnModeIsEnabled} />
    Retornar material.
  </label>
  <span class="cur-time">{curTime.toLocaleString()}</span>
</div>

<style lang="scss">
  div.footer {
    background-color: #061933;
    bottom: 0;
    color: white;
    height: 2rem;
    left: 0;
    line-height: 2em;
    overflow: hidden;
    padding: 0 0.25em;
    position: absolute;
    right: 0;
    > span.cur-time {
      float: right;
    }

    > label.material-return-checkbox {
      cursor: pointer;
      display: inline-block;
    }
  }
</style>
