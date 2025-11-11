using StudentAffairs.Enums;
using StudentAffairs.Models;
using StudentAffairs.Services.Users;
using StudentAffairs.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StudentAffairs.Authorization
{
    public class Authorized : ActionFilterAttribute, IExceptionFilter
    {
        public Role Role { get; set; } = 0;
        public Guid UserId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid SchoolId { get; set; }

        public string[] UserScreens = null;
        public string ScreenId = null;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string search = "," + ScreenId + ",";
            UsersServices usersServices = new UsersServices();
            var controller = filterContext.Controller as Controller;
            if (controller != null)
            {
                VTSAuth auth = new VTSAuth() { CookieValues = new UserInfo { } };
                var load = auth.LoadDataFromCookies();
                var user = usersServices.Get(auth.CookieValues.UserId);

                if (!load || user == null)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "SignIn", returnUrl = filterContext.HttpContext.Request.Url.ToString() }));
                    return;
                }
                else if ((auth.CookieValues.RoleId & Role) == 0)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "SignIn", returnUrl = filterContext.HttpContext.Request.Url.ToString() }));

                }
                else if (ScreenId == null)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "SignIn", returnUrl = filterContext.HttpContext.Request.Url.ToString() }));

                }
                else if (user.UserScreens == null)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "SignIn", returnUrl = filterContext.HttpContext.Request.Url.ToString() }));

                }
                else if (!user.UserScreens.Contains(search))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "SignIn", returnUrl = filterContext.HttpContext.Request.Url.ToString() }));
                }
                filterContext.Controller.TempData["UserInfo"] = auth.CookieValues;
                filterContext.Controller.TempData["UserName"] = auth.CookieValues.UserName;
                filterContext.Controller.TempData["UserId"] = auth.CookieValues.UserId;
                filterContext.Controller.TempData["EmployeeId"] = auth.CookieValues.EmployeeId;
                filterContext.Controller.TempData["SchoolId"] = auth.CookieValues.SchoolId;
                filterContext.Controller.TempData["RoleId"] = auth.CookieValues.RoleId;
                filterContext.Controller.ViewBag.EmployeeName = auth.CookieValues.EmployeeName;

                filterContext.Controller.ViewBag.UserScreens = user.UserScreens;
                filterContext.Controller.ViewBag.UserName = user.Username;
                
                filterContext.Controller.TempData["SettingLogo"] = "/Uploads/Logo/ProgramLogo.PNG";
                filterContext.Controller.TempData["SettingTitle"] = "Vision Tech";
                

            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "SignIn", returnUrl = filterContext.HttpContext.Request.Url.ToString() }));
                return;
            }
        }

        public void OnException(ExceptionContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new {  controller = "Account", action = "SignIn", returnUrl = filterContext.HttpContext.Request.Url.ToString() }));
        }
    }
    public class UserInfo
    {
        public Guid UserId { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public Guid SchoolId { get; set; }
        public string SchoolName { get; set; }

        public Role RoleId { get; set; }

        public string UserName { get; set; }
        public string UserScreens { get; set; }

    }

}