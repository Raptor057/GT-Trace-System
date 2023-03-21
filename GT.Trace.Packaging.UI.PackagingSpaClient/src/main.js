import App from "./components/App.svelte";

const urlSearchParams = new URLSearchParams(window.location.search);
const params = Object.fromEntries(urlSearchParams.entries());

const app = new App({
	target: document.body,
	props: {
		hostname: params["line"]
	}
});

export default app;