const mutations = {
  setUser(state, payload) {
    state.user = payload;
  },
  setError(state, payload) {
    state.error = payload;
  },
  setApps(state, payload) {
    state.apps = payload;
  },
  setMenuInactive(state, payload) {
    state.isMenuInactive = payload;
  }
};

export default mutations;