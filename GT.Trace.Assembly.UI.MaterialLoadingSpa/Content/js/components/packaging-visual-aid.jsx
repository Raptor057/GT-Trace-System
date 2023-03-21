const PackagingVisualAidComponent = ({ station }) => (<table border="0">
    <caption>
        <b><u>{station.workOrder.packType.isBox ? "Caja" : "Contenedor"}</u></b><br />
        <small>empaque</small>
    </caption>
    <tbody>
        <tr>
            <td>
                <img src={`data:image/png;base64,${station.pallet.packagingImageBase64Data}`} style={{ maxWidth: '100%' }} />
            </td>
        </tr>
        <tr><td><sub>Etiqueta individual</sub><br />{station.bomLabel.number} {station.bomLabel.description.replace(`${station.bomLabel.number}:`, '')}</td></tr>
    </tbody>
</table>);