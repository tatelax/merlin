<template>
    <div class="surface-0 flex align-items-center justify-content-center min-h-screen min-w-screen overflow-hidden">
        <div class="grid justify-content-center p-2 lg:p-0" style="min-width:80%">
            <div class="col-12 mt-5 xl:mt-0 text-center">
                <img :src="'layout/images/logo-' + logoColor + '.svg'" alt="Sakai logo" class="mb-5"
                    style="width:81px; height:60px;">
            </div>
            <div class="col-12 xl:col-6"
                style="border-radius:56px; padding:0.3rem; background: linear-gradient(180deg, var(--primary-color), rgba(33, 150, 243, 0) 30%);">
                <div class="h-full w-full m-0 py-7 px-4"
                    style="border-radius:53px; background: linear-gradient(180deg, var(--surface-50) 38.9%, var(--surface-0));">
                    <div class="text-center mb-5">
                        <img src="layout/images/avatar.png" alt="Image" height="50" class="mb-3">
                        <div class="text-900 text-3xl font-medium mb-3">Welcome!</div>
                        <span class="text-600 font-medium">Sign in to continue</span>
                    </div>
<!--LOGIN-->
                    <TabView ref="auth" style="">
                        <TabPanel header="Login">
                            <div class="w-full md:w-10 mx-auto">
                                <label for="email1" class="block text-900 text-xl font-medium mb-2">Email</label>
                                <InputText id="email1" v-model="email" type="text" class="w-full mb-3"
                                    :class="{ 'p-invalid': (!validateEmail() && login_BadEmail) || loginFailed }" placeholder="Email"
                                    style="padding:1rem;" />

                                <label for="password1" class="block text-900 font-medium text-xl mb-2">Password</label>
                                <Password id="password1" v-model="password" placeholder="Password" :toggleMask="true"
                                    class="w-full mb-3" :class="{ 'p-invalid': (!validatePassword() && login_BadPassword) || loginFailed }"
                                    inputClass="w-full" inputStyle="padding:1rem" :feedback="false"></Password>

                                <div class="flex align-items-center justify-content-between mb-5">
                                    <div class="flex align-items-center">
                                        <Checkbox id="rememberme1" v-model="rememberMeChecked" :binary="true" class="mr-2">
                                        </Checkbox>
                                        <label for="rememberme1">Remember me</label>
                                    </div>
                                    <a class="font-medium no-underline ml-2 text-right cursor-pointer"
                                        style="color: var(--primary-color)">Forgot password?</a>
                                </div>
                                <Button label="Sign In" class="w-full p-3 text-xl" @click.prevent="login()"></button>
                            </div>
                        </TabPanel>
<!--REGISTER-->
                        <TabPanel header="Register">
                            <div class="w-full md:w-10 mx-auto">
                                <label for="email1" class="block text-900 text-xl font-medium mb-2">Email</label>
                                <InputText id="email1" v-model="email" type="text" class="w-full mb-3"
                                    :class="{ 'p-invalid': !validateEmail() && register_BadEmail}" placeholder="Email"
                                    style="padding:1rem;" />

                                <label for="password1" class="block text-900 font-medium text-xl mb-2">Password</label>
                                <Password id="password1" v-model="password" placeholder="Password" :toggleMask="true"
                                    class="w-full mb-3" :class="{ 'p-invalid': !validatePassword() && register_BadPassword }"
                                    inputClass="w-full" inputStyle="padding:1rem"></Password>

                                <div class="flex align-items-center justify-content-between mb-5">
                                    <a class="font-medium no-underline ml-2 text-right cursor-pointer"
                                        style="color: var(--primary-color)">Forgot password?</a>
                                </div>
                                <Button label="Register" class="w-full p-3 text-xl"
                                    @click.prevent="register()"></button>
                            </div>
                        </TabPanel>
                    </Tabview>
                </div>
            </div>
        </div>
    </div>
</template>

<script>

import { useVuelidate } from "@vuelidate/core";
import { mapActions, mapGetters } from "vuex";

export default {
    setup: () => ({ v$: useVuelidate() }),
    data() {
        return {
            email: '',
            password: '',
            rememberMeChecked: false,
            login_BadEmail: false,
            login_BadPassword : false,
            register_BadEmail: false,
            register_BadPassword: false,
            loginFailed : false,
            validationErrors: [],
            tabItems: [
                { label: 'Home', icon: 'pi pi-fw pi-home', to: '/error' },
                { label: 'Calendar', icon: 'pi pi-fw pi-calendar' },
                { label: 'Edit', icon: 'pi pi-fw pi-pencil' },
                { label: 'Documentation', icon: 'pi pi-fw pi-file' },
                { label: 'Settings', icon: 'pi pi-fw pi-cog' }
            ]
        }
    },
    computed: {
        ...mapGetters(["isUserAuth"]),
        logoColor() {
            if (this.$appState.darkTheme) return 'white';
            return 'dark';
        }
    },
    mounted() {
        this.authAction();
    },
    methods: {
        ...mapActions(["signInAction", "signUpAction", "authAction"]),
        resetError() {
            this.validationErrors = [];
        },
        validateEmail() {
            if (!this.email) {
                return false;
            }
            if (/.+@.+/.test(this.email) != true) {
                return false;
            }

            return true;
        },
        validatePassword() {
            if (!this.password) {
                return false;
            }
            if (/.{6,}/.test(this.password) != true) {
                return false;
            }

            return true;
        },
        login() {
            if(!this.validateEmail()) {
                this.login_BadEmail = true;
            } else {
                this.login_BadEmail = false;
            }

            if(!this.validatePassword()) {
                this.login_BadPassword = true;
            } else {
                this.login_BadPassword = false;
            }

            if(this.login_BadEmail || this.login_BadPassword) {
                return;
            }

            this.signInAction({ email: this.email, password: this.password }).then(() => {
                this.$router.push('/');
            }).catch(() => {
                this.login_BadEmail = true;
                this.login_BadPassword = true;
                this.loginFailed = true;
            });
        },
        register() {
            if(!this.validateEmail()) {
                this.login_BadEmail = true;
            } else {
                this.login_BadEmail = false;
            }

            if(!this.validatePassword()) {
                this.login_BadPassword = true;
            } else {
                this.login_BadPassword = false;
            }

            if(this.login_BadEmail || this.login_BadPassword) {
                return;
            }

            this.signUpAction({ email: this.email, password: this.password }).then(() => {
                this.$router.push('/login');
            }).catch(() => {
                this.login_BadEmail = true;
                this.login_BadPassword = true;
                this.loginFailed = true;
            });
        }
    }
}
</script>

<style scoped>
.pi-eye {
    transform: scale(1.6);
    margin-right: 1rem;
}

.pi-eye-slash {
    transform: scale(1.6);
    margin-right: 1rem;
}
</style>