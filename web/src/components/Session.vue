<template>
  <p>{{ this.$route.params.sessionId }}</p>
</template>

<script>
export default {
  data() {
    return {
      connection: null
    }
  },
  created() {
    this.startSocket();
  },
  methods: {
    startSocket() {
      this.connection = new WebSocket("http://localhost:5000");

      this.connection.onmessage = function (event) {
        console.log(event);
      }

      this.connection.onopen = function (event) {
        console.log(event)
        console.log("Successfully connected to the echo websocket server...")

        this.connection.send("getApps");
      }
    },
  },
};
</script>
