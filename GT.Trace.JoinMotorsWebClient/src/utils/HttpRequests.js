/*
Esta clase es un módulo que utiliza la API fetch de JavaScript para hacer solicitudes HTTP a un servidor y procesar las respuestas.
La primera función handleRejectedResponse se encarga de procesar los errores de respuesta de la solicitud HTTP. 
Toma como parámetro el objeto error que representa la respuesta HTTP con errores. 
La función imprime el error en la consola, y luego construye un mensaje de error a partir del status y statusText de la respuesta HTTP. 
Luego, si la respuesta contiene datos JSON, la función llama a la función processJson para procesar los datos JSON. 
Si la respuesta no es JSON, llama a la función processText para procesar el texto. 
Si la función processJson encuentra errores en la respuesta JSON, construye un mensaje de error más detallado. 
En última instancia, la función devuelve una promesa rechazada con el mensaje de error.
La segunda función getOptions construye un objeto de opciones de solicitud HTTP. 
Toma como parámetros el método HTTP (por ejemplo, GET, PUT, POST o DELETE) y los datos de la solicitud en formato JSON (opcional). 
La función construye un objeto de encabezados con un tipo de contenido JSON y otros encabezados personalizados necesarios. 
Luego construye un objeto de opciones con el método, los encabezados y el modo "cors". Si se proporcionan datos, los agrega al objeto de opciones como el cuerpo de la solicitud.
La clase HttpRequest contiene un objeto httpRequest que es una función asíncrona que toma como parámetros el método HTTP, la URL y los datos (opcional) de la solicitud. 
La función utiliza la API fetch para realizar una solicitud HTTP con los parámetros proporcionados y devuelve una promesa. 
Si la respuesta es exitosa, la función devuelve los datos de respuesta en formato JSON. 
Si la respuesta tiene errores, la función rechaza la promesa y llama a la función handleRejectedResponse para procesar los errores.
La clase HttpRequest también contiene cuatro métodos que envuelven httpRequest y le proporcionan una abstracción de nivel superior para realizar solicitudes HTTP. 
Los métodos get, put, post y delete toman la URL y los datos (opcional) de la solicitud y llaman a httpRequest con los parámetros adecuados para el método HTTP correspondiente. 
Cada método devuelve una promesa que resuelve los datos de respuesta en formato JSON o la rechaza si la solicitud tiene errores.
*/
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

export const JoinMotors = (function (apiUrl) {
    //apiUrl = 'https://localhost:7274';
    return {
        Join: (ezLabel,motor1,motor2) =>
            HttpRequest.post(`${apiUrl}/api/JoinMotorsEZ2000/${ezLabel}/${motor1}/${motor2}`),
    };
})("http://mxsrvapps.gt.local/gtt/services/JoinMotors");