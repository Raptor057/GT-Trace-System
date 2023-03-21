const ChangeoverApi = (function (apiUrl) {
    apiUrl = 'http://localhost:5063';
    return {
        getLine: (lineCode, onSuccess, onFailure) =>
            HttpRequest.get(`${apiUrl}/api/lines/${lineCode}`, onSuccess, onFailure),
        getWorkOrderByLineID: (lineID, onSuccess, onFailure) =>
            HttpRequest.get(`${apiUrl}/api/workorders?lineID=${lineID}`, onSuccess, onFailure),
        getGamma: (lineCode, partNo, onSuccess, onFailure) =>
            HttpRequest.get(`${apiUrl}/api/lines/${lineCode}/gamma/${partNo}`, onSuccess, onFailure),
        applyChangeover: (lineCode, onSuccess, onFailure) =>
            HttpRequest.put(`${apiUrl}/api/lines/${lineCode}/codew`, { }, onSuccess, onFailure),
    };
})('http://mxsrvapps/gtt/services/packaging');