<script>
  export let text = null;
  export let time = null;
  export let type = "normal";
</script>

<li class="item {type}">
  <small class="time">{time} {type}</small>
  <p class="text">{text}</p>
</li>

<style lang="scss">
  $color: #ff3e00;
  .item {
    border-bottom: 1px #ffffff99 solid;
    border-left: 1em #ffffff99 solid;
    padding: 0.5em;
    > .time {
      color: $color;
      display: block;
      font-size: small;
      font-weight: bold;
    }
    > .text {
      margin: 0;
      padding: 0.5em;
    }
    &.error {
      border-left: 1em #f14668 solid;
    }

    &.info {
      border-left: 1em #3e8ed0 solid;
    }

    &.success {
      border-left: 1em #48c78e solid;
    }

    &.warning {
      border-left: 1em #ffe08a solid;
    }
  }
</style>
