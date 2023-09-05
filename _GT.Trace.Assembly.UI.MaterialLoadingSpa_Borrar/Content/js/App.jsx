let { useState, useEffect, useRef } = React;

const defaultState = {
    loadedEtis: [
        { etiNo: 'ETINO', partNo: 'PART', workOrderCode: 'CODEW', compNo: 'COMP', date: '22-00-00' }
    ],
    pointOfUseCode: null,
    lineCode: null
};

const EtiInput = ({ onEtiEntered, etiNoInputRef }) => {
    const onFormSubmit = (e) => {
        e.preventDefault();
        onEtiEntered(e.target.children[0].value);
        return false;
    };
    return (<form onSubmit={onFormSubmit}>
        <input type="text" placeholder="ETI" ref={etiNoInputRef} />
    </form>);
};

const PointOfUseInput = ({ pointOfUseCode, onPointOfUseEntered, pointOfUseCodeInputRef }) => {
    const onFormSubmit = (e) => {
        e.preventDefault();
        onPointOfUseEntered(e.target.children[0].value);
        e.target.children[0].value = '';
        return false;
    };

    return (<form onSubmit={onFormSubmit}>
        <input type="text" placeholder="Túnel" value={pointOfUseCode} ref={pointOfUseCodeInputRef} />
    </form>);
};

const LoadedEtisList = ({ pointOfUseCode, loadedEtis, lineCode, onEtiRemoved }) => {
    if (!pointOfUseCode) {
        return <div>No se ha seleccionado un túnel.</div>;
    }
    //console.log(loadedEtis)

    const handleOnUnloadEtiButtonClick = (e) => {
        const etiNo = e.target.parentElement.parentElement.children[0].innerText;
        unloadEti(pointOfUseCode, etiNo, lineCode, '5766', () => {
            onEtiRemoved(etiNo);
        }, alert);
    };

    return (<table style={{ tableLayout: 'fixed', width: '100%', textAlign: 'left' }}>
        <caption>ETIs cargadas en el túnel <strong>{pointOfUseCode}</strong></caption>
        <thead>
            <tr>
                <th>#ETI</th>
                <th>#Parte</th>
                <th>Orden</th>
                <th>#Componente</th>
                <th>Fecha de Carga</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            {loadedEtis.map((item, index) => {
                const dateOptions = { dateStyle: 'short' };
                const timeOptions = { timeStyle: 'short' };
                const effectiveTime = new Date(item.effectiveTime);
                return (<tr>
                    <td>{item.etiNo}</td>
                    <td>{item.partNo}</td>
                    <td>{item.workOrderCode}</td>
                    <td>{item.componentNo}</td>
                    <td>{effectiveTime.toLocaleDateString('us-EN', dateOptions)} {effectiveTime.toLocaleTimeString('us-EN', timeOptions)}</td>
                    <td><button type="button" onClick={handleOnUnloadEtiButtonClick}>&times;</button></td>
                </tr>);
            })}
        </tbody>
    </table>);
};

const EtiLoader = ({ partNo, lineCode, workOrderCode, etiNoInputRef, pointOfUseCodeInputRef, pointOfUseFilterInputRef }) => {
    const [operationsPointsOfUse, setOperationsPointsOfUse] = useState([]);
    const [pointsOfUse, setPointsOfUse] = useState([]);

    useEffect(async () => {
        if (lineCode) {
            getLinePointsOfUse(lineCode, (items) => {
                setOperationsPointsOfUse(items.filter(item => item.canBeLoadedByOperations));
            });
        }
    }, [lineCode]);

    const handleOnEtiEntered = (etiNo) => {
        if (etiNo) {
            fetchEtiPointsOfUse(etiNo, partNo, lineCode,
                (data) => {
                    const filteredPointsOfUse = data.filter(item => operationsPointsOfUse.findIndex(ops => ops.code === item) >= 0);
                    if (filteredPointsOfUse.length > 0) {
                        setPointsOfUse(filteredPointsOfUse);
                        pointOfUseCodeInputRef.current.focus();
                        pointOfUseCodeInputRef.current.select();
                    }
                    else {
                        alert(`Los túneles asociados a la ETI #${etiNo} [${data.join(',')}] no corresponden con ningún túnel aprobado para carga por operaciones [ ${operationsPointsOfUse.map(item => item.code).join(',')} ].`);
                        etiNoInputRef.current.select();
                    }
                },
                (msg) => {
                    alert(msg);
                    setPointsOfUse([]);
                });
            etiNoInputRef.current.select();
        }
    };

    const handleOnPointOfUseEntered = (pointOfUseCode) => {
        //if (!pointOfUseCode) {
        //    alert('El túnel no ha sido ingresado.');
        //}
        //else if (pointsOfUse.indexOf(pointOfUseCode) < 0) {
        //    alert(`El túnel "${pointOfUseCode}" no es válido.`);
        //}
        //else {
        //    loadEti(pointOfUseCode, etiNoInputRef.current.value, lineCode, '5766', partNo, workOrderCode,
        //        () => {
        //            pointOfUseCodeInputRef.current.value = '';
        //            etiNoInputRef.current.value = '';
        //            etiNoInputRef.current.focus();
        //            setPointsOfUse([]);

        //            pointOfUseFilterInputRef.current.value = pointOfUseCode;
        //            pointOfUseFilterInputRef.current.parentElement.dispatchEvent(new Event('submit', { cancelable: true, bubbles: true }));
        //        },
        //        (msg) => {
        //            alert(msg);
        //        });
        //}
    };

    return (<React.Fragment>
        <div class="item1">
            <EtiInput onEtiEntered={handleOnEtiEntered} etiNoInputRef={etiNoInputRef} />
        </div>
        <div class="item2" style={{ textAlign: 'center' }}>
            {pointsOfUse.length === 0 ? <span>-</span> : pointsOfUse.map((item, index) => <span>{item}</span>)}
        </div>
        <div class="item3">
            <PointOfUseInput onPointOfUseEntered={handleOnPointOfUseEntered} pointOfUseCodeInputRef={pointOfUseCodeInputRef} />
        </div>
    </React.Fragment>);
};

const MessageLog = () => {
    const [messages, setMessages] = useState([]);

    useEffect(async () => {
        const defaultAlertMethod = window.alert;
        window.alert = (msg) => {
            const MAX_MESSAGES = 50;
            const now = new Date();
            const entry = `${now.toLocaleTimeString()}.- ${msg}`;
            messages.unshift(entry);
            delete messages[MAX_MESSAGES];
            setMessages(messages.slice());
        };
        return () => window.alert = defaultAlertMethod;
    }, []);

    return <div className="item4">{messages.map(item => <span>{item}</span>)}</div>;
};

const App = () => {
    const [state, setState] = useState(defaultState);
    const [loadedEtis, setLoadedEtis] = useState([]);
    const pointOfUseCodeInputRef = useRef(null);
    const etiNoInputRef = useRef(null);
    const lineFilterRef = useRef(null);

    const handleLoadedOnEtisListEtiRemoved = (etiNo) => {
        setLoadedEtis(loadedEtis.filter(item => item.etiNo != etiNo));
    };

    const [line, setLine] = useState({ code: '---', activePart: { number: '---', revision: null }, workOrder: { code: '---' } });

    useEffect(async () => {
        const urlSearchParams = new URLSearchParams(window.location.search);
        const params = Object.fromEntries(urlSearchParams.entries());
        const lineCode = params['line'];
        if (lineCode && lineCode.length > 0) {
            getLine(lineCode, setLine);
        }
        else {
            alert("Parámetro \"line\" no encontrado.");
        }
    }, []);

    return (<React.Fragment>
        <div id="title-bar">
            Carga de Materiales
        </div>
        <div class="main-app-container">
            <div>
                <label>Línea <strong>{line.code}</strong></label>
                <span>&nbsp;</span>
                <label>Parte <strong>{`${line.activePart.number}${line.activePart.revision ? ` Rev ${line.activePart.revision}` : ''}`}</strong></label>
                <span>&nbsp;</span>
                <label>Orden <strong>{line.workOrder.code}</strong></label>
            </div>
            <div>
                <div class="flex-container">
                    <EtiLoader
                        partNo={line.activePart.number}
                        lineCode={line.code}
                        workOrderCode={line.workOrder.code}
                        etiNoInputRef={etiNoInputRef}
                        pointOfUseCodeInputRef={pointOfUseCodeInputRef}
                        pointOfUseFilterInputRef={lineFilterRef} />
                </div>
            </div>
            <div>
                <LoadedEtisList pointOfUseCode={state.pointOfUseCode} lineCode={line.code} loadedEtis={loadedEtis} onEtiRemoved={handleLoadedOnEtisListEtiRemoved} />
            </div>
            <MessageLog />
        </div>
    </React.Fragment>);
}

const urlSearchParams = new URLSearchParams(window.location.search);
const params = Object.fromEntries(urlSearchParams.entries());

ReactDOM.render(<App params={params} />, document.querySelector('#app'));