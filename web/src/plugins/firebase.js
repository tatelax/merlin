import { initializeApp } from "firebase/app";
import { getFirestore, collection, query, where, getDocs} from "firebase/firestore";
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
    var apps = await getApps(user.uid);
    store.commit("setApps", apps);
    store.commit("setUser", user);
  } else {
    store.commit("setUser", null);
  }
});

async function getApps(userID) {
  var q = query(collection(db, "apps"), where("user", "==", userID));
  var querySnapshot = await getDocs(q);
  var data = new Map();

  for(let i = 0; i < querySnapshot.docs.length; i++) {
    data.set(querySnapshot.docs[i].id, querySnapshot.docs[i].data());
  }

  return data;
}

export{
  db
}