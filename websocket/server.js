import { createServer } from "http";
import { Server } from "socket.io";
import { instrument } from "@socket.io/admin-ui";

const httpServer = createServer();

const io = new Server(httpServer, {
  cors: {
    origin: ["http://localhost:8080"],
    credentials: false
  }
});

const apps = new Map();

apps.set('12345', {
  name: "Hello World",
  entities: 2
});

instrument(io, {
  auth: false
});

io.on("connection", (socket) => {
  socket.on("registerApp", (app) => {
    socket.app = app;
    apps.set(app.id, app);
  });

  socket.on("getApps", () => {
    socket.emit("apps", [...apps.entries()]);
  });
});

httpServer.listen(3000);