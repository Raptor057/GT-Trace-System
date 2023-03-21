<script>
  import Customer from "./Customer.svelte";
  import Pack from "./Pack.svelte";
  import Pallet from "./Pallet.svelte";
  import Quality from "./Quality.svelte";
  import WorkOrder from "./WorkOrder.svelte";

  export let palletSize = -1;
  export let palletQuantity = -1;
  export let containerSize = -1;
  export let containerQuantity = -1;
  export let workOrderCode = null;
  export let workOrderSize = null;
  export let workOrderQuantity = null;
  export let approval = null;
  export let picking = null;
  export let customerNo = "";
  export let customerName = "";
  export let customerDescription = "";
  export let customerPartNo = "";
  export let poNumber = null;
  export let packType = "";
  export let bomLabel = null;
  export let sampleImageBase64Data = "";
  export let packagingImageBase64Data = "";

  export let lineCode = "";
  export let headcount = null;
  export let addMessage = null;
</script>

<div id="app-body" class="app-child">
  <Pallet
    {palletSize}
    {palletQuantity}
    {containerSize}
    {containerQuantity}
    {packType}
    {workOrderCode}
  />
  <WorkOrder
    {workOrderCode}
    {workOrderSize}
    {workOrderQuantity}
    {headcount}
    {lineCode}
    {addMessage}
  />
  <Quality
    approval={{ ...approval }}
    picking={{ ...picking }}
    {sampleImageBase64Data}
    {palletQuantity}
  />
  <Customer
    {customerNo}
    {customerName}
    {customerDescription}
    {customerPartNo}
    {poNumber}
    {workOrderCode}
  />
  <Pack {packType} {bomLabel} {packagingImageBase64Data} />
</div>
