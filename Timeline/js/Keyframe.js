/**
 * Created with JetBrains PhpStorm.
 * User: Chance Snow
 * Date: 5/23/12
 * Time: 11:06 PM
 *
 * Animatum Timeline Keyframe Interop Object Keyframe.js JavaScript Document
 */

function Keyframe(keyframe) {
	this.appKeyframe = keyframe;
	if (keyframe !== null && keyframe !== undefined) {
		this.time = keyframe.Time;
		this.type = keyframe.Type;
		this.transform =
		{
			x: keyframe.Transformation.X,
			y: keyframe.Transformation.Y,
			z: keyframe.Transformation.Z
		};
	}

	this.fromTimelineKeyframe = function (keyframe) {
		var time = $(keyframe).attr('data-time');
		time = parseFloat(time);
		var type = $(keyframe).attr('data-type');
		type = parseInt(type, 10);
		var coords = $(keyframe).attr('data-transform').split(':');
		var x = parseFloat(coords[0]);
		var y = parseFloat(coords[1]);
		var z = parseFloat(coords[2]);

		this.time = time;
		this.type = type;
		this.transform = {
			x: 0, y: 0, z: 0
		};
		this.transform.x = x;
		this.transform.y = y;
		this.transform.z = z;

		return {Time: time, Type: type, Transform: this.transform};
	};
}