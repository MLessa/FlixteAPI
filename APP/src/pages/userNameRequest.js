import React, { Component } from "react";
import { View, Text, StyleSheet, TouchableHighlight } from "react-native";
import TextField from "../components/textField";
import Button from "../components/button";
import authenticationService from "../services/authenticationService";
import { translate } from "../locales";

var _usernameTestTimeout = null;

export default class userNameRequest extends Component {
  constructor(props) {
    super(props);
    const { navigation } = this.props;
    this.state = {
      username: null,
      email: navigation.getParam("email", null),
      googleUserData: navigation.getParam("googleUserData", null),
      disableRegisterButton: true,
      showLoader: false,
      usernameValidationMessage: null
    };
  }

  validateUsername = username => {
    this.setState({
      disableRegisterButton: true,
      showLoader: true,
      usernameValidationMessage: null
    });

    if (_usernameTestTimeout) clearTimeout(_usernameTestTimeout);
    _usernameTestTimeout = setTimeout(() => {
      if (username.length >= 4) {
        this.setState({ showLoader: true });
        authenticationService.isUser(
          username,
          response => {
            if (response.data.Success) {
              this.setState({
                disableRegisterButton: false,
                showLoader: false,
                usernameValidationMessage: translate("thisNameIsFree")
              });
            } else {
              this.setState({
                disableRegisterButton: true,
                showLoader: false,
                usernameValidationMessage: translate("usernameAlreadyTaken")
              });
            }
          },
          () => {
            this.setState({
              disableRegisterButton: true,
              showLoader: false,
              usernameValidationMessage: translate("usernameAlreadyTaken")
            });
          }
        );
      } else if (username.length == 0)
        this.setState({ showLoader: false, usernameValidationMessage: null });
      else {
        this.setState({
          showLoader: false,
          usernameValidationMessage: translate("usernameLength")
        });
      }
    }, 600);
  };

  registerWithGoogle = async () => {
    const { navigation } = this.props;
    const { popToTop } = navigation;
    authenticationService.registerWithGoogle(
      {
        ...this.state.googleUserData,
        username: this.state.username
      },
      data => {
        console.log(data);
        popToTop();
      }, //Go back to the main screen
      () => console.log("error")
    );
  };

  render() {
    const { navigation } = this.props;
    const { push } = navigation;

    return (
      <View style={styles.container}>
        <Text style={[styles.infoText, styles.infoTextTitle]}>
          {translate("usernameCreationTitle")}
        </Text>
        <Text style={styles.infoText}>{translate("usernameCreationInfo")}</Text>
        <TextField
          style={styles.baseComponentStyle}
          externalValidationMessage={this.state.usernameValidationMessage}
          showLoader={this.state.showLoader}
          width="80%"
          placeholder={translate("username")}
          onUpdate={username => {
            this.validateUsername(username);
            this.setState({ username: username });
          }}
        />

        {this.state.email ? (
          <Button
            style={styles.baseComponentStyle}
            width="80%"
            text={translate("next")}
            action={() =>
              push("PasswordRequest", {
                username: this.state.username,
                email: this.state.email
              })
            }
            disabled={this.state.disableRegisterButton}
          />
        ) : (
          <Button
            style={styles.baseComponentStyle}
            width="80%"
            text={translate("continueWithGoogle")}
            action={this.registerWithGoogle}
            disabled={this.state.disableRegisterButton}
            googleBtn={true}
          />
        )}
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
