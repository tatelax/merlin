<template>
  <p>{{ this.$route.params.sessionId }}</p>
</template>

<script>
import { io } from "socket.io-client";

export default {
  created() {
    this.startSocket();
  },
  methods: {
    startSocket() {
      const socket = io("http://localhost:3000");
      socket.on("connect", () => {
        console.log("Connected!");
        socket.emit("getApps", this.$route.params.appId);
      });

      socket.on("apps", (apps) => {
        console.log(apps);
      });
    },
  },
};
</script>
