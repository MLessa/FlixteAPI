import React, { Component } from "react";
import { View, Text, StyleSheet, ImageBackground } from "react-native";
import { translate } from "../locales";

const LargeVideoCard = ({
  thumbURL,
  cardName,
  nextVideoTitle,
  nextVideoDuration
}) => {
  return (
    <View style={styles.stationCard}>
      <ImageBackground source={{ uri: thumbURL }} style={styles.imageContainer}>
        {cardName ? (
          <View style={styles.stationFooter}>
            <Text style={styles.stationTitle}>{cardName}</Text>
          </View>
        ) : null}
      </ImageBackground>

      {nextVideoTitle ? (
        <View style={{ marginTop: 10 }}>
          <View
            style={{
              justifyContent: "space-between",
              flexDirection: "row",
              alignContent: "space-between"
            }}
          >
            <Text
              style={{ fontFamily: "Heavitas", fontSize: 12, color: "#fefefe" }}
            >
              {translate("nextVideo").toUpperCase()}
            </Text>
            <Text
              style={{
                fontFamily: "Heavitas",
                fontSize: 11,
                color: "#fefefe"
              }}
            >
              {nextVideoDuration} {translate("min")}
            </Text>
          </View>
          <Text
            style={{
              fontFamily: "Heavitas",
              fontSize: 15,
              color: "#fefefe",
              marginTop: 2
            }}
          >
            {nextVideoTitle}
          </Text>
        </View>
      ) : null}
    </View>
  );
};

const styles = StyleSheet.create({
  stationFooter: {
    height: 40,
    backgroundColor: "rgba(212, 111, 2, 0.85)",
    flexDirection: "row",
    alignItems: "center"
  },
  imageContainer: {
    flex: 1,
    flexDirection: "column-reverse"
  },
  stationCard: {
    width: 310,
    height: 230,
    marginLeft: -5
  },
  stationTitle: {
    fontFamily: "Heavitas",
    fontSize: 20,
    color: "#fff",
    letterSpacing: 1,
    textTransform: "uppercase",
    marginLeft: 5,
    marginTop: "3%"
  }
});

export default LargeVideoCard;
