<template>
  <div class="grid">
    <div class="col-12">
      <div class="card">
        <h5>Live Sessions</h5>
        <Toast />

        <DataTable
          :value="sessions"
          :paginator="true"
          class="p-datatable-gridlines"
          :rows="10"
          dataKey="id"
          :rowHover="true"
          v-model:filters="filters1"
          filterDisplay="menu"
          :loading="loading1"
          :filters="filters1"
          responsiveLayout="scroll"
          :globalFilterFields="['name']"
          v-model:selection="selectedSession"
          selectionMode="single"
          @rowSelect="onRowSelect"
          @rowUnselect="onRowUnselect"
        >
          <template #header>
            <div class="flex justify-content-between flex-column sm:flex-row">
              <Button
                type="button"
                icon="pi pi-filter-slash"
                label="Clear"
                class="p-button-outlined mb-2"
                @click="clearFilter1()"
              />
              <span class="p-input-icon-left mb-2">
                <i class="pi pi-search" />
                <InputText
                  v-model="filters1['global'].value"
                  placeholder="Keyword Search"
                  style="width: 100%"
                />
              </span>
            </div>
          </template>
          <template #empty> No live sessions found. </template>
          <template #loading> Loading live sessions. Please wait. </template>
          <Column field="name" header="Name" style="min-width: 12rem">
            <template #body="{ data }">
              {{ data.name }}
            </template>
            <template #filter="{ filterModel }">
              <InputText
                type="text"
                v-model="filterModel.value"
                class="p-column-filter"
                placeholder="Search by name"
              />
            </template>
          </Column>
          <Column field="entityCount" header="Entities" style="min-width: 12rem">
            <template #body="{ data }">
              {{ data.entityCount }}
            </template>
            <template #filter="{ filterModel }">
              <InputText
                type="number"
                v-model="filterModel.value"
                class="p-column-filter"
                placeholder="Search by entity count"
              />
            </template>
          </Column>
        </DataTable>
      </div>
    </div>
  </div>
</template>

<script>
import { FilterMatchMode, FilterOperator } from "primevue/api";
import { io } from "socket.io-client";

export default {
  data() {
    return {
      selectedSession: null,
      filters1: null,
      loading1: true,
      connection: null,
      sessions: null,
    };
  },
  created() {
    this.initFilters1();
    this.startSocket();
  },
  mounted() {},
  methods: {
    startSocket() {
      const socket = io("http://localhost:3000");
      socket.on("connect", () => {
        console.log("Connected!");
        socket.emit("getApps", this.$route.params.appId);
      });

      socket.on("apps", (apps) => {
        console.log(apps);
        this.messageReceived(apps);
        this.loading1 = false;
      });
    },
    messageReceived(event) {
      this.sessions = event;
    },
    initFilters1() {
      this.filters1 = {
        global: { value: null, matchMode: FilterMatchMode.CONTAINS },
        name: {
          operator: FilterOperator.AND,
          constraints: [
            { value: null, matchMode: FilterMatchMode.STARTS_WITH },
          ],
        },
      };
    },
    clearFilter1() {
      this.initFilters1();
    },
    expandAll() {
      this.expandedRows = this.products.filter((p) => p.id);
    },
    collapseAll() {
      this.expandedRows = null;
    },
    formatCurrency(value) {
      return value.toLocaleString("en-US", {
        style: "currency",
        currency: "USD",
      });
    },
    formatDate(value) {
      return value.toLocaleDateString("en-US", {
        day: "2-digit",
        month: "2-digit",
        year: "numeric",
      });
    },
    onRowSelect(event) {
      this.$toast.add({
        severity: "info",
        summary: "Product Selected",
        detail: "Name: " + event.data.name,
        life: 3000,
      });
    },
    onRowUnselect(event) {
      this.$toast.add({
        severity: "warn",
        summary: "Product Unselected",
        detail: "Name: " + event.data.name,
        life: 3000,
      });
    },
  },
};
</script>

<style scoped lang="scss">
@import "../assets/demo/badges.scss";

::v-deep(.p-datatable-frozen-tbody) {
  font-weight: bold;
}

::v-deep(.p-datatable-scrollable .p-frozen-column) {
  font-weight: bold;
}
</style>
