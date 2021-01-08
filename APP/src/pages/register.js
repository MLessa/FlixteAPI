import React, { Component } from "react";
import {
  View,
  Image,
  StyleSheet,
  Text,
  TouchableHighlight
} from "react-native";
import TextField from "../components/textField";
import Button from "../components/button";
import { translate } from "../locales";
import authenticationService from "../services/authenticationService";
import LoaderOverlay from "../components/loaderOverlay";
import InfoOverlay from "../components/infoOverlay";

export default class RegisterUser extends Component {
  constructor(props) {
    super(props);
    this.state = {
      showInfoOverlay: false,
      overlayInfo: null,
      nameValidationPassed: false,
      emailValidationPassed: false,
      passwordValidationPassed: false,
      showloadingOverlay: false,
      disableRegisterButton: true,
      user: {
        username: "",
        password: ""
      }
    };
  }

  componentDidCatch(error, info) {
    this.setState({ showloadingOverlay: false });
  }

  register = async () => {
    const { navigation } = this.props;
    const { popToTop } = navigation;

    this.setState({ showloadingOverlay: true });
    await authenticationService.registerUser(
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

  validateUsername = username => {
    if (username.length >= 4) {
      authenticationService.isNewUsernameValid(
        username,
        response => {
          if (response.data.Success) {
            this.setState({ disableRegisterButton: false });
            return null;
          }
        },
        response => {
          console.log(response);
          this.setState({ disableRegisterButton: true });
          return translate("usernameAlreadyTaken");
        }
      );
    }
  };

  validatePassword = text => {
    if (text.length == 0) return null;
    let reg = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$/;
    if (reg.test(text) === true) {
      this.setState({ passwordValidationPassed: true });

      if (this.state.emailValidationPassed && this.state.user.name.length >= 3)
        this.setState({ disableRegisterButton: false });
      else this.setState({ disableRegisterButton: true });

      return null;
    } else {
      this.setState({ disableRegisterButton: true });
      this.setState({ passwordValidationPassed: false });
      return translate("passwordValidationMessage");
    }
  };

  render() {
    const { navigation } = this.props;
    const { pop, push } = navigation;
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
        <Text style={styles.caption}>{translate("createAccount")}</Text>
        <TextField
          style={styles.baseComponentStyle}
          width="80%"
          placeholder={translate("username")}
          validationFunction={this.validateUsername}
          onUpdate={username => {
            this.setState({ user: { ...this.state.user, username } });
          }}
        />
        <TextField
          secure={true}
          style={styles.baseComponentStyle}
          width="80%"
          placeholder={translate("password")}
          validationFunction={this.validatePassword}
          onUpdate={password => {
            this.setState({ user: { ...this.state.user, password } });
          }}
        />
        <Button
          style={styles.baseComponentStyle}
          width="80%"
          text={translate("register")}
          disabled={this.state.disableRegisterButton}
          action={this.register}
        />
        <View style={styles.divider} />
        <Button
          style={styles.baseComponentStyle}
          width="80%"
          text={translate("registerGoogle").toUpperCase()}
          action={() => push("UsernameRequest")}
          googleBtn={true}
        />
        <TouchableHighlight onPress={() => pop()}>
          <Text style={[styles.alreadyHaveAccount, styles.baseComponentStyle]}>
            {translate("alreadyHaveAnAccount")}
          </Text>
        </TouchableHighlight>
      </View>
    );
  }
}

const styles = StyleSheet.create({
  alreadyHaveAccount: {
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
    alignItems: "center"
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
