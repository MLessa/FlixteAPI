import React, { Component } from "react";
import { View, Text, StyleSheet, ScrollView, FlatList } from "react-native";
import authenticationService from "../services/authenticationService";
import { Avatar } from "react-native-elements";
import Button from "../components/button";
import { translate } from "../locales";
import SmallVideoCard from "../components/smallVideoCard";
import ScrollableTabView, {
  DefaultTabBar,
  ScrollableTabBar
} from "react-native-scrollable-tab-view-forked";

export default class profile extends Component {
  constructor(props) {
    super(props);

    const people = [
      {
        userID: 1,
        userName: "Amanda Sotero",
        userAvatarURL: "http://tiny.cc/j883az"
      },
      {
        userID: 2,
        userName: "Matheus Lessa",
        userAvatarURL: "http://tiny.cc/1183az"
      },
      {
        userID: 3,
        userName: "Leandro Mascarenhas",
        userAvatarURL: "http://tiny.cc/i783az"
      },
      {
        userID: 4,
        userName: "Amanda Sotero",
        userAvatarURL: "http://tiny.cc/j883az"
      },
      {
        userID: 5,
        userName: "Taíse da Hora",
        userAvatarURL: "http://tiny.cc/0983az"
      },
      {
        userID: 6,
        userName: "Matheus Lessa",
        userAvatarURL: "http://tiny.cc/1183az"
      },
      {
        userID: 7,
        userName: "Leandro Mascarenhas",
        userAvatarURL: "http://tiny.cc/i783az"
      },
      {
        userID: 8,
        userName: "Amanda Sotero",
        userAvatarURL: "http://tiny.cc/j883az"
      },
      {
        userID: 9,
        userName: "Leandro Mascarenhas",
        userAvatarURL: "http://tiny.cc/i783az"
      }
    ];
    const playlists = [
      {
        playlistID: 1,
        name: "Variados",
        nextVideoTitle: "Postura de castrado?",
        nextVideoDuration: "6:00",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/IhJZivtBpg8/maxresdefault.jpg",
        description:
          "Brasil Selvagem, PMN, KondZilla, Pipocando, MMA BR, Krav Maga das Américas..."
      },
      {
        playlistID: 2,
        name: "Stand Up Comedy BR",
        nextVideoTitle: "Carnaval no quarto 69",
        nextVideoDuration: "6:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/6rZ7uf8a5Ys/maxresdefault.jpg",
        description:
          "Brasil Selvagem, PMN, KondZilla, Pipocando, MMA BR, Krav Maga das Américas..."
      },
      {
        playlistID: 3,
        name: "Variados",
        nextVideoTitle: "Postura de castrado?",
        nextVideoDuration: "6:00",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/IhJZivtBpg8/maxresdefault.jpg",
        description:
          "Thiago Ventura, Nil Agra, Afonso Padilha, Junior Chicó, Bruna Louise, Murilo Couto etc"
      },
      {
        playlistID: 4,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg",
        description:
          "Nerdologia, Ciência todo dia, Canal do Pirula, O físico turista e muito mais"
      },
      {
        playlistID: 5,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg",
        description:
          "Nerdologia, Ciência todo dia, Canal do Pirula, O físico turista e muito mais"
      },
      {
        playlistID: 6,
        name: "Stand Up Comedy BR",
        nextVideoTitle: "Carnaval no quarto 69",
        nextVideoDuration: "6:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/6rZ7uf8a5Ys/maxresdefault.jpg",
        description:
          "Brasil Selvagem, PMN, KondZilla, Pipocando, MMA BR, Krav Maga das Américas..."
      },
      {
        playlistID: 7,
        name: "Variados",
        nextVideoTitle: "Postura de castrado?",
        nextVideoDuration: "6:00",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/IhJZivtBpg8/maxresdefault.jpg",
        description:
          "Thiago Ventura, Nil Agra, Afonso Padilha, Junior Chicó, Bruna Louise, Murilo Couto etc"
      },
      {
        playlistID: 8,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg",
        description:
          "Thiago Ventura, Nil Agra, Afonso Padilha, Junior Chicó, Bruna Louise, Murilo Couto etc"
      },
      {
        playlistID: 9,
        name: "Stand Up Comedy BR",
        nextVideoTitle: "Carnaval no quarto 69",
        nextVideoDuration: "6:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/6rZ7uf8a5Ys/maxresdefault.jpg",
        description:
          "Nerdologia, Ciência todo dia, Canal do Pirula, O físico turista e muito mais"
      },
      {
        playlistID: 10,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg",
        description:
          "Nerdologia, Ciência todo dia, Canal do Pirula, O físico turista e muito mais"
      }
    ];
    const stations = [
      {
        stationID: 11,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg",
        description:
          "Nerdologia, Ciência todo dia, Canal do Pirula, O físico turista e muito mais"
      },
      {
        stationID: 22,
        name: "Stand Up Comedy BR",
        nextVideoTitle: "Carnaval no quarto 69",
        nextVideoDuration: "6:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/6rZ7uf8a5Ys/maxresdefault.jpg",
        description:
          "Thiago Ventura, Nil Agra, Afonso Padilha, Junior Chicó, Bruna Louise, Murilo Couto etc"
      },
      {
        stationID: 33,
        name: "Variados",
        nextVideoTitle: "Postura de castrado?",
        nextVideoDuration: "6:00",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/IhJZivtBpg8/maxresdefault.jpg",
        description:
          "Brasil Selvagem, PMN, KondZilla, Pipocando, MMA BR, Krav Maga das Américas..."
      },
      {
        stationID: 44,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg",
        description:
          "Nerdologia, Ciência todo dia, Canal do Pirula, O físico turista e muito mais"
      },
      {
        stationID: 55,
        name: "Stand Up Comedy BR",
        nextVideoTitle: "Carnaval no quarto 69",
        nextVideoDuration: "6:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/6rZ7uf8a5Ys/maxresdefault.jpg",
        description:
          "Thiago Ventura, Nil Agra, Afonso Padilha, Junior Chicó, Bruna Louise, Murilo Couto etc"
      },
      {
        stationID: 66,
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
      userData: props.navigation.getParam("userData", null),
      stations: stations,
      playlists: playlists,
      people: people
    };
  }

  logout = async () => {
    const { navigation } = this.props;
    const { popToTop } = navigation;
    await authenticationService.logOut(() => popToTop());
  };

  renderCardList = (entity, keyFunction, isPeople = false) => {
    let cardlistsMatrix = [];
    let cardListsColumn = [];
    const listLength = isPeople ? 4 : 2;

    entity.forEach((card, index) => {
      cardListsColumn.push(card);
      if (cardListsColumn.length == listLength) {
        cardlistsMatrix.push(cardListsColumn);
        cardListsColumn = [];
      }
    });
    if (cardListsColumn.length > 0) cardlistsMatrix.push(cardListsColumn);

    return (
      <View style={styles.stationView}>
        <FlatList
          contentContainerStyle={{ paddingBottom: 50 }}
          style={styles.flatList}
          data={cardlistsMatrix}
          horizontal={false}
          keyExtractor={(item, index) => `${index}`}
          showsVerticalScrollIndicator={false}
          renderItem={plColumn => (
            <View
              style={[
                styles.flatListLine,
                isPeople ? { marginLeft: "2%" } : {}
              ]}
            >
              {plColumn.item.map((card, index) => {
                if (isPeople)
                  return (
                    <Avatar
                      key={keyFunction(card)}
                      rounded
                      size="large"
                      source={{
                        uri: card.userAvatarURL
                      }}
                      onPress={() =>
                        push("Profile", { userData: this.state.userData })
                      }
                    />
                  );
                else
                  return (
                    <SmallVideoCard
                      key={keyFunction(card)}
                      thumbURL={card.nextVideoThumbURL}
                      cardName={card.name}
                      cardDescription={card.description}
                    />
                  );
              })}
            </View>
          )}
        />
      </View>
    );
  };

  render() {
    return (
      <View style={styles.container}>
        <View style={styles.userInfoView}>
          <View style={{ flexDirection: "row" }}>
            {this.state.userData.isGoogleUser ? (
              <Avatar
                size="large"
                rounded
                source={{
                  uri: this.state.userData.googleUserInfo.user.photo
                }}
              />
            ) : (
              <Avatar size="large" rounded title={this.state.userData.Title} />
            )}
            <View style={styles.names}>
              <Text style={styles.user}>Matheus</Text>
              <Text style={styles.user}>Lessa</Text>
              <Text style={styles.username}>>>mlessa</Text>
            </View>
          </View>
          <Button
            style={styles.profileAction}
            width="25%"
            text={translate("logoff")}
            action={() => this.logout()}
            googleBtn={true}
          />
        </View>
        <ScrollableTabView
          style={styles.tabView}
          tabStyle={styles.tab}
          tabBarTextStyle={styles.tabBarTextStyle}
          tabBarInactiveTextColor={"rgba(255, 255, 255, 0.15)"}
          tabBarActiveTextColor={"#ffffff"}
          tabBarUnderlineStyle={styles.underlineStyle}
        >
          <View tabLabel={translate("stations")}>
            {this.renderCardList(this.state.stations, station =>
              station.stationID.toString()
            )}
          </View>

          <View tabLabel={translate("Playlists")}>
            {this.renderCardList(this.state.playlists, playlist =>
              playlist.playlistID.toString()
            )}
          </View>
          <View tabLabel={translate("people")}>
            {this.renderCardList(
              this.state.people,
              people => people.userID.toString(),
              true
            )}
          </View>
        </ScrollableTabView>
      </View>
    );
  }
}

const styles = StyleSheet.create({
  flatListLine: {
    marginRight: 3,
    flexDirection: "row",
    flex: 1,
    justifyContent: "space-around",
    marginBottom: "3%"
  },
  flatList: {
    marginLeft: -20,
    paddingTop: 50
  },
  stationView: {
    marginLeft: "1.5%"
  },
  tab: {
    height: 200
  },
  underlineStyle: {
    backgroundColor: "#d46f02",
    height: 0.9
  },
  tabBarTextStyle: {
    fontSize: 11,
    fontWeight: "normal",
    fontFamily: "Heavitas",
    fontStyle: "normal",
    textAlign: "left"
  },
  tabView: {
    backgroundColor: "#1c1a1a",
    marginTop: "10%",
    paddingTop: 10,
    paddingBottom: 10
  },
  profileAction: {
    marginRight: 5
  },
  names: {
    marginLeft: "5%",
    marginTop: "5%"
  },
  username: {
    marginTop: 5,
    color: "#d46f02",
    fontFamily: "FranklinGothic",
    fontSize: 14
  },
  user: {
    fontFamily: "Heavitas",
    color: "#fefefe",
    fontSize: 18
  },
  optionsView: {
    marginTop: 30
  },
  userInfoView: {
    paddingLeft: "5%",
    // alignItems: "flex-start",
    flexDirection: "row",
    justifyContent: "space-between"
  },
  container: {
    paddingTop: 15,
    flex: 1,
    backgroundColor: "#221f1f"
  }
});
