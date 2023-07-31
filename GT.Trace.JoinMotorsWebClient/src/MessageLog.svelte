<script>
  import { onMount } from "svelte";

  export let addMessage = () => {};

  let messages = [];

  onMount(() => {
    messages = [...JSON.parse(localStorage["messages"] || "[]")];
    addMessage = (message) => {
      messages = [
        {time: new Date().toLocaleTimeString(), message: message },
        ...messages.slice(0, 30),
      ];
      localStorage["messages"] = JSON.stringify(messages);
    };
  });
</script>

<div id="message-log">
  {#each messages as message, i}
    <span>{message.time}.- {message.message}</span>
  {/each}
</div>

<style>
  #message-log {
    flex-grow: 1;
    border: 2px solid #333;
    border-radius: 0.25rem;
    /*Aqui se define el margen*/
    margin: 0.25em;
    background-color: #000000fa;
    color: white;
    font-family: "Consolas", "Courier New";
    overflow-y: auto;
    font-size: 0.8em;
    padding: 0.25em;
  }

  #message-log > span {
    /* Aqui se define el interlineado del texto en el message-log */
    display: block;
    padding: 0.25rem;
  }
</style>
