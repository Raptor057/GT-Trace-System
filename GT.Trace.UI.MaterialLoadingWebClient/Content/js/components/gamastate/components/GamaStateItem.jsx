const GamaStateItem = ({ item, onGamaItemSelected, activeItem }) => {
    const quantity = item.etis.length;
    const className = (quantity == 0
        ? 'empty'
        : quantity > item.capacity
            ? 'over'
            : quantity === item.capacity
                ? 'full'
                : 'partial')
                + (item.componentNo == activeItem.componentNo && item.pointOfUseCode == activeItem.pointOfUseCode ? ' active' : '');
    return <tr className={className} onClick={(e) => onGamaItemSelected(item)}>
        <td>{item.pointOfUseCode}</td>
        <td>{item.componentNo}</td>
        <td> {quantity} / {item.capacity}</td>
    </tr>;
}