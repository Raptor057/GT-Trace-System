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
        console.debug(method, url);
        return fetch(url, getOptions(method, data))
            .then(response => {
                console.debug(response);
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

export const MaterialLoadingApi = (function (apiUrl) {
    //apiUrl = 'http://localhost:5183';
    return {
        getLine: (lineCode) =>
            HttpRequest.get(`${apiUrl}/api/lines/${lineCode}`)
    };
})("http://mxsrvapps.gt.local/gtt/services/materialloading");

export const PackagingApi = (function (apiUrl) {
    apiUrl = 'http://localhost:5270'; //Este se usa cuando se ejecuta la api con debug de manera local
    return {
        JoinFramelessMotors: (scannerInputUnitID,scannerInputComponentID,lineCode,partNo) =>
        HttpRequest.post(`${apiUrl}/api/JoinFramelessMotors/`,{ ScannerInputUnitID: scannerInputUnitID, ScannerInputComponentID: scannerInputComponentID, LineCode: lineCode, PartNo: partNo }),
        
        JoinEZMotors: (scannerInputUnitID,scannerOutputMotorID1,scannerOutputMotorID2,isEnable) =>
        HttpRequest.post(`${apiUrl}/api/JoinEZMotors/`,{ ScannerInputUnitID: scannerInputUnitID, ScannerOutputMotorID1: scannerOutputMotorID2,ScannerOutputMotorID2: scannerOutputMotorID1,IsEnable: isEnable}),
        
        JoinPallet: (scannerInputUnitID,scannerInputPalletID,lineCode,isEnable) =>
        HttpRequest.post(`${apiUrl}/api/PalletQR/`,{ ScannerInputUnitID: scannerInputUnitID, ScannerInputPalletID: scannerInputPalletID,LineCode: lineCode,IsEnable: isEnable})
    };
})("http://mxsrvapps.gt.local/gtt/services/packaging");