<template>
  <p>{{ this.$route.params.sessionId }}</p>

  <Tree :value="nodes"></Tree>
</template>

<script>
import store from "../store";

export default {
  data() {
    return {
      connection: null,
      nodes: [],
      expandedKeys: {}
    }
  },
  created() {
    store.commit("setMenuInactive", true);
    this.startSocket();
  },
  methods: {
    startSocket() {
      this.connection = new WebSocket(`ws://localhost:2414/?userID=79&appID=${this.$route.params.appId}&sessionID=${this.$route.params.sessionId}`, "SessionObserver");

      this.connection.onmessage = (event) => {
        event.data.text().then(text => {
          var json = JSON.parse(text);

          this.nodes.push({
            key: json['updateData'].entityID,
            label: json['updateData'].entityID
          });
        });
      }

      this.connection.onopen = () => {
        console.log("Successfully connected to the echo websocket server...")
        this.connection.send("gimme state update");
      }
    }
  }
};
</script>
