const ErrorEntry = ({ title, description }) => <div>
    <span style={{ color: 'rgb(227, 96, 73)' }} >{title}</span>&nbsp;<span>{description}</span><br />
</div>;

const ErrorOverlay = ({ errors }) => {
    const [state, setState] = useState('none');
    useEffect(async () => {
        setState(errors && errors.length > 0 ? 'block' : 'none');
    }, [errors]);

    const overlayStyle = { display: state, background: '#000000CC', position: 'fixed', box_sizing: 'border-box', inset: 0, width: '100vw', height: '100vh', background_color: 'rgba(0, 0, 0, 0.85)', color: 'rgb(232, 232, 232)', font_family: 'Menlo, Consolas, monospace', font_size: 'large', padding: '2rem', line_height: 1.2, white_space: 'pre-wrap', overflow: 'auto' };
    return (<div onClick={(e) => setState('none')} id="error-overlay-div" style={overlayStyle}><span>Se encontraron algunos problemas durante la ejecución del programa:</span><br /><br />{errors.map((v, i, a) => <ErrorEntry key={`error_entry_${i}`} title={<span>&bull;</span>} description={v} />)}</div>);
};