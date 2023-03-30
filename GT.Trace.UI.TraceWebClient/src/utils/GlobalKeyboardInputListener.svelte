<script>
  import { onMount } from "svelte";
  import { createEventDispatcher } from "svelte";

  const dispatch = createEventDispatcher();

  export let triggerKey = "Enter";
  export let input = "";

  onMount(async () => {
    window.addEventListener(
      "keypress",
      async (e) => {
        if (e.key === triggerKey && input) {
          dispatch("triggerpress", {
            input: input.replace("]C1", "").toUpperCase(),
          });
          input = "";
        } else {
          input += e.key;
          dispatch("keypress", { input: input, key: e.key });
        }
      },
      false
    );
  });
</script>
