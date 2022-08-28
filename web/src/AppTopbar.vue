<template>
  <div class="layout-topbar">
    <button class="p-link layout-menu-button layout-topbar-button" @click="onMenuToggle">
      <i class="pi pi-bars"></i>
    </button>

    <router-link to="/" class="layout-topbar-logo">
      <img alt="Logo" :src="topbarImage()" />
      <span>Merlin</span>
    </router-link>

    <button class="p-link layout-topbar-menu-button layout-topbar-button" v-styleclass="{
      selector: '@next',
      enterClass: 'hidden',
      enterActiveClass: 'scalein',
      leaveToClass: 'hidden',
      leaveActiveClass: 'fadeout',
      hideOnOutsideClick: true,
    }">
      <i class="pi pi-ellipsis-v"></i>
    </button>
    <ul class="layout-topbar-menu hidden lg:flex origin-top">
      <li>
        <button class="p-link layout-topbar-button" v-tooltip="'All Apps'" @click="returnToAppList">
          <i class="pi pi-th-large"></i>
          <span>All Apps</span>
        </button>
      </li>
      <li>
        <Dropdown v-model="currentAppDropdownItem" :options="appItems" optionLabel="name" placeholder="Select an App"
          v-on:change="setSelectedAppDropdown" :loading="appListLoading" />
      </li>
      <li>
        <button class="p-link layout-topbar-button">
          <i class="pi pi-cog"></i>
          <span>Settings</span>
        </button>
      </li>
      <li>
        <button class="p-link layout-topbar-button" @click="toggleProfileMenu">
          <i class="pi pi-user"></i>
          <span>Profile</span>
        </button>
        <Menu id="profileMenu" ref="menu" :model="items" :popup="true" />
      </li>
    </ul>
  </div>
</template>

<script>
import { mapActions } from "vuex";
import store from "./store";

export default {
  data() {
    return {
      appListLoading: true,
      currentAppDropdownItem: null,
      items: [
        {
          label: "Sign Out",
          icon: "pi pi-sign-out",
          command: () => {
            this.signOut();
          },
        },
      ],
    };
  },
  computed: {
    appItems() {
      let items = [];

      if (store.getters.getApps.size > 0) {
        store.getters.getApps.forEach((value, key) => {
          items.push({
            name: value.name,
            value: key,
          });
        });
      }

      return items;
    },
    darkTheme() {
      return this.$appState.darkTheme;
    },
  },
  mounted() {
    this.currentAppDropdownItem = this.appItems.find(
      (value) => value.value == this.$route.params.appId
    );
  },
  watch: {
    $route(to) {
      this.currentAppDropdownItem = this.appItems.find(
        (value) => value.value == to.params.appId
      );

      this.appListLoading = false;
    },
    appItems() {
      this.currentAppDropdownItem = this.appItems.find(
        (value) => value.value == this.$route.params.appId
      );

      this.appListLoading = false;
    },
  },
  methods: {
    ...mapActions(["signOutAction", "setSelectedAppAction"]),
    signOut() {
      this.signOutAction().then(() => {
        this.$router.push("/login");
      });
    },
    onMenuToggle(event) {
      this.$emit("menu-toggle", event);
    },
    onTopbarMenuToggle(event) {
      this.$emit("topbar-menu-toggle", event);
    },
    topbarImage() {
      return this.$appState.darkTheme
        ? "/images/logo-white.svg"
        : "/images/logo-dark.svg";
    },
    toggleProfileMenu(event) {
      this.$refs.menu.toggle(event);
    },
    setSelectedAppDropdown(dropdownItem) {
      this.$router.push({
        name: "sessions",
        params: { appId: dropdownItem.value.value },
      });
    },
    returnToAppList() {
      this.$router.push("/apps");
    },
  },
};
</script>
