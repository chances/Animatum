/**
 * Created with JetBrains PhpStorm.
 * User: Chance Snow
 * Date: 5/27/12
 * Time: 11:05 AM
 *
 * Animatum Timeline KeyframeArea Object KeyframeArea.js JavaScript Document
 */

function KeyframeArea() {

	this.addKeyframeRow = function (bone) {
		var row = $('<div class="keyframe-row" data-bone="' + bone.Name + '">');
		$('#keyframes').append(row);

		if (bone.Animation !== null && bone.Animation !== undefined) {
			if (bone.Animation.length > 1) {
				$(row).append($('<span class="line">'));
				if ($(row).is(':first-child')) { $(row).find('.line').css('top', '32px'); }
			}
			for (var i in bone.Animation) {
				var ani = bone.Animation[i];
				var x = ani.Transformation.X;
				var y = ani.Transformation.Y;
				var z = ani.Transformation.Z;
				var trans = x + ":" + y + ":" + z;
				var keyframe = $('<span class="keyframe" data-time="' + ani.Time + '" data-type="' + ani.Type + '" data-transform="' + trans + '">');
				$(row).append(keyframe);
				$(keyframe).click(function () {
					keyframeArea.selectKeyframe(this);
				});
			}
		}
		adjustMetrics();
	};

	this.addKeyframe = function (bone) {
		var row = $('#keyframes div[data-bone=' + bone.Name + ']')[0];
		if ($(row).find('.keyframe').size() === 1) {
			$(row).append($('<span class="line">'));
			if ($(row).is(':first-child')) { $(row).find('.line').css('top', '32px'); }
		}
		var keyframe = $('<span class="keyframe" data-time="0" data-type="0" data-transform="0:0:0">');
		$(row).append(keyframe);
		this.selectKeyframe(keyframe);
		$(keyframe).click(function () {
			keyframeArea.selectKeyframe(this);
		});

		var newKeyframe = new Keyframe().fromTimelineKeyframe(keyframe);
		model.createKeyframe(bone.Name, newKeyframe);
		checkPlaybackEnabled();
	};

	this.updateKeyframe = function (keyframe) {
		var bone = model.bones.get( $(keyframe).parent().attr('data-bone') );
		var index = this.getKeyframeIndex(bone, keyframe);
		keyframe = new Keyframe().fromTimelineKeyframe(keyframe);
		var trans = keyframe.Transform.x + ":" + keyframe.Transform.y + ":" + keyframe.Transform.z;
		model.setKeyframe(bone.Name, index, keyframe.Time, keyframe.Type, trans);
		adjustMetrics();
		checkForConflicts();
	};

	this.deleteKeyframe = function (keyframe) {
		var bone = model.bones.get( $(keyframe).parent().attr('data-bone') );
		model.deleteKeyframe(bone.Name, this.getKeyframeIndex(bone, keyframe));
		//Remove line if deleting down to one keyframe
		var numKeyframes = $(keyframe).parent().find('.keyframe').size();
		if (numKeyframes === 2) {
			$(keyframe).parent().find('.line').remove();
		} else if (numKeyframes === 1) {
			//Disable clear button
			$('#clear').attr('disabled','');
		}
		$(keyframe).remove();
		selectedKeyframe = null;
		//Hide keyframe info
		$('#keyframeInfo').hide();
		$('#message').show();
		//Disable delete button
		$('#delete').attr('disabled','');
		adjustMetrics();
		checkPlaybackEnabled();
	};

	this.selectKeyframe = function (keyframe) {
		$('#message').hide();
		$('#keyframeInfo').show();
		var bone = model.bones.get( $(keyframe).parent().attr('data-bone') );
		$('#keyframes .keyframe').removeClass('selected sel');
		$(keyframe).addClass('selected');
		//Select the row it's on
		$('#boneList li').removeClass('selected').css({color: '', backgroundColor: ''});
		$('#boneList li').each(function () {
			if ($(this).text() === bone.Name) {
				$(this).addClass('selected').css({
					color: ext.getSystemColor('highlightText'),
					backgroundColor: ext.getSystemColor('highlight')
				});
			}
		});
		//Enable add button
		$('#add').removeAttr('disabled');
		//Enable delete button
		$('#delete').removeAttr('disabled');
		//Enable clear button
		$('#clear').removeAttr('disabled');
		selectedKeyframe = {bone: bone.Name, index: this.getKeyframeIndex(bone, keyframe)};
		checkForConflicts();
		//Show keyframe props
		$('#bone').text(bone.Name);
		var time = parseFloat( $(keyframe).attr('data-time') );
		var apiKeyframe = new Keyframe().fromTimelineKeyframe(keyframe);
		$('#time').text(apiKeyframe.Time);
		if (apiKeyframe.Type === 0) {
			$('#translation').attr('checked','');
			$('#rotation').removeAttr('checked');
		} else {
			$('#translation').removeAttr('checked');
			$('#rotation').attr('checked','');
		}
		updateUnitLabels();
		var transform = apiKeyframe.Transform;
		$('#x').val(transform.x);
		$('#y').val(transform.y);
		$('#z').val(transform.z);
	};

	this.getKeyframeIndex = function (bone, keyframe) {
		return $('div[data-bone=' + bone.Name + '] .keyframe').index(keyframe);
	};
}