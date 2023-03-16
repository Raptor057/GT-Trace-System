//1 endpoint were added in the Packaging api
//1) set a line lock and unlock when the assembly UI scans for something wrong.
//    Date: 3 / 10 / 2023
        
const PackagingApi = (function (apiUrl) {
    //apiUrl = 'http://localhost:5183';
    return {
        SetStationBlocked: (is_blocked, lineName, onSuccess) =>
            httpPut(`${apiUrl}/api/StationBlocked/${is_blocked}/${lineName}`, {}, onSuccess),
    };
})('http://mxsrvapps.gt.local/gtt/services/packaging');