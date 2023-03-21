const HttpRequest = (function () {
    const headers = new Headers();
    headers.append('Content-Type', 'application/json');

    const getOptions = (method, data = null) => {
        const options = ({ method: method, headers: headers, mode: 'cors' });
        return data == null ? options : { ...options, body: JSON.stringify(data) }
    }

    const httpRequest = (method, url, onSuccess, onFailure, data = null) => {
        fetch(url, getOptions(method, data))
            .then(response => response.json())
            .then((jsonResponse) => {
                console.log(url, jsonResponse);
                if (jsonResponse.hasOwnProperty('status') && jsonResponse.status != 200) {
                    onFailure ? onFailure(jsonResponse.errors) : console.error('Unhandled Exception', `${jsonResponse.title}${jsonResponse.errors.reduce((prev, curr) => `${prev}\n-${curr}`, '')}`);
                }
                else {
                    if (jsonResponse.isSuccess) {
                        onSuccess(jsonResponse.hasOwnProperty('data') ? jsonResponse.data : null);
                    }
                    if (jsonResponse.hasOwnProperty('errors')) {
                        onFailure ? onFailure(jsonResponse.errors) : console.error('Unhandled Exception', jsonResponse.errors.reduce((prev, curr) => `${prev}\n-${curr}`, ''));
                    }
                }
            },
                (error) => {
                    onFailure ? onFailure(error.message) : console.error('Unhlandled Error', error.messsage);
                }
            )
    };
    return {
        get: (url, onSuccess, onFailure) => httpRequest('GET', url, onSuccess, onFailure),
        put: (url, data, onSuccess, onFailure) => httpRequest('PUT', url, onSuccess, onFailure, data),
        post: (url, data, onSuccess, onFailure) => httpRequest('POST', url, onSuccess, onFailure, data),
        delete: (url, data, onSuccess, onFailure) => httpRequest('DELETE', url, onSuccess, onFailure, data),
    };
})();