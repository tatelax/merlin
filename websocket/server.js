import { WebSocketServer } from 'ws';

const users = new Map();
const wss = new WebSocketServer({ port: 8080 });

wss.getUniqueID = function () {
  function s4() {
    return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
  }
  return s4() + s4() + '-' + s4();
};

const dummyData = [
  {
    name: "Example Session"
  },
  {
    name: "Example Session 2"
  },
  {
    name: "Example Session 3"
  }]

wss.on('listening', function open() {
  console.log(`Server Started on ${wss.address().address}:${wss.address().port}`);
});

wss.on('connection', function connection(ws, req) {
  ws.id = req.url.replace('/?id=', '')

  ws.on('message', function message(data) {
    console.log('received: %s', data);
    users.set(ws.id, {
      json: JSON.parse(data)
    });

    console.log(users.get(ws.id).json)
  });

  console.log(ws.id);

  ws.send(JSON.stringify(dummyData));
});