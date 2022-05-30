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

apps.set("oIhL9KaVJ9cn0pKV7fyk", [
  {
    name: "Hello World!",
    entityCount: 2
  },
  {
    name: "Foo Bar",
    entityCount: 872
  }
]);

apps.set("cod2WUx7So3Je9pxt6mx", [
  {
    name: "Something!",
    entityCount: 2
  }
]);

instrument(io, {
  auth: false
});

io.on("connection", (socket) => {
  socket.on("registerApp", (app) => {
    socket.app = app;
    apps.set(app.id, app);
  });

  socket.on("getApps", (id) => {
    // let data = [];

    // for(let i = 0; i < arr.length; i++) {
    //   console.log(apps.get(arr[i]));
    //   data[i] = {
    //     name: apps.get(arr[i]).name,
    //     entityCount: apps.get(arr[i]).entityCount
    //   };
    // }

    socket.emit("apps", apps.get(id));
  });
});

httpServer.listen(3000);