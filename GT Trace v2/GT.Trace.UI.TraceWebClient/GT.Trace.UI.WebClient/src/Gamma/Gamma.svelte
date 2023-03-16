<script>
  import { onMount } from "svelte";
  import GammaItem from "./GammaItem.svelte";

  export let lineCode = null;
  export let items = [];
  export let materialReturnModeIsEnabled = false;

  const referenceWidth = 172;
  const referenceHeight = 114;
  // 172 and 114 are reference dimensions, where the design looked good
  let refItem = null;
  let itemWidth = 0;
  let itemHeight = 0;

  $: if (items.length > 0) {
    const sqrt = Math.sqrt(items.length);
    let itemsPerRow = Math.floor(sqrt);
    let itemsPerCol = itemsPerRow;
    if (sqrt - itemsPerCol > 0) {
      itemsPerCol += 1;
    }

    if (itemsPerCol * itemsPerRow < items.length) {
      itemsPerRow += 1;
    }

    itemWidth = 100.0 / itemsPerCol;
    itemHeight = 100.0 / itemsPerRow;
  }

  $: dim = refItem
    ? refItem.getBoundingClientRect()
    : { width: referenceWidth, height: referenceHeight };

  $: style = `font-size: ${(dim.width > dim.height ? Math.min : Math.max)(
    (100 * dim.width) / referenceWidth,
    (100 * dim.height) / referenceHeight
  ).toFixed(0)}%; background-color: ${
    materialReturnModeIsEnabled ? "yellow" : "whitesmoke"
  }`;

  const handleItemClick = (e) => {
    // const targetItem = items.find(
    //   (item) =>
    //     item.code === e.detail.pointOfUseCode &&
    //     item.component.number === e.detail.componentNo
    // );
    // Animator.flipInX(targetItem);
    // Sfx.playEtiUsedAudio();
  };

  onMount(async () => {
    // we just need to check one element as all of them
    // have the same dimensions
    window.addEventListener(
      "resize",
      (e) =>
        (dim = refItem
          ? refItem.getBoundingClientRect()
          : { width: 0, height: 0 }),
      true
    );
  });
</script>

<div class="gamma" {style}>
  {#if items.length > 0}
    {#each items as item, i}
      <GammaItem
        width={itemWidth}
        height={itemHeight}
        {...item}
        bind:item={refItem}
        on:itemClick={handleItemClick}
      />
    {/each}
  {:else}
    <h1 class="message">
      {#if !lineCode}
        <span>No se ha especificado el código de la línea</span>
      {:else}
        <span>
          no se encontró información
          <br />
          sobre la línea <b>{lineCode}</b>
        </span>
      {/if}
    </h1>
  {/if}
</div>

<style lang="scss">
  $color: #ff3e00;
  .gamma {
    bottom: 2rem;
    left: 0;
    overflow: hidden;
    position: absolute;
    right: 0;
    top: 2rem;
    > .message {
      color: $color;
      font-weight: normal;
      margin: 0;
      margin-top: 10%;
      text-align: center;
      text-transform: uppercase;
    }
  }
</style>
