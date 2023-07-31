import { writable } from 'svelte/store';
	
export let studentsArray = writable([
	{
		"Name" : "Maria",
		"Favorite Subject" : "Math",
		"Age" : 14
	},
	{
		"Name" : "Jose",
		"Favorite Subject" : "Science",
		"Age" : 13
	},
]);

export let colors = writable([
	{
		"Name" : "Cyan",
		"HEX" : "#00FFFF"
	},
	{
		"Name" : "Yellow",
		"HEX" : "#FFFF00"
	},
	{
		"Name" : "Blue",
		"HEX" : "#0000FF"
	},
	{
		"Name" : "Lime",
		"HEX" : "#00FF00"
	},
	{
		"Name" : "Red",
		"HEX" : "#FF0000"
	},
	{
		"Name" : "White",
		"HEX" : "#FFFFFF"
	},
	{
		"Name" : "Black",
		"HEX" : "#000000"
	},
])

