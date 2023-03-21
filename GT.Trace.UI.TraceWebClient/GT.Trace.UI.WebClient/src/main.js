import App from './App.svelte';

const urlSearchParams = new URLSearchParams(window.location.search);
const params = Object.fromEntries(urlSearchParams.entries());

const app = new App({
	target: document.body,
	props: {
		lineCode: params["station"] //<== Aqui se define el parametro que se quiere establecer en el navegador despues de la direccion del servidor.
	}
});

export default app;