using StudentAffairs.Authorization;
using StudentAffairs.Dtos.Account;
using StudentAffairs.Models;
using StudentAffairs.Services;
using StudentAffairs.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentAffairs.Controllers
{
    public class AccountController : Controller
    {
        AccountServices accountService = new AccountServices();
        // GET: Account
        public ActionResult SignIn()
        {

            TempData["SettingLogo"] = "/Uploads/Logo/ProgramLogo.PNG";
            TempData["SettingTitle"] = "Vision Tech";
            var pass = Security.Encrypt("StudentAffairs@123");

            
            return View(new SignInDto());
        }
        [HttpPost]
        public ActionResult SignIn(SignInDto userInfo)
        {
            TempData["SettingLogo"] = "/Uploads/Logo/ProgramLogo.PNG";
            TempData["SettingTitle"] = "Vision Tech";

            var result = accountService.Login(userInfo.UserName, userInfo.Password);
            if (result.IsSuccess)
            {
                VTSAuth auth = new VTSAuth();
                auth.SaveToCookies(result.Result);
                string returnUrl = Request.QueryString["returnUrl"];
                if (!string.IsNullOrWhiteSpace(returnUrl))
                    return Redirect(returnUrl);

                TempData["success"] = result.Message;
                return RedirectToAction("Index", "Home");

            }
            else
            {
                TempData["warning"] = result.Message;
                userInfo.Password = "";
                return View(userInfo);
            }
        }
        public ActionResult SignOut()
        {
            VTSAuth auth = new VTSAuth();
            auth.LoadDataFromCookies();
            auth.ClearCookies();
            return RedirectToAction("Index", "Home");
        }

    }
}