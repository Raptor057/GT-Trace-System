const ErrorEntry = ({ title, description }) => (<div><span style={{ color: 'rgb(227, 96, 73)' }} >{title}</span><br /><br /><div>{description}</div><br /><br /></div>);

const ErrorOverlay = ({ errorList }) => {
    const handleCloseButtonClick = (e) => e.target.parentElement.parentElement.removeChild(e.target.parentElement);
    const overlayStyle = { background: '#000000CC', position: 'fixed', box_sizing: 'border-box', inset: 0, width: '100vw', height: '100vh', background_color: 'rgba(0, 0, 0, 0.85)', color: 'rgb(232, 232, 232)', font_family: 'Menlo, Consolas, monospace', font_size: 'large', padding: '2rem', line_height: 1.2, white_space: 'pre-wrap', overflow: 'auto' };
    const buttonStyle = { background: 'transparent', border: 'none', font_size: '20px', font_weight: 'bold', color: 'white', cursor: 'pointer', float: 'right' };
    return (<div id="error-overlay-div" style={overlayStyle}><span>Se encontraron algunos problemas durante la ejecución del programa:</span><button onClick={handleCloseButtonClick} style={buttonStyle}>X</button><br /><br />{errorList.map((v, i, a) => <ErrorEntry key={`error_entry_${i}`} title={v.title} description={v.description} />)}</div>);
};