import React, { Component } from "react";
import { View, Text, StyleSheet, TouchableHighlight } from "react-native";
import TextField from "../components/textField";
import Button from "../components/button";
import authenticationService from "../services/authenticationService";
import LoaderOverlay from "../components/loaderOverlay";
import InfoOverlay from "../components/infoOverlay";

import { translate } from "../locales";

export default class PasswordRequest extends Component {
  constructor(props) {
    super(props);
    const { navigation } = this.props;

    this.state = {
      user: {
        password: null,
        username: navigation.getParam("username", "tapioca"),
        email: navigation.getParam("email", null)
      },
      passwordValidationPassed: false,
      showloadingOverlay: false,
      showInfoOverlay: false,
      overlayInfo: null
    };
  }

  validatePassword = text => {
    if (text.length < 4) {
      this.setState({ passwordValidationPassed: false });
      return translate("passwordValidationMessage");
    }
    let reg = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$/;
    if (reg.test(text) === true) {
      this.setState({ passwordValidationPassed: true });
      return null;
    } else {
      this.setState({ passwordValidationPassed: false });
      return translate("passwordValidationMessage");
    }
  };

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

  render() {
    return (
      <View style={styles.container}>
        <LoaderOverlay isVisible={this.state.showloadingOverlay} />
        <InfoOverlay
          visibilityController={this.state.showInfoOverlay}
          info={this.state.overlayInfo}
        />
        <Text style={[styles.infoText, styles.infoTextTitle]}>
          {translate("passwordCreationTitle")}
        </Text>
        <Text style={styles.infoText}>{translate("passwordCreationInfo")}</Text>
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
          disabled={!this.state.passwordValidationPassed}
          action={this.register}
        />
      </View>
    );
  }
}

const styles = StyleSheet.create({
  infoTextTitle: {
    fontSize: 20
  },
  infoText: {
    fontFamily: "Heavitas",
    fontSize: 11,
    color: "#fefefe",
    padding: 20
  },
  baseComponentStyle: {
    marginTop: 20
  },
  container: {
    flex: 1,
    backgroundColor: "#221f1f",
    flexDirection: "column",
    justifyContent: "flex-start",
    alignItems: "center"
  }
});
