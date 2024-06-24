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
            HttpRequest.get(`${apiUrl}/api/lines/${lineCode}`),
        /**
         * Poka Yoke implemented to block the lines requested as a corrective action for 8D ACIN-2223-005.
        2 endpoints were added in the Packaging api
        1) set a line lock and unlock when the assembly UI scans for something wrong.
        2) supervisor password validation.
        */
        getEtiPointsOfUse: async (etiNo,lineCode,partNo ) =>
        HttpRequest.get(`${apiUrl}/api/etis/${etiNo}/pointsofuse?lineCode=${lineCode}&partNo=${partNo}`),      

    };
})("http://mxsrvapps.gt.local/gtt/services/materialloading");

export const ProcessHistoryApi = (function (apiUrl) {
    apiUrl = 'http://localhost:5270';
    return {
        UpdateEtis: async (EtiNo) =>
            HttpRequest.post(`${apiUrl}/api/eti/${EtiNo}`),

        SaveElectricalMotors: async (ProductionID,LineCode) =>
//        HttpRequest.post(`${apiUrl}/api/ProductionID/${ProductionID}/Line/${LineCode}`),
        HttpRequest.post(`${apiUrl}/api/ProductionID/${ProductionID}/Line/${LineCode}`),
        ///api/ProductionID/{ProductionID}/Line/{LineCode}

        RecordProcessNo: async (UnitID,LineCode) =>
        //HttpRequest.post(`${apiUrl}/api/units/{unitID}/lines/${LineCode}/processes/${UnitID}`),
        HttpRequest.post(`${apiUrl}/api/UnitID/${UnitID}/Line/${LineCode}`),
        ///api/UnitID/{unitID}/Line/{LineCode}
    };
})("http://mxsrvapps/gtt/services/processhistory");