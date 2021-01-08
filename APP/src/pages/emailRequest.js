import React, { Component } from "react";
import { View, Text, StyleSheet, TouchableHighlight } from "react-native";
import TextField from "../components/textField";
import Button from "../components/button";
import authenticationService from "../services/authenticationService";
import { translate } from "../locales";

var _emailTestTimeout = null;

export default class EmailRequest extends Component {
  constructor(props) {
    super(props);
    this.state = {
      email: null,
      disableRegisterButton: true,
      showLoader: false,
      emailValidationMessage: null
    };
  }

  validateEmail = email => {
    this.setState({
      disableRegisterButton: true,
      showLoader: true,
      emailValidationMessage: null
    });

    if (_emailTestTimeout) clearTimeout(_emailTestTimeout);

    _emailTestTimeout = setTimeout(() => {
      if (email.length >= 4) {
        let re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (re.test(String(email).toLowerCase())) {
          authenticationService.isUser(
            email,
            response => {
              console.log(response);
              if (response.data.Success) {
                this.setState({
                  disableRegisterButton: false,
                  showLoader: false,
                  emailValidationMessage: ""
                });
              } else {
                this.setState({
                  disableRegisterButton: true,
                  showLoader: false,
                  emailValidationMessage: translate("emailAlreadyTaken")
                });
              }
            },
            () => {
              this.setState({
                disableRegisterButton: true,
                showLoader: false,
                emailValidationMessage: translate("emailAlreadyTaken")
              });
            }
          );
        } else {
          this.setState({
            disableRegisterButton: true,
            showLoader: false,
            emailValidationMessage: translate("emailValidationMessage")
          });
        }
      } else if (email.length == 0) {
        this.setState({ showLoader: false, emailValidationMessage: null });
      } else {
        this.setState({
          showLoader: false,
          emailValidationMessage: translate("emailValidationMessage")
        });
      }
    }, 600);
  };

  render() {
    const { navigation } = this.props;
    const { push } = navigation;

    return (
      <View style={styles.container}>
        <Text style={[styles.infoText, styles.infoTextTitle]}>
          {translate("emailCreationTitle")}
        </Text>
        <TextField
          style={styles.baseComponentStyle}
          externalValidationMessage={this.state.emailValidationMessage}
          showLoader={this.state.showLoader}
          width="80%"
          placeholder={translate("email")}
          onUpdate={email => {
            this.validateEmail(email);
            this.setState({ email: email });
          }}
        />

        <Button
          style={styles.baseComponentStyle}
          width="80%"
          text={translate("next")}
          action={() => push("UsernameRequest", { email: this.state.email })}
          disabled={this.state.disableRegisterButton}
          googleBtn={true}
        />
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
