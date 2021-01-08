using Flixte.APIV2.Models;
using Flixte.Core.Models;
using Flixte.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Http.Cors;
using System.Web.Mvc;

namespace Flixte.APIV2.Controllers
{
    //[EnableCors("*","*","*")]
    public class AuthenticationController : Controller
    {
        #region API   
        [AllowAnonymous]
        [HttpPost]
        public JsonResult LoginAPP(RequestModel<LoginViewModel> model)
        {
            Usuario userAccountInformation = null;
            try
            {                
                string token = "";
                string message = "OK";
                var success = false;
                if (!string.IsNullOrEmpty(model.Data.Email) && !string.IsNullOrEmpty(model.Data.Password))
                {
                    try
                    {
                        userAccountInformation = Flixte.Core.Services.UsuarioService.Login(model.Data.Email, model.Data.Password);
                        if (userAccountInformation != null)
                        {
                            var tokenEntity = TokenService.FindByUser(userAccountInformation.ID);
                            if (tokenEntity == null)
                                Core.Services.TokenService.SaveToken(new Token() { TokenValue = Guid.NewGuid().ToString(), Ativo = true, Data = DateTime.Now, IDUsuario = userAccountInformation.ID });


                            tokenEntity = TokenService.FindByUser(userAccountInformation.ID);
                            if (!string.IsNullOrEmpty(model.DeviceID))
                            {
                                tokenEntity.DeviceID = model.DeviceID;
                                tokenEntity.App = model.AppVersion;
                                tokenEntity.UltimoAcesso = DateTime.Now;
                                TokenService.Update(tokenEntity);
                            }
                            token = tokenEntity.TokenValue;
                            if (Response.Cookies[Core.Util.CookieTokenName] != null)
                                Response.Cookies.Remove(Core.Util.CookieTokenName);
                            Response.Cookies.Add(new HttpCookie(Core.Util.CookieTokenName, token));
                            success = true;
                        }
                        else
                        {
                            message = "Falha ao executar operação";
                            return Json(new ResponseModel()
                            {
                                Code = ResponseCodeType.GENERAL_ERROR,
                                Success = false,
                                Message = message,
                                Data = null
                            }, JsonRequestBehavior.DenyGet);
                        }
                    }
                    catch
                    {
                        message = "Usuário ou senha não encontrados, tente novamente!";
                        return Json(new ResponseModel()
                        {
                            Code = ResponseCodeType.GENERAL_ERROR,
                            Success = false,
                            Message = message,
                            Data = null
                        }, JsonRequestBehavior.DenyGet);
                    }
                }
                else
                {
                    return Json(new ResponseModel()
                    {
                        Code = ResponseCodeType.GENERAL_ERROR,
                        Success = false,
                        Message = "Request invalido",
                        Data = null
                    }, JsonRequestBehavior.DenyGet);
                }

                return Json(new ResponseModel()
                {
                    Code = ResponseCodeType.SUCCESS,
                    Success = success,
                    Message = message,
                    Data = new { @Token = token, @User = userAccountInformation.APIResponseUser }
                }, JsonRequestBehavior.DenyGet);
            }
            catch
            {
                return Json(new ResponseModel()
                {
                    Code = ResponseCodeType.OPERATION_FAULT,
                    Success = false,
                    Message = "Error"
                }, JsonRequestBehavior.DenyGet);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult RegisterAPP(RequestModel<RegisterViewModel> model)
        {
            Usuario userAccountInformation = null;
            try
            {
                string token = "";
                string message = "Error";
                var success = false;
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(model.Data.Email) || string.IsNullOrEmpty(model.Data.UserName) || string.IsNullOrEmpty(model.Data.Password))
                            return Json(new ResponseModel()
                            {
                                Code = ResponseCodeType.GENERAL_ERROR,
                                Success = false,
                                Message = "Request invalido",
                                Data = null
                            }, JsonRequestBehavior.DenyGet);
                        new System.Net.Mail.MailAddress(model.Data.Email);
                        if (Core.Services.UsuarioService.FindByFilter(null, model.Data.Email, null,null,null).Count == 0)
                        {
                            Core.Services.UsuarioService.Insert(new Usuario() { Admin = false, Ativo = true, Data = DateTime.Now, Login = model.Data.Email, Nome = model.Data.Name, Password = model.Data.Password, UltimoAcesso = DateTime.Now, IDTipoUsuario = (int)Core.Util.UserType.Padrao, UserName = model.Data.UserName });
                            var user = Core.Services.UsuarioService.FindByFilter(null, model.Data.Email, null, null, null).FirstOrDefault();
                            userAccountInformation = user;
                            Core.Services.TokenService.SaveToken(new Token() { TokenValue = Guid.NewGuid().ToString(), Ativo = true, Data = DateTime.Now, IDUsuario = user.ID });
                            int userID = user.ID;
                            var tokenEntity = TokenService.FindByUser(userID);
                            if (!string.IsNullOrEmpty(model.DeviceID))
                            {
                                tokenEntity.DeviceID = model.DeviceID;
                                tokenEntity.App = model.AppVersion;
                                tokenEntity.UltimoAcesso = DateTime.Now;
                                TokenService.Update(tokenEntity);
                            }
                            token = tokenEntity.TokenValue;
                            if (Response.Cookies[Core.Util.CookieTokenName] != null)
                                Response.Cookies.Remove(Core.Util.CookieTokenName);
                            Response.Cookies.Add(new HttpCookie(Core.Util.CookieTokenName, token));
                            message = "OK";
                            success = true;
                        }
                        else
                        {
                            message = "Usuário já existente!";
                            return Json(new ResponseModel() { Code = ResponseCodeType.USER_NOT_FOUND, Success = false, Message = message }, JsonRequestBehavior.DenyGet);
                        }
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        string erro = ex.Message;
                        erro += "";
                        message = "Falha geral no cadadastro, tente novamente!";
                        return Json(new ResponseModel()
                        {
                            Code = ResponseCodeType.OPERATION_FAULT,
                            Success = false,
                            Message = message
                        }, JsonRequestBehavior.DenyGet);
                    }
                }
                else
                {
                    return Json(new ResponseModel()
                    {
                        Code = ResponseCodeType.EMPTY_FIELDS,
                        Success = false,
                        Message = "Request invalido"
                    }, JsonRequestBehavior.DenyGet);
                }

                return Json(new ResponseModel()
                {
                    Code = ResponseCodeType.SUCCESS,
                    Success = success,
                    Message = message,
                    Data = new { @Token = token, @User = userAccountInformation.APIResponseUser }
                }, JsonRequestBehavior.DenyGet);
            }
            catch
            {
                return Json(new ResponseModel()
                {
                    Code = ResponseCodeType.GENERAL_ERROR,
                    Success = false,
                    Message = "Error"
                }, JsonRequestBehavior.DenyGet);
            }
        }
        #endregion

        [HttpPost]
        public JsonResult TryLoginWithGoogle(RequestModel<string> model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Data))
                    return Json(new ResponseModel()
                    {
                        Code = ResponseCodeType.GENERAL_ERROR,
                        Success = false,
                        Message = "Request invalido",
                        Data = null
                    }, JsonRequestBehavior.DenyGet);
                var userList = Flixte.Core.Services.UsuarioService.FindByFilter(null, null, null, null, model.Data);
                Usuario userAccountInformation = null;
                if (userList.Count > 0)
                {
                    userAccountInformation = userList.FirstOrDefault();

                    var tokenEntity = TokenService.FindByUser(userAccountInformation.ID);
                    if (tokenEntity == null)
                        Core.Services.TokenService.SaveToken(new Token() { TokenValue = Guid.NewGuid().ToString(), Ativo = true, Data = DateTime.Now, IDUsuario = userAccountInformation.ID });
                    tokenEntity = TokenService.FindByUser(userAccountInformation.ID);
                    if (!string.IsNullOrEmpty(model.DeviceID))
                    {
                        tokenEntity.DeviceID = model.DeviceID;
                        tokenEntity.App = model.AppVersion;
                        tokenEntity.UltimoAcesso = DateTime.Now;
                        TokenService.Update(tokenEntity);
                    }
                    string token = tokenEntity.TokenValue;
                    if (Response.Cookies[Core.Util.CookieTokenName] != null)
                        Response.Cookies.Remove(Core.Util.CookieTokenName);
                    Response.Cookies.Add(new HttpCookie(Core.Util.CookieTokenName, token));
                    return Json(new ResponseModel()
                    {
                        Code = ResponseCodeType.SUCCESS,
                        Success = true,
                        Message = "OK",
                        Data = new { @Token = token, @User = userAccountInformation.APIResponseUser }
                    }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    string message = "Usuário ou senha não encontrados, tente novamente!";
                    return Json(new ResponseModel()
                    {
                        Code = ResponseCodeType.GENERAL_ERROR,
                        Success = false,
                        Message = message,
                        Data = null
                    }, JsonRequestBehavior.DenyGet);
                }
            }
            catch
            {
                return Json(new ResponseModel() { Code = ResponseCodeType.OPERATION_FAULT, Success = false, Message = "Error" }, JsonRequestBehavior.DenyGet);
            }
        }
        public JsonResult RegisterWithGoogle(RequestModel<GoogleLoginRegister> model)
        {
            Usuario userAccountInformation = null;
            model.Data.Email = Flixte.Core.Util.GeneratePassword() + "_" + DateTime.Now.Ticks + "@tapioca.com";
            model.Data.Name = Flixte.Core.Util.GeneratePassword() + "_" + DateTime.Now.Ticks;
            if (string.IsNullOrEmpty(model.Data.UserName) || string.IsNullOrEmpty(model.Data.IDToken) || string.IsNullOrEmpty(model.Data.ServerAuthCode) || string.IsNullOrEmpty(model.Data.GoogleID))
                return Json(new ResponseModel()
                {
                    Code = ResponseCodeType.GENERAL_ERROR,
                    Success = false,
                    Message = "Request invalido",
                    Data = null
                }, JsonRequestBehavior.DenyGet);
            try
            {
                string token = "";
                string message = "Error";
                var success = false;
                if (ModelState.IsValid)
                {
                    try
                    {
                        new System.Net.Mail.MailAddress(model.Data.Email);
                        if (Core.Services.UsuarioService.FindByFilter(null, null, null, model.Data.UserName, null).Count > 0)
                        {
                            message = "UserName já existente!";
                            return Json(new ResponseModel() { Code = ResponseCodeType.USER_NOT_FOUND, Success = false, Message = message }, JsonRequestBehavior.DenyGet);
                        }
                        if (Core.Services.UsuarioService.FindByFilter(null, model.Data.Email, null, null, null).Count == 0)
                        {
                            Core.Services.UsuarioService.Insert(new Usuario() { Admin = false, Ativo = true, Data = DateTime.Now, Login = model.Data.Email, Nome = model.Data.Name, Password = Flixte.Core.Util.GeneratePassword(), UltimoAcesso = DateTime.Now, IDTipoUsuario = (int)Core.Util.UserType.Padrao, UserName = model.Data.UserName });
                            var user = Core.Services.UsuarioService.FindByFilter(null, model.Data.Email, null, null, null).FirstOrDefault();
                            userAccountInformation = user;
                            Core.Services.TokenService.SaveToken(new Token() { TokenValue = Guid.NewGuid().ToString(), Ativo = true, Data = DateTime.Now, IDUsuario = user.ID });
                            int userID = user.ID;
                            var tokenEntity = TokenService.FindByUser(userID);
                            if (!string.IsNullOrEmpty(model.DeviceID))
                            {
                                tokenEntity.DeviceID = model.DeviceID;
                                tokenEntity.App = model.AppVersion;
                                tokenEntity.UltimoAcesso = DateTime.Now;
                                TokenService.Update(tokenEntity);
                            }
                            token = tokenEntity.TokenValue;
                            if (Response.Cookies[Core.Util.CookieTokenName] != null)
                                Response.Cookies.Remove(Core.Util.CookieTokenName);
                            Response.Cookies.Add(new HttpCookie(Core.Util.CookieTokenName, token));
                            message = "OK";
                            success = true;
                        }
                        else
                        {
                            message = "Usuário já existente!";
                            return Json(new ResponseModel() { Code = ResponseCodeType.USER_NOT_FOUND, Success = false, Message = message }, JsonRequestBehavior.DenyGet);
                        }
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        string erro = ex.Message;
                        erro += "";
                        message = "Falha geral no cadadastro, tente novamente!";
                        return Json(new ResponseModel()
                        {
                            Code = ResponseCodeType.OPERATION_FAULT,
                            Success = false,
                            Message = message
                        }, JsonRequestBehavior.DenyGet);
                    }
                }
                else
                {
                    return Json(new ResponseModel()
                    {
                        Code = ResponseCodeType.EMPTY_FIELDS,
                        Success = false,
                        Message = "Request invalido"
                    }, JsonRequestBehavior.DenyGet);
                }

                return Json(new ResponseModel()
                {
                    Code = ResponseCodeType.SUCCESS,
                    Success = success,
                    Message = message,
                    Data = new { @Token = token, @User = userAccountInformation.APIResponseUser }
                }, JsonRequestBehavior.DenyGet);
            }
            catch
            {
                return Json(new ResponseModel()
                {
                    Code = ResponseCodeType.GENERAL_ERROR,
                    Success = false,
                    Message = "Error"
                }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public JsonResult LoginGoogle(RequestModel<GoogleTokenValidation> model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Data.user_id) || string.IsNullOrEmpty(model.Data.audience))
                    return Json(new ResponseModel()
                    {
                        Code = ResponseCodeType.GENERAL_ERROR,
                        Success = false,
                        Message = "Request invalido",
                        Data = null
                    }, JsonRequestBehavior.DenyGet);
                GoogleTokenValidation2 response;
                try
                {
                    string data = Core.Util.GetURL("https://oauth2.googleapis.com/tokeninfo?id_token=" + model.Data.audience);
                    response = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleTokenValidation2>(data);
                }
                catch
                {
                    return Json(new { @success = true, @message = "Error" }, JsonRequestBehavior.DenyGet);
                }
                if (response.aud != System.Configuration.ConfigurationManager.AppSettings["GoogleClientID"])
                {
                    return Json(new { @success = false, @message = "Error" }, JsonRequestBehavior.DenyGet);
                }
                var userList = Flixte.Core.Services.UsuarioService.FindByFilter(response.email, null, null, null, null);
                Usuario userAccountInformation = null;
                if (userList.Count > 0)
                {
                    userAccountInformation = userList.FirstOrDefault();
                }
                else
                {
                    Core.Services.UsuarioService.Insert(new Usuario() { Admin = false, Ativo = true, Data = DateTime.Now, Login = response.email, Nome = response.name, Password = System.Configuration.ConfigurationManager.AppSettings["MasterKey"], ImageURL=response.picture, UltimoAcesso = DateTime.Now, IDTipoUsuario = (int)Core.Util.UserType.Padrao, GGAccountID = response.sub, GGToken = model.Data.audience, GGEmailAddress = response.email  });
                    userAccountInformation = Core.Services.UsuarioService.FindByFilter(null, response.email, null, null, null).FirstOrDefault();
                }
                var tokenEntity = TokenService.FindByUser(userAccountInformation.ID);
                if (tokenEntity == null)
                    Core.Services.TokenService.SaveToken(new Token() { TokenValue = Guid.NewGuid().ToString(), Ativo = true, Data = DateTime.Now, IDUsuario = userAccountInformation.ID });
                tokenEntity = TokenService.FindByUser(userAccountInformation.ID);
                if (!string.IsNullOrEmpty(model.DeviceID))
                {
                    tokenEntity.DeviceID = model.DeviceID;
                    tokenEntity.App = model.AppVersion;
                    tokenEntity.UltimoAcesso = DateTime.Now;
                    TokenService.Update(tokenEntity);
                }
                string token = tokenEntity.TokenValue;
                if (Response.Cookies[Core.Util.CookieTokenName] != null)
                    Response.Cookies.Remove(Core.Util.CookieTokenName);
                Response.Cookies.Add(new HttpCookie(Core.Util.CookieTokenName, token));
                return Json(new ResponseModel()
                {
                    Code = ResponseCodeType.SUCCESS,
                    Success = true,
                    Message = "OK",
                    Data = new { @Token = token, @User = userAccountInformation.APIResponseUser }
                }, JsonRequestBehavior.DenyGet);
            }
            catch
            {
                return Json(new ResponseModel() { Code = ResponseCodeType.OPERATION_FAULT, Success = false, Message = "Error" }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public JsonResult GetLoggedUser(RequestModel<string> model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Data))
                    return Json(new ResponseModel()
                    {
                        Code = ResponseCodeType.GENERAL_ERROR,
                        Success = false,
                        Message = "Request invalido",
                        Data = null
                    }, JsonRequestBehavior.DenyGet);
                var usuario = TokenService.FindByToken(model.Data);
                if (usuario == null)
                    return Json(new ResponseModel()
                    {
                        Code = ResponseCodeType.GENERAL_ERROR,
                        Success = false,
                        Message = "Token not found"
                    }, JsonRequestBehavior.DenyGet);
                else
                {
                    return Json(new ResponseModel()
                    {
                        Code = ResponseCodeType.SUCCESS,
                        Success = true,
                        Message = "OK",
                        Data = new { @token = model.Data, @User = usuario.APIResponseUser }
                    }, JsonRequestBehavior.DenyGet);
                }
            }
            catch
            {
                return Json(new ResponseModel()
                {
                    Code = ResponseCodeType.OPERATION_FAULT,
                    Success = false,
                    Message = "Error"
                }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public JsonResult IsUser(RequestModel<string> model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Data))
                    return Json(new ResponseModel()
                    {
                        Code = ResponseCodeType.GENERAL_ERROR,
                        Success = false,
                        Message = "Request invalido",
                        Data = null
                    }, JsonRequestBehavior.DenyGet);
                var usuario = UsuarioService.FindByFilter(null, model.Data.IndexOf("@") != -1 ? model.Data : null, null, model.Data.IndexOf("@") == -1 ? model.Data : null, null);
                if (usuario.Count == 0)
                    return Json(new ResponseModel()
                    {
                        Code = ResponseCodeType.SUCCESS,
                        Success = true,
                        Message = "User Free",
                        Data = new { @username = model.Data }
                    }, JsonRequestBehavior.DenyGet);
                else
                {
                    return Json(new ResponseModel()
                    {
                        Code = ResponseCodeType.EMPTY_FIELDS,
                        Success = false,
                        Message = "User Exists",
                        Data = new { @username = model.Data }
                    }, JsonRequestBehavior.DenyGet);
                }
            }
            catch
            {
                return Json(new ResponseModel()
                {
                    Code = ResponseCodeType.OPERATION_FAULT,
                    Success = false,
                    Message = "Error"
                }, JsonRequestBehavior.DenyGet);
            }
        }
    }
}