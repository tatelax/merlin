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
    apps: [
      {name: 'Hello World!', code: 'hw'},
      {name: 'Example', code: 'example'}
    ]
  };
};

export default createStore({
  state: initialState(),
  mutations: mutations,
  actions: actions,
  getters: getters,
  plugins: [vuexLocal.plugin]
});