import axios from "axios";
const APPJson = require("../../app.json");
import DeviceInfo from "react-native-device-info";
const qs = require("qs");

class baseService {
  constructor(serviceName) {
    this.service = axios.create({
      baseURL: "http://api.flixte.com/" + serviceName
    });
  }

  get = async (path, par, successCallback, errorCallback) => {
    try {
      if (successCallback)
        successCallback(await this.service.get(path, { params: par }));
      else return await this.service.get(path, { params: par });
    } catch (e) {
      if (errorCallback) errorCallback(e);
      else return e;
    }
  };

  post = async (path, obj, params, successCallback, errorCallback) => {
    try {
      let body = {data: obj, ...this.getBasicPostBody()};
      console.log(path+" posted data -> "+ JSON.stringify(body));
      if (params) path += "?" + qs.stringify(params);
      if (successCallback) successCallback(await this.service.post(path, body));
      else return await this.service.post(path, body);
    } catch (e) {
      if (errorCallback) errorCallback(e);
      else return e;
    }
  };
  
  getBasicPostBody = () => {
    return {
      AppVersion: APPJson.appVersion,
      DeviceID: DeviceInfo.getUniqueID(),
      IsAppRequest: true,
      ResponseLocale: DeviceInfo.getDeviceLocale()
    };
  };
}

export default baseService;
