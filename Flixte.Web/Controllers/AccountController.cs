using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Flixte.Web.Models;
using System.Web.Security;
using Flixte.Core.Models;
using Flixte.Core.Services;

namespace Flixte.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {        
        public AccountController()
        {
        }
                
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

            ViewBag.ReturnUrl = returnUrl;         
            if (User.Identity.IsAuthenticated)
                return RedirectToLocal(returnUrl);
            return View();
        }

        [AllowAnonymous]
        public ActionResult LoginGoogle(string token)
        {
            GoogleTokenValidation response;
            try
            {
                string data = Core.Util.GetURL("https://oauth2.googleapis.com/tokeninfo?id_token=" + token);
                response = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleTokenValidation>(data);
            }
            catch
            {
                return Json(new { @success = true, @message = "Error" }, JsonRequestBehavior.DenyGet);
            }
            if (response.aud != System.Configuration.ConfigurationManager.AppSettings["GoogleClientID"])
            {
                return Json(new { @success = false, @message = "Error" }, JsonRequestBehavior.DenyGet);
            }
            var userList = Flixte.Core.Services.UsuarioService.FindByFilter(null, response.email, null, null, null);
            Usuario userAccountInformation = null;
            if (userList.Count > 0)
            {
                userAccountInformation = userList.FirstOrDefault();                
            }
            else
            {
                Core.Services.UsuarioService.Insert(new Usuario() { Admin = false, Ativo = true, Data = DateTime.Now, Login = response.email, Nome = response.name,ImageURL= response.picture, Password = System.Configuration.ConfigurationManager.AppSettings["MasterKey"], UltimoAcesso = DateTime.Now, IDTipoUsuario = (int)Core.Util.UserType.Padrao, GGAccountID = response.sub, GGToken = token, GGEmailAddress = response.email });
                userAccountInformation = Core.Services.UsuarioService.FindByFilter(null, response.email, response.sub, null, null).FirstOrDefault();                
            }
            var tokenEntity = TokenService.FindByUser(userAccountInformation.ID);
            if (tokenEntity == null)
                Core.Services.TokenService.SaveToken(new Token() { TokenValue = Guid.NewGuid().ToString(), Ativo = true, Data = DateTime.Now, IDUsuario = userAccountInformation.ID });
            return MakeLogin(userAccountInformation.Login, System.Configuration.ConfigurationManager.AppSettings["MasterKey"], null);
        }
       // POST: /Account/Login
       [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    return MakeLogin(model.Email, model.Password, returnUrl);
                }
                catch (Exception ex)
                {
                    ViewData["Erro"] = "-ERROR-";
                    string erro = ex.Message;
                    AddErrors(new IdentityResult(new string[] { "Usuário ou senha não encontrados, tente novamente!" }));
                }
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }
        private ActionResult MakeLogin(string email, string password, string returnUrl)
        {
            Usuario userAccountInformation = Flixte.Core.Services.UsuarioService.Login(email, password);
            if (userAccountInformation != null)
            {
                var authTicket = new FormsAuthenticationTicket(999,
            string.Format("{0}|{1}|{2}|{3}", userAccountInformation.ID, userAccountInformation.Admin, userAccountInformation.Login, userAccountInformation.Nome != null ? userAccountInformation.Nome.Replace('|', ' ') : string.Empty),
            DateTime.Now.AddHours(-10),
            DateTime.Now.AddYears(10),
            true, userAccountInformation.Admin.ToString().ToUpper());
                var authCookie = new HttpCookie(
                    FormsAuthentication.FormsCookieName,
                    FormsAuthentication.Encrypt(authTicket)
                )
                {
                    //HttpOnly = true,
                    //Shareable = true,                
                    Expires = DateTime.Now.AddYears(2)

                };
                Response.AppendCookie(authCookie);
                ViewData["UserID"] = userAccountInformation.ID;
                var tokenEntity = TokenService.FindByUser(userAccountInformation.ID);
                if (tokenEntity == null)
                    Core.Services.TokenService.SaveToken(new Token() { TokenValue = Guid.NewGuid().ToString(), Ativo = true, Data = DateTime.Now, IDUsuario = userAccountInformation.ID });

                return RedirectToLocal(returnUrl);
            }
            else
            {
                ViewData["Erro"] = "-ERROR-";
                AddErrors(new IdentityResult(new string[] { "Usuário ou senha não encontrados, tente novamente!" }));
            }
            ViewBag.ReturnUrl = returnUrl;
            return View("Login");
        }
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(FormsAuthentication.FormsCookieName);
            string cookieName = FormsAuthentication.FormsCookieName;
            if (Response.Cookies == null ||
                Response.Cookies[cookieName] == null)
            {
                Response.Cookies.Remove(cookieName);
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            string message = "";
            bool success = false;
            Usuario userAccountInformation = null;
            if (ModelState.IsValid)
            {
                try
                {
                    new System.Net.Mail.MailAddress(model.Email);
                    if (Flixte.Core.Services.UsuarioService.FindByFilter(null, model.Email, null,null,null).Count == 0)
                    {
                        Core.Services.UsuarioService.Insert(new Usuario() { Admin = false, Ativo = true, Data = DateTime.Now, Login = model.Email, Nome = model.Name, Password = model.Password, UltimoAcesso = DateTime.Now, IDTipoUsuario = (int)Core.Util.UserType.Padrao, UserName = model.UserName });
                        var user = Core.Services.UsuarioService.FindByFilter(null, model.Email, null, null, null).FirstOrDefault();
                        userAccountInformation = user;
                        Core.Services.TokenService.SaveToken(new Token() { TokenValue = Guid.NewGuid().ToString(), Ativo = true, Data = DateTime.Now, IDUsuario = user.ID });
                        int userID = user.ID;
                        var tokenEntity = TokenService.FindByUser(userID);                        
                        var token = tokenEntity.TokenValue;
                        if (Response.Cookies[Core.Util.CookieTokenName] != null)
                            Response.Cookies.Remove(Core.Util.CookieTokenName);
                        Response.Cookies.Add(new HttpCookie(Core.Util.CookieTokenName, token));
                        return MakeLogin(model.Email, model.Password, "/Home/Index");                        
                    }
                    else
                    {
                        message = "Usuário já existente!";                        
                    }
                }
                catch (Exception ex)
                {
                    success = false;
                    string erro = ex.Message;
                    erro += "";
                    message = "Falha geral no cadadastro, tente novamente!";
                    ViewData["Erro"] = "-ERROR-";                    
                    AddErrors(new IdentityResult(new string[] { "Usuário ou senha não encontrados, tente novamente!" }));
                }
            }
            
            // Se chegamos até aqui e houver alguma falha, exiba novamente o formulário
            return View(model);
        }

        
        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userList = Flixte.Core.Services.UsuarioService.FindByFilter(null, model.Email, null, null, null);
                if (userList == null || userList.Count == 0)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                string password = Flixte.Core.Util.GeneratePassword();
                var userTemp = userList.FirstOrDefault();

                userTemp.Password = Flixte.Core.Util.MD5Encode(password);
                Flixte.Core.Services.UsuarioService.Update(userTemp);
                //PushService.SavePush(new Push() { DataCriacao = DateTime.Now, Destinatario = model.Email, Enviado = false, DataEnvio = null, Tipo = (int)PushType.Email, Body = "Foi solicitada a redefinição de senha do seu usuário em nosso sistema. <br/><br/>Para acessar novo site, utilize a senha: " + password + "<br/><br/>Agradecemos o contato e em caso de problemas, entre em contato por e-mail ou pelo formulário no site.<br/><br/>Atenciosamente,<br/>Equipe ConstruCode", ExtraParameters = "Sua senha foi atualizada!" });
                //string result = Flixte.Core.Util.SendMailSMTPSecure("Sua senha foi atualizada!", "Foi solicitada a redefinição de senha do seu usuário em nosso sistema. <br/><br/>Para acessar novo site, utilize a senha: " + password + "<br/><br/>Agradecemos o contato e em caso de problemas, entre em contato por e-mail ou pelo formulário no site.<br/><br/>Atenciosamente,<br/>Equipe ConstruCode", new string[] { model.Email }, null, "contato@construcode.com.br", "ConstruCode");
                //if (!string.IsNullOrEmpty(result))
                //    throw new Exception(result);
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // Se chegamos até aqui e houver alguma falha, exiba novamente o formulário
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }       
       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            base.Dispose(disposing);
        }

        #region Auxiliares
        // Usado para proteção XSRF ao adicionar logons externos
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}