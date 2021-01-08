import React from "react";
import {
  createStackNavigator,
  createAppContainer,
  createBottomTabNavigator
} from "react-navigation";

import Main from "./pages/main";
import Register from "./pages/register";
import Login from "./pages/login";
import List from "./pages/list";
import UsernameRequest from "./pages/userNameRequest";
import PasswordRequest from "./pages/passwordRequest";
import EmailRequest from "./pages/emailRequest";
import Profile from "./pages/profile";
import Search from "./pages/search";
import Stations from "./pages/stations";
import Playlists from "./pages/playlists";
import { Image, Text } from "react-native";
import { translate } from "./locales";
import images from "./images";

const headerNavigation = {
  Register: {
    screen: Register
  },
  Login: {
    screen: Login
  },
  UsernameRequest: {
    screen: UsernameRequest
  },
  EmailRequest: {
    screen: EmailRequest
  },
  PasswordRequest: {
    screen: PasswordRequest
  },
  Profile: {
    screen: Profile
  }
};

const MainStack = createStackNavigator(
  {
    Main: {
      screen: Main
    },
    List: {
      screen: List
    },
    ...headerNavigation
  },
  {
    initialRouteName: "Main",
    defaultNavigationOptions: {
      headerTintColor: "#d46f02",
      headerStyle: {
        backgroundColor: "#221f1f",
        borderBottomWidth: 0
      }
    }
  }
);

const StationStack = createStackNavigator(
  {
    Station: {
      screen: Stations
    },
    ...headerNavigation
  },
  {
    initialRouteName: "Station",
    defaultNavigationOptions: {
      headerTintColor: "#d46f02",
      headerStyle: {
        backgroundColor: "#221f1f",
        borderBottomWidth: 0
      }
    }
  }
);

const PlaylistStack = createStackNavigator(
  {
    Playlist: {
      screen: Playlists
    },
    ...headerNavigation
  },
  {
    initialRouteName: "Playlist",
    defaultNavigationOptions: {
      headerTintColor: "#d46f02",
      headerStyle: {
        backgroundColor: "#221f1f",
        borderBottomWidth: 0
      }
    }
  }
);

const SearchStack = createStackNavigator(
  {
    Search: {
      screen: Search
    },
    ...headerNavigation
  },
  {
    initialRouteName: "Search",
    defaultNavigationOptions: {
      headerTintColor: "#d46f02",
      headerStyle: {
        backgroundColor: "#221f1f",
        borderBottomWidth: 0
      }
    }
  }
);

MainStack.navigationOptions = ({ navigation }) => {
  let tabBarVisible = false;
  let topPage =
    navigation.state.routes[navigation.state.routes.length - 1].routeName;
  if (topPage === "Main" || topPage === "StationList") {
    return {
      tabBarVisible: true
    };
  }

  return {
    tabBarVisible
  };
};

const TabNavigator = createBottomTabNavigator(
  {
    Home: MainStack,
    Stations: StationStack,
    Playlists: PlaylistStack,
    Search: SearchStack
  },
  {
    defaultNavigationOptions: ({ navigation }) => ({
      tabBarLabel: ({ focused, tintColor }) => {
        const { routeName } = navigation.state;
        return (
          <Text
            style={{
              marginTop: 7,
              color: "#fff",
              margin: 3,
              fontFamily: "Heavitas",
              fontSize: 9
            }}
          >
            {translate(routeName)}
          </Text>
        );
      },
      tabBarIcon: ({ focused, horizontal, tintColor }) => {
        const { routeName } = navigation.state;
        let footerImage = images.footer.home;
        if (routeName === "Search") footerImage = images.footer.search;
        else if (routeName === "Playlists")
          footerImage = images.footer.playlists;
        else if (routeName === "Stations") footerImage = images.footer.stations;
        return (
          <Image
            style={{
              width: 26,
              height: 26,
              tintColor: tintColor,
              marginBottom: 25
            }}
            source={footerImage}
          />
        );
      }
    }),
    tabBarOptions: {
      activeTintColor: "#FFFFFF",
      inactiveTintColor: "#5F5F5F",
      labelStyle: { color: "white" },
      tabStyle: {
        backgroundColor: "#221f1f",
        borderBottomWidth: 4,
        borderBottomColor: "#d46f02",
        paddingTop: 30,
        height: 50
      }
    }
  }
);

const App = createAppContainer(TabNavigator);

export default App;
