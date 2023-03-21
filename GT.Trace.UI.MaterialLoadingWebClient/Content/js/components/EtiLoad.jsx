const EtiLoad = ({ pointOfUseRef, etiNoRef, selectedLineCode, selectedPartNo, selectedWorkOrderCode, onEtiAdded }) => {
    const [pointsOfUse, setPointsOfUse] = useState([]);
    const [etiInfo, setEtiInfo] = useState({ componentNo: null, etiNo: null, pointOfUseCode: null, startTime: null, usageTime: null, endTime: null, isDepleted: false });

    //These 2 functions are used to change the color of the charge indicator
    function squareColorOKClass(colorClass) { return Figure.className = "cuadradoOK"; }
    function squareColorFailClass(colorClass) { return Figure.className = "cuadradoFAIL"; }

    function handleOnEtiNoSubmit(e) {
        e.preventDefault();

        const etiNo = etiNoRef.current.value;
        etiNoRef.current.disabled = true;

        etiMovementsApi.getEtiInfo(etiNo, setEtiInfo);

        materialLoadingApi.getEtiPointsOfUse(etiNo, selectedPartNo, selectedLineCode,
            (data) => {
                setPointsOfUse([...data]);
                if (data.length == 0) {
                    squareColorFailClass();//Here you make the color change
                    alert(`ETI "${etiNo}" no corresponde con ningún punto de uso para la gama "${selectedPartNo} ${selectedLineCode}".`);
                                        //<=== poner aqui cuando aparezca el mensaje de arriba
                    etiNoRef.current.disabled = false;
                    pointOfUseRef.current.value = '';
                    etiNoRef.current.focus();
                }
                else {
                    etiNoRef.current.disabled = false;
                    pointOfUseRef.current.focus();
                }
            },
            (error) => {
                setPointsOfUse([]);
                etiNoRef.current.disabled = false;
            }
        );
        return false;
    };

    function handleOnPointOfUseSubmit(e) {
        e.preventDefault();
        const pointOfUseCode = pointOfUseRef.current.value;
        const etiNo = etiNoRef.current.value;

        if (pointsOfUse.findIndex((item) => item === pointOfUseCode) >= 0) {
            pointOfUseRef.current.disabled = true;
            etiMovementsApi.loadEti(pointOfUseCode, etiNo, selectedLineCode, '005766', selectedPartNo, selectedWorkOrderCode,
                (data) => {
                    pointOfUseRef.current.disabled = false;
                    onEtiAdded(pointOfUseCode, etiNo, etiInfo.componentNo);
                    pointOfUseRef.current.value = '';
                    etiNoRef.current.value = '';
                    etiNoRef.current.focus();
                },
                squareColorOKClass(),//Here you make the color change
                (error) => {
                    pointOfUseRef.current.disabled = false;
                    etiNoRef.current.focus();
                });
        }
        else {
            squareColorFailClass(); //Here you make the color change
            alert(`ETI ${etiNo} solamente se puede cargar en los túneles: ${pointsOfUse.splice(', ')}.`);
        }
        return false;
    };

    return <table>
        <tr>
            <td>ETI No.</td>
            <th>
                <form style={{ display: 'inline-block' }} onSubmit={handleOnEtiNoSubmit}>
                    <input type="text" placeholder="ETI No." ref={etiNoRef} />
                </form>
            </th>
        </tr>
        <tr>
            <td>Componente</td>
            <th>{etiInfo.componentNo} Rev {etiInfo.revision}</th>
        </tr>
        <tr>
            <th style={{ border: '1px solid white', borderStyle: 'solid none', padding: '0.5em 0' }} colSpan="2">{etiInfo.status || 'OK'}</th>
        </tr>
        <tr>
            <td>Túneles Válidos</td>
            <th>{pointsOfUse.join(', ')}</th>
        </tr>
        <tr>
            <td>Túnel</td>
            <th>
                <form style={{ display: 'inline-block' }} onSubmit={handleOnPointOfUseSubmit}>
                    <input type="text" placeholder="Túnel" ref={pointOfUseRef} style={{ width: '3em' }} />
                </form>
            </th>
        </tr>
        <tr>
            <td>Carga ETI</td>
            <div id="Figure" class="cuadrado"  ></div>
        </tr>
    </table>;
};