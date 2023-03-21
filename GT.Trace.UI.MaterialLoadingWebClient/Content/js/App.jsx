let { useState, useEffect, useRef } = React;

const defaultState = {
    line: { id: null, code: null },
    workOrder: { code: null },
    pointOfUseCode: null,
    gamaState: [],
    gamaItem: { pointOfUseCode: null, componentNo: null, etis: [] },
};

/*
 * Initializes a signalR client wrapper instance exposing the 'on' method
 * used to listen and react to events.
 * */
const getSignalRClient = (name, url) => {
    let client = {};

    const connection = new signalR.HubConnectionBuilder()
        .withUrl(url)
        .configureLogging(signalR.LogLevel.Information)
        .build();

    client.start = async function start() {
        try {
            await connection.start({ withCredentials: false });
            console.log(`${name} Connected!`, url);
        } catch (err) {
            console.error(name, err);
            setTimeout(start, 3000);
        }
    };

    connection.onclose(async () => await client.start());

    client.on = (messageName, callback) => {
        connection.on(messageName, callback);
        return client;
    };

    return client;
};

const App = () => {
    const [state, setState] = useState(defaultState);

    const etiNoRef = useRef(null);
    const pointOfUseRef = useRef(null);
    const lineFilterRef = useRef(null);

    // Updates the selected line in the state and sets the focus in the corresponding element.
    const handleOnLineChanged = (line) => {
        if (line != null) {
            setState({ ...state, line: line, workOrder: { code: line.activeWorkOrderCode, partNo: line.activePart.number } });
            etiNoRef.current.focus();
        }
        else {
            setState(defaultState);
            lineFilterRef.current.focus();
        }
    };

    // Updates the selected work order in the state.
    const handleOnWorkOrderChanged = (workOrder) =>
        setState({ ...state, workOrder: workOrder });

    // Updates the selected gama item in the state.
    const handleOnGamaItemSelected = (item) =>
        setState({ ...state, gamaItem: item });

    // Updates the gama state whenever it changes (maybe handlers for etiremoved and eti added should call this method from the component).
    const handleGamaState = (gamaState) => setState({ ...state, gamaState: gamaState });

    // Updates the gama state and selected item accordingly every time an ETI is unloaded or used.
    function handleOnEtiRemoved(eti) {
        const stateGamaItem = state.gamaState.find((item) => item.componentNo === eti.componentNo && item.pointOfUseCode === eti.pointOfUseCode);
        if (stateGamaItem != null) {
            const gamaItem = { ...stateGamaItem, etis: stateGamaItem.etis.filter((item) => eti.etiNo !== item.etiNo) };
            setState({
                ...state,
                gamaState: state.gamaState.map((item) => item.componentNo === gamaItem.componentNo && item.pointOfUseCode === gamaItem.pointOfUseCode ? gamaItem : item),
                gamaItem: gamaItem.componentNo === state.gamaItem.componentNo && gamaItem.pointOfUseCode === state.gamaItem.pointOfUseCode ? gamaItem : state.gamaItem
            });
        }
    };

    // Update gama state and selected item every time an ETI is loaded into the point of use.
    function handleOnEtiAdded(pointOfUseCode, etiNo, componentNo) {
        const stateGamaItem = state.gamaState.find((item) => item.componentNo === componentNo && item.pointOfUseCode === pointOfUseCode);
        if (stateGamaItem != null) {
            const gamaItem = { ...stateGamaItem, etis: [...state.gamaItem.etis, { pointOfUseCode: pointOfUseCode, etiNo: etiNo, componentNo: state.gamaItem.componentNo, effectiveTime: new Date() }] };
            setState({
                ...state,
                gamaState: state.gamaState.map((item) => item.componentNo === gamaItem.componentNo && item.pointOfUseCode === gamaItem.pointOfUseCode ? gamaItem : item),
                gamaItem: gamaItem
            });
        }
    };

    // Map the EtiUsed signal to the window.onEtiUsed method the first time this component is rendered.
    useEffect(async () => {
        const onFocus = (e) => {
            if (e.target.tagName === 'INPUT' && e.target.type === 'text') {
                e.target.select();
            }
        };

        document.addEventListener('focus', onFocus, true);

        getSignalRClient('ETI Movements SignalR Client', 'http://mxsrvapps/gtt/services/etimovements/hubs/etimovements')
            .on("EtiUsed", (lineCode, etiNo, componentNo, pointOfUseCode) => {
                const eti = { etiNo: etiNo, pointOfUseCode: pointOfUseCode, componentNo: componentNo };
                console.log('EtiUsed', eti);
                window.onEtiUsed(eti);
            })
            .start();

        return () => document.removeEventListener('focus', onFocus);
    }, []);

    // Map the window.onEtiUsed method to the handleOnEtiRemoved handler every time
    // this component is rendered to prevent using the initial state all the time.
    useEffect(async () => {
        window.onEtiUsed = handleOnEtiRemoved;
        return () => window.onEtiUsed = null;
    });

    return <React.Fragment>
        <ErrorOverlay />
        <div>
            <LineSearch lineFilterRef={lineFilterRef} onLineChanged={handleOnLineChanged} />
            <PartSelection line={state.line} selectedWorkOrderCode={state.workOrder.code} onWorkOrderChanged={handleOnWorkOrderChanged} />
            <hr />
            <EtiLoad etiNoRef={etiNoRef} pointOfUseRef={pointOfUseRef} selectedLineCode={state.line.code} selectedPartNo={state.workOrder.partNo} selectedWorkOrderCode={state.workOrder.code} onEtiAdded={handleOnEtiAdded} />
        </div>
        <GamaState gamaState={state.gamaState} lineCode={state.line.code} partNo={state.workOrder.partNo} onGamaItemSelected={handleOnGamaItemSelected} onGamaState={handleGamaState} activeItem={state.gamaItem} />
        <LoadedEtis line={state.line} gamaItem={state.gamaItem} onEtiRemoved={handleOnEtiRemoved} />
    </React.Fragment>;
}

ReactDOM.render(<App />, document.querySelector('#app'));