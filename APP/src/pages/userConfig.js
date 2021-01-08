import React, { Component } from "react";
import { View, Text, StyleSheet, ScrollView } from "react-native";
import authenticationService from "../services/authenticationService";
import { Avatar } from "react-native-elements";
import Button from "../components/button";
import SectionTitle from "../components/sectionTitle";
import { translate } from "../locales";

export default class userConfig extends Component {
  constructor(props) {
    super(props);
    this.state = {
      userData: props.navigation.getParam("userData", null)
    };
    console.log(this.state.userData);
  }

  logout = async () => {
    const { navigation } = this.props;
    const { popToTop } = navigation;
    await authenticationService.logOut(() => popToTop());
  };

  render() {
    return (
      <View style={styles.container}>
        <SectionTitle
          title={translate("settings").toUpperCase()}
          useTopShadow={true}
          stickyTop={true}
        />
        <View style={styles.userInfoView}>
          {this.state.userData.isGoogleUser ? (
            <Avatar
              size="medium"
              rounded
              source={{
                uri: this.state.userData.googleUserInfo.user.photo
              }}
              onPress={() => this.logout()}
            />
          ) : (
            <Avatar
              size="medium"
              rounded
              title={this.state.userData.Title}
              onPress={() => this.logout()}
            />
          )}
          <Text>@{this.state.userData.username}</Text>
        </View>
        <ScrollView style={styles.optionsView}>
          <View>
            <Button width={100} text="Logoff" action={() => this.logout()} />
          </View>
        </ScrollView>
      </View>
    );
  }
}

const styles = StyleSheet.create({
  optionsView: {
    marginTop: 30
  },
  userInfoView: {
    alignItems: "flex-start",
    flexDirection: "row",
    justifyContent: "flex-start"
  },
  container: {
    paddingTop: 15,
    flex: 1,
    backgroundColor: "#221f1f"
    // justifyContent: "center",
    // alignItems: "center"
  }
});
