const HourlyProductionComponent = ({ lineName, data }) => (<table>
    <caption>
        <b><u>{lineName}</u></b><br />
        <small>producción por hora</small>
    </caption>
    <thead>
        <tr>
            <th>Hora</th>
            <th>#Parte</th>
            <th>Meta</th>
            <th>Actual</th>
        </tr>
    </thead>
    <tbody>
        {data.map((item, index, array) => (<tr key={`hourly_production_${index}`}>
            <td style={{ textAlign: 'right' }}>{item.hour}</td>
            <td>{item.partNo}</td>
            <td style={{ textAlign: 'right' }}>{item.targetQuantity}</td>
            <td style={{ textAlign: 'right' }}>{item.quantity}</td>
        </tr>))}
    </tbody>
</table>);