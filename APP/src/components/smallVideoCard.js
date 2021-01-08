import React, { Component } from "react";
import { View, Text, StyleSheet, ImageBackground } from "react-native";

const SmallVideoCard = ({ thumbURL, cardName, cardDescription }) => {
  return (
    <View>
      <View style={styles.content}>
        <ImageBackground source={{ uri: thumbURL }} style={styles.imageBkg}>
          <View style={styles.cardNameFooter}>
            <Text style={styles.cardNameFooterText}>{cardName}</Text>
          </View>
        </ImageBackground>
      </View>
      {cardDescription ? (
        <Text style={styles.cardDescription}>{cardDescription}</Text>
      ) : null}
    </View>
  );
};

const styles = StyleSheet.create({
  verticalText: {
    flex: 1,
    flexDirection: "row",
    marginLeft: 10,
    marginBottom: 20
  },
  cardDescription: {
    fontFamily: "FranklinGothic",
    fontSize: 13,
    color: "#fefefe",
    marginTop: 10,
    width: 165,
    marginLeft: 20
  },
  cardNameFooterText: {
    fontFamily: "Heavitas",
    fontSize: 12,
    color: "#fff",
    letterSpacing: 1,
    textTransform: "uppercase",
    marginLeft: 5,
    marginTop: "3%"
  },
  cardNameFooter: {
    height: 20,
    backgroundColor: "rgba(28, 26, 26, 0.85)",
    flexDirection: "row",
    alignItems: "center"
  },
  content: {
    marginLeft: 20,
    width: 165,
    height: 105
  },
  imageBkg: {
    flex: 1,
    flexDirection: "column-reverse"
  }
});

export default SmallVideoCard;
