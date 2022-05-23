import firebase from "firebase/compat/app";

const actions = {
  authAction({ commit }) {
    firebase.auth().setPersistence(firebase.auth.Auth.Persistence.LOCAL);

    firebase.auth().onAuthStateChanged(user => {
      if (user) {
        commit("setUser", user);
      } else {
        commit("setUser", null);
      }
    });
  },
  signUpAction({ commit }, payload) {
    firebase
      .auth()
      .createUserWithEmailAndPassword(payload.email, payload.password)
      .catch(error => {
        commit("setError", error.message);
      });
  },
  signInAction({ commit }, payload) {
    return new Promise((resolve, reject) => {
      firebase
      .auth()
      .signInWithEmailAndPassword(payload.email, payload.password)
      .then(() => {resolve();})
      .catch(error => {
        console.log(error.message);
        reject(error.message)
        commit("setError", error.message);
      });
    });
  },
  signOutAction({ commit }) {
    return new Promise((resolve, reject) => {
    firebase
      .auth()
      .signOut()
      .then(() => {
        commit("setUser", null);
        resolve();
      })
      .catch(error => {
        commit("setError", error.message);
        reject(error.message);
      });
    });
  }
};

export default actions;