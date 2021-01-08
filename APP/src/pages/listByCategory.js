import React, { Component } from "react";
import {
  View,
  Text,
  StyleSheet,
  FlatList,
  ScrollView,
  ActivityIndicator
} from "react-native";
import SmallVideoCard from "../components/smallVideoCard";
import SectionTitle from "../components/sectionTitle";
import playlistService from "../services/playlistService";
import stationService from "../services/stationService";
import { translate } from "../locales";
var Enumerable = require("../../node_modules/linq");

export default class ListByCategory extends Component {
  static navigationOptions = {
    headerStyle: {
      backgroundColor: "#1c1a1a",
      borderBottomColor: "#1c1a1a"
    }
  };

  constructor(props) {
    super(props);
    const { navigation } = this.props;

    const listType = navigation.getParam("listType", 1);

    this.state = {
      listType: listType,
      pageTitle:
        listType == 1
          ? translate("stationsByCategory")
          : listType == 2
          ? translate("featuredPlaylists")
          : translate("playlistsByCategory"),
      videosByCategory: [],
      featuredPlaylists: []
    };
  }

  componentDidMount = async () => {
    switch (this.state.listType) {
      case 1:
        await stationService.getStationsByCategory(response => {
          this.setState({ videosByCategory: response });
        });
        break;
      case 2:
        await playlistService.getMoreFeaturedPlaylists([], 10, response => {
          this.setState({ featuredPlaylists: response });
        });
        break;
      case 3:
        await playlistService.getPlaylistsByCategory(response => {
          this.setState({ videosByCategory: response });
        });
        break;
    }
  };

  getMoreFeaturedPlaylists = async () => {
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
  };
  renderFeatured = () => {
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
            onEndReachedThreshold={0.2}
            bounces={false}
            onEndReached={this.getMoreFeaturedPlaylists}
            ListFooterComponent={() => (
              <View style={styles.columnFlatListLoading}>
                <ActivityIndicator />
              </View>
            )}
            renderItem={plColumn => (
              <View style={{ marginRight: 3 }}>
                {plColumn.item.map(playlist => {
                  return (
                    <View
                      style={{ marginBottom: 20 }}
                      key={playlist.playlistID} 
                    >
                      <SmallVideoCard
                        key={playlist.playlistID}
                        thumbURL={playlist.nextVideoThumbURL}
                        cardName={playlist.name}
                        cardDescription={playlist.description}
                      />
                    </View>
                  );
                })}
              </View>
            )}
          />
        </View>
      </View>
    );
  };

  getMoreByCategory = async (categoryID, list, service) => {
    const alreadyLoaded = Enumerable.from(list)
      .where(sc => sc.categoryID == categoryID)
      .select(sc => sc.stations || sc.playlists)
      .firstOrDefault();

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
        } else {
          category.playlists = [...category.playlists, ...response];
        }
        this.setState({ videosByCategory: list });
      }
    );
  };

  getMoreToRenderByCategory = async categoryID => {
    let isStation = this.state.listType == 1;
    await this.getMoreByCategory(
      categoryID,
      this.state.videosByCategory,
      isStation ? stationService : playlistService
    );
  };

  renderByCategory = () => {
    return (
      <View style={{ marginBottom: 20 }}>
        {this.state.videosByCategory.map(el => {
          return (
            <View key={el.categoryID} style={styles.stByCategoryView}>
              <Text style={styles.flatListTitle}>{el.categoryName}</Text>
              <FlatList
                style={styles.flatList}
                data={el.stations || el.playlists}
                horizontal={true}
                keyExtractor={(item, i) => i.toString()}
                showsHorizontalScrollIndicator={false}
                onEndReachedThreshold={0.2}
                bounces={false}
                ListFooterComponent={() => (
                  <View style={styles.flatListLoading}>
                    <ActivityIndicator />
                  </View>
                )}
                onEndReached={() =>
                  this.getMoreToRenderByCategory(el.categoryID)
                }
                renderItem={video => (
                  <SmallVideoCard
                    thumbURL={video.item.nextVideoThumbURL}
                    cardName={video.item.name}
                    cardDescription={video.item.description}
                  />
                )}
              />
            </View>
          );
        })}
      </View>
    );
  };

  render() {
    return (
      <ScrollView
        style={styles.container}
        vertical={true}
        stickyHeaderIndices={[0]}
      >
        <SectionTitle
          title={this.state.pageTitle}
          useTopShadow={true}
          stickyTop={true}
        />
        {this.state.listType == 2
          ? this.renderFeatured()
          : this.renderByCategory()}
      </ScrollView>
    );
  }
}

const styles = StyleSheet.create({
  flatList: {
    marginLeft: -20
  },
  stByCategoryView: {
    marginTop: 10,
    marginLeft: 20
  },
  container: {
    paddingTop: 15,
    flex: 1,
    backgroundColor: "#1c1a1a"
  },
  flatListTitle: {
    marginBottom: 10,
    fontFamily: "Heavitas",
    color: "#fefefe",
    fontSize: 15
  },
  columnFlatListLoading: {
    paddingLeft: 20,
    top: "45%"
  },
  flatListLoading: {
    marginTop: 40,
    paddingLeft: 20
  },
});
