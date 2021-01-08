import baseService from "./baseService";
import AsyncStorage from "@react-native-community/async-storage";
import RNSecureKeyStore, { ACCESSIBLE } from "react-native-secure-key-store";
import { GoogleSignin, statusCodes } from "react-native-google-signin";

class authenticationService extends baseService {
  constructor() {
    super("Authentication");
  }

  logOut = callback => {
    RNSecureKeyStore.remove("accessToken").then(
      async () => {
        await AsyncStorage.removeItem("flxUser");
        const isSignedIn = await GoogleSignin.isSignedIn();
        if (isSignedIn) {
          await GoogleSignin.revokeAccess();
          await GoogleSignin.signOut();
        }
        callback();
      },
      err => {
        console.log(err);
      }
    );
  };

  registerAccessToken = accessToken => {
    RNSecureKeyStore.set("accessToken", accessToken, {
      accessible: ACCESSIBLE.ALWAYS_THIS_DEVICE_ONLY
    });
  };

  getLoginData = async (onLoggedInCallback, onLoggedOutCallback) => {
    RNSecureKeyStore.get("accessToken").then(
      async tokenID => {
        try {
          var loggedUser = JSON.parse(await AsyncStorage.getItem("flxUser"));
          const isGoogleSignedIn = await GoogleSignin.isSignedIn();
          let googleUserInfo = null;
          if (isGoogleSignedIn) {
            googleUserInfo = await GoogleSignin.getCurrentUser();
            if (!googleUserInfo)
              googleUserInfo = await GoogleSignin.signInSilently();
          }

          onLoggedInCallback({
            ...loggedUser,
            tokenID,
            googleUserInfo,
            isGoogleUser: googleUserInfo ? true : false
          });
        } catch (error) {
          if (error.code === statusCodes.SIGN_IN_REQUIRED) {
            onLoggedInCallback({ ...loggedUser, tokenID, isGoogleUser: false });
          } else if (error.code === statusCodes.IN_PROGRESS) {
          } else {
            // TODO: tratar esse erro
            console.log(error);
          }
        }
      },
      () => {
        onLoggedOutCallback();
      }
    );
  };

  isUser = async (email, successCallback, errorCallback) => {
    await this.post("isUser", email, null, successCallback, errorCallback);
  };

  callAPILogin = async (
    action,
    body,
    params,
    successCallback,
    errorCallback
  ) => {
    await this.post(
      action,
      body,
      params,
      async response => {
        let data = response.data;
        console.log(data);
        if (data.Success) {
          this.registerAccessToken(data.Data.Token);
          await AsyncStorage.setItem("flxUser", JSON.stringify(data.Data.User));
          successCallback(data);
        } else errorCallback(data);
      },
      errorCallback
    );
  };

  tryLoginWithGoogle = async (model, successCallback, errorCallback) => {
    await this.post(
      "TryLoginWithGoogle",
      model,
      null,
      async response => {
        if (response.data.Success) {
          let data = response.data;
          this.registerAccessToken(data.Data.Token);
          await AsyncStorage.setItem("flxUser", JSON.stringify(data.Data.User));
          successCallback(data);
        } else {
          successCallback(null);
        }
      },
      errorCallback
    );
  };

  registerWithGoogle = async (model, successCallback, errorCallback) => {
    await this.post(
      "RegisterWithGoogle",
      model,
      null,
      async response => {
        if (response.data.Success) {
          let data = response.data;
          this.registerAccessToken(data.Data.Token);
          await AsyncStorage.setItem("flxUser", JSON.stringify(data.Data.User));
          successCallback(data);
        } else {
          successCallback(null);
        }
      },
      errorCallback
    );
  };

  login = async (model, successCallback, errorCallback) => {
    await this.callAPILogin(
      "LoginAPP",
      model,
      null,
      successCallback,
      errorCallback
    );
  };

  registerUser = async (user, successCallback, errorCallback) => {
    await this.callAPILogin(
      "RegisterApp",
      user,
      null,
      successCallback,
      errorCallback
    );
  };
}

export default new authenticationService();
