import { createStore } from 'vuex'
import mutations from "./auth-mutations";
import actions from "./auth-actions";
import getters from "./auth-getters";

import VuexPersistence from 'vuex-persist'

const vuexLocal = new VuexPersistence({
  storage: window.localStorage
})

const initialState = () => {
  return { user: null, error: null };
};

export default createStore({
  state: initialState(),
  mutations: mutations,
  actions: actions,
  getters: getters,
  plugins: [vuexLocal.plugin]
});