const etiMovementsApi = (function (apiUrl) {
    //apiUrl = 'http://localhost:5072';
    return {
        loadEti: (pointOfUseCode, etiNo, lineCode, operatorNo, partNo, workOrderCode, onSuccess, onFailure) =>
            httpPost(
                `${apiUrl}/api/lines/${lineCode}/etis`,
                { EtiInput: etiNo, PointOfUseCode: pointOfUseCode },
                onSuccess, onFailure),

        unloadEti: (pointOfUseCode, etiNo, lineCode, operatorNo, onSuccess, onFailure) =>
            httpDel(
                `${apiUrl}/api/lines/${lineCode}/etis`,
                { EtiInput: etiNo, IsReturn: false },
                onSuccess, onFailure),

        getEtiInfo: (etiNo, onSuccess) =>
            httpGet(`${apiUrl}/api/etis/${etiNo}`, onSuccess),
    };
})('http://mxsrvapps.gt.local/gtt/services/etimovements');