const WorkOrderComponent = ({ station }) => {
    const progress = station.workOrder.size == 0 ? 0 : 100.0 * station.workOrder.quantity / station.workOrder.size;
    return (<table border="0">
        <caption>
            <b><u>{station.workOrder.code}</u></b><br /><small>orden de fabricación</small>
        </caption>
        <tbody>
            <tr><th>Requerimiento</th><td>{station.workOrder.size}</td></tr>
            <tr><th>Pendiente</th><td>{Math.max(0, station.workOrder.size - station.workOrder.quantity)}</td></tr>
            <tr>
                <th>Avance</th>
                <td>
                    <span>{progress.toFixed(2)} %</span>
                    <div style={{ border: '2px solid green', width: `${Math.min(100, progress)}%` }}></div>
                </td>
            </tr>
        </tbody>
    </table>);
};