import React, { Component } from "react";
import {
  View,
  ScrollView,
  StyleSheet,
  Animated,
  FlatList
} from "react-native";
import SectionTitle from "../components/sectionTitle";
import SmallVideoCard from "../components/smallVideoCard";
import HeaderBar from "../components/headerBar";
import CollectionCreationButton from "../components/collectionCreationButton";
import AnimatedImageBackground from "../components/AnimatedImageBackground";

import { translate } from "../locales";

var _bkgFadeController = null;
var bkgOpacity = new Animated.Value(0.2);

class stations extends Component {
  static navigationOptions = {
    header: null
  };
  constructor(props) {
    super(props);

    const stations = [
      {
        stationID: 1,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg",
        description:
          "Nerdologia, Ciência todo dia, Canal do Pirula, O físico turista e muito mais"
      },
      {
        stationID: 2,
        name: "Stand Up Comedy BR",
        nextVideoTitle: "Carnaval no quarto 69",
        nextVideoDuration: "6:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/6rZ7uf8a5Ys/maxresdefault.jpg",
        description:
          "Thiago Ventura, Nil Agra, Afonso Padilha, Junior Chicó, Bruna Louise, Murilo Couto etc"
      },
      {
        stationID: 3,
        name: "Variados",
        nextVideoTitle: "Postura de castrado?",
        nextVideoDuration: "6:00",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/IhJZivtBpg8/maxresdefault.jpg",
        description:
          "Brasil Selvagem, PMN, KondZilla, Pipocando, MMA BR, Krav Maga das Américas..."
      },
      {
        stationID: 4,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg",
        description:
          "Nerdologia, Ciência todo dia, Canal do Pirula, O físico turista e muito mais"
      },
      {
        stationID: 5,
        name: "Stand Up Comedy BR",
        nextVideoTitle: "Carnaval no quarto 69",
        nextVideoDuration: "6:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/6rZ7uf8a5Ys/maxresdefault.jpg",
        description:
          "Thiago Ventura, Nil Agra, Afonso Padilha, Junior Chicó, Bruna Louise, Murilo Couto etc"
      },
      {
        stationID: 6,
        name: "Variados",
        nextVideoTitle: "Postura de castrado?",
        nextVideoDuration: "6:00",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/IhJZivtBpg8/maxresdefault.jpg",
        description:
          "Brasil Selvagem, PMN, KondZilla, Pipocando, MMA BR, Krav Maga das Américas..."
      }
    ];
    this.state = {
      sugestedStations: stations,
      sugestedStationSelectedIndex: 0
    };
  }

  renderUserStations = () => {
    return (
      <View style={{ marginRight: 3, flexDirection: "row", flexWrap: "wrap" }}>
        {this.state.sugestedStations.map((station, index) => {
          return (
            <View style={{ marginBottom: 15 }} key={station.stationID}>
              <SmallVideoCard
                key={station.stationID}
                thumbURL={station.nextVideoThumbURL}
                cardName={station.name}
                cardDescription={station.description}
              />
            </View>
          );
        })}
      </View>
    );
  };

  render() {
    return (
      <View style={styles.container}>
        <ScrollView
          style={styles.content}
          stickyHeaderIndices={[3]}
          vertical={true}
          showsVerticalScrollIndicator={false}
        >
          <AnimatedImageBackground
            blurRadius={10}
            source={{
              uri: this.state.sugestedStations[
                this.state.sugestedStationSelectedIndex
              ].nextVideoThumbURL
            }}
            imageStyle={{ opacity: bkgOpacity }}
            style={styles.stationImageContainer}
          >
            <HeaderBar />

            <SectionTitle
              title={translate("sugestedToYou").toUpperCase()}
              useTopShadow={false}
            />
            <FlatList
              style={styles.stationCarouselContainer}
              data={this.state.sugestedStations}
              horizontal={true}
              keyExtractor={item => item.stationID.toString()}
              showsHorizontalScrollIndicator={false}
              onScroll={event => {
                let sugestedStationSelectedIndex = Math.ceil(
                  event.nativeEvent.contentOffset.x / 230
                );
                if (_bkgFadeController) clearTimeout(_bkgFadeController);
                _bkgFadeController = setTimeout(() => {

                  Animated.timing(bkgOpacity, {
                    toValue: 0,
                    duration: 500
                  }).start(() => {
                    this.setState({ sugestedStationSelectedIndex });
                    Animated.timing(bkgOpacity, {
                      toValue: 0.3,
                      duration: 800
                    }).start();
                  });
                }, 800);
              }}
              renderItem={station => (
                <SmallVideoCard
                  thumbURL={station.item.nextVideoThumbURL}
                  cardName={station.item.name}
                  cardDescription={station.item.description}
                  stickyTop={false}
                />
              )}
            />
          </AnimatedImageBackground>
          <SectionTitle
            title={translate("myStations").toUpperCase()}
            useTopShadow={true}
          />
          <View style={{ }}>
            <CollectionCreationButton stationCreationButton={true} />
            {this.renderUserStations()}
          </View>
        </ScrollView>
      </View>
    );
  }
}

const styles = StyleSheet.create({
  stationImageContainer: {
    // flex: 1
  },
  container: {
    // flex: 1,
    backgroundColor: "#1c1a1a",
    justifyContent: "center",
    alignItems: "center",
    flexDirection: "column"
  },
  stationCarouselContainer: {
    backgroundColor: "transparent",
    marginBottom: 15
  }
});

export default stations;
