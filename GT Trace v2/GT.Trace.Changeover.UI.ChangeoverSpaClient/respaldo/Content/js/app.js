let { useState, useEffect } = React;

const changeoverStatues = {
    UNKNOWN: "No es posible determinar si se requiere un cambio de modelo.",
    REQUIRED: "Cambio de módelo requerido.",
    MATCH: "La información de producción asociada a la línea coincide con la órden de frabricación activa."
};
Object.freeze(changeoverStatues);

const IncomingHeader = (props) => <React.Fragment>
    <span>{props.modelo} {props.active_revision}</span>
    <br />
    <small>{props.codew}</small>
</React.Fragment>;

const OutgoingHeader = (props) => <React.Fragment>
    <span>{props.part_number} {props.rev}</span>
    <br />
    <small>{props.codew}</small>
</React.Fragment>;

const Cell = (props) => <td><div>
    <span>{props.CompNo} {props.CompRev}</span>
    <br />
    {/*<small>{props.CompDesc}</small>*/}
</div></td>;

const Row = ({ item }) => <tr className={(item.incoming && item.outgoing ? 'keep' : item.incoming ? 'load' : 'unload')}>
    <th>{item.incoming ? item.incoming.CompDesc : item.outgoing.CompDesc}</th>
    <Cell {...item.incoming} />
    <td>{item.pointOfUseCode}</td>
    <Cell {...item.outgoing} />
</tr>;

const App = ({ lineCode }) => {
    const [errors, setErrors] = useState([]);
    const [state, setState] = useState({ line: null, workOrder: null, status: changeoverStatues.UNKNOWN, gamma: [] });
    const [showOnlyComponentsToReturn, setShowOnlyComponentsToReturn] = useState(true);

    useEffect(async function () {
        ChangeoverApi.getLine(lineCode, (line) => {
            setState({ ...state, line: line });
            ChangeoverApi.getWorkOrderByLineID(line.id, (workOrder) => {
                const status = false && line.codew === workOrder.codew && line.part_number === workOrder.modelo
                    ? changeoverStatues.MATCH
                    : changeoverStatues.REQUIRED;

                if (changeoverStatues.REQUIRED === status) {
                    ChangeoverApi.getGamma(lineCode, /*workOrder.part_number*/'84901', (incomingGamma) => {
                        ChangeoverApi.getGamma(lineCode, line.modelo, (outgoingGamma) => {
                            // merge point of use codes from both incoming and outgoing gamas, and remove duplicates
                            const pointsOfUse = Array.from(new Set(incomingGamma.map(item => item.PointOfUse).concat(outgoingGamma.map(item => item.PointOfUse)))).sort();
                            const gammaStatus = [];
                            for (let i in pointsOfUse) {
                                // get components from the incoming gamma assigned to the current point of use code
                                const inc = incomingGamma.filter(item => item.PointOfUse === pointsOfUse[i]).sort();
                                // get components from the outgoing gamma assigned to the current point of use code
                                const out = outgoingGamma.filter(item => item.PointOfUse === pointsOfUse[i]).sort();
                                // combine component models, remove duplicates and sort it
                                const comps = Array.from(new Set(inc.map(item => `${item.CompNo} ${item.CompRev2}`).concat(out.map(item => `${item.CompNo} ${item.CompRev2}`)))).sort();
                                for (let j in comps) {
                                    // add one row for each component number while trying to set gamma entries from both sides:
                                    // incoming component to the left and outgoing to the right
                                    gammaStatus.push({
                                        pointOfUseCode: pointsOfUse[i],
                                        incoming: inc.find(item => `${item.CompNo} ${item.CompRev2}` === comps[j]),
                                        outgoing: out.find(item => `${item.CompNo} ${item.CompRev2}` === comps[j]),
                                    });
                                }
                            }
                            setState({ ...state, line: line, workOrder: workOrder, status: status, gamma: gammaStatus });
                        }, setErrors);
                    }, setErrors);
                }
                else {
                    setState({ ...state, line: line, workOrder: workOrder, status: status });
                }
            }, setErrors);
        }, setErrors);
    }, []);

    const handleShowOnlyComponentsToReturnChanged = (e) =>
        setShowOnlyComponentsToReturn(e.target.checked);

    const handleApplyChangeoverClick = (e) => {
        ChangeoverApi.applyChangeover(lineCode, console.log, setErrors);
    };

    return (<React.Fragment>
        <ErrorOverlay errors={errors} />
        <table>
            <thead>
                <tr>
                    <th>
                        <label className="no-text-highlight">
                            <input type="checkbox" onChange={handleShowOnlyComponentsToReturnChanged} checked={showOnlyComponentsToReturn} />
                            Mostrar sólo componentes a retornar
                        </label>
                    </th>
                    <th>
                        <IncomingHeader {...state.line} />
                    </th>
                    <th>Túnel</th>
                    <th>
                        <OutgoingHeader {...state.workOrder} />
                    </th>
                </tr>
            </thead>
            <tbody>
                {state.status === changeoverStatues.REQUIRED

                    ? state.gamma.map((item) => (showOnlyComponentsToReturn && item.incoming)
                        ? null
                        : <Row item={item} />)

                    : <tr><td colspan="4">{state.status}</td>
                </tr>}
            </tbody>
        </table>
        <button onClick={handleApplyChangeoverClick}>Aplicar Cambio de Modelo</button>
    </React.Fragment >);
}