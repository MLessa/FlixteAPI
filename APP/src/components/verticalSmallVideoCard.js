import React, { Component } from "react";
import { View, Text, StyleSheet, ImageBackground } from "react-native";

const verticalSmallVideoCard = ({ thumbURL, cardName, cardDescription }) => {
  return (
    <View style={styles.verticalVideoCard}>
      <View style={styles.content}>
        <ImageBackground source={{ uri: thumbURL }} style={styles.imageBkg}>
          <View style={styles.cardNameFooter}>
            <Text style={styles.cardNameFooterText}>{cardName}</Text>
          </View>
        </ImageBackground>
      </View>
      {cardDescription ? (
        <View style={{ flex: 0.5 }}>
          <Text
            ellipsizeMode="tail"
            numberOfLines={12}
            style={styles.cardDescription}
          >
            {cardDescription}
          </Text>
        </View>
      ) : null}
    </View>
  );
};

const styles = StyleSheet.create({
  verticalVideoCard: {
    flex: 1,
    flexDirection: "row",
    marginLeft: 20,
    marginBottom: 20,
    justifyContent: "space-between"
  },
  cardDescription: {
    fontFamily: "FranklinGothic",
    fontSize: 13,
    color: "#fefefe",
    paddingRight: 10
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
    flex: 0.5,
    marginRight: "20%"
  },
  imageBkg: {
    width: 200,
    height: 120,
    flexDirection: "column-reverse"
  }
});

export default verticalSmallVideoCard;
