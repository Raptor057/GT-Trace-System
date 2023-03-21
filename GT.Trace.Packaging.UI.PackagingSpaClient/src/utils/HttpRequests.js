const handleRejectedResponse = async (error) => {
    let message = error.message || `${error.status}: ${error.statusText}`;
    if (typeof error.json === "function") {
        message = await error.json().then((json) => {
            console.debug("JSON error from API", json);
            if (json.hasOwnProperty('errors')) {
                let message = json.title;
                for (let index in json.errors) {
                    message += `\n- ${json.errors[index]}`;
                }
                return message;
            }
            return json.message;
        }).catch(async genericError => {
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

export const PackagingApi = (function (apiUrl) {
    //apiUrl = 'http://localhost:5183';
    return {
        fetchHourlyProduction: async (lineCode) =>
            HttpRequest.get(`${apiUrl}/api/lines/${lineCode}/hourlyproduction`),
        
        getStationState: (hostname, containerSize, palletSize, lineCode) =>
            HttpRequest.get(`${apiUrl}/api/lines/${hostname}?containerSize=${(containerSize > 0 ? containerSize : '')}&palletSize=${(palletSize > 0 ? palletSize : '')}&lineCode=${(lineCode ? lineCode : '')}`),

        packUnit: (hostname, scannerInput, palletSize, containerSize, poNumber) =>
            HttpRequest.post(`${apiUrl}/api/lines/${hostname}/container`, { ScannerInput: scannerInput, PalletSize: palletSize, ContainerSize: containerSize, PoNumber: poNumber }),

        printWipLabel: (hostname) =>
            HttpRequest.post(`${apiUrl}/api/lines/${hostname}/container/wip`, null),

        printPartialLabel: (hostname) =>
            HttpRequest.post(`${apiUrl}/api/lines/${hostname}/container/partial`, null),

        unpackUnit: (lineCode, scannerInput) =>
            HttpRequest.delete(`${apiUrl}/api/lines/${lineCode}/container`, scannerInput),

        setHeadcount: (lineCode, workOrderCode, value) =>
            HttpRequest.put(`${apiUrl}/api/lines/${lineCode}/workorders/${workOrderCode}/headcount/${value}`),
    };
})("http://mxsrvapps.gt.local/gtt/services/packaging");