<script>
  import Body from "./Body.svelte";
  import Header from "./Header.svelte";
  import Input from "./Input.svelte";
  import Footer from "./Footer.svelte";


</script>

<main>
  <Header/>
  <Input/>
  <Footer/>
</main>

<style>
  :global(*) {
    box-sizing: border-box;
  }

  :global(body) {
    font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif, sans-serif;
    font-size: 1vw;
    padding: 0;
    overflow: hidden;
  }
</style>
