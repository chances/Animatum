/**
 * Created with JetBrains PhpStorm.
 * User: Chance Snow
 * Date: 5/23/12
 * Time: 1:00 AM
 *
 * Animatum Timeline api.js JavaScript Document
 */

function API() {
	var aniApi = window.external;

	this.colors = {
		getSystemColor: function (name) { return aniApi.getSystemColor(name); }
	};
}