const QcApprovalComponent = ({ isQcApproved }) =>
    isQcApproved
        ? <span style={{ backgroundColor: 'blue', color: 'white', width: '100%', textAlign: 'center', display: 'inline-block' }}>QC OK</span>
        : <span style={{ backgroundColor: 'red', color: 'white', width: '100%', textAlign: 'center', display: 'inline-block' }}>QC NOK</span>;

const PackagingComponent = ({ station }) => (<table id="pallet-table">
    <caption>
        <b><u>Empaque</u></b><br /><small>piezas en curso</small>
    </caption>
    <tbody>
        <tr>
            <td>
                <sub>{station.workOrder.packType.isBox ? "Caja" : "Contenedor"}</sub>
                <br />
                <b>{station.pallet.container.quantity}</b>
                <br />
                <small>/ {station.pallet.container.size}</small>
            </td>
            <td>
                <sub>Tarima</sub>
                <br />
                <b>{station.pallet.quantity}</b>
                <br />
                <small>/ {station.pallet.size}</small>
            </td>
        </tr>
    </tbody>
    <tfoot>
        <tr>
            <td colSpan="2"><QcApprovalComponent isQcApproved={station.pallet.container.isQcApproved} /></td>
        </tr>
        <tr>
            <td colSpan="2">
                <img style={{ maxWidth: '100%' }} src={`data:image/png;base64,${station.pallet.sampleImageBase64Data}`} />
            </td>
        </tr>
    </tfoot>
</table>);