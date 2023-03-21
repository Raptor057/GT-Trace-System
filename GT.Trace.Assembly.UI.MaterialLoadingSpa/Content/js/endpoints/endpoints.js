const headers = new Headers();
headers.append('Content-Type', 'application/json');

const getOptions = (method, data = null) => {
    const options = ({ method: method, headers: headers, mode: 'cors' });
    return data == null ? options : { ...options, body: JSON.stringify(data) }
}

const httpRequest = (method, url, onSuccess, data = null) => {
    fetch(url, getOptions(method, data))
        .then(response => response.json())
        .then((jsonResponse) => {
            console.log(url, jsonResponse);
            if (jsonResponse.hasOwnProperty('status') && jsonResponse.status != 200) {
                let message = jsonResponse.title;
                for (index in jsonResponse.errors) {
                    message += `\n- ${jsonResponse.errors[index]}`;
                }
                // `${jsonResponse.title}:${Object.keys(jsonResponse.errors).map(key => `\n\t- ${key}: ${jsonResponse.errors[key]}`)}`
                alert(message);
                console.error(message);
            }
            else {
                if (jsonResponse.isSuccess)
                    onSuccess(jsonResponse.data);
                else
                    alert(jsonResponse.message);
            }
        },
            (error) => {
                console.error('failure');
                alert(error);
            }
        )
};

const httpGet = (url, onSuccess, onFailure) =>
    httpRequest('GET', url, onSuccess, null);
    //fetch(url)
    //    .then(response => response.json())
    //    .then(jsonResponse => {
    //        if ('status' in jsonResponse && jsonResponse.status != 200) {
    //            console.log('ERRORS');
    //            onFailure(`${jsonResponse.title}:${Object.keys(jsonResponse.errors).map(key => `\n\t- ${key}: ${jsonResponse.errors[key]}`)}`);
    //            return;
    //        }

    //        if (jsonResponse.isSuccess) {
    //            onSuccess(jsonResponse.data);
    //        }
    //        else {
    //            onFailure(jsonResponse.message);
    //        }
    //    });

const httpPost = (url, data, onSuccess, onFailure) =>
    fetch(url, {
        method: 'POST',
        mode: 'cors',
        cache: 'no-cache',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    })
        .then(response => response.json())
        .then(jsonResponse => {
            if (jsonResponse.isSuccess) {
                console.log('HTTP Response', jsonResponse.data);
                onSuccess(jsonResponse.data);
            }
            else {
                onFailure(jsonResponse.message);
            }
        });

const httpDelete = (url, onSuccess, onFailure) =>
    fetch(url, {
        method: 'DELETE',
        mode: 'cors',
        cache: 'no-cache',
        headers: { 'Content-Type': 'application/json' }
    })
        .then(response => response.json())
        .then(jsonResponse => {
            if ('status' in jsonResponse && jsonResponse.status != 200) {
                onFailure(`${jsonResponse.title}:${Object.keys(jsonResponse.errors).map(key => `\n\t- ${key}: ${jsonResponse.errors[key]}`)}`);
                return;
            }

            if (jsonResponse.isSuccess) {
                onSuccess(jsonResponse.data);
            }
            else {
                onFailure(jsonResponse.message);
            }
        });

const apiServer = 'http://mxsrvapps.gt.local/gtt/services/materialloading';
//const apiServer = 'http://localhost:5082';

const fetchLines = (onSuccess, onFailure) =>
    httpGet(`${apiServer}/api/lines`, onSuccess, onFailure);

const fetchLineWorkOrders = (lineID, onSuccess, onFailure) =>
    httpGet(`${apiServer}/api/lines/${lineID}/workorders`, onSuccess, onFailure);

const fetchPointOfUseLines = (pointOfUseCode, onSuccess, onFailure) =>
    httpGet(`${apiServer}/api/pointsofuse/${pointOfUseCode}/lines`, onSuccess, onFailure);

const fetchPointOfUseEtis = (pointOfUseCode, onSuccess, onFailure) => {
    console.log('fetchPointOfUseEtis', pointOfUseCode);
    httpGet(`${apiServer}/api/pointsofuse/${pointOfUseCode}/etis`, onSuccess, onFailure);
}

const fetchEtiPointsOfUse = (etiNo, partNo, lineCode, onSuccess, onFailure) =>
    httpGet(`${apiServer}/api/etis/${etiNo}/pointsofuse?lineCode=${lineCode}&partNo=${partNo}`, onSuccess, onFailure);

const loadEti = (pointOfUseCode, etiNo, lineCode, operatorNo, partNo, workOrderCode, onSuccess, onFailure) =>
    httpPost(
        `${apiServer}/api/pointsofuse/${pointOfUseCode}/etis`,
        { lineCode: lineCode, etiNo: etiNo, operatorNo, operatorNo, partNo, workOrderCode },
        onSuccess, onFailure);

const unloadEti = (pointOfUseCode, etiNo, lineCode, operatorNo, onSuccess, onFailure) =>
    httpDelete(
        `${apiServer}/api/pointsofuse/${pointOfUseCode}/etis/${etiNo}?operatorNo=${operatorNo}&lineCode=${lineCode}`,
        onSuccess, onFailure);

const getLine = (lineCode, onSuccess) =>
    httpGet(`${apiServer}/api/lines/${lineCode}`, onSuccess, alert);

const getLinePointsOfUse = (lineCode, onSuccess) =>
    httpGet(`http://mxsrvapps.gt.local/gtt/services/common/api/lines/${lineCode}/pointsofuse`, onSuccess, alert);