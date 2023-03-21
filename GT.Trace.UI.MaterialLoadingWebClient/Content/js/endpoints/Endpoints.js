const headers = new Headers();
headers.append('Content-Type', 'application/json');

const getOptions = (method, data = null) => {
    const options = ({ method: method, headers: headers, mode: 'cors' });
    return data == null ? options : { ...options, body: JSON.stringify(data) }
}

const httpRequest = (method, url, onSuccess, data = null, onFailure = null) => {
    fetch(url, getOptions(method, data))
        .then(response => {
            return response.json();
        })
        .then((jsonResponse) => {
            console.log(url, jsonResponse);
            if (jsonResponse.hasOwnProperty('status') && jsonResponse.status != 200) {
                let message = jsonResponse.title;
                for (index in jsonResponse.errors) {
                    message += `\n- ${jsonResponse.errors[index]}`;
                }
                console.log('INFO', message);
                alert(message);
                if (onFailure != null) onFailure(message);
            }
            else {
                if (jsonResponse.isSuccess) {
                    onSuccess(jsonResponse.data);
                }
                else {
                    console.error('ERROR', jsonResponse.message);
                    alert(jsonResponse.message);
                    if (onFailure != null) onFailure(jsonResponse.message);
                }
            }
        },
            (error) => {
                console.error('CRITICAL', error);
                alert(`${error.message}: ${url}`);
                //if (onFailure != null) onFailure(error.message);
            }
        )
};

const httpGet = (url, onSuccess, onFailure = null) => httpRequest('GET', url, onSuccess, null, onFailure);
const httpPut = (url, data, onSuccess) => httpRequest('PUT', url, onSuccess, data);
const httpPost = (url, data, onSuccess, onFailure = null) => httpRequest('POST', url, onSuccess, data, onFailure);
const httpDel = (url, data, onSuccess, onFailure = null) => httpRequest('DELETE', url, onSuccess, data, onFailure);

//apiServer = 'http://localhost:5082';