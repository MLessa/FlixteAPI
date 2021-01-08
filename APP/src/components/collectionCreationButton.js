import React, { Component } from "react";
import { View, Text, StyleSheet, Image } from "react-native";
import images from "../images";
import { translate } from "../locales";

class collectionCreationButton extends Component {
  constructor(props) {
    super(props);
    this.state = {};
  }

  render() {
    const { stationCreationButton } = this.props;
    return (
      <View style={styles.content}>
        <View style={styles.container}>
          <Image
            resizeMode="contain"
            style={styles.logo}
            source={images.logoFade}
          />
          <Text style={styles.iconDescription}>
            Click to create a{" "}
            {stationCreationButton
              ? translate("station")
              : translate("playlist")}
          </Text>
        </View>
        <View style={styles.textContainer}>
          <Text style={styles.infoText}>
            {stationCreationButton
              ? translate("createStationInfoText")
              : translate("createPlaylistInfoText")}
          </Text>
          <Text style={styles.flixtYourself}>{translate("flixtYourself")}</Text>
        </View>
      </View>
    );
  }
}

const styles = StyleSheet.create({
  flixtYourself: {
    marginTop: 10,
    color: "#d46f02",
    fontFamily: "FranklinGothic"
  },
  infoText: {
    // textAlign: "justify",
    color: "#fefefe",
    fontFamily: "FranklinGothic",
    fontSize: 17
  },
  textContainer: {
    flex: 0.5,
    height: 100,
    marginLeft: 20
  },
  iconDescription: {
    fontFamily: "Heavitas",
    fontSize: 10,
    color: "#444444",
    marginTop: 10,
    alignSelf: "center"
  },
  logo: {
    width: 40,
    height: 40,
    alignSelf: "center"
  },
  container: {
    borderStyle: "dashed",
    borderColor: "#d46f02",
    justifyContent: "center",
    flex: 0.5,
    height: 100,
    borderStyle: "dashed",
    borderWidth: 2,
    position: "relative",
    overflow: "hidden",
    borderRadius: 5,
    padding: 5
  },
  content: {
    flex: 1,
    flexDirection: "row",
    marginLeft: 20,
    marginBottom: 25
  }
});

export default collectionCreationButton;
