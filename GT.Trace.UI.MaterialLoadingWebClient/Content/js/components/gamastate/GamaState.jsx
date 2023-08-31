const GamaState = ({ gamaState, partNo, lineCode, onGamaItemSelected, onGamaState, activeItem }) => {
    const [hideFullPointsOfUse, setHideFullPointsOfUse] = useState(true);
    useEffect(async () => {
        if (partNo && lineCode) {
            commonApi.getLineMaterialStatus(lineCode, partNo, onGamaState, alert);
        }
        else {
            onGamaState([]);
        }
    }, [partNo, lineCode]);

    const handleOnClick = () => {
        if (partNo && lineCode) {
            commonApi.getLineMaterialStatus(lineCode, partNo, onGamaState, alert);
            commonApi.updateLineBom(partNo, lineCode);
        }
        else {
            onGamaState([]);
        }
    };

    return <div>
        <label>
            <input type="checkbox" checked={hideFullPointsOfUse} onChange={(e) => setHideFullPointsOfUse(e.target.checked)} />
            <span>Ocultar túneles llenos.</span>
        </label>
        <table id="gama-state">
            <caption>
                {(partNo && lineCode || true
                    ? <React.Fragment>
                        <span>Gama {partNo || '-'} {lineCode || '-'}</span>
                        <button id="update-gama-state-button" onClick={handleOnClick}>ACTUALIZAR</button>
                    </React.Fragment>
                    : <span>Gama - -</span>)}
            </caption>
            <thead>
                <tr>
                    <th>Túnel</th>
                    <th>Component</th>
                    <th>Estatus</th>
                </tr>
            </thead>
            <tbody>
                {gamaState.map((item) => (hideFullPointsOfUse && item.capacity === item.etis.length ? null : <GamaStateItem item={item} onGamaItemSelected={onGamaItemSelected} activeItem={activeItem} />))}
            </tbody>
        </table>
        <table style={{ tableLayout: 'fixed', textAlign: 'center', fontSize: '50%', borderSpacing: '1px', marginTop: '0.5em', width: '75%', float: 'right', verticalAlign: 'midle' }}>
            <caption>Código de Colores</caption>
            <tr>
                <td style={{ padding: '0.25em' }} className="empty">Túnel Vacío</td>
                <td style={{ padding: '0.25em' }} className="partial">Túnel Parcial</td>
            </tr>
            <tr>
                <td style={{ padding: '0.25em' }} className="full">Túnel Lleno</td>
                <td style={{ padding: '0.25em' }} className="over">Túnel Sobrecargado</td>
            </tr>
        </table>
    </div>;
};