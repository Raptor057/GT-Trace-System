const ErrorEntry = ({ message, timeStamp }) => {
    return <tr>
        <td style={{ color: 'rgb(227, 96, 73)', whiteSpace: 'nowrap', minWidth: '6em', width: '6em', verticalAlign: 'top' }} >{(new Date(timeStamp)).toLocaleTimeString()}</td>
        <td style={{ verticalAlign: 'top' }}>{message.split('\n').filter((item) => item).map((item) => <div>{item}</div>)}</td>
    </tr>;
};