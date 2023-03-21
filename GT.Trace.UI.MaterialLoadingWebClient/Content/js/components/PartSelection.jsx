const PartSelection = ({ line, selectedWorkOrderCode, onWorkOrderChanged }) => {
    const [workOrders, setWorkOrders] = useState([]);

    function handleOnChange(e) {
        const workOrder = workOrders.find((item) => item.code === e.target.value);
        onWorkOrderChanged(workOrder);
    };

    const selectedWorkOrder = workOrders.find((workOrder) => workOrder.code === selectedWorkOrderCode)
        || { code: null, partNo: null, revision: null };

    useEffect(async () => line.id ? materialLoadingApi.getLineWorkOrders(line.id, setWorkOrders) : setWorkOrders([]), [line]);

    const partNumbers = workOrders.filter((value, index, self) => self.findIndex((item) => item.partNo === value.partNo && item.revision === value.revision) === index);

    return (<React.Fragment>
        <table>
            <tr>
                <td>Línea</td>
                <th>{line.code}</th>
            </tr>
            <tr>
                <td>Modelo</td>
                <th>
                    <select value={selectedWorkOrder.code} onChange={handleOnChange}>
                        {partNumbers.map((item) => <option value={item.code}>{item.partNo} Rev {item.revision}</option>)}
                    </select>
                </th>
            </tr>
            <tr>
                <td>Cliente</td>
                <th>{selectedWorkOrder.client}</th>
            </tr>
            <tr>
                <td>Línea</td>
                <th>{selectedWorkOrder.line}</th>
            </tr>
            <tr>
                <td>Orden</td>
                <th>{selectedWorkOrder.order}</th>
            </tr>
        </table>
    </React.Fragment>);
};