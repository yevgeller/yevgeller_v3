var mineData;// = [];
var TOTALMINES;// = 5;
var currentMinesCount;// = TOTALMINES;
var sec = 0;
var timerId;
var correctlyLocatedMines;// = 0;
var wins = [], losses = [];
var FIELDSIZE;// = 5;

function init() {
  $('#restart').hide();
  sec = 0;
  timerId = setInterval(displayTimer, 1000);
  mineData = [];
  FIELDSIZE = 7;
  TOTALMINES = 10;
  currentMinesCount = TOTALMINES;
  correctlyLocatedMines = 0;
  setMinesDiscoveryProgressText();
  initMines();

  //display table:
  $('#mswpr').find('tr').remove();
  for (i = 0; i < mineData.length; i++) {
    var $row = $('<tr></tr>');
    for (j = 0; j < mineData[i].length; j++) {
      var $cell;
      $cell = $('<td class=\"by\" id="' + i + '-' + j + '">&nbsp;</td>');
      $row.append($cell);
    }
    //end: display table
    $('#mswpr').append($row);
  }

  var $tds = $('#mswpr td');
  $tds.each(function () {
    $(this).mousedown(cellClick);
  });
}

function initMines() {
  var i = 0, j = 0;
  for (i = 0; i < FIELDSIZE; i++) {
    var rowArr = [];
    var runningTotal = 0;
    for (j = 0; j < FIELDSIZE; j++) {
      rowArr[j] = 0;
    }
    mineData[i] = rowArr;
  }

  while (runningTotal < TOTALMINES) {
    var x = Math.floor((Math.random() * FIELDSIZE));
    var y = Math.floor((Math.random() * FIELDSIZE));
    if (mineData[x][y] == 0) {
      mineData[x][y] = 1;
      runningTotal += 1;
    }
  }
}

$(document).ready(function () {
  $('#restart').click(function () {
    init();
  });

  init();
});

function cellClick(event) {
  var ii = $(this).attr('id').split('-');
  switch (event.which) {
    case 1:
      $(this).removeClass('by');
      countMines(ii[0], ii[1]);
      break;
      //case 2: //don't care, middle mouse button
      //break;
    case 3:
      if ($(this).hasClass('by')) {
        $(this).removeClass('by').addClass('bb');
        if (hasMine(ii[0], ii[1]))
          correctlyLocatedMines += 1;
        currentMinesCount -= 1;
        if (currentMinesCount <= 0) {
          endGame(correctlyLocatedMines == TOTALMINES, "");
          break;
        }
      } else {
        $(this).removeClass('bb').addClass('by');
        if (hasMine(ii[0], ii[1]))
          correctlyLocatedMines -= 1;
        currentMinesCount += 1;
      }
      setMinesDiscoveryProgressText();
      break;
  }
}

function hasMine(a, b) {
  if (mineData[a][b] == 1)
    return true;

  return false;
}

function setMinesDiscoveryProgressText() {
  $('#info').html(currentMinesCount + ' mines left.');
}

function revealMines() {
  for (var i = 0; i < mineData.length; i++) {
    for (var j = 0; j < mineData[i].length; j++) {
      var $cell = $('#' + i + '-' + j);
      if (mineData[i][j] == 1) {
        if ($cell.hasClass('by')) {
          $cell.removeClass('by').addClass('bw').html('X');
        }
        if ($cell.hasClass('bb')) {
          $cell.removeClass('bb').addClass('bg').html('X');
        }
      } else {
        if ($cell.hasClass('bb')) {
          $cell.removeClass('bb').addClass('br').html('--');
        }
      }
    }
  }
}

function endGame(win, cellId) {
  revealMines();
  clearInterval(timerId);
  if (win == false) {
    $('#info').html('You lose');
    $('#' + cellId).addClass('br').html('X');
    //alert('ka boom!');
    losses.push({ time: sec, correct: correctlyLocatedMines, total: TOTALMINES });
  } else {
    wins.push(sec);
    $('#info').html('Victory!');
  }
  displayScores();
  $('#mswpr td').off('mousedown');
  $('#restart').fadeIn('slow');
}

function displayScores() {
  $('#scores').html('');
  if (wins.length > 0) {
    wins.sort();
    $('#scores').append('<h3>Wins</h3>');
    for (var i = 0; i < (wins.length < 10 ? wins.length : 10) ; i++) {
      $('#scores').append((i + 1) + '. ' + convertSecondsToTime(wins[i])+ '<br />');
    }
  }

  if (losses.length > 0) {
    losses.sort(
      function (a, b) {
        if (a.correct / a.total == b.correct / b.total) {
          if (a.time < b.time) return -1;
          if (a.time > b.time) return 1;
          return 0;
        }
        if (a.correct / a.total > b.correct / b.total)
          return -1;
        if (a.correct / a.total < b.correct / b.total)
          return 1;

        return 0;
      });
    $('#scores').append('<h3>Losses</h3>');
    for (var i = 0; i < (losses.length < 10 ? losses.length : 10) ; i++) {
      $('#scores').append((i + 1) + '. ');
      $('#scores').append(losses[i].correct + '/' + losses[i].total + ' mines, ' + convertSecondsToTime(losses[i].time) + '<br />');
    }
  }
}

function countMines(a, b) {
  var row = parseInt(a, 10), col = parseInt(b, 10);
  var cellId = a + '-' + b;
  //console.log('processing cell: ' + cellId);
  if ($('#' + cellId).text().trim() != '') {
    //console.log('cell ' + cellId + ' is not empty, current value: ' + $txt + ', exiting');
    return 1;
  }
  var count = 0;
  if (mineData[row][col] == 1) {
    endGame(false, cellId);
    return 1;
  } else if (row == 0 && col == 0) {
    for (i = 0; i < 2; i++)
      for (j = 0; j < 2; j++)
        count += mineData[row + i][col + j];
  } else if (row == 0 && col > 0 && col < FIELDSIZE - 1) {
    for (i = 0; i < 2; i++)
      for (j = -1; j < 2; j++)
        count += mineData[row + i][col + j];
  } else if (row > 0 && row < FIELDSIZE - 1 && col == 0) {
    for (i = -1; i < 2; i++)
      for (j = 0; j < 2; j++)
        count += mineData[row + i][col + j];
  } else if (row > 0 && row < FIELDSIZE - 1 && col > 0 && col < FIELDSIZE - 1) {
    for (i = -1; i < 2; i++)
      for (j = -1; j < 2; j++)
        count += mineData[row + i][col + j];
  } else if (row == FIELDSIZE - 1 && col == 0) {
    for (i = -1; i < 1; i++)
      for (j = 0; j < 2; j++)
        count += mineData[row + i][col + j];
  } else if (row == FIELDSIZE - 1 && col > 0 && col < FIELDSIZE - 1) {
    for (i = -1; i < 1; i++)
      for (j = -1; j < 2; j++)
        count += mineData[row + i][col + j];
  } else if (row == FIELDSIZE - 1 && col == FIELDSIZE - 1) {
    for (i = -1; i < 1; i++)
      for (j = -1; j < 1; j++)
        count += mineData[row + i][col + j];
  } else if (row == 0 && col == FIELDSIZE - 1) {
    for (i = 0; i < 2; i++)
      for (j = -1; j < 1; j++)
        count += mineData[row + i][col + j];
  } else if (row > 0 && row < FIELDSIZE - 1 && col == FIELDSIZE - 1) {
    for (i = -1; i < 2; i++)
      for (j = -1; j < 1; j++)
        count += mineData[row + i][col + j];
  }
  $('#' + cellId).html(count);
  $('#' + cellId).off('mousedown');
  //console.log('cellId: ' + cellId + ', mines: ' + count + ', done.');
}

function displayTimer() {
  sec += 1;
  $('#timer').html(convertSecondsToTime(sec));
}

function convertSecondsToTime(seconds) {
  var ret = "";
  var hr = Math.floor(seconds / (60 * 24)),
    min = Math.floor(seconds / 60),
    s = seconds % 60;
  ret += hr < 10 ? "0" + hr + ":" : hr + ":";
  ret += min < 10 ? "0" + min + ":" : min + ":";
  ret += s < 10 ? "0" + s : s;
  return ret;
}