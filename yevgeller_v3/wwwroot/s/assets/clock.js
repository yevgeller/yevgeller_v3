var hourInnerRadius = 100;
var hourOuterRadius = 130;
var minuteInnerRadius = 133;
var minuteOuterRadius = 153;
var secondInnerRadius = 156;
var secondOuterRadius = 168;
var cX = 200;
var cY = 200;

function drawClock() {
  drawNumbers();
  drawMinutes();
  drawHours();
  setInterval(displayTime, 250);
  setInterval(drawSeconds, 41);//24 frames per sec
  setInterval(drawMinutes, 900);
  //setInterval(drawHours, 900);
}

function displayTime() {
  var span = document.getElementById("time");
  span.innerText = new Date().toTimeString();
}

function getAngle(regions, units) {
  //regions: total amount of regions in the circle
  //units: how many units this position occupies
  return Math.PI * 2 / (regions * 2) * (units + 1) - Math.PI / 2;
}

function drawSeconds() {
  var units = 60;
  var drawing = document.getElementById("seconds");
  if (drawing.getContext) {
    var context = drawing.getContext("2d");
    context.clearRect(0, 0, 400, 400);
    context.beginPath();
    context.lineWidth = 0.5;
    context.strokeStyle = "#896805";
    context.fillStyle = "rgba(255,198,27,0.8)";//80,80,80,0.8)";
    var dtSec = new Date();
    var val2 = dtSec.getSeconds();
    var val3 = dtSec.getMilliseconds();
    var leftBorderRegion = val2 * 2 - 1;
    var rightBorderRegion = leftBorderRegion + 2;
    var msOffset = Math.PI * 2 * val3 / (units * 1000);
    var leftAngle = Math.PI * 2 / (units * 2) * leftBorderRegion - Math.PI / 2 + msOffset;
    var rightAngle = Math.PI * 2 / (units * 2) * rightBorderRegion - Math.PI / 2 + msOffset;
    var lefterInnerX = Math.cos(leftAngle) * secondInnerRadius + cX;
    var lefterInnerY = Math.sin(leftAngle) * secondInnerRadius + cY;
    var righterOuterX = Math.cos(rightAngle) * secondOuterRadius + cX;
    var righterOuterY = Math.sin(rightAngle) * secondOuterRadius + cY;
    context.arc(cX, cY, secondInnerRadius, leftAngle, rightAngle, true); //L-R, inner
    context.lineTo(righterOuterX, righterOuterY);
    context.arc(cX, cY, secondOuterRadius, rightAngle, leftAngle, false); //R-L, outer
    context.lineTo(lefterInnerX, lefterInnerY);
    context.closePath();
    context.fill();
    context.stroke();
  }
}

function drawMinutes() {
  var units = 60;
  var drawing = document.getElementById("minutes");
  if (drawing.getContext) {
    var context = drawing.getContext("2d");
    context.clearRect(0, 0, 400, 400);
    context.beginPath();
    context.lineWidth = 0.5;
    context.strokeStyle = "#896805";
    context.fillStyle = "rgba(255,198,27,0.8)";//80,80,80,0.8)";
    var dtSec = new Date();
    var val2 = dtSec.getMinutes();
    var val3 = dtSec.getSeconds();
    var leftBorderRegion = val2 * 2 - 1;
    var rightBorderRegion = leftBorderRegion + 2;
    //var minOffset = Math.PI * 2 * val3 / (units*units); //this is for the smooth flow
    var leftAngle = Math.PI * 2 / (units * 2) * leftBorderRegion - Math.PI / 2;// + minOffset;
    var rightAngle = Math.PI * 2 / (units * 2) * rightBorderRegion - Math.PI / 2;// + minOffset;
    var lefterInnerX = Math.cos(leftAngle) * minuteInnerRadius + cX;
    var lefterInnerY = Math.sin(leftAngle) * minuteInnerRadius + cY;
    var righterOuterX = Math.cos(rightAngle) * minuteOuterRadius + cX;
    var righterOuterY = Math.sin(rightAngle) * minuteOuterRadius + cY;
    context.arc(cX, cY, minuteInnerRadius, leftAngle, rightAngle, true); //L-R, inner
    context.lineTo(righterOuterX, righterOuterY);
    context.arc(cX, cY, minuteOuterRadius, rightAngle, leftAngle, false); //R-L, outer
    context.lineTo(lefterInnerX, lefterInnerY);
    context.closePath();
    context.fill();
    context.stroke();
  }
}

function drawHours() {
  var units = 12;
  var drawing = document.getElementById("hours");
  if (drawing.getContext) {
    var context = drawing.getContext("2d");
    context.clearRect(0, 0, 400, 400);
    context.beginPath();
    context.lineWidth = 0.5;
    context.strokeStyle = "#896805";
    context.fillStyle = "rgba(255,198,27,0.8)";//80,80,80,0.8)";
    var dtSec = new Date();
    var val2 = dtSec.getHours();
    if (val2 > 12) val2 -= 12;
    var val3 = dtSec.getMinutes();
    var leftBorderRegion = val2 * 2 - 1;
    var rightBorderRegion = leftBorderRegion + 2;
    //var hrOffset = Math.PI * 2 * val3 / 720;
    var leftAngle = Math.PI * 2 / (units * 2) * leftBorderRegion - Math.PI / 2;// + hrOffset;
    var rightAngle = Math.PI * 2 / (units * 2) * rightBorderRegion - Math.PI / 2;// + hrOffset;
    var lefterInnerX = Math.cos(leftAngle) * hourInnerRadius + cX;
    var lefterInnerY = Math.sin(leftAngle) * hourInnerRadius + cY;
    var righterOuterX = Math.cos(rightAngle) * hourOuterRadius + cX;
    var righterOuterY = Math.sin(rightAngle) * hourOuterRadius + cY;
    context.arc(cX, cY, hourInnerRadius, leftAngle, rightAngle, true); //L-R, inner
    context.lineTo(righterOuterX, righterOuterY);
    context.arc(cX, cY, hourOuterRadius, rightAngle, leftAngle, false); //R-L, outer
    context.lineTo(lefterInnerX, lefterInnerY);
    context.closePath();
    context.fill();
    context.stroke();
  }
}

function drawNumbers() {
  var drawing = document.getElementById("clock");
  if (drawing.getContext) {
    var context = drawing.getContext("2d");
    context.font = "28px Arial";
    for (var i = 1; i <= 12; i++) {
      var angle = getAngle(12, i * 2 - 1);
      context.fillStyle = "black";
      var x = Math.cos(angle) * hourInnerRadius + cX;
      var y = Math.sin(angle) * hourInnerRadius + cY;
      //context.fillRect(x, y, 2, 2);
      context.save();
      context.translate(x, y);
      var text = i.toString() + ((i == 6 || i == 9) ? "." : "");
      var tWidth = context.measureText(text).width;
      var tHeight = context.measureText(text).height;
      context.rotate(Math.PI /2+angle);
      context.textalign = "center";
      context.fillText(i.toString() + ((i == 6 || i == 9) ? "." : ""), -tWidth/2, -3);
      context.restore();
    }
    context.font = "10px Arial";
    for (var i = 0; i <= 59; i++) {
      var angle = getAngle(60, i * 2 - 1);
      context.fillStyle = "black";
      var x = Math.cos(angle) * minuteInnerRadius + cX;
      var y = Math.sin(angle) * minuteInnerRadius + cY;
      context.save();
      context.translate(x, y);
      context.rotate(angle);
      context.fillText(i.toString(), 5, 5);
      context.restore();
    }
    var cm = context.currentTransform;
    context.font = "8px Arial";
    for (var i = 0; i <= 59; i++) {
      //context.transform(1,0,0,1,0,0);
      var angle = getAngle(60, i * 2 - 1);
      context.fillStyle = "black";
      var x = Math.cos(angle) * secondInnerRadius + cX;
      var y = Math.sin(angle) * secondInnerRadius + cY;
      context.save();
      context.translate(x, y);
      context.rotate(angle);
      context.fillText(i.toString(), 1, 1);
      context.restore();
    }
  }
}