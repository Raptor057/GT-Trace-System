<script>
    import { onMount } from "svelte";
  
    export let curTime = new Date();
  
    onMount(async () => setInterval(async () => (curTime = new Date()), 500));
  </script>
  
  <div id="app-footer" class="app-child">
    <span class="cur-time">{curTime.toLocaleString()}</span>
  </div>
  
  <style lang="scss">
    div.app-child {
      background-color: #061933;
      bottom: 0;
      color: white;
      height: 1.6rem;
      left: 0;
      line-height: 1em;
      overflow: hidden;
      padding: 0 0.25em;      
      //position: absolute;

      right: 0;

      > span.cur-time {
        float: right;
      }

    }
  </style>
  