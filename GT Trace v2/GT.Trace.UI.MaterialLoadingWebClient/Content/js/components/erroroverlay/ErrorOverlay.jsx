const ErrorOverlay = () => {
    const [state, setState] = useState({ display: 'none' });
    useEffect(async () => {
        window.errors = JSON.parse(window.localStorage['_errors'] || '[]');

        const backup = window.alert;
        window.alert = (message) => {
            window.errors = [{ message: message, timeStamp: new Date() }, ...window.errors.filter((v, i) => i < 9)];

            setState({ display: 'block' });
            window.localStorage['_errors'] = JSON.stringify(window.errors);
        };
        //setState({ display: 'block' });
        return () => window.alert = backup;
    }, []);

    const close = () => setState({ ...state, display: 'none' });
    const overlayStyle = { display: state.display, background: '#000000CC', position: 'fixed', box_sizing: 'border-box', inset: 0, width: '100vw', height: '100vh', background_color: 'rgba(0, 0, 0, 0.85)', color: 'rgb(232, 232, 232)', font_family: 'Menlo, Consolas, monospace', font_size: 'large', padding: '2rem', line_height: 1.2, white_space: 'pre-wrap', overflow: 'auto' };
    return <div onClick={close} onKeyDown={close} id="error-overlay-div" style={overlayStyle}>
        <span>Se encontraron algunos problemas durante la ejecución del programa:</span>
        <table>
            {(window.errors || []).map((v, i, a) => (v.message ? <ErrorEntry key={`error_entry_${i}`} {...v} /> : null))}
        </table>
    </div>;
};