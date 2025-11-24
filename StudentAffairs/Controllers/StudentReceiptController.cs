using StudentAffairs.Authorization;
using StudentAffairs.Models;
using StudentAffairs.Services.StudentReceipt;
using StudentAffairs.Services.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentAffairs.Enums
{
    [Authorized(ScreenId = "18")]
    public class StudentReceiptController : Controller
    {
        StudentReceiptServices studentReceiptServices = new StudentReceiptServices();
        StudentsServices studentsServices = new StudentsServices();
        // GET: Cities
        public ActionResult Index(Guid? SchoolId)
        {
            if ((Role)TempData["RoleId"] == Role.Super_Admin && SchoolId == null)
            {
                return View(new List<StudentReceipt>());
            }
            var model = studentReceiptServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            if (SchoolId != null)
                model = model.Where(x => x.SchoolId == SchoolId).ToList();
            return View(model);
        }
        public ActionResult Create()
        {
            return View("Upsert", new StudentReceipt());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(StudentReceipt city)
        {
            city.Id = Guid.NewGuid();
            //city.SchoolId = (Guid)TempData["SchoolId"];
            var result = studentReceiptServices.Create(city, (Guid)TempData["UserId"]);
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
            var result = studentReceiptServices.Delete(Id, (Guid)TempData["UserId"]);
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

        public ActionResult getStudent(string code,Guid? SchoolId)
        {
            if (code == null)
            {
                return Json(new Student(), JsonRequestBehavior.AllowGet);
            }
            var model = studentsServices.GetByCode(code, SchoolId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}