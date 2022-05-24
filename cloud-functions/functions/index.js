const functions = require("firebase-functions");
const admin = require("firebase-admin");
const generateApiKey = require("generate-api-key");

admin.initializeApp();

exports.createNewUserMetadata = functions.auth.user()
    .onCreate((user) => {
      return admin.firestore()
          .collection("users")
          .doc(user.uid)
          .create({"api-key": generateApiKey()});
    });
