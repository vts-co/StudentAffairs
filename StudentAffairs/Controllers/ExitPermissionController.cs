using StudentAffairs.Authorization;
using StudentAffairs.Models;
using StudentAffairs.Services.ExitPermission;
using StudentAffairs.Services.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentAffairs.Controllers
{
    [Authorized(ScreenId = "16")]
    public class ExitPermissionController : Controller
    {
        ExitPermissionServices exitPermissionServices = new ExitPermissionServices();
        StudentsServices studentsServices = new StudentsServices();
        // GET: Cities
        public ActionResult Index()
        {
            var model = exitPermissionServices.GetAll();
            return View(model);
        }
        public ActionResult Create()
        {
            return View("Upsert", new ExitPermission());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(ExitPermission city)
        {
            city.Id = Guid.NewGuid();
            city.SchoolId = (Guid)TempData["SchoolId"];

            var result = exitPermissionServices.Create(city, (Guid)TempData["UserId"]);
            if (result.IsSuccess)
            {
                TempData["success"] = result.Message;
                return RedirectToAction("Index");
            }
            else
            {
                city.Id = Guid.Empty;

                TempData["warning"] = result.Message;
                return View("Upsert", city);
            }
        }

        public ActionResult Delete(Guid Id)
        {
            var result = exitPermissionServices.Delete(Id, (Guid)TempData["UserId"]);
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

        public ActionResult getStudent(string code)
        {
            if (code == null)
            {
                return Json(new Student(), JsonRequestBehavior.AllowGet);
            }
            var model = studentsServices.GetByCode(code);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}