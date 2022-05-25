const getters = {
    getUser(state) {
      return state.user;
    },
    isUserAuth(state) {
      return state.user;
    },
    getError(state) {
      return state.error;
    },
    getSelectedApp(state) {
      return state.currentApp;
    },
    getApps(state) {
      return state.apps;
    }
  };
  
  export default getters;