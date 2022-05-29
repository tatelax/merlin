<template>
    <div class="layout-main-container">

        <Card  style="width: 25rem; margin-bottom: 2em">
            <template #content>
                <Button @click="openCreateAppModal">Create App</Button>
            </template>
        </Card>
        <div v-for="[key, value] in apps" :key="value">
            <Card class="app-card" style="width: 25rem; margin-bottom: 2em" @click="appClicked(key)">
                <template #content>
                    <span>{{ value.name }}</span>
                </template>
            </Card>
        </div>
    </div>

    <Dialog header="Create New App" v-model:visible="displayCreateAppModal" :style="{ width: '20vw' }" :modal="true">
        <div class="field">
            <label for="newAppName">App Name</label>
            <InputText id="newAppName" type="text" v-model="newAppName" />
        </div>

        <template #footer>
            <Button label="Cancel" icon="pi pi-times" @click="closeCreateAppModal" class="p-button-text" />
            <Button label="Create" icon="pi pi-check" @click="createNewApp" autofocus />
        </template>
    </Dialog>
</template>

<script>
import store from '../store'
import { mapActions } from "vuex";

export default {
    data() {
        return {
            displayCreateAppModal: false,
            newAppName: null
        };
    },
    methods: {
        ...mapActions(["createNewAppAction", "setSelectedAppAction"]),
        openCreateAppModal() {
            this.displayCreateAppModal = true;
        },
        closeCreateAppModal() {
            this.displayCreateAppModal = false;
        },
        createNewApp() {
            this.createNewAppAction({ name: this.newAppName, userID: store.getters.getUser.uid});
        },
        appClicked(appID) {
            this.$router.push(`/apps/${appID}/dashboard`);
        },
        footerImage() {
            return this.$appState.darkTheme ? '/images/logo-white.svg' : '/images/logo-dark.svg';
        }
    },
    computed: {
        apps() {
            return store.getters.getApps;
        },
        darkTheme() {
            return this.$appState.darkTheme;
        }
    }
}
</script>

<style lang="scss" scoped>
.sizes {
    .p-inputtext {
        display: block;
        width: 100%;

        &:last-child {
            margin-bottom: 0;
        }
    }
}

.field * {
    display: block;
}

.app-card {
    cursor:pointer;
}
</style>