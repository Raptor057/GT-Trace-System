<script>
  import { onMount } from "svelte";

  export let materialReturnModeIsEnabled = false;
  export let curTime = new Date();

  onMount(async () => setInterval(async () => (curTime = new Date()), 500));
</script>

<div class="footer">
  <span class="cur-time">{curTime.toLocaleString()}</span>
</div>

<style lang="scss">
  div.footer {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  background-color: #061933;
  color: white;
  height: 2rem;
  padding: 0 0.25em;
  overflow: hidden;
  display: flex;
  justify-content: space-between;
  align-items: center;
  span.cur-time {
    margin-left: auto;
  }
}
</style>
