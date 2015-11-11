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

  function Snapshot(name, companyName, price, change, changePercent, marketCap) {
    this.name = name;
    this.companyName = companyName;
    this.price = price;
    this.change = change;
    this.changePercent = changePercent;
    this.marketCap = marketCap;
  }

  Snapshot.prototype.merge = function(args) {
    var changes = [];
    var attrs = ["price", "change", "changePercent"];
    for (var i=0,j=attrs.length;i<j;i++) {
      var a = attrs[i];
      if (args[a] !== null && args[a] !== this[a]) {
        this[a] = args[a];
        changes.push(a);
      }
    }
    return changes;
  }

  function parseNumber(s) {
    if (s.trim() === "") return null;
    var result = parseFloat(s);
    var last = s[s.length-1];
    if (last === "B") return result * 1000000000;
    if (last === "M") return result * 1000000;
    return result;
  }

  function parseSnapshot(text) {
    var cells = text.split(",");
    if (cells.length == 1)
      return parseInt(cells[0], 10);

    return new Snapshot(
      cells[0].trim(),
      cells[1].trim(),
      parseNumber(cells[2].trim()),
      parseNumber(cells[3].trim()),
      parseNumber(cells[4].trim()),
      cells[5]
    );
  }

  function parseSnapshots(text) {
    var results = [];
    var lines = text.split("\n");
    for (var i=1,j=lines.length;i<j;i++) {
      results.push(parseSnapshot(lines[i]));
    }
    return results;
  }

  function parseDeltas(text) {
    var results = [];
    var lines = text.split("\n");
    for (var i=1,j=lines.length;i<j;i++) {
      results.push(parseSnapshot(lines[i]));
    }
    return results;
  }

  function TickerEngine(snapshots, deltas) {
    this.snapshots = snapshots;
    this.deltas = deltas;
    this.snapshotIndex = 0;
    this.deltaIndex = 0;
    this.running = false;
  }

  TickerEngine.prototype.tick = function() {
    if (!this.running) {
      if (typeof this.onstop !== "undefined")
        this.onstop(this);
      return;
    }
    if (this.deltaIndex >= this.deltas.length)
      this.deltaIndex = 0;
    var delta = this.deltas[this.deltaIndex++];
    if (typeof delta === "object") {
      if (this.snapshotIndex >= this.snapshots.length)
        this.snapshotIndex = 0;
      var snapshot = this.snapshots[this.snapshotIndex++];
      var changes = snapshot.merge(delta);
      if (changes.length > 0 && typeof this.onchange !== "undefined") {
        this.onchange(snapshot, changes);
      }
      this.tick();
    } else {
      console.log("sleep for " + delta + "...");
      var self = this;
      window.setTimeout(function() { self.tick(); }, delta);
    }
  };

  TickerEngine.prototype.start = function() {
    if (!this.running) {
      this.running = true;
      if (typeof this.onstart !== "undefined")
        this.onstart(this);
      this.tick();
    }
  };

  TickerEngine.prototype.stop = function() {
    if (this.running) {
      this.running = false;
    }
  };

  function initialise(callback) {
    load("data/snapshot.csv",
      function(snapshot) {
        load("data/deltas.csv",
          function(deltas) {
            var s = parseSnapshots(snapshot.trim());
            var d = parseDeltas(deltas.trim());
            callback(new TickerEngine(s, d));
          },
          function(statusCode, error) {
            alert("Coud not load deltas ~ " + error);
            callback(false);
          });
      },
      function(statusCode, error) {
        alert("Could not load snaspshot ~ " + error);
        callback(false);
      });
  }

  ticker.initialise = initialise;
})();
