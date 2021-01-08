import React, { Component } from "react";
import {
  View,
  Image,
  StyleSheet,
  Text,
  TouchableHighlight,
  ScrollView
} from "react-native";
import TextField from "../components/textField";
import Button from "../components/button";
import authenticationService from "../services/authenticationService";
import { translate } from "../locales";
import { GoogleSignin } from "react-native-google-signin";
import LoaderOverlay from "../components/loaderOverlay";
import InfoOverlay from "../components/infoOverlay";

export default class login extends Component {
  constructor(props) {
    super(props);
    this.state = {
      user: {
        email: "",
        password: ""
      },
      showloadingOverlay: false,
      showInfoOverlay: false,
      overlayInfo: null
    };
  }

  login = async () => {
    const { navigation } = this.props;
    const { popToTop } = navigation;

    this.setState({ showloadingOverlay: true });
    await authenticationService.login(
      this.state.user,
      () => {
        this.setState({ showloadingOverlay: false });
        popToTop();
      },
      error => {
        this.setState({
          showloadingOverlay: false,
          showInfoOverlay: true,
          overlayInfo: error.Message
        });
        setTimeout(() => this.setState({ showInfoOverlay: false }), 2000);
      }
    );
  };

  loginGoogle = async () => {
    try {
      const { navigation } = this.props;
      const { popToTop, push } = navigation;

      let allowProceed = await GoogleSignin.hasPlayServices();
      if (allowProceed) {
        const userInfo = await GoogleSignin.signIn();
        authenticationService.tryLoginWithGoogle(
          userInfo.idToken,
          response => {
            console.log(response);
            if (response) {
              popToTop();
            } else {
              push("UsernameRequest", {
                googleUserData: {
                  serverAuthCode: userInfo.serverAuthCode,
                  idToken: userInfo.idToken,
                  googleID: userInfo.user.id
                }
              });
            }
          },
          () => {
            console.log(1);
            this.setState({
              showInfoOverlay: true,
              overlayInfo: translate("somethingWentWrong")
            });
            setTimeout(() => this.setState({ showInfoOverlay: false }), 2000);
          }
        );
      } else {
        this.setState({
          showInfoOverlay: true,
          overlayInfo: translate("playServiceNotExists")
        });
        setTimeout(() => this.setState({ showInfoOverlay: false }), 2000);
      }
    } catch (error) {
      if (error.code === statusCodes.SIGN_IN_CANCELLED) {
        // user cancelled the login flow
      } else if (error.code === statusCodes.IN_PROGRESS) {
        // operation (f.e. sign in) is in progress already
      } else if (error.code === statusCodes.PLAY_SERVICES_NOT_AVAILABLE) {
        // play services not available or outdated
      } else {
        // some other error happened
      }
    }
  };

  render() {
    const { navigation } = this.props;
    const { push } = navigation;

    return (
      <View style={styles.container}>
        <LoaderOverlay isVisible={this.state.showloadingOverlay} />
        <InfoOverlay
          visibilityController={this.state.showInfoOverlay}
          info={this.state.overlayInfo}
        />
        <Image
          resizeMode="contain"
          style={styles.logo}
          source={require("../images/logo-flixte.png")}
        />
        <Text style={styles.caption}>LOGIN</Text>
        <TextField
          style={styles.baseComponentStyle}
          width="70%"
          placeholder={translate("emailOrUsername")}
          onUpdate={email =>
            this.setState({ user: { ...this.state.user, email: email } })
          }
        />
        <TextField
          secure={true}
          style={styles.baseComponentStyle}
          width="70%"
          placeholder={translate("password")}
          onUpdate={password =>
            this.setState({
              user: { ...this.state.user, password: password }
            })
          }
        />
        <Button
          style={styles.baseComponentStyle}
          width="70%"
          text={translate("login")}
          action={this.login}
        />

        <View style={styles.divider} />

        <Button
          style={styles.baseComponentStyle}
          width="70%"
          text={translate("enterGoogle")}
          action={this.loginGoogle}
          googleBtn={true}
        />
        <TouchableHighlight onPress={() => push("EmailRequest")}>
          <Text style={[styles.haventAccount, styles.baseComponentStyle]}>
            {translate("haventAccountYet")}
          </Text>
        </TouchableHighlight>
      </View>
    );
  }
}

const styles = StyleSheet.create({
  haventAccount: {
    color: "white",
    fontSize: 15,
    textDecorationLine: "underline",
    fontFamily: "FranklinGothic",
    fontWeight: "100",
    letterSpacing: 1
  },
  baseComponentStyle: {
    marginTop: 20
  },
  divider: {
    borderTopWidth: 2,
    borderBottomWidth: 0,
    borderLeftWidth: 0,
    borderRightWidth: 0,
    borderTopColor: "#444444",
    width: "60%",
    marginTop: 20
  },
  container: {
    flex: 1,
    backgroundColor: "#221f1f",
    flexDirection: "column",
    justifyContent: "flex-start",
    alignItems: "center",
    paddingTop: 30
  },
  caption: {
    color: "#fff",
    marginTop: 8,
    fontFamily: "Heavitas",
    fontSize: 20
  },
  logo: {
    width: "50%"
  }
});
