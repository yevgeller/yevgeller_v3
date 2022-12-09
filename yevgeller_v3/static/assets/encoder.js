function processString() {
  var src = document.getElementById("source").value;
  var res = "";
  var buffer = "";

  for (var i = 0; i < src.length; i++) {
    if (isAlpha(src[i])) {
      buffer += src[i];
    } else {
      res += encode(buffer);
      buffer = "";
      res += src[i];
    }
  }

  if (buffer.length > 0) {
    res += encode(buffer);
  }

  buffer = null;
  var spn = document.getElementById("result");
  spn.appendChild(document.createTextNode(res));
}

function encode(word) {
  if (word.length < 4)
    return word;
  if (word.length == 4)
    return word[0] + word[2] + word[1] + word[3];
  else {
    var begin = word[0];
    var end = word[word.length - 1];
    var toScramble = word.slice(1, word.length - 1);
    var scrambled = scrambleLetters(toScramble);
    while (scrambled == toScramble)
      scrambled = scrambleLetters(toScramble);

    return begin + scrambled + end;
  }
}

function copyToClipboard() {
  var sp = document.getElementById("result").firstChild.textContent;
  window.prompt("Copy to clipboard: Ctrl+C/Cmd+C, Enter", sp);
}

function isAlpha(a) {
  var unicode = XRegExp("^\\p{L}$");
  return unicode.test(a);
  //return /\w/.test(a); //only works with Latin alphabet
}

function scrambleLetters(word) {
  var a = word;
  for (var i = 0; i < a.length * 2; i++) {
    var rnd = Math.floor(Math.random() * a.length);
    var ltr = a.slice(rnd, rnd + 1);
    a = ltr + spliceString(a,  rnd, 1);
  }
  return a;
}

function spliceString(str, start, num) {
  if(start==0)
    return str.substring(num, str.length);
  if(start == str.length-1)
    return str.substring(0, str.length-1);
  else {
    var a = str.substring(0, start);
    var b = str.substring(start+num, str.length);
    return a+b;
  }
}

function clearValues() {
  document.getElementById("source").value = "";
  var a = document.getElementById("result");
  a.removeChild(a.firstChild);
}