import baseService from "./baseService";

class stationService extends baseService {
  constructor() {
    super("Station");
  }

  // getStationsByCategory = async callback => {
  //   const stations = [
  //     {
  //       stationID: 1,
  //       name: "Top ciência BR",
  //       nextVideoTitle: "De onde vem o sexo",
  //       nextVideoDuration: "3:25",
  //       nextVideoThumbURL:
  //         "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg",
  //       description:
  //         "Aviões e Músicas, Aero por trás da aviação, Hoje no mundo militar..."
  //     },
  //     {
  //       stationID: 2,
  //       name: "Stand Up Comedy BR",
  //       nextVideoTitle: "Carnaval no quarto 69",
  //       nextVideoDuration: "6:25",
  //       nextVideoThumbURL:
  //         "http://i3.ytimg.com/vi/6rZ7uf8a5Ys/maxresdefault.jpg",
  //       description:
  //         "Aviões e Músicas, Aero por trás da aviação, Hoje no mundo militar..."
  //     },
  //     {
  //       stationID: 3,
  //       name: "Variados",
  //       nextVideoTitle: "Postura de castrado?",
  //       nextVideoDuration: "6:00",
  //       nextVideoThumbURL:
  //         "http://i3.ytimg.com/vi/IhJZivtBpg8/maxresdefault.jpg",
  //       description:
  //         "Aviões e Músicas, Aero por trás da aviação, Hoje no mundo militar..."
  //     },
  //     {
  //       stationID: 4,
  //       name: "Top ciência BR",
  //       nextVideoTitle: "De onde vem o sexo",
  //       nextVideoDuration: "3:25",
  //       nextVideoThumbURL:
  //         "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg",
  //       description:
  //         "Aviões e Músicas, Aero por trás da aviação, Hoje no mundo militar..."
  //     },
  //     {
  //       stationID: 5,
  //       name: "Stand Up Comedy BR",
  //       nextVideoTitle: "Carnaval no quarto 69",
  //       nextVideoDuration: "6:25",
  //       nextVideoThumbURL:
  //         "http://i3.ytimg.com/vi/6rZ7uf8a5Ys/maxresdefault.jpg",
  //       description:
  //         "Aviões e Músicas, Aero por trás da aviação, Hoje no mundo militar..."
  //     },
  //     {
  //       stationID: 6,
  //       name: "Variados",
  //       nextVideoTitle: "Postura de castrado?",
  //       nextVideoDuration: "6:00",
  //       nextVideoThumbURL:
  //         "http://i3.ytimg.com/vi/IhJZivtBpg8/maxresdefault.jpg",
  //       description:
  //         "Aviões e Músicas, Aero por trás da aviação, Hoje no mundo militar..."
  //     }
  //   ];
  //   callback([
  //     {
  //       categoryID: 1,
  //       categoryName: "Música",
  //       stations: stations
  //     },
  //     {
  //       categoryID: 2,
  //       categoryName: "VLOGs",
  //       stations: stations
  //     },
  //     {
  //       categoryID: 3,
  //       categoryName: "Comédia",
  //       stations: stations
  //     },
  //     {
  //       categoryID: 4,
  //       categoryName: "Aviões",
  //       stations: stations
  //     },
  //     {
  //       categoryID: 5,
  //       categoryName: "Ciências",
  //       stations: stations
  //     }
  //   ]);
  //   // this.post("GetStationsByCategory", null, null , callback, callback);
  // };

  getEmptyStations = count => {
    let stations = [];
    let index = 0;
    for (let i = 0; i < count; i++) {
      stations.push({
        stationID: index - i,
        name: "",
        nextVideoTitle: "",
        nextVideoDuration: "",
        nextVideoThumbURL:
          "https://res.cloudinary.com/practicaldev/image/fetch/s--bIcIUu5D--/c_limit%2Cf_auto%2Cfl_progressive%2Cq_auto%2Cw_880/https://thepracticaldev.s3.amazonaws.com/i/t7u2rdii5u9n4zyqs2aa.jpg",
        fake: true
      });
    }
    return stations;
  };

  getFeaturedStations = async (callback, count) => {
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
      },
      {
        stationID: 4,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg"
      },
      {
        stationID: 5,
        name: "Stand Up Comedy BR",
        nextVideoTitle: "Carnaval no quarto 69",
        nextVideoDuration: "6:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/6rZ7uf8a5Ys/maxresdefault.jpg"
      },
      {
        stationID: 6,
        name: "Variados",
        nextVideoTitle: "Postura de castrado?",
        nextVideoDuration: "6:00",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/IhJZivtBpg8/maxresdefault.jpg"
      }
    ];
    callback(stations);
      // this.post(
      //   "GetFeaturedStations",
      //   null,
      //   { count },
      //   callback,
      //   callback
      // );
  };

  getMoreByCategory = async (
    categoryID,
    alreadyLoadedStations,
    count,
    callback
  ) => {
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
      },
      {
        stationID: 4,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg"
      },
      {
        stationID: 5,
        name: "Stand Up Comedy BR",
        nextVideoTitle: "Carnaval no quarto 69",
        nextVideoDuration: "6:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/6rZ7uf8a5Ys/maxresdefault.jpg"
      }
    ];
    callback(stations);
    // this.post(
    //   "GetMoreByCategory",
    //   null,
    //   { categoryID, count, alreadyLoadedStations},
    //   callback,
    //   callback
    // );
  };
}

export default new stationService();
