import React, { Component } from "react";
import { StyleSheet, Text, View } from "react-native";
import ShadowView from "react-native-simple-shadow-view";

export default class SectionTitle extends Component {
  render() {
    let { title, useTopShadow, stickyTop, style } = this.props;

    if (useTopShadow)
      return (
        <View style={[(stickyTop ? styles.titleBar : ''), style]}>
          <View style={{ height: 10, backgroundColor: "#1c1a1a" }}>
            <ShadowView style={styles.shadowBox} />
          </View>
          <View style={{ height: 50, backgroundColor: "#1c1a1a" }}>
            <Text style={styles.text}>{title}</Text>
          </View>
        </View>
      );
    else return <Text style={[styles.text, style]}>{title}</Text>;
  }
}

const styles = StyleSheet.create({
  titleBar:{
    top: -15
  },
  text: {
    color: "#d46f02",
    fontFamily: "Heavitas",
    fontSize: 17,
    textAlign: "left",
    marginLeft: 20,
    marginBottom: 10,
    paddingTop: 15
  },
  shadowBox: {
    flex: 1,
    backgroundColor: "#1c1a1a",

    shadowColor: "black",
    shadowOpacity: 20.5,
    shadowRadius: 6.5,
    shadowOffset: { width: 1.7, height: 1.1 }
  }
});
