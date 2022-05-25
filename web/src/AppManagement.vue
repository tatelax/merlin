<template>
    <div :class="containerClass" @click="onWrapperClick">
        <AppTopBar @menu-toggle="onMenuToggle" />
    </div>

    <div class="layout-main-container">
        <Card style="width: 25rem; margin-bottom: 2em">

            <template #content>
                <Button @click="openCreateAppModal">Create App</Button>
            </template>
        </Card>
    </div>

    <Dialog header="Create New App" v-model:visible="displayCreateAppModal" :style="{ width: '20vw' }" :modal="true">
        <div class="field">
            <label for="username1">App Name</label>
            <InputText id="username1" type="username" aria-describedby="username1-help" />
            <small id="username1-help">Enter your username to reset your password.</small>
        </div>

        <template #footer>
            <Button label="Cancel" icon="pi pi-times" @click="closeCreateAppModal" class="p-button-text" />
            <Button label="Create" icon="pi pi-check" @click="closeCreateAppModal" autofocus />
        </template>
    </Dialog>
</template>

<script>
import AppTopBar from './AppTopbar.vue';

export default {
    name: "AppFooter",
    data() {
        return {
            displayCreateAppModal: false,
        }
    },
    methods: {
		openCreateAppModal() {
			this.displayCreateAppModal = true;
		},
		closeCreateAppModal() {
			this.displayCreateAppModal = false;
		},
        footerImage() {
            return this.$appState.darkTheme ? '/images/logo-white.svg' : '/images/logo-dark.svg';
        }
    },
    computed: {
        darkTheme() {
            return this.$appState.darkTheme;
        }
    },
    components: {
        'AppTopBar': AppTopBar,
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
</style>