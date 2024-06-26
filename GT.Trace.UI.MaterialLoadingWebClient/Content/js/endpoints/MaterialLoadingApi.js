﻿const materialLoadingApi = (function (apiUrl) {
    return {
        getLineWorkOrders: (lineID, onSuccess) =>
            httpGet(`${apiUrl}/api/lines/${lineID}/workorders`, onSuccess),

        getLines: (onSuccess) =>
            httpGet(`${apiUrl}/api/lines`, onSuccess),

        getLineByCode: (lineCode, onSuccess, onFailure) =>
            httpGet(`${apiUrl}/api/lines/${lineCode}`, onSuccess, onFailure),

        getPointOfUseLines: (pointOfUseCode, onSuccess, onFailure) =>
            httpGet(`${apiUrl}/api/pointsofuse/${pointOfUseCode}/lines`, onSuccess, onFailure),

        getEtiPointsOfUse: (etiNo, partNo, lineCode, onSuccess, onFailure) =>
            httpGet(`${apiUrl}/api/etis/${etiNo}/pointsofuse?lineCode=${lineCode}&partNo=${partNo}`, onSuccess, onFailure),

        getPointOfUseEtis: (pointOfUseCode, onSuccess, onFailure) =>
            httpGet(`${apiUrl}/api/pointsofuse/${pointOfUseCode}/etis`, onSuccess, onFailure),
    };
})('http://mxsrvapps.gt.local/gtt/services/materialloading');