import baseService from "./baseService";

class searchService extends baseService {
  constructor() {
    super("Search");
  }

  search = async (searchTerm, successCallback, errorCallback) => {
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
    const initialContent = [
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
    setTimeout(function() {
      successCallback(initialContent);
    }, 2000);

    //await this.post("search", searchTerm, null, successCallback, errorCallback);
  };
}

export default new searchService();
