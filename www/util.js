util = {};

(function() {
  "use strict";

  function addClass(e, c) {
    removeClass(e, c);
    e.className = (e.className + " " + c).trim();
  }

  function removeClass(e, c) {
    if (typeof e.className === "undefined") return;
    var classes = e.className.split(" ");
    var r = [];
    for (var i=0,j=classes.length;i<j;i++)
      if (classes[i] !== c)
        r.push(classes[i])
    e.className = r.join(" ");
  }

  util.addClass = addClass;
  util.removeClass = removeClass;

})();
