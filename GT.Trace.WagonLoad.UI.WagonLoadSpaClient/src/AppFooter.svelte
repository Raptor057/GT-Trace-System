<!-- <script>
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
      right: 0;

      > span.cur-time {
        float: right;
      }

    }
  </style>
   -->
   <script>
    import { onMount } from "svelte";
  
    export let curTime = new Date();
  
    onMount(async () => setInterval(async () => (curTime = new Date()), 500));
  </script>
  
  <style lang="scss">
    body {
      margin: 0;
      padding: 0;
      height: 100%; /* Asegurarse de que el cuerpo ocupe todo el alto de la página */
    }
  
    #app {
      display: flex;
      flex-direction: column;
      min-height: 100vh; /* Asegurarse de que el contenido ocupe todo el alto de la página */
    }
  
    div.app-child {
      background-color: #061933;
      color: white;
      height: 1.6rem;
      line-height: 1em;
      overflow: hidden;
      padding: 0 0.25em;
      position: fixed;
      bottom: 0;
      left: 0;
      right: 0;
    }
  
     span.cur-time {
      float: right;
    }
  </style>
  
  <!-- El contenido del componente -->
  <div id="app">
    <!-- Tu contenido principal aquí -->
  
    <!-- Pie de página anclado en la parte inferior -->
    <div class="app-child">
      <span class="cur-time">{curTime.toLocaleString()}</span>
    </div>
  </div>
  