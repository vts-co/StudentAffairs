using StudentAffairs.Authorization;
using StudentAffairs.Dtos.SchoolInfo;
using StudentAffairs.Enums;
using StudentAffairs.Services.Cities;
using StudentAffairs.Services.SchoolInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentAffairs.Controllers
{
    [Authorized(ScreenId = "10")]
    public class SchoolInfoController : Controller
    {
        CitiesServices citiesServices = new CitiesServices();
        SchoolInfoServices schoolInfoServices = new SchoolInfoServices();
        // GET: Cities
        public ActionResult Index()
        {
            if((Role)TempData["RoleId"]!=Role.Super_Admin)
            {
                TempData["warning"] = "لا يوجد لديك صلاحية لهذه الصفحة";
                return RedirectToAction("SignIn", "Account");
            }
            var model = schoolInfoServices.GetAll();
            return View(model);
        }
        public ActionResult Create()
        {
            if ((Role)TempData["RoleId"] != Role.Super_Admin)
            {
                TempData["warning"] = "لا يوجد لديك صلاحية لهذه الصفحة";
                return RedirectToAction("SignIn", "Account");
            }
            var cities = citiesServices.GetAll();
            ViewBag.CityId = new SelectList(cities, "Id", "Name");

            return View("Upsert", new SchoolInfoDto());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(SchoolInfoDto schoolInfo, HttpPostedFileBase Image1)
        {
            if ((Role)TempData["RoleId"] != Role.Super_Admin)
            {
                TempData["warning"] = "لا يوجد لديك صلاحية لهذه الصفحة";
                return RedirectToAction("SignIn", "Account");
            }

            schoolInfo.Id = Guid.NewGuid();
            if (Image1 != null)
            {
                schoolInfo.Image = "/Uploads/SchoolInfo/";

                if (!Directory.Exists(Server.MapPath("~" + schoolInfo.Image + schoolInfo.Id)))
                    Directory.CreateDirectory(Server.MapPath("~" + schoolInfo.Image + schoolInfo.Id));
                schoolInfo.Image = schoolInfo.Image + schoolInfo.Id + "/" + schoolInfo.Id + ".jpg";
                Image1.SaveAs(Server.MapPath("~" + schoolInfo.Image));
            }
            var result = schoolInfoServices.Create(schoolInfo, (Guid)TempData["UserId"]);
            if (result.IsSuccess)
            {
                TempData["success"] = result.Message;
                return RedirectToAction("Index");
            }
            else
            {
                schoolInfo.Id = Guid.Empty;
                var cities = citiesServices.GetAll();
                ViewBag.CityId = new SelectList(cities, "Id", "Name", schoolInfo.CityId);

                TempData["warning"] = result.Message;
                return View("Upsert", schoolInfo);
            }
        }
       
        public ActionResult Edit(Guid Id)
        {
            if ((Role)TempData["RoleId"] != Role.Super_Admin&&Id!= (Guid)TempData["SchoolId"])
            {
                TempData["warning"] = "لا يوجد لديك صلاحية لهذه الصفحة";
                return RedirectToAction("SignIn", "Account");
            }
            var cities = citiesServices.GetAll();

            var schoolInfo = schoolInfoServices.Get();
            if (schoolInfo != null)
            {
                ViewBag.CityId = new SelectList(cities, "Id", "Name", schoolInfo.CityId);
                return View("Upsert", schoolInfo);
            }
            else
            {
                ViewBag.CityId = new SelectList(cities, "Id", "Name");
                return View("Upsert", new SchoolInfoDto() { Id = Guid.Empty });
            }
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(SchoolInfoDto schoolInfo, HttpPostedFileBase Image1)
        {
            if ((Role)TempData["RoleId"] != Role.Super_Admin && schoolInfo.Id != (Guid)TempData["SchoolId"])
            {
                TempData["warning"] = "لا يوجد لديك صلاحية لهذه الصفحة";
                return RedirectToAction("SignIn", "Account");
            }

            var cities = citiesServices.GetAll();
            ViewBag.CityId = new SelectList(cities, "Id", "Name", schoolInfo.CityId);


            if (Image1 != null)
            {
                schoolInfo.Image = "/Uploads/SchoolInfo/";

                if (!Directory.Exists(Server.MapPath("~" + schoolInfo.Image + schoolInfo.Id)))
                    Directory.CreateDirectory(Server.MapPath("~" + schoolInfo.Image + schoolInfo.Id));
                schoolInfo.Image = schoolInfo.Image + schoolInfo.Id + "/" + schoolInfo.Id + ".jpg";
                Image1.SaveAs(Server.MapPath("~" + schoolInfo.Image));
            }

            var result = schoolInfoServices.Edit(schoolInfo, (Guid)TempData["UserId"]);
            if (result.IsSuccess)
            {
                TempData["success"] = result.Message;
                return View("Upsert", schoolInfo);
            }
            else
            {
                TempData["warning"] = result.Message;
                return View("Upsert", schoolInfo);
            }


        }

        public ActionResult Delete(Guid Id)
        {
            if ((Role)TempData["RoleId"] != Role.Super_Admin)
            {
                TempData["warning"] = "لا يوجد لديك صلاحية لهذه الصفحة";
                return RedirectToAction("SignIn", "Account");
            }

            var result = schoolInfoServices.Delete(Id, (Guid)TempData["UserId"]);
            if (result.IsSuccess)
            {
                TempData["success"] = result.Message;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["warning"] = result.Message;
                return RedirectToAction("Index");
            }
        }
        public ActionResult getCityDepartments(Guid? Id)
        {
            var result = citiesServices.GetAll().Select(x => new { x.Id, x.Name, x.Code }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);

        }


    }
}