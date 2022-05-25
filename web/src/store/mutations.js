const mutations = {
    setUser(state, payload) {
      state.user = payload;
    },
    setError(state, payload) {
      state.error = payload;
    },
    setSelectedApp(state, payload) {
      state.selectedApp = payload;
    },
    setApps(state, payload) {
      state.apps = payload;
    }
  };
  
  export default mutations;