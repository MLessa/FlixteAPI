import baseService from "./baseService";

class playlistService extends baseService {
  constructor() {
    super("Playlist");
  }

  getEmptyPlaylists = count => {
    let playlists = [];
    let index = 0;
    for (let i = 0; i < count; i++) {
      playlists.push({
        playlistID: index - i,
        name: "",
        nextVideoTitle: "",
        nextVideoDuration: "",
        nextVideoThumbURL:
          "https://res.cloudinary.com/practicaldev/image/fetch/s--bIcIUu5D--/c_limit%2Cf_auto%2Cfl_progressive%2Cq_auto%2Cw_880/https://thepracticaldev.s3.amazonaws.com/i/t7u2rdii5u9n4zyqs2aa.jpg"
      });
    }
    return playlists;
  };

  getFeaturedPlaylistsByCategory = async callback => {
    const playlists = [
      {
        playlistID: 1,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg",
        description:
          "Aviões e Músicas, Aero por trás da aviação, Hoje no mundo militar..."
      },
      {
        playlistID: 2,
        name: "Stand Up Comedy BR",
        nextVideoTitle: "Carnaval no quarto 69",
        nextVideoDuration: "6:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/6rZ7uf8a5Ys/maxresdefault.jpg",
        description:
          "Aviões e Músicas, Aero por trás da aviação, Hoje no mundo militar..."
      },
      {
        playlistID: 3,
        name: "Variados",
        nextVideoTitle: "Postura de castrado?",
        nextVideoDuration: "6:00",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/IhJZivtBpg8/maxresdefault.jpg",
        description:
          "Aviões e Músicas, Aero por trás da aviação, Hoje no mundo militar..."
      },
      {
        playlistID: 4,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg",
        description:
          "Aviões e Músicas, Aero por trás da aviação, Hoje no mundo militar..."
      },
      {
        playlistID: 5,
        name: "Stand Up Comedy BR",
        nextVideoTitle: "Carnaval no quarto 69",
        nextVideoDuration: "6:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/6rZ7uf8a5Ys/maxresdefault.jpg",
        description:
          "Aviões e Músicas, Aero por trás da aviação, Hoje no mundo militar..."
      },
      {
        playlistID: 6,
        name: "Variados",
        nextVideoTitle: "Postura de castrado?",
        nextVideoDuration: "6:00",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/IhJZivtBpg8/maxresdefault.jpg",
        description:
          "Aviões e Músicas, Aero por trás da aviação, Hoje no mundo militar..."
      }
    ];
    callback([
      {
        categoryID: 1,
        categoryName: "Cultura",
        playlists: playlists
      },
      {
        categoryID: 2,
        categoryName: "Animes",
        playlists: playlists
      },
      {
        categoryID: 3,
        categoryName: "Mundo Nerd",
        playlists: playlists
      },
      {
        categoryID: 4,
        categoryName: "ASMR",
        playlists: playlists
      },
      {
        categoryID: 5,
        categoryName: "Funk",
        playlists: playlists
      }
    ]);
    // this.post("GetFeaturedPlaylistsByCategory", null, null , callback, callback);
  };

  getPlaylistsByCategory = async callback => {
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
      },
      {
        playlistID: 4,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg"
      },
      {
        playlistID: 5,
        name: "Stand Up Comedy BR",
        nextVideoTitle: "Carnaval no quarto 69",
        nextVideoDuration: "6:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/6rZ7uf8a5Ys/maxresdefault.jpg"
      },
      {
        playlistID: 6,
        name: "Variados",
        nextVideoTitle: "Postura de castrado?",
        nextVideoDuration: "6:00",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/IhJZivtBpg8/maxresdefault.jpg"
      }
    ];
    callback([
      {
        categoryID: 1,
        categoryName: "Cultura",
        playlists: playlists
      },
      {
        categoryID: 2,
        categoryName: "Animes",
        playlists: playlists
      },
      {
        categoryID: 3,
        categoryName: "Mundo Nerd",
        playlists: playlists
      },
      {
        categoryID: 4,
        categoryName: "ASMR",
        playlists: playlists
      },
      {
        categoryID: 5,
        categoryName: "Funk",
        playlists: playlists
      }
    ]);
    // this.post("GetPlaylistsByCategory", null, null , callback, callback);
  };

  getMoreFeaturedPlaylists = async (
    alreadyLoadedPlaylists,
    count,
    callback
  ) => {
    const playlists = [
      {
        playlistID: 1,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg",
        description:
          "Aviões e Músicas, Aero por trás da aviação, Hoje no mundo militar..."
      },
      {
        playlistID: 2,
        name: "Stand Up Comedy BR",
        nextVideoTitle: "Carnaval no quarto 69",
        nextVideoDuration: "6:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/6rZ7uf8a5Ys/maxresdefault.jpg",
        description:
          "Aviões e Músicas, Aero por trás da aviação, Hoje no mundo militar..."
      },
      {
        playlistID: 3,
        name: "Variados",
        nextVideoTitle: "Postura de castrado?",
        nextVideoDuration: "6:00",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/IhJZivtBpg8/maxresdefault.jpg",
        description:
          "Aviões e Músicas, Aero por trás da aviação, Hoje no mundo militar..."
      },
      {
        playlistID: 4,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg",
        description:
          "Aviões e Músicas, Aero por trás da aviação, Hoje no mundo militar..."
      },
      {
        playlistID: 5,
        name: "Stand Up Comedy BR",
        nextVideoTitle: "Carnaval no quarto 69",
        nextVideoDuration: "6:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/6rZ7uf8a5Ys/maxresdefault.jpg",
        description:
          "Aviões e Músicas, Aero por trás da aviação, Hoje no mundo militar..."
      },
      {
        playlistID: 6,
        name: "Variados",
        nextVideoTitle: "Postura de castrado?",
        nextVideoDuration: "6:00",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/IhJZivtBpg8/maxresdefault.jpg",
        description:
          "Aviões e Músicas, Aero por trás da aviação, Hoje no mundo militar..."
      }
    ];
    callback(playlists);
    // this.post(
    //   "GetMoreFeaturedPlaylists",
    //   null,
    //   { count, alreadyLoadedPlaylists },
    //   callback,
    //   callback
    // );
  };

  getMoreByCategory = async (
    categoryID,
    alreadyLoadedPlaylists,
    count,
    callback
  ) => {
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
      },
      {
        playlistID: 4,
        name: "Top ciência BR",
        nextVideoTitle: "De onde vem o sexo",
        nextVideoDuration: "3:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/KRdFCVUkM9s/maxresdefault.jpg"
      },
      {
        playlistID: 5,
        name: "Stand Up Comedy BR",
        nextVideoTitle: "Carnaval no quarto 69",
        nextVideoDuration: "6:25",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/6rZ7uf8a5Ys/maxresdefault.jpg"
      },
      {
        playlistID: 6,
        name: "Variados",
        nextVideoTitle: "Postura de castrado?",
        nextVideoDuration: "6:00",
        nextVideoThumbURL:
          "http://i3.ytimg.com/vi/IhJZivtBpg8/maxresdefault.jpg"
      }
    ];
    callback(playlists);
    // this.post(
    //   "GetMoreByCategory",
    //   null,
    //   { categoryID, count, alreadyLoadedPlaylists},
    //   callback,
    //   callback
    // );
  };
}

export default new playlistService();
