import { getAuth, createUserWithEmailAndPassword, signInWithEmailAndPassword } from "firebase/auth";
import { addDoc, collection } from "firebase/firestore";
import { db } from "../plugins/firebase";

const actions = {
  signUpAction({ commit }, payload) {
    createUserWithEmailAndPassword(payload.email, payload.password)
      .catch(error => {
        commit("setError", error.message);
      });
  },
  signInAction({ commit }, payload) {
    return new Promise((resolve, reject) => {
      signInWithEmailAndPassword(payload.email, payload.password)
        .then(() => { resolve(); })
        .catch(error => {
          reject(error.message)
          commit("setError", error.message);
        });
    });
  },
  signOutAction({ commit }) {
    return new Promise((resolve, reject) => {
      getAuth()
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
  },
  setSelectedAppAction({ commit }, payload) {
    commit("setSelectedApp", payload);
  },
  createNewAppAction({ commit }, payload) {
    return new Promise((resolve, reject) => {
      addDoc(collection(db, "apps"), {
        name: payload.name
      })
      .then(function () {
        commit("setSelectedApp", payload)
        resolve();
      })
    .catch(error => {
        reject(error);
      });
    })
  }
};

export default actions;