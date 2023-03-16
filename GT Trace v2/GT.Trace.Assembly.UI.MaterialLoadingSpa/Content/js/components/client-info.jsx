const PurchaseOrderComponent = ({ purchaseOrder }) =>
    purchaseOrder
        ? <span>{purchaseOrder}</span>
        : <span style={{ color: 'red' }}>N/A</span>;

const ClientInfoComponent = ({ station }) => (<table border="0">
    <caption>
        <b><u>{station.workOrder.client.name}</u></b><br />
        <small>modelo</small>
    </caption>
    <tbody>
        <tr>
            <th>Código</th>
            <td>{station.workOrder.client.code}</td>
        </tr>
        <tr>
            <th>Nombre</th>
            <td>{station.workOrder.client.description}</td>
        </tr>
        <tr>
            <th>Parte</th>
            <td>{station.workOrder.client.partNo}</td>
        </tr>
        <tr>
            <th>PO#</th>
            <td><PurchaseOrderComponent purchaseOrder={station.workOrder.po} /></td>
        </tr>
    </tbody>
</table>);