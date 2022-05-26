import { createStore } from 'vuex'
import mutations from "./mutations";
import actions from "./actions";
import getters from "./getters";

import VuexPersistence from 'vuex-persist'

const vuexLocal = new VuexPersistence({
  storage: window.localStorage
})

const initialState = () => {
  return {
    user: null,
    error: null,
    selectedApp: null,
    apps: null
  };
};

export default createStore({
  state: initialState(),
  mutations: mutations,
  actions: actions,
  getters: getters,
  plugins: [vuexLocal.plugin]
});