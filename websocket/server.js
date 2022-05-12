const express = require('express');
const app = express();
const http = require('http');
const server = http.createServer(app);
const { Server } = require("socket.io");
const io = new Server(server);

app.get('/', (req, res) => {
  res.sendFile(__dirname + '/index.html');
});

io.on("connection", (socket) => {
    socket.on("client-handshake", () => {
        console.log("Received client handshake");
        
        socket.emit("server-handshake");
        console.log("Sent server handshake!");
    });
});

server.listen(3000, () => {
  console.log('listening on *:3000');
});