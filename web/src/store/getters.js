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
  getApps(state) {
    return state.apps;
  },
  getIsMenuInactive(state) {
    return state.isMenuInactive;
  }
};

export default getters;