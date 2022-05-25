import { initializeApp } from "firebase/app";
import { getFirestore } from "firebase/firestore";
import { onAuthStateChanged, browserLocalPersistence, setPersistence, getAuth } from "firebase/auth";
import store from '../store'

// Firebase app config
const config = {
  apiKey: process.env.VUE_APP_API_KEY,
  authDomain: process.env.VUE_APP_AUTH_DOMAIN,
  databaseURL: process.env.VUE_APP_DATABASE_URL,
  projectId: process.env.VUE_APP_PROJECT_ID,
  storageBucket: process.env.VUE_APP_STORAGE_BUCKET,
  messagingSenderId: process.env.VUE_APP_MESSAGING_SENDER_ID,
  appId: process.env.VUE_APP_APP_ID
};

// Init our firebase app
const app = initializeApp(config);
const auth = getAuth();
// Initialize Cloud Firestore and get a reference to the service
const db = getFirestore(app);

setPersistence(auth, browserLocalPersistence);

onAuthStateChanged(auth, async user => {
  if (user) {
    store.commit("setUser", user);
  } else {
    store.commit("setUser", null);
  }
});

export{
  db
}