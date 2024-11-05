<script>
  import Body from "./Body.svelte";
  import Footer from "./Footer.svelte";
  import Header from "./Header.svelte";
  import { ChangeoverApi,CommonApi } from "./utils/HttpRequests";
  export let lineCode = "";

  const changeoverStatues = {
    UNKNOWN: "No es posible determinar si se requiere un cambio de modelo.",
    REQUIRED: "Cambio de módelo requerido.",
    MATCH: "La órden cargada en la línea coincide con la órden activa.",
  };
  Object.freeze(changeoverStatues);

  let showToKeep;
  let showToPut;
  let showToTake;
  let line = {};
  let workOrder = {};
  let status = changeoverStatues.UNKNOWN;
  let gamma = [];

  const updateLine = () => {
    ChangeoverApi.getLine(lineCode)
      .then((data) => {
        console.debug("getLine", data);
        line = { ...data };
        updateWorkOrder();
      })
      .catch(window.alert);
  };

  const updateWorkOrder = () => {
    ChangeoverApi.getWorkOrderByLineID(line.id)
      .then((data) => {
        console.debug("getWorkOrderByLineID", data);
        workOrder = { ...data };
        status =
          line.workOrderCode === data.code && line.partNo === data.partNo
            ? changeoverStatues.MATCH
            : changeoverStatues.REQUIRED;
            //CommonApi.UpdateGama(line.partNo,workOrder.partNo,line.code,line.code); //Esto se comento debido a que se actualizaba la gama en automatico al actualizar la pantalla
      })
      .catch(window.alert);
  };

//   const updateWorkOrder = () => {
//   ChangeoverApi.getWorkOrderByLineID(line.id)
//     .then((data) => {
//       console.debug("getWorkOrderByLineID", data);
//       workOrder = { ...data };
//       if (line.workOrderCode === data.code && line.partNo === data.partNo) {
//         status = changeoverStatues.MATCH;
//       } else {
//         status = changeoverStatues.REQUIRED;
//         CommonApi.UpdateGama(workOrder.partNo, line.code);
//           .then(() => window.location.reload(true))
//           .catch((msg) => {
//             disabled = false;
//             window.alert(msg);
//           });
//       }
//     })
//     .catch(window.alert);
// };


  /**
   * Get gammas for the line and work order correspondiong models
   * comparing them to indicate incoming and outgoing components.
   */
  const compareGammas = () => {
    ChangeoverApi.getGamma(lineCode, workOrder.partNo)
      .then((incomingGamma) => {
        console.debug("incomingGamma", incomingGamma);
        ChangeoverApi.getGamma(lineCode, line.partNo)
          .then((outgoingGamma) => {
            // merge point of use codes from both incoming and outgoing
            // gamas, and remove duplicates
            console.debug("outgoingGamma", outgoingGamma);
            const pointsOfUse = Array.from(
              new Set(
                incomingGamma
                  .map((item) => item.pointOfUseCode)
                  .concat(outgoingGamma.map((item) => item.pointOfUseCode))
              )
            ).sort();
            const gammaStatus = [];
            for (let i in pointsOfUse) {
              // get components from the incoming gamma assigned to the current point of use code
              const inc = incomingGamma
                .filter((item) => item.pointOfUseCode === pointsOfUse[i])
                .sort();
              // get components from the outgoing gamma assigned to the current point of use code
              const out = outgoingGamma
                .filter((item) => item.pointOfUseCode === pointsOfUse[i])
                .sort();
              // combine component models, remove duplicates and sort it
              const comps = Array.from(
                new Set(
                  inc
                    .map((item) => `${item.compNo} ${item.compRev2}`)
                    .concat(
                      out.map((item) => `${item.compNo} ${item.compRev2}`)
                    )
                )
              ).sort();
              for (let j in comps) {
                // add one row for each component number while trying to set gamma entries from both sides:
                // incoming component to the left and outgoing to the right
                gammaStatus.push({
                  pointOfUseCode: pointsOfUse[i],
                  incoming: inc.find(
                    (item) => `${item.compNo} ${item.compRev2}` === comps[j]
                  ),
                  outgoing: out.find(
                    (item) => `${item.compNo} ${item.compRev2}` === comps[j]
                  ),
                });
              }
            }
            gamma = [...gammaStatus];
            console.log(gamma);
          })
          .catch(window.alert);
      })
      .catch(window.alert);
  };

  $: if (lineCode) updateLine();

  $: if (changeoverStatues.REQUIRED === status) compareGammas();
</script>

<main>
  <Header {line} {workOrder} {status} />
  {#if status == changeoverStatues.REQUIRED}
    <Body {showToKeep} {showToTake} {showToPut} {gamma} />
  {:else}
    <div class="body">
      <h1>{status}</h1>
    </div>
  {/if}
  <Footer bind:showToKeep bind:showToPut bind:showToTake />
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

  .body {
    bottom: 2em;
    position: absolute;
    left: 0;
    top: 2em;
    width: 100%;
  }
  .body > h1 {
    left: 50%;
    position: absolute;
    text-align: center;
    top: 35%;
    transform: translate(-50%, -50%);
    color: #ff3e00;
    text-transform: uppercase;
    font-size: 2em;
    font-weight: 100;
  }
</style>
