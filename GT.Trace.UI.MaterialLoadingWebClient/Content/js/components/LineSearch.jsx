const LineSearch = ({ onLineChanged, lineFilterRef }) => {
    function handleOnSubmit(e) {
        e.preventDefault();
        if (lineFilterRef.current.value) {
            materialLoadingApi.getPointOfUseLines(
                lineFilterRef.current.value,
                (lines) => {
                    if (lines.length > 0) {
                        materialLoadingApi.getLineByCode(lines[0], onLineChanged, alert);
                    }
                    else {
                        onLineChanged(null);
                    }
                },
                alert);
        }
        else {
            onLineChanged(null);
        }
        return false;
    };
    return <form onSubmit={handleOnSubmit}>
        <input type="text" ref={lineFilterRef} autoFocus placeholder="Código de Túnel" />
        <br />
        <sup>Ingresar el código de túnel para seleccionar la línea.</sup>
    </form>;
};