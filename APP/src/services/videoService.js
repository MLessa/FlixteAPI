import baseService from "./baseService";

class appService extends baseService {
  constructor() {
    super("Home");
  }

  getHomeData = async callback => {
    const stations = [
      {
        stationID: 1,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg"
      },
      {
        stationID: 2,
        name: "Stand Up Comedy BR",
        nextVideoTitle: "Carnaval no quarto 69",
        nextVideoDuration: "6:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/6rZ7uf8a5Ys/maxresdefault.jpg"
      },
      {
        stationID: 3,
        name: "Variados",
        nextVideoTitle: "Postura de castrado?",
        nextVideoDuration: "6:00",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/IhJZivtBpg8/maxresdefault.jpg"
      }
    ];
    const playlists = [
      {
        playlistID: 1,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg"
      },
      {
        playlistID: 2,
        name: "Stand Up Comedy BR",
        nextVideoTitle: "Carnaval no quarto 69",
        nextVideoDuration: "6:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/6rZ7uf8a5Ys/maxresdefault.jpg"
      },
      {
        playlistID: 3,
        name: "Variados",
        nextVideoTitle: "Postura de castrado?",
        nextVideoDuration: "6:00",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/IhJZivtBpg8/maxresdefault.jpg"
      }
    ];
    response = {
      stationsByCategory: [ // sempre 3
        {
          categoryID: 1,
          categoryName: "Música",
          stations: stations
        },
        {
          categoryID: 2,
          categoryName: "Comédia",
          stations: stations
        },
        {
          categoryID: 3,
          categoryName: "VLOGs",
          stations: stations
        }
      ],
      playlistsByCategory: [ // sempre 3
        {
          categoryID: 1,
          categoryName: "Educativos",
          playlists: playlists
        },
        {
          categoryID: 2,
          categoryName: "Pornográficos",
          playlists: playlists
        },
        {
          categoryID: 3,
          categoryName: "Biju",
          playlists: playlists
        }
      ],
      featuredPlaylists: [...playlists, ...playlists, ...playlists] // sempre multiplus de 3 e no mínimo 9
    };
    callback(response);
    //   this.post(
    //     "GetHomeData",
    //     null,
    //     { pageIndex, pageSize },
    //     callback,
    //     callback
    //   );
    // };
  };
}

export default new appService();
