/**
 * Created with JetBrains PhpStorm.
 * User: Chance Snow
 * Date: 5/23/12
 * Time: 10:36 AM
 *
 * Animatum Timeline Model Object Model.js JavaScript Document
 */

function Model(model) {
	var appModel = model;
	var that = this;

	this.update = function () {
		window.external.setModel(jQuery.stringify(appModel));
	};

	this.bones = {
		length: model.Bones.length,
		get: function (query) {
			if (jQuery.isNumeric(query)) {
				if (query > -1 && query < appModel.Bones.length) {
					return appModel.Bones[query];
				} else {
					return null;
				}
			} else {
				return that.getBoneByName(query);
			}
		},
		set: function (bone) {
			for (var i in appModel.Bones) {
				if (appModel.Bones[i].Name === bone.name) {
					appModel.Bones[i] = bone.appBone;
					that.update();
					break;
				}
			}
		}
	};

	this.hasChildren = appModel.HasChildren;
	this.children = appModel.Children;
	this.currentTime = window.external.getCurrentTime();

	//Get root bones
	this.rootBones = [];
	for (var i in this.children) {
		var child = this.children[i];
		if (child.Type === "bone") {
			this.rootBones.push(child);
		}
	}

	this.getBoneByName = function (name) {
		for (var i in appModel.Bones) {
			if (appModel.Bones[i].Name === name) {
				return appModel.Bones[i];
			}
		}
		return null;
	};

	this.createKeyframe = function(boneName, keyframe) {
		var trans = keyframe.Transform.x + ":" + keyframe.Transform.y + ":" + keyframe.Transform.z;
		window.external.createKeyframe(boneName, keyframe.Time, keyframe.Type, trans);
	};

	this.getKeyframe = function (boneName, index) {
		return window.external.getKeyframe(boneName, index);
	};

	this.setKeyframe = function (boneName, index, time, type, transform) {
		window.external.setKeyframe(boneName, index, time, type, transform);
	};

	this.deleteKeyframe = function (boneName, index) {
		window.external.deleteKeyframe(boneName, index);
	};

	this.getKeyframesByTime = function (bone, time) {
		var keyframes = [];
		for (var i in bone.Animation) {
			if (bone.Animation[i].Time === time) {
				keyframes.push(bone.Animation[i]);
			}
		}
		return keyframes;
	};
}