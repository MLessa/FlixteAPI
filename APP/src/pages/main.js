import React, { Component } from "react";
import {
  View,
  Text,
  StyleSheet,
  ScrollView,
  Dimensions,
  FlatList,
  TouchableHighlight,
  ActivityIndicator,
  Animated
} from "react-native";
import HeaderBar from "../components/headerBar";
import SmallVideoCard from "../components/smallVideoCard";
import LargeVideoCard from "../components/largeVideoCard";
import SectionTitle from "../components/sectionTitle";
import Carousel from "react-native-snap-carousel";
import authenticationService from "../services/authenticationService";
import stationService from "../services/stationService";
import playlistService from "../services/playlistService";
import videoService from "../services/videoService";
import AnimatedImageBackground from "../components/AnimatedImageBackground";

import { translate } from "../locales";
var Enumerable = require("../../node_modules/linq");

const { width: screenWidth } = Dimensions.get("window");
const maximumListedItens = 30;
var _bkgFadeController = null;
var bkgOpacity = new Animated.Value(0.2);

export default class Main extends Component {
  static navigationOptions = {
    header: null
  };

  constructor(props) {
    super(props);
    this.state = {
      // Home data
      carrouselStations: stationService.getEmptyStations(3),
      stationsByCategory: [
        {
          categoryID: 1,
          categoryName: "",
          stations: []
        },
        {
          categoryID: 2,
          categoryName: "",
          stations: []
        },
        {
          categoryID: 3,
          categoryName: "",
          stations: []
        }
      ],
      featuredPlaylists: [],
      playlistsByCategory: [
        {
          categoryID: 1,
          categoryName: "",
          stations: []
        },
        {
          categoryID: 2,
          categoryName: "",
          stations: []
        },
        {
          categoryID: 3,
          categoryName: "",
          stations: []
        }
      ],
      ///
      selectedStationIndex: 0,
      isLoggedIn: false
    };
    this.stationCarousel = null;
    this.myRef = React.createRef();
  }

  async componentDidMount() {
    this.checkLoginState();
    await this.getNextCarrouselStationPage();
    await videoService.getHomeData(response => {
      this.setState({
        stationsByCategory: response.stationsByCategory,
        featuredPlaylists: response.featuredPlaylists,
        playlistsByCategory: response.playlistsByCategory
      });
    });
  }

  checkLoginState = async () => {
    await authenticationService.getLoginData(
      async () => {
        this.setState({ ...this.state, isLoggedIn: true });
      },
      async () => {
        this.setState({ ...this.state, isLoggedIn: false });
      }
    );
  };

  stationRenderFunction({ item }) {
    return (
      <LargeVideoCard
        thumbURL={item.nextVideoThumbURL}
        cardName={item.name}
        nextVideoTitle={item.nextVideoTitle}
        nextVideoDuration={item.nextVideoDuration}
      />
    );
  }

  getMoreByCategory = async (categoryID, list, service) => {
    const alreadyLoaded = Enumerable.from(list)
      .where(sc => sc.categoryID == categoryID)
      .select(sc => sc.stations || sc.playlists)
      .firstOrDefault();

    if (alreadyLoaded.length < maximumListedItens) {
      await service.getMoreByCategory(
        categoryID,
        Enumerable.from(alreadyLoaded)
          .select(s => s.stationID || s.playlistID)
          .toJoinedString(","),
        5,
        response => {
          let category = Enumerable.from(list)
            .where(sc => sc.categoryID == categoryID)
            .firstOrDefault();
          if (category.stations) {
            category.stations = [...category.stations, ...response];
            this.setState({ stationsByCategory: list });
          } else {
            category.playlists = [...category.playlists, ...response];
            this.setState({ playlistsByCategory: list });
          }
        }
      );
    }
  };

  getMoreToRenderByCategory = async (categoryID, isStation) => {
    let list = isStation
      ? this.state.stationsByCategory
      : this.state.playlistsByCategory;

    await this.getMoreByCategory(
      categoryID,
      list,
      isStation ? stationService : playlistService
    );
  };

  renderByCategory = (entity, isStation) => {
    const { navigation } = this.props;
    const { push } = navigation;

    return (
      <View>
        {entity.map(el => {
          return (
            <View key={el.categoryID} style={styles.stByCategoryView}>
              <Text style={styles.flatListTitle}>{el.categoryName}</Text>
              <FlatList
                style={styles.flatList}
                data={el.stations || el.playlists}
                horizontal={true}
                keyExtractor={(item, i) => i.toString()}
                showsHorizontalScrollIndicator={false}
                onEndReachedThreshold={0.1}
                bounces={false}
                onEndReached={() =>
                  this.getMoreToRenderByCategory(el.categoryID, isStation)
                }
                ListFooterComponent={() => {
                  const list = isStation ? el.stations : el.playlists;
                  if (!list) return null;

                  if (list.length < maximumListedItens) {
                    return (
                      <View style={styles.flatListLoading}>
                        <ActivityIndicator />
                      </View>
                    );
                  } else return null;
                }}
                renderItem={video => (
                  <SmallVideoCard
                    thumbURL={video.item.nextVideoThumbURL}
                    cardName={video.item.name}
                    stickyTop={false}
                  />
                )}
              />
            </View>
          );
        })}

        <TouchableHighlight
          style={{ marginLeft: 20, marginTop: 10 }}
          onPress={() =>
            push("List", { listType: isStation ? 1 : 3 })
          }
          underlayColor="black"
        >
          <Text style={styles.seeAll}>{translate("seeAll")}</Text>
        </TouchableHighlight>
      </View>
    );
  };

  getMoreFeaturedPlaylists = async () => {
    if (this.state.featuredPlaylists.length < maximumListedItens) {
      const alreadyLoaded = Enumerable.from(this.state.featuredPlaylists)
        .select(p => p.playlistID)
        .toJoinedString(",");

      await playlistService.getMoreFeaturedPlaylists(
        alreadyLoaded,
        6,
        response => {
          this.setState({
            featuredPlaylists: [...this.state.featuredPlaylists, ...response]
          });
        }
      );
    }
  };

  renderFeaturedPlaylists = () => {
    const { navigation } = this.props;
    const { push } = navigation;

    let featuredPlaylistsMatrix = [];
    let featuredPlaylistsColumn = [];

    this.state.featuredPlaylists.forEach(playlist => {
      featuredPlaylistsColumn.push(playlist);
      if (featuredPlaylistsColumn.length == 3) {
        featuredPlaylistsMatrix.push(featuredPlaylistsColumn);
        featuredPlaylistsColumn = [];
      }
    });
    if (featuredPlaylistsColumn.length > 0)
      featuredPlaylistsMatrix.push(featuredPlaylistsColumn);

    return (
      <View>
        <View style={styles.stByCategoryView}>
          <FlatList
            style={styles.flatList}
            data={featuredPlaylistsMatrix}
            horizontal={true}
            keyExtractor={(item, index) => `${index}`}
            showsHorizontalScrollIndicator={false}
            onEndReachedThreshold={0.1}
            bounces={false}
            onEndReached={this.getMoreFeaturedPlaylists}
            ListFooterComponent={() => {
              if (this.state.featuredPlaylists.length < 30)
                return (
                  <View style={styles.columnFlatListLoading}>
                    <ActivityIndicator />
                  </View>
                );
              else return null;
            }}
            renderItem={plColumn => (
              <View style={{ marginRight: 3 }}>
                {plColumn.item.map(playlist => {
                  return (
                    <View
                      style={{ marginBottom: 15 }}
                      key={playlist.playlistID}
                    >
                      <SmallVideoCard
                        key={playlist.playlistID}
                        thumbURL={playlist.nextVideoThumbURL}
                        cardName={playlist.name}
                      />
                    </View>
                  );
                })}
              </View>
            )}
          />
        </View>
        <TouchableHighlight
          style={{ marginLeft: 20, marginTop: 10 }}
          onPress={() => push("List", { listType: 2 })}
          underlayColor="black"
        >
          <Text style={styles.seeAll}>{translate("seeAll")}</Text>
        </TouchableHighlight>
      </View>
    );
  };

  getNextCarrouselStationPage = async () => {
    if (
      !this.state.isLoadingCarrousel &&
      this.state.carrouselStations.length < maximumListedItens
    ) {
      // //add loading cards
      // this.setState({
      //   carrouselStations: [
      //     ...this.state.carrouselStations,
      //     ...stationService.getEmptyStations(this.state.carrouselPageSize)
      //   ],
      //   isLoadingCarrousel: true
      // });
      // ///
      await stationService.getFeaturedStations(response => {
        this.setState({
          carrouselStations: [
            ...this.state.carrouselStations.filter(s => !s.fake),
            ...response
          ],
          isLoadingCarrousel: false
        });
      }, 5);
    }
  };

  render() {
    const { navigation } = this.props;

    return (
      <View style={styles.container}>
        <ScrollView
          style={styles.content}
          stickyHeaderIndices={[1, 3, 5]}
          vertical={true}
          showsVerticalScrollIndicator={false}
        >
          <AnimatedImageBackground
            blurRadius={10}
            source={{
              uri: this.state.carrouselStations[this.state.selectedStationIndex]
                .nextVideoThumbURL
            }}
            imageStyle={{ opacity: bkgOpacity }}
            style={styles.stationImageContainer}
          >
            <HeaderBar />

            <SectionTitle
              title={
                this.state.isLoggedIn
                  ? translate("watchNext").toUpperCase()
                  : translate("featuredStations").toUpperCase()
              }
              useTopShadow={false}
            />
            <View style={styles.stationCarouselContainer}>
              <Carousel
                ref={c => {
                  this.stationCarousel = c;
                }}
                data={this.state.carrouselStations}
                renderItem={this.stationRenderFunction}
                sliderWidth={screenWidth}
                itemWidth={320}
                onSnapToItem={index => {
                  if (_bkgFadeController) clearTimeout(_bkgFadeController);
                  _bkgFadeController = setTimeout(() => {
                    Animated.timing(bkgOpacity, {
                      toValue: 0,
                      duration: 500
                    }).start(() => {
                      this.setState({ selectedStationIndex: index });
                      Animated.timing(bkgOpacity, {
                        toValue: 0.3,
                        duration: 800
                      }).start();
                    });
                  }, 800);
                }}
                onEndReachedThreshold={1.5}
                onEndReached={this.getNextCarrouselStationPage}
              />
            </View>
          </AnimatedImageBackground>

          <SectionTitle
            title={translate("stationsByCategory").toUpperCase()}
            useTopShadow={true}
          />
          {this.renderByCategory(this.state.stationsByCategory, true)}

          <SectionTitle
            title={translate("featuredPlaylists").toUpperCase()}
            useTopShadow={true}
          />
          {this.renderFeaturedPlaylists()}

          <SectionTitle title="Playlist by Category" useTopShadow={true} />
          {this.renderByCategory(this.state.playlistsByCategory, false)}
        </ScrollView>
      </View>
    );
  }
}

const styles = StyleSheet.create({
  columnFlatListLoading: {
    paddingLeft: 20,
    marginTop: 160
  },
  flatListLoading: {
    marginTop: 40,
    paddingLeft: 20
  },
  seeAll: {
    marginBottom: 10,
    fontFamily: "Heavitas",
    color: "#d46f02",
    fontSize: 13
  },
  flatList: {
    marginLeft: -20
  },
  stByCategoryView: {
    marginTop: 10,
    marginLeft: 20
  },
  flatListTitle: {
    marginBottom: 10,
    fontFamily: "Heavitas",
    color: "#fefefe",
    fontSize: 13
  },
  stationCarouselContainer: {
    backgroundColor: "transparent",
    marginBottom: 15
  },
  stationImageContainer: {
    flex: 1
  },
  imageContainer: {
    flex: 1,
    flexDirection: "column-reverse"
  },
  content: {
    flex: 1
  },
  container: {
    flex: 1,
    backgroundColor: "#1c1a1a",
    justifyContent: "center",
    alignItems: "center"
  }
});
