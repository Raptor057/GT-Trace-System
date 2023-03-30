<script>
  import { createEventDispatcher } from "svelte";
  const dispatch = createEventDispatcher();

  export let height = 100;
  export let width = 100;

  export let cardinality = 0;
  export let capacity = 0;
  export let load = 0;
  export let code = "A00";
  export let activeEti = { number: "E000000-T000000" };
  export let component = { number: "00000", revision: "R00", description: "" };

  export let item = null;

  $: style = `width: ${width}%; height: ${height}%;`;

  $: className =
    (activeEti && activeEti.number ? "active" : "") +
    " " +
    (load == 0
      ? "empty"
      : load > capacity
      ? "overloaded"
      : load == capacity
      ? "full"
      : "partial");

  const handleItemClick = (pointOfUseCode, componentNo) => {
    dispatch("itemClick", {
      pointOfUseCode: pointOfUseCode,
      componentNo: componentNo,
    });
  };

</script>

<div class="wrapper" {style}>
  <div
    id={`${code}_${component.number}`}
    class="item animate__animated {className}"
    on:click={(e) => handleItemClick(code, component.number)}
  >

    <div class="head">
      <span class="point-of-use-code">{code}</span>
      <!-- <span class="eti-no">
        {activeEti ? activeEti.number : ""}
      </span> -->
    </div>


    <div class="body" bind:this={item}>
      <!-- <strong class="status"><span class="load">{load}</span>/{capacity}</strong> -->
            <strong class="status">
            <!-- <span class="point-of-use-code">{code}</span> -->
            <span class="eti-no">{activeEti ? activeEti.number : ""}</span>
            </strong>
            
    </div>


    <!-- Esto el el pie del cuadro -->
    <div class="foot">
      <strong class="model"
        >{component.number} {component.revision} &times;{cardinality}</strong>
      <small class="description">{component.description}</small>
    </div>

  </div>
</div>

<style type="text/scss">
.wrapper {
  display: inline-block;
  position: relative;
  padding: 0.25em;

  .item {
    position: relative;
    height: 100%;
    border: 1px solid red;
    border-radius: 0.25rem;

    .head {
      position: relative;
      height: 20%;
      display: flex;
      justify-content: center;
      align-items: center;

      > .point-of-use-code {
        position: relative;
        text-align: center;
        font-size: 180%;
        font-weight: bold;
        padding: 0 0.25em;
        color: white !important;
        fill: white !important;
        background-color: #00000088;
        border-radius: 0.25rem 0 0 0;
      }

      > .eti-no {
        position: relative;
        text-align: center;
        font-size: 250%;
        flex-grow: 1;
        white-space: nowrap;
        overflow: hidden;
        
        &:empty:before {
          content: "-ESCANEAR ETI-";
          color: red;
          font-weight: bold;
        }
      }
    }

    .body {
      position: relative;
      height: 60%;
      text-align: center;
      background-color: #ffffffaa;
      > .status {
        
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        text-align: center;
        font-size: 250%;
        overflow: hidden;
        white-space: nowrap;
        // Esto hace que el texto se centre verticalmente
        display: flex;
        align-items: center;
        justify-content: center;
      }
    }

    .foot {
      position: relative;
      height: 20%;
      text-align: center;
      font-size: 75%;
      white-space: nowrap;
      overflow: hidden;

      > .model {
        font-size: 120%;
        font-weight: bold;
      }

      > .description {
        display: block;
        font-size: 100%;
        text-transform: uppercase;
      }
    }

    &.active {
      border: 1px #00000033 solid;

      .body {
        background-color: #00000033;
      }
    }

    &.empty {
      background-color: #feecf0;
      color: #cc0f35;

      &.active {
        background-color: #f14668;
        color: #fff;
      }
    }

    &.partial {
      background-color: #eff5fb;
      color: #296fa8;

      &.active {
        background-color: #3e8ed0;
        color: #fff;
      }
    }

    &.full {
      background-color: #effaf5;
      color: #257953;

      &.active {
        background-color: #48c78e;
        color: #fff;
      }
    }

    &.overloaded {
      background-color: #fffaeb;
      color: #946c00;

      &.active {
        background-color: #ffe08a;
        color: rgba(0, 0, 0, 0.7);
      }
    }
  }
}

//-------------------------------------
  // .wrapper {
  //   border: 0 transparent none;
  //   display: inline-block;
  //   padding: 0.25em;
  //   position: relative;
  //   > .item {
  //     border: 1px red solid;
  //     border-radius: 0.25rem;
  //     height: 100%;
  //     .head {
  //       display: flex;
  //       height: 20%;
  //       position: relative;
  //       justify-content: center; /* Centra horizontalmente */
  //       align-items: center; /* Centra verticalmente */
  //       > .point-of-use-code {
  //         background-color: #00000088;
  //         border-radius: 0.25rem 0 0 0;
  //         color: white !important;
  //         fill: white !important;
  //         font-size: 180%;
  //         font-weight: bold;
  //         padding: 0 0.25em;
  //         position: relative;
  //         text-align: center;
  //       }
  //       > .eti-no {
  //         flex-grow: 1;
  //         font-size: 180%;
  //         overflow: hidden;
  //         position: relative;
  //         text-align: center;
  //         white-space: nowrap;
          
  //       }
  //       > .eti-no:empty:before {
  //         color: red;
  //         font-weight: bold;
  //         content: "-ESCANEAR ETI-";
  //       }
  //     }
  //     > .body {
  //       background-color: #ffffffaa;
  //       font-size: 100%;
  //       height: 60%;
  //       position: relative;
  //       text-align: center;
  //       > .status {
  //         font-size: 250%;
  //         height: 100%;
  //         left: 0;
  //         overflow: hidden;
  //         position: absolute;
  //         text-align: center;
  //         top: 0;
  //         width: 100%;
  //         white-space: nowrap;
  //       }
  //     }
  //     .foot {
  //       font-size: 75%;
  //       height: 20%;
  //       overflow: hidden;
  //       text-align: center;
  //       white-space: nowrap;
  //       > .model {
  //         font-size: 120%;
  //         font-weight: bold;
  //       }
  //       > .description {
  //         display: block;
  //         font-size: 100%;
  //         text-transform: uppercase;
  //       }
  //     }
  //   }
  //   > .item.active {
  //     border: 1px #00000033 solid;
  //     > .body {
  //       background-color: #00000033;
  //     }
  //   }
  // }

  // .item {
  //   display: flex;
  //   /* flex-flow: column; */
  //   flex-direction: column;
  //   > div {
  //     border-radius: 0.25rem 0.25rem 0 0;
  //   }
  //   > div:last-child {
  //     border-radius: 0 0 0.25rem 0.25rem;
  //   }
  //   > div.body {
  //     border-radius: 0;
  //     /* flex:1 1 auto; */
  //     flex-grow: 1;
  //     align-items: center;
  //     justify-content: center;
  //   }
  // }

  // .empty {
  //   background-color: #feecf0;
  //   color: #cc0f35;
  // }

  // .partial {
  //   background-color: #eff5fb;
  //   color: #296fa8;
  // }

  // .full {
  //   background-color: #effaf5;
  //   color: #257953;
  // }

  // .overloaded {
  //   background-color: #fffaeb;
  //   color: #946c00;
  // }

  // .empty.active {
  //   background-color: #f14668;
  //   color: #fff;
  // }

  // .partial.active {
  //   background-color: #3e8ed0;
  //   color: #fff;
  // }

  // .full.active {
  //   background-color: #48c78e;
  //   color: #fff;
  // }

  // .overloaded.active {
  //   background-color: #ffe08a;
  //   color: rgba(0, 0, 0, 0.7);
  // }
</style>
