/**
 * Created with JetBrains PhpStorm.
 * User: Chance Snow
 * Date: 5/23/12
 * Time: 12:55 AM
 *
 * Animatum Timeline index.js JavaScript Document
 */

var api = null;
var model = null;
var boneList = new BoneList();
var keyframeArea = new KeyframeArea();
var ext = window.external;

var selectedKeyframe = null;

var isMouseDown = false;
var mouseX = null;
var scrollX = 0;

var isPlaying = false;
var animationEnded = false;

$(function () {
	api = new API();

	//Prevent text selection
	$('*').disableTextSelect();

	adjustMetrics();
	$(window).resize(function () { adjustMetrics(); });

	$('#tools, #props, #playback').css('background-color',api.colors.getSystemColor('control'));

	$('#keyframeInfo').hide();
	$('#keyframeList').hide();
	$('#keyframeList').mouseleave(function () {
		$(this).fadeOut(100);
	});

	$('#add').click(function () {
		var bone = model.bones.get($('#boneList li.selected').text());
		keyframeArea.addKeyframe(bone);
		$('#keyframeList').fadeOut(100);
		checkForConflicts();
		checkPlaybackEnabled();
	});

	$('#delete').click(function () {
		var keyframe = $('.keyframe.selected')[0];
		keyframeArea.deleteKeyframe(keyframe);
		$('#keyframeList').fadeOut(100);
		checkForConflicts();
		checkPlaybackEnabled();
	});

	$('#clear').click(function () {
		var bone = model.bones.get( $('#boneList li.selected').text() );
		var msg = "Are you sure you want to clear all key frames belonging to " + bone.Name + "?";
		if (ext.showConfirm("Are you sure?", msg, "no") === true) {
			$('#keyframeList').fadeOut(100);
			$('.keyframe-row[data-bone=' + bone.Name + ']').find('.keyframe').each(function () {
				keyframeArea.deleteKeyframe(this);
				checkPlaybackEnabled();
			});
			$(this).attr('disabled','');
		}
	});

	$('#timeline').scroll(function () {
		$('#scale').stop().animate({'top': $('#timeline').scrollTop() + 'px'}, 100);
	});

	$('#playHead').draggable({
		axis: "x",
		containment: "#scale",
		scroll: true,
		drag: function () {
			var left = $(this).position().left;
			var scrollLeft = $('#keyframeArea').scrollLeft();
			left = (left + scrollLeft);
			$('#playHeadLine').css('left', left + 'px');
			ext.setCurrentTime(parseFloat(left / 80));
		},
		stop: function () {
			var left = $(this).position().left;
			var scrollLeft = $('#keyframeArea').scrollLeft();
			left = (left + scrollLeft);
			$('#playHeadLine').css('left', left + 'px');
			ext.setCurrentTime(parseFloat(left / 80));
			checkPlaybackEnabled();
		}
	});

	$(window).keydown(function (event) {
		if ($(event.target).is('input')) { return; }
		var currentTime = ext.getCurrentTime();
		var scrollLeft = $('#keyframeArea').scrollLeft();
		if (event.keyCode === 37) { //Left arrow key
			if (currentTime > 0) {
				currentTime -= 0.1;
			}
			event.preventDefault();
		} else if (event.keyCode === 39) { //Right arrow key
			currentTime += 0.1;
		}
		var left = currentTime * 80;
		var scrollLeft = $('#keyframeArea').scrollLeft();
		var nonScrollWidth = ($('#keyframeArea').width() + scrollLeft - 27);
		if (left < nonScrollWidth) {
			event.preventDefault();
		}
		if (left < scrollLeft) {
			scrollLeft = scrollLeft - 20;
			if (scrollLeft < 0) { scrollLeft = 0; }
			$('#keyframeArea').stop(true, true).animate({scrollLeft: scrollLeft}, 200);
		}
		if (currentTime < 0) { currentTime = 0; }
		if (currentTime * 80 > $('#keyframes').outerWidth(true) - 26) {
			currentTime = ($('#keyframes').outerWidth(true) - 26) / 80;
		}
		updateCurrentTime(parseFloat(currentTime));
		ext.setCurrentTime(parseFloat(currentTime));
	});

	$('#time').click(function () {
		if (!$(this).hasClass('focused')) {
			var val = $(this).text();
			$(this).empty();
			var input = $('<input type="text" value="' + val + '">').css('width','25px');
			$(this).append(input);
			$('*').enableTextSelect();
			$(this).addClass('focused');
			var that = this;
			$(this).find('input').focus().select().blur(function () {
				var val = $(this).val();
				if (jQuery.isNumeric(val)) {
					$(that).empty();
					$(that).text(val);
					$('*').disableTextSelect();
					$(that).removeClass('focused');

					var keyframe = $('.keyframe.selected')[0];
					$(keyframe).attr('data-time', val);
					keyframeArea.updateKeyframe(keyframe);
					$('#keyframeList').fadeOut(100);
					checkForConflicts();
					checkPlaybackEnabled();
				} else {
					$(this).css('border-color', 'red');
					$(that).focus().select();
				}
			}).keydown(function (event) {
				if (event.keyCode === 13) {
					$(this).blur();
				}
			});
		}
	});

	$('#translation, #rotation').change(function () {
		var type = 0;
		if ($('#rotation').is(':checked')) {
			type = 1;
		}
		var keyframe = $('.keyframe.selected')[0];
		$(keyframe).attr('data-type', type);
		keyframeArea.updateKeyframe(keyframe);
		$('#keyframeList').fadeOut(100);
		checkForConflicts();
		checkPlaybackEnabled();
		updateUnitLabels();
	});

	$('#x, #y, #z').focus(function () {
		$('*').enableTextSelect();
		$(this).select();
	}).blur(function () {
		var val = $(this).val();
		if (jQuery.isNumeric(val)) {
			this.style.borderColor = '';
			$('*').disableTextSelect();

			var keyframe = $('.keyframe.selected')[0];
			var trans = $('#x').val() + ":" + $('#y').val() + ":" + $('#z').val();
			$(keyframe).attr('data-transform', trans);
			keyframeArea.updateKeyframe(keyframe);
			$('#keyframeList').fadeOut(100);
			checkForConflicts();
		} else {
			$(this).css('border-color', 'red');
			$(this).focus().select();
		}
	}).keydown(function (event) {
		if (event.keyCode === 13) {
			$(this).blur();
		}
	});

	$('#playPause').click(function () {
		if (!isPlaying) {
			isPlaying = true;
			if (!ext.getCanPlay() && animationEnded === true) {
				updateCurrentTime(parseFloat(0.0));
				ext.setCurrentTime(parseFloat(0.0));
				animationEnded = false;
			}
			$(this).text("Pause");
			$('#stop').text("Stop");
			$('#stop').removeAttr('disabled');
			ext.play();
		} else {
			isPlaying = false;
			$(this).text("Play");
			$('#stop').text("Reset");
			$('#stop').removeAttr('disabled');
			ext.pause();
		}
	});

	$('#stop').click(function () {
		isPlaying = false;
		$('#playPause').text("Play");
		animationEnded = false;
		ext.stop();
	});
});

function adjustMetrics() {
	//Bone list minimum height
	var listHeight = $(window).height() - $('#playback').outerHeight(true) - $('#tools').outerHeight(true) - 4;
	$('#boneList').css('min-height', listHeight + 'px');
	listHeight = $('#timeline').height() -  $('#boneList').css('margin-top');
	//Keyframe area and scale minimum width
	var kareaWidth = $('#timeline').width() - $('#boneList').width() - 7;
	var kareaHeight = $(document).height() - $('#playback').outerHeight(true);
	if ($('#keyframes').outerHeight(true) > kareaHeight) { kareaWidth -= 18; }
	$('#keyframeArea').css({
		'min-width': kareaWidth + 'px',
		'max-width': kareaWidth + 'px',
		'height': kareaHeight + 'px',
		'max-height': kareaHeight + 'px'
	});
	//Keyframe x positions
	var farW = 0;
	$('.keyframe').each(function () {
		var time = $(this).attr('data-time');
		time = parseFloat(time);
		var x = Math.floor(time * 80);
		$(this).css('left', x + 'px');
		//The farthest to the right
		if (x > farW) {
			farW = x + $(this).width() + 30;
		}
	});
	var width = kareaWidth;
	if (farW > kareaWidth) { width = farW; }
	//Keyframes div and scale
	$('#keyframes').css({
		'min-width': farW - 20 - 4 + 'px',
		'min-height': kareaHeight + 'px'
	});
	var scaleWidth = $('#keyframes').outerWidth(true);
	$('#scale').css('width', scaleWidth + 'px');
	//Auto unit numbering for scale
	var nums = Math.ceil( ($('#scale').outerWidth(true) - 25) / 80 ) + 1;
	$('#scale').empty();
	for (var i=0; i < nums; i++) {
		//Scale num
		var numSpan = $('<span>' + i + '</span>');
		var margin = $('#scale span:first-child').css('margin-right') + (i);
		$('#scale').append(numSpan);
		$(numSpan).css('margin-right', margin + 'px');
	}
	//Scrubber line
	$('#playHeadLine').css('height', $('#keyframes').height() - 6);
	//Keyframe line x position and width
	$('.keyframe-row').each(function () {
		if ($(this).find('.keyframe').size() > 1) {
			var x = $(this).find('.keyframe:first').position().left + 8;
			var w = $(this).find('.keyframe:last').position().left + 8;
			//Find farthest to the left and to the right
			$(this).find('.keyframe').each(function () {
				var posX = $(this).position().left;
				if (posX < x) { x = posX + 8; }
				if (posX > w - 8) { w = posX + 8; }
			});
			w -= x;
			$(this).find('.line').css({'left': x + 'px', 'width': w + 'px'});
		}
	});
}

function onModelUpdated() {
	if (ext.isModelLoaded) {
		model = new Model(jQuery.parseJSON(ext.getModel()));
		$('#boneList').empty();
		$('#keyframes').empty();
		if (model.rootBones.length >= 1) {
			boneList.addBonesToList(model.rootBones, $('#boneList')[0], 0);
			//Reposition things
			adjustMetrics();
		}
		//Play head x position
		var x = ext.getCurrentTime() * 80;
		$('#playHead').css('left', x + 'px');
		$('#playHeadLine').css('left', x + 'px');
		//Select persisted keyframe
		if ($('.keyframe').size() > 0) {
			if (selectedKeyframe !== null) {
				$('#keyframeInfo').show();
				$('#message').hide();
				var keyframe = $('div[data-bone=' + selectedKeyframe.bone + ']');
				keyframe = keyframe.find('.keyframe')[selectedKeyframe.index];
				keyframeArea.selectKeyframe(keyframe);
				alert(selectedKeyframe.bone + "[" + selectedKeyframe.index + "]");
			} else {
				$('#keyframeInfo').hide();
				$('#message').show();
			}
		} else {
			$('#keyframeInfo').hide();
			$('#message').show();
		}
		checkForConflicts();
		checkPlaybackEnabled();

	} else {
		$('#status').text('There is no model.');
	}
}

function onCurrentTimeChanged() {
	updateCurrentTime(ext.getCurrentTime());
}

function onAnimationEnded() {
	animationEnded = true;
	$('#playPause').text("Play");
	$('#playPause').attr('disabled','');
	$('#stop').text("Reset");
}

function updateCurrentTime(time) {
	if (ext.isModelLoaded) {
		//Play head x position
		var x = time * 80;
		$('#playHead').css('left', x + 'px');
		$('#playHeadLine').css('left', x + 'px');
	}
}

function updateUnitLabels() {
	$('.info-row .units').text('meters');
	if ($('#rotation').is(':checked')) {
		$('.info-row .units').text('degrees');
	}
}

function checkPlaybackEnabled() {
	//Enable/Disable play/pause button
	if (ext.getPlaybackEnabled()) {
		$('#playPause').removeAttr('disabled');
		$('#stop').removeAttr('disabled');
		if (ext.getCanPlay()) {
			$('#playPause').removeAttr('disabled');
		}
	} else {
		$('#playPause').attr('disabled','');
		$('#stop').attr('disabled','');
	}
}

function checkForConflicts() {
	$('.keyframe-row').each(function () {
		var bone = model.bones.get( $(this).attr('data-bone') );
		var that = this;
		$(this).find('.keyframe').removeClass('multiple list sel').unbind('mouseenter');
		$(this).find('.keyframe').each(function () {
			if (!$(this).hasClass('multiple')) {
				//Get keyframes with the same time
				var thatKf = this;
				var time = $(this).attr('data-time');
				var keyframes = [];
				$(that).find('.keyframe').each(function () {
					if (this !== thatKf && $(this).attr('data-time') === time) {
						keyframes.push(this);
					}
				});
				//If there are conflicts
				if (keyframes.length > 0) {
					if (!$(this).hasClass('list')) { $(this).addClass('list'); }
					//Make them multiples and change item to its keyframe index
					var last = keyframes[keyframes.length - 1];
					for (var i = 0; i < keyframes.length; i++) {
						$(keyframes[i]).addClass('multiple');
						keyframes[i] = keyframeArea.getKeyframeIndex(bone, keyframes[i]);
					}
					keyframes.splice(0, 0, keyframeArea.getKeyframeIndex(bone, thatKf));
					//Convert list of indices to a string
					var indices = "";
					for (i = 0; i < keyframes.length; i++) { indices += keyframes[i] + ","; }
					indices = indices.substr(0, indices.length - 1);
					if (selectedKeyframe !== null) {
					$('#status').text(bone.Name + ' - ' + indices + ' - ' + selectedKeyframe.index);
					}
					indices = indices.split(",");
					//Any selected?
					for (i = 0; i < indices.length; i++) {
						if (selectedKeyframe !== null) {
							if (selectedKeyframe.bone === bone.Name && selectedKeyframe.index === indices[i]) {
								//Show the multi keyframe select image
								var multSel = $(that).find('.keyframe')[ indices[indices.length - 1] ];
								$(multSel).addClass('sel');
							}
						}
					}
					//Add list of conflicts to last conflict, because it's on top
					$(last).attr('data-conflict',keyframes).bind('mouseenter', function () {
						//Clear the list
						$('#keyframeList').empty();
						var indices = $(this).attr('data-conflict').split(',');
						//Add conflicting keyframes to list
						for (var i in indices) {
							var keyframe = $(that).find('.keyframe')[ parseInt(indices[i], 10) ];
							var type = $(keyframe).attr('data-type');
							var trans = $(keyframe).attr('data-transform');
							var kf = $('<span class="keyframe-alias" data-index="'+indices[i]+'">');
							$('#keyframeList').append(kf);
							if (type === '0') { type = "Translation"; }
							else { type = "Rotation"; }
							//window.alert(trans);
							$(kf).text(type + ": " + trans.replace(/:/g, ", "));
							if (selectedKeyframe !== null) {
								if (selectedKeyframe.bone === bone.Name && selectedKeyframe.index === indices[i]) {
									//Show the multi keyframe select image
									var multSel = $(that).find('.keyframe')[ indices[0] ];
									$(multSel).addClass('sel');
									//Select the keyframe alias
									$(kf).addClass('selected');
								}
							}
							$(kf).click(function () {
								var selKf = $(that).find('.keyframe')[ $(this).attr('data-index') ];
								keyframeArea.selectKeyframe(selKf);
								$('#keyframeList .keyframe-alias').removeClass('selected');
								$('#keyframes .keyframe').removeClass('sel');
								var multSel = $(that).find('.keyframe')[ indices[0] ];
								$(multSel).addClass('sel');
								//Select the kayframe alias
								$(kf).addClass('selected');
								$(this).addClass('selected');
								window.setTimeout(function () {
									$('#keyframeList').fadeOut(100);
								}, 115);
							});
						}
						//Show the list
						var height = $('#keyframeList').height();
						var top = $(last).offset().top - Math.round(height / 3);
						var left = $(last).offset().left + $(last).width() + 3;
						$('#keyframeList').css({top: top + 'px', left: left + 'px'}).fadeIn(100);
					});
				}
			}
		});
	});
}