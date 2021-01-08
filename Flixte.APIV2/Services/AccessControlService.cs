using Flixte.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flixte.APIV2.Services
{
    public class AccessControlService : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LogAction(filterContext);
            base.OnActionExecuting(filterContext);
        }

        private void LogAction(ActionExecutingContext filterContext)
        {
            if ((System.Configuration.ConfigurationManager.AppSettings["LogActivated"] ?? "true") == "true" &&
                filterContext.Controller.ControllerContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var controller = (string)filterContext.RouteData.Values["controller"];
                var action = (string)filterContext.RouteData.Values["action"];
                int? userid = filterContext.Controller.ControllerContext.HttpContext.User.Identity.IsAuthenticated
                                  ? (int?)
                                    Convert.ToInt32(
                                        filterContext.Controller.ControllerContext.HttpContext.User.Identity.Name.Split('|').
                                            First())
                                  : null;
                string formValues = string.Empty;
                string parameters = string.Empty;
                string ip = filterContext.RequestContext.HttpContext.Request.UserHostAddress;
                foreach (var item in filterContext.ActionParameters)
                {
                    parameters += item.Key + ":" + (item.Value == null ? "null" : item.Value.ToString()) + " | ";
                }
                foreach (var item in filterContext.Controller.ControllerContext.HttpContext.Request.Form.AllKeys)
                {
                    formValues += item + ":" + filterContext.Controller.ControllerContext.HttpContext.Request.Form[item] + " | ";
                }
                LogService.SaveLog(ip, userid, parameters, formValues, controller, action);
            }
            var rolesOfUser = new string[] { "" };
            if (filterContext.Controller.ControllerContext.HttpContext.User.Identity.IsAuthenticated)
            {
                string roles = ((System.Web.Security.FormsIdentity)filterContext.Controller.ControllerContext.HttpContext.User.Identity).Ticket.UserData;
                rolesOfUser = roles.Split(',');
            }         
        }
    }
}