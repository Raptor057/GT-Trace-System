const commonApi = (function (apiUrl) {
    //apiUrl = 'http://localhost:5160';
    return {
        getLineMaterialStatus: (lineCode, partNo, onSuccess, onFailure) =>
            httpGet(`${apiUrl}/api/lines/${lineCode}/etis/${partNo}`, onSuccess, onFailure),
    };
})('http://mxsrvapps.gt.local/gtt/services/common');