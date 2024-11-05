const handleRejectedResponse = async (error) => {
    console.error(error);
    let message = error.message || `${error.status}: ${error.statusText}`;

    const processJson = (json) => {
        console.debug("JSON error from API", json);
        if (json.hasOwnProperty('errors')) {
            let message = json.title;
            for (let index in json.errors) {
                message += `\n- ${json.errors[index]}`;
            }
            return message;
        }
        return json.message;
    };

    const processText = (text) => {
        console.debug("Text error from API", text);
        return text;
    };

    if (typeof error.json === "function") {
        let isJSON = error.headers.get('content-type').includes('application/json');
        message = await (isJSON ? error.json().then(processJson) : error.text().then(processText)).catch(async genericError => {
            console.debug("Generic error from API", genericError);
            return `${error.status}: ${error.statusText}`;
        });
    }
    return Promise.reject(message);
};

const getOptions = (method, data = null) => {
    const headers = { "Access-Control-Expose-Headers": "Content-Length", "Content-Type": "application/json" };
    const options = ({ method: method, headers: headers, mode: 'cors' });
    return data == null ? options : { ...options, body: JSON.stringify(data) }
}

const HttpRequest = (function () {
    const httpRequest = async (method, url, data = null) => {
        console.debug("method", method);
        console.debug("url", url);
        console.debug("data", data);
        return fetch(url, getOptions(method, data))
            .then(response => {
                console.debug("response", response);
                if (!response.ok) {
                    return Promise.reject(response);
                }
                return response.json();
            })
            .then ((json) => json.data)
            .catch (handleRejectedResponse);
    };
    return {
        get: async (url) => httpRequest('GET', url),
        put: async (url, data) => httpRequest('PUT', url, data),
        post: async (url, data) => httpRequest('POST', url, data),
        delete: async (url, data) => httpRequest('DELETE', url, data),
    };
})();

export const ChangeoverApi = (function (apiUrl) {
    //apiUrl = 'http://localhost:5063';
    return {
        getLine: (lineCode) =>
            HttpRequest.get(`${apiUrl}/api/lines/${lineCode}`),
        getWorkOrderByLineID: (lineID) =>
            HttpRequest.get(`${apiUrl}/api/workorders?lineID=${lineID}`),
        getGamma: (lineCode, partNo) =>
            HttpRequest.get(`${apiUrl}/api/lines/${lineCode}/gamma/${partNo}`),
        applyChangeover: (lineCode) =>
            HttpRequest.put(`${apiUrl}/api/lines/${lineCode}/codew`, { }),
    };
})("http://mxsrvapps.gt.local/gtt/services/changeovers");

export const CommonApi = (function (apiUrl) {
    //apiUrl = 'http://localhost:5160';
    return {
        UpdateGama: async (ogpartNo,icpartNo,oglineCode,iclineCode) =>
            //HttpRequest.put(`${apiUrl}/api/lines/updategama/partno/${partNo}/lineCode/${lineCode}`),
        HttpRequest.put(`${apiUrl}/api/lines/updategama/partno/${ogpartNo}/${icpartNo}/lineCode/${oglineCode}/${iclineCode}`),

        DeleteGama: async (ogpartNo,icpartNo,oglineCode,iclineCode) =>
        HttpRequest.put(`${apiUrl}/api/lines/deletegama/partno/${ogpartNo}/${icpartNo}/lineCode/${oglineCode}/${iclineCode}`),
    };
})("http://mxsrvapps/gtt/services/common");