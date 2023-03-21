const LoadedEtis = ({ line, gamaItem, onEtiRemoved }) => {
    function handleOnClick(eti, button) {
        button.disabled = true;
        if (window.confirm(`Estas a punto de descargar la ETI "${eti.etiNo}". Presióna OK para continuar...`)) {
            etiMovementsApi.unloadEti('', eti.etiNo, line.code, '',
                (data) => {
                    button.disabled = false;
                    onEtiRemoved(eti);
                },
                (err) => {
                    button.disabled = false;
                });
        }
    };

    return <div>
        <table id="loaded-material">
            <caption>{line.code || "Línea"} \ {gamaItem.pointOfUseCode || "Túnel"} \ {gamaItem.componentNo || "Comp."}</caption>
            <thead></thead>
            <tbody>
                {gamaItem.etis.map((item) => {
                    const effectiveTime = new Date(item.effectiveTime);
                    return <tr>
                        <td>
                            <button className="unload-eti-button" onClick={(e) => handleOnClick(item, e.target)}>&times;</button>
                            <b>{item.etiNo}</b>
                            <br />
                            <small>Cargada el día {effectiveTime.toLocaleDateString()} a las {effectiveTime.toLocaleTimeString()} hrs.</small>
                        </td>
                    </tr>;
                })}
            </tbody>
        </table>
    </div>;
};