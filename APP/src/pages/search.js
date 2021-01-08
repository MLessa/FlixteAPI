import React, { Component } from "react";
import { View, Image, StyleSheet, ScrollView, FlatList } from "react-native";
import HeaderBar from "../components/headerBar";
import SectionTitle from "../components/sectionTitle";
import SmallVideoCard from "../components/smallVideoCard";
import { translate } from "../locales";
import TextField from "../components/textField";
import images from "../images";
import searchService from "../services/searchService";
import { Avatar } from "react-native-elements";

var _APISearchCall = null;

export default class search extends Component {
  static navigationOptions = {
    header: null
  };
  
  constructor(props) {
    super(props);

    const videoList1 = [
      {
        id: 1,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg",
        description:
          "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do"
      },
      {
        id: 2,
        name: "Stand Up Comedy BR",
        nextVideoTitle: "Carnaval no quarto 69",
        nextVideoDuration: "6:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/6rZ7uf8a5Ys/maxresdefault.jpg",
        description:
          "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do"
      },
      {
        id: 3,
        name: "Variados",
        nextVideoTitle: "Postura de castrado?",
        nextVideoDuration: "6:00",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/IhJZivtBpg8/maxresdefault.jpg",
        description:
          "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do"
      },
      {
        id: 4,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg",
        description:
          "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do"
      }
    ];
    const videoList2 = [
      {
        id: 5,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg",
        description:
          "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do"
      },
      {
        id: 6,
        name: "Stand Up Comedy BR",
        nextVideoTitle: "Carnaval no quarto 69",
        nextVideoDuration: "6:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/6rZ7uf8a5Ys/maxresdefault.jpg",
        description:
          "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do"
      },
      {
        id: 7,
        name: "Variados",
        nextVideoTitle: "Postura de castrado?",
        nextVideoDuration: "6:00",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/IhJZivtBpg8/maxresdefault.jpg",
        description:
          "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do"
      },
      {
        id: 8,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg",
        description:
          "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do"
      }
    ];
    var initialContent = [
      {
        sectionName: "People",
        isPeopleSection: true,
        contentList: [
          {
            userID: 1,
            userName: "Matheus Lessa",
            userAvatarURL: "http://tiny.cc/1183az"
          },
          {
            userID: 2,
            userName: "Leandro Mascarenhas",
            userAvatarURL: "http://tiny.cc/i783az"
          },
          {
            userID: 3,
            userName: "Amanda Sotero",
            userAvatarURL: "http://tiny.cc/j883az"
          },
          {
            userID: 4,
            userName: "Taíse da Hora",
            userAvatarURL: "http://tiny.cc/0983az"
          }
        ]
      },
      {
        sectionName: "Stations",
        isPeopleSection: false,
        contentList: videoList1
      },
      {
        sectionName: "Playlists",
        isPeopleSection: false,
        contentList: videoList2
      }
    ];

    this.state = {
      searchTerm: null,
      showLoadingIcon: false,
      showSearchResult: true,
      searchResult: initialContent
    };
  }

  renderSearchResult = () => {
    if (this.state.showSearchResult) {
      return this.state.searchResult.map((section, index) => {
        return (
          <View key={index}>
            <SectionTitle
              title={section.sectionName}
              useTopShadow={index !== 0}
            />
            <FlatList
              style={styles.flatList}
              data={section.contentList}
              horizontal={true}
              keyExtractor={(content, index) =>
                section.isPeopleSection
                  ? content.userID.toString()
                  : content.id.toString()
              }
              showsHorizontalScrollIndicator={false}
              renderItem={content => {
                return (
                  <View style={{ marginRight: 3 }}>
                    {section.isPeopleSection ? (
                      <View
                        style={{ marginBottom: 15 }}
                        key={content.item.userID}
                      >
                        <Avatar
                          containerStyle={{ marginLeft: 20 }}
                          size="large"
                          rounded
                          source={{
                            uri: content.item.userAvatarURL
                          }}
                          onPress={() =>
                            push("UserProfile", {
                              userID: content.item.userID
                            })
                          }
                        />
                      </View>
                    ) : (
                      <View style={{ marginBottom: 15 }} key={content.item.id}>
                        <SmallVideoCard
                          key={content.item.stationID}
                          thumbURL={content.item.nextVideoThumbURL}
                          cardName={content.item.name}
                          cardDescription={content.item.description}
                        />
                      </View>
                    )}
                  </View>
                );
              }}
            />
          </View>
        );
      });
    }
  };

  search = searchTerm => {
    if (searchTerm.length >= 3) {
      this.setState({ showLoadingIcon: true, showSearchResult: false });
      if (_APISearchCall) clearTimeout(_APISearchCall);
      _APISearchCall = setTimeout(() => {
        searchService.search(
          searchTerm,
          successResponse => {
            this.setState({
              showLoadingIcon: false,
              showSearchResult: true,
              searchResult: successResponse
            });
          },
          errorResponse => {}
        );
      }, 600);
    }
  };

  render() {
    return (
      <View style={styles.container}>
        <ScrollView
          style={styles.content}
          vertical={true}
          showsVerticalScrollIndicator={false}
        >
          <HeaderBar />

          <SectionTitle
            title={translate("Search").toUpperCase()}
            useTopShadow={false}
          />
          <View style={styles.searchBox}>
            <TextField
              style={styles.baseComponentStyle}
              width="92%"
              placeholder={translate("searchFor")}
              showSearchIcon={true}
              onUpdate={searchTerm => {
                this.search(searchTerm);
                this.setState({ searchTerm });
              }}
            />
          </View>

          {this.renderSearchResult()}

          {this.state.showLoadingIcon ? (
            <Image
              resizeMode="contain"
              style={styles.loadingImage}
              source={images.misc.loadingIcon}
            />
          ) : null}
        </ScrollView>
      </View>
    );
  }
}

const styles = StyleSheet.create({
  loadingImage: {
    width: "50%",
    height: "50%",
    justifyContent: "center",
    alignSelf: "center",
    marginTop: "20%"
  },
  baseComponentStyle: {
    marginTop: 20
  },
  searchBox: {
    backgroundColor: "#221f1f",
    height: 85,
    marginBottom: 30
  },
  container: {
    flex: 1,
    backgroundColor: "#1c1a1a",
    justifyContent: "center",
    alignItems: "center",
    flexDirection: "column"
  },
  content: {
    flexDirection: "column"
  }
});
