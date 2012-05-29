/**
 * Created with JetBrains PhpStorm.
 * User: Chance Snow
 * Date: 5/27/12
 * Time: 11:00 AM
 *
 * Animatum Timeline BoneList Object BoneList.js JavaScript Document
 */

function BoneList() {

	this.addBonesToList = function (bones, ul, level) {
		for (var i in bones) {
			var bone = bones[i];
			var boneLi = $('<li>' + bone.Name + '</li>');
			$(ul).append(boneLi);
			keyframeArea.addKeyframeRow(bone);
			if (bone.Children.length > 0) {
				var l = level + 1;
				this.addBonesToList(bone.Children, ul, l);
			}
			if (level > 0) {
				$(boneLi).css('margin-left', level * 12 + "px");
			}
			$(boneLi).click(function () {
				var name = $(this).text();
				var bone = model.bones.get(name);
				if (bone !== null) {
					$('#boneList li').removeClass('selected').css({color: '', backgroundColor: ''});
					$(this).addClass('selected').css({
						color: ext.getSystemColor('highlightText'),
						backgroundColor: ext.getSystemColor('highlight')
					});
					//Enable add button
					$('#add').removeAttr('disabled');
					var numKeyframes = $('.keyframe-row[data-bone=' + bone.Name + ']').find('.keyframe').size();
					if (numKeyframes > 0) {
						//Enable clear button
						$('#clear').removeAttr('disabled');
					}
				}
			});
		}
	};
}