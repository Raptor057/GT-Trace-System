<script>
  import { onMount } from "svelte";
  export let curTime = new Date();
  onMount(async () => setInterval(async () => (curTime = new Date()), 500));
</script>

<footer>
  <span class="cur-time">{curTime.toLocaleString()}</span>
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
  footer > span.cur-time {
      float: right;
    }
</style>
