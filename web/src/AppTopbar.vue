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
			selector: '@next', enterClass: 'hidden', enterActiveClass: 'scalein',
			leaveToClass: 'hidden', leaveActiveClass: 'fadeout', hideOnOutsideClick: true
		}">
			<i class="pi pi-ellipsis-v"></i>
		</button>
		<ul class="layout-topbar-menu hidden lg:flex origin-top">
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

export default {
	data() {
		return {
			items: [
				{
					label: 'Sign Out',
					icon: 'pi pi-sign-out',
					command: () => {
						this.signOut();
					}
				}
			]
		}
	},
	methods: {
		...mapActions(["signOutAction"]),
		signOut() {
			this.signOutAction().then(() => {
                    this.$router.push('/login');
            });
		},
		onMenuToggle(event) {
			this.$emit('menu-toggle', event);
		},
		onTopbarMenuToggle(event) {
			this.$emit('topbar-menu-toggle', event);
		},
		topbarImage() {
			return this.$appState.darkTheme ? './images/logo-white.svg' : 'images/logo-dark.svg';
		},
		toggleProfileMenu(event) {
			this.$refs.menu.toggle(event);
		}
	},
	computed: {
		darkTheme() {
			return this.$appState.darkTheme;
		}
	}
}
</script>