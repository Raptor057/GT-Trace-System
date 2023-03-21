const urlSearchParams = new URLSearchParams(window.location.search);
const params = Object.fromEntries(urlSearchParams.entries());
if (params.hasOwnProperty('line')) {
    ReactDOM.render(<App lineCode={params['line']} />, document.querySelector('#app'));
}
else {
    ReactDOM.render(<React.Fragment>
        <p>El parámetro <b>line</b> no se encuentra presente en la dirección URL:</p>
        <code style={{ padding: '1em' }}>{window.location.origin}<b>?line=LA</b></code>
    </React.Fragment>, document.querySelector('#app'));
}