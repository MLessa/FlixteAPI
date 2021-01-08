import React, { Component } from "react";
import { View, StyleSheet, Image, ActivityIndicator } from "react-native";
import Button from "../components/button";
import authenticationService from "../services/authenticationService";
import { Avatar } from "react-native-elements";
import { withNavigation } from "react-navigation";
import { NavigationEvents } from "react-navigation";

class headerBar extends Component {
  state = {
    isLoggedIn: false,
    isHeaderLoaded: false
  };

  async componentDidMount() {
    await this.checkLoginState();
  }

  checkLoginState = async () => {
    await authenticationService.getLoginData(
      async response => {
        this.setState({
          ...this.state,
          userData: response,
          isLoggedIn: true,
          isHeaderLoaded: true
        });
      },
      async () => {
        this.setState({
          ...this.state,
          isLoggedIn: false,
          isHeaderLoaded: true
        });
      }
    );
  };

  logout = async () => {
    await authenticationService.logOut(this.checkLoginState);
  };

  keepLoginState = () => {
    this.checkLoginState();
  };

  renderUserHeaderSide = () => {
    const { navigation } = this.props;
    const { push } = navigation;
    if (this.state.isHeaderLoaded) {
      if (this.state.isLoggedIn) {
        if (this.state.userData.isGoogleUser) {
          return (
            <Avatar
              rounded
              source={{
                uri: this.state.userData.googleUserInfo.user.photo
              }}
              onPress={() =>
                push("Profile", { userData: this.state.userData })
              }
            />
          );
        } else {
          return (
            <Avatar
              rounded
              title={this.state.userData.Title}
              onPress={() =>
                push("Profile", { userData: this.state.userData })
              }
            />
          );
        }
      } else {
        return <Button width={100} text="LOGIN" action={() => push("Login")} />;
      }
    } else {
      return <ActivityIndicator size="large" color="#d46f02" />;
    }
  };

  render() {
    return (
      <View style={styles.container}>
        <NavigationEvents onWillFocus={() => this.checkLoginState()} />
        <Image
          resizeMode="contain"
          style={styles.logo}
          source={require("../images/icon-flixte.png")}
        />

        {this.renderUserHeaderSide()}
      </View>
    );
  }
}

const styles = StyleSheet.create({
  container: {
    display: "flex",
    marginBottom: 20,
    backgroundColor: "transparent",
    height: 45,
    justifyContent: "space-between",
    flexDirection: "row",
    padding: 10,
    marginTop: 20
  },
  logo: {
    width: 50,
    height: 50
  }
});

export default withNavigation(headerBar);
