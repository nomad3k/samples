var http = require('http');

function handle(req, res) {

};

var server = http.createServer(handle);

var port = 3000;
server.listen(port, function() {
  console.log('http server listening on port ' + port);
})
