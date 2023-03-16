let { useState, useEffect } = React;

const ScanInputComponent = () => {
    const [input, setInput] = useState(null);

    const handleFormOnSubmit = (e) => {
        e.preventDefault();
        alert(input);
        setInput('')
        return false;
    };

    const handeInputOnChange = (e) =>
        setInput(e.target.value);

    const handleOnInputBlur = (e) =>
        e.target.focus();

    return (<form onSubmit={handleFormOnSubmit}>
        <input value={input} type="text" onBlur={handleOnInputBlur} onChange={handeInputOnChange} autoFocus placeholder="Favor de escanear la etiqueta individual." style={{ width: '100%' }} />
    </form>);
};