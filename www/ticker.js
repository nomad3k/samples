var ticker = {};

(function() {
  "use strict";

  function load(url, success, failure) {
    var http = new XMLHttpRequest();
    http.onreadystatechange = function() {
      if (http.readyState == 4) {
        if (http.status == 200) {
          success(http.responseText);
        } else {
          failure(http.status, http.error);
        }
      }
    };
    http.open("GET", url, true);
    http.send(null);
  }

  function initialise(callback) {
    load("data/snapshot.csv",
      function(snapshot) {
        load("data/deltas.csv",
          function(deltas) {
            callback();
          },
          function(statusCode, error) {
            alert("Coud not load deltas ~ " + error);
            callback();
          });
      },
      function(statusCode, error) {
        alert("Could not load snaspshot ~ " + error);
        callback();
      });
  }

  ticker.initialise = initialise;
})();
