<template>
  <Menubar :model="menubarItems"></Menubar>
  <!-- <p>{{ this.$route.params.sessionId }}</p> -->

  <div class="p-4 flex" style="min-height: 200px; height: 75vh;">
    <div class="card mx-2" style="height: 100%; width: 250px;">
      <h5>Worlds</h5>
      <ScrollPanel style="width: 100%; height: 97%;">
        <Tree :value="nodes" :filter="true" filterMode="lenient" style="width: 100%;"></Tree>
      </ScrollPanel>
    </div>
    <div class="card mx-2" style="height: 100%; width: 200px;">
      <h5>Entities</h5>
      <ScrollPanel style="width: 100%; height: 97%;">
        <Tree :value="nodes" :filter="true" filterMode="lenient" style="width: 100%;"></Tree>
      </ScrollPanel>
    </div>
    <div class="card mx-2" style="height: 100%; width: 200px;">
      <h5>Components</h5>
      <ScrollPanel style="width: 100%; height: 97%;">
        <Tree :value="nodes" :filter="true" filterMode="lenient" style="width: 100%;"></Tree>
      </ScrollPanel>
    </div>
    <div class="card mx-2" style="height: 100%; width: 200px;">
      <h5>Data</h5>
      <ScrollPanel style="width: 100%; height: 97%;">
        <Tree :value="nodes" :filter="true" filterMode="lenient" style="width: 100%;"></Tree>
      </ScrollPanel>
    </div>
  </div>
</template>

<script>
import store from "../store";

export default {
  data() {
    return {
      connection: null,
      nodes: [],
      expandedKeys: {},
      menubarItems: [
        {
          label: 'File',
          icon: 'pi pi-fw pi-file',
          items: [
            {
              label: 'New',
              icon: 'pi pi-fw pi-plus',
              items: [
                {
                  label: 'Bookmark',
                  icon: 'pi pi-fw pi-bookmark'
                },
                {
                  label: 'Video',
                  icon: 'pi pi-fw pi-video'
                }
              ]
            },
            {
              label: 'Delete',
              icon: 'pi pi-fw pi-trash'
            },
            {
              separator: true
            },
            {
              label: 'Export',
              icon: 'pi pi-fw pi-external-link'
            }
          ]
        },
        {
          label: 'Edit',
          icon: 'pi pi-fw pi-pencil',
          items: [
            {
              label: 'Left',
              icon: 'pi pi-fw pi-align-left'
            },
            {
              label: 'Right',
              icon: 'pi pi-fw pi-align-right'
            },
            {
              label: 'Center',
              icon: 'pi pi-fw pi-align-center'
            },
            {
              label: 'Justify',
              icon: 'pi pi-fw pi-align-justify'
            }
          ]
        },
        {
          label: 'Users',
          icon: 'pi pi-fw pi-user',
          items: [
            {
              label: 'New',
              icon: 'pi pi-fw pi-user-plus',

            },
            {
              label: 'Delete',
              icon: 'pi pi-fw pi-user-minus',

            },
            {
              label: 'Search',
              icon: 'pi pi-fw pi-users',
              items: [
                {
                  label: 'Filter',
                  icon: 'pi pi-fw pi-filter',
                  items: [
                    {
                      label: 'Print',
                      icon: 'pi pi-fw pi-print'
                    }
                  ]
                },
                {
                  icon: 'pi pi-fw pi-bars',
                  label: 'List'
                }
              ]
            }
          ]
        },
        {
          label: 'Events',
          icon: 'pi pi-fw pi-calendar',
          items: [
            {
              label: 'Edit',
              icon: 'pi pi-fw pi-pencil',
              items: [
                {
                  label: 'Save',
                  icon: 'pi pi-fw pi-calendar-plus'
                },
                {
                  label: 'Delete',
                  icon: 'pi pi-fw pi-calendar-minus'
                }
              ]
            },
            {
              label: 'Archieve',
              icon: 'pi pi-fw pi-calendar-times',
              items: [
                {
                  label: 'Remove',
                  icon: 'pi pi-fw pi-calendar-minus'
                }
              ]
            }
          ]
        },
        {
          label: 'Quit',
          icon: 'pi pi-fw pi-power-off'
        }
      ]
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
