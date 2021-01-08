import React, { Component } from "react";
import { View, ScrollView, StyleSheet, FlatList, Animated } from "react-native";
import SectionTitle from "../components/sectionTitle";
import SmallVideoCard from "../components/smallVideoCard";
import VerticalSmallVideoCard from "../components/verticalSmallVideoCard";
import HeaderBar from "../components/headerBar";
import CollectionCreationButton from "../components/collectionCreationButton";
import AnimatedImageBackground from "../components/AnimatedImageBackground";

import { translate } from "../locales";

var _bkgFadeController = null;
var bkgOpacity = new Animated.Value(0.2);

class playlists extends Component {
  static navigationOptions = {
    header: null
  };

  constructor(props) {
    super(props);

    const playlists = [
      {
        playlistID: 1,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg",
        description:
          "Especialidades super especiais de especiarias específicas especificamente selecionadas por especialidades especiais do espaço escuro"
      },
      {
        playlistID: 2,
        name: "Stand Up Comedy BR",
        nextVideoTitle: "Carnaval no quarto 69",
        nextVideoDuration: "6:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/6rZ7uf8a5Ys/maxresdefault.jpg",
        description: "Melhores episódios em pé na rede"
      },
      {
        playlistID: 3,
        name: "Variados",
        nextVideoTitle: "Postura de castrado?",
        nextVideoDuration: "6:00",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/IhJZivtBpg8/maxresdefault.jpg",
        description: "Especial sobre crimes"
      },
      {
        playlistID: 4,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg",
        description: "Especial sobre energia nuclear"
      },
      {
        playlistID: 5,
        name: "Stand Up Comedy BR",
        nextVideoTitle: "Carnaval no quarto 69",
        nextVideoDuration: "6:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/6rZ7uf8a5Ys/maxresdefault.jpg",
        description: "Melhores episódios em pé na rede"
      },
      {
        playlistID: 6,
        name: "Variados",
        nextVideoTitle: "Postura de castrado?",
        nextVideoDuration: "6:00",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/IhJZivtBpg8/maxresdefault.jpg",
        description: "Especial sobre crimes"
      }
    ];
    this.state = {
      trendPlaylists: playlists,
      trendPlaylistSelectedIndex: 0
    };
  }

  renderUserPlaylists = () => {
    return (
      <FlatList
        data={this.state.trendPlaylists}
        keyExtractor={item => item.playlistID.toString()}
        showsVerticalScrollIndicator={false}
        renderItem={playlist => (
          <VerticalSmallVideoCard
            thumbURL={playlist.item.nextVideoThumbURL}
            cardName={playlist.item.name}
            stickyTop={false}
            cardDescription={playlist.item.description}
          />
        )}
      />
    );
  };

  render() {
    return (
      <View style={styles.container}>
        <ScrollView
          stickyHeaderIndices={[3]}
          vertical={true}
          showsVerticalScrollIndicator={false}
        >
          <AnimatedImageBackground
            blurRadius={10}
            source={{
              uri: this.state.trendPlaylists[
                this.state.trendPlaylistSelectedIndex
              ].nextVideoThumbURL
            }}
            imageStyle={{ opacity: bkgOpacity }}
            style={styles.stationImageContainer}
          >
            <HeaderBar />

            <SectionTitle
              title={translate("trendPlaylists").toUpperCase()}
              useTopShadow={false}
            />
            <FlatList
              style={styles.stationCarouselContainer}
              data={this.state.trendPlaylists}
              horizontal={true}
              keyExtractor={item => item.playlistID.toString()}
              showsHorizontalScrollIndicator={false}
              onScroll={event => {
                let trendPlaylistSelectedIndex = Math.ceil(
                  event.nativeEvent.contentOffset.x / 230
                );
                if (_bkgFadeController) clearTimeout(_bkgFadeController);
                _bkgFadeController = setTimeout(() => {
                  Animated.timing(bkgOpacity, {
                    toValue: 0,
                    duration: 500
                  }).start(() => {
                    this.setState({ trendPlaylistSelectedIndex });
                    Animated.timing(bkgOpacity, {
                      toValue: 0.3,
                      duration: 800
                    }).start();
                  });
                }, 800);
              }}
              renderItem={playlist => (
                <SmallVideoCard
                  thumbURL={playlist.item.nextVideoThumbURL}
                  cardName={playlist.item.name}
                  cardDescription={playlist.item.description}
                  stickyTop={false}
                />
              )}
            />
          </AnimatedImageBackground>
          <SectionTitle
            title={translate("myPlaylists").toUpperCase()}
            useTopShadow={true}
          />
          <View style={{ flex: 1 }}>
            <CollectionCreationButton stationCreationButton={false} />
            {this.renderUserPlaylists()}
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

export default playlists;
