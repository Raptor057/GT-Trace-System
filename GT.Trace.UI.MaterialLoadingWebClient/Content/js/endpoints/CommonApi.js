const commonApi = (function (apiUrl) {
    //apiUrl = 'http://localhost:5160';
    return {
        getLineMaterialStatus: (lineCode, partNo, onSuccess, onFailure) =>
            httpGet(`${apiUrl}/api/lines/${lineCode}/etis/${partNo}`, onSuccess, onFailure),

        updateLineBom: (partNo, lineCode, onSuccess) =>
        //httpPut(`${apiUrl}/api/lines/updategama/partno/${partNo}/lineCode/${lineCode}`, onSuccess),
            httpPut(`${apiUrl}/api/lines/updategama/partno/${partNo}/${partNo}/lineCode/${lineCode}/${lineCode}`, onSuccess),
    };
})('http://mxsrvapps.gt.local/gtt/services/common');