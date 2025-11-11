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
    [Authorized(ScreenId = "15")]
    public class StudentReceiptController : Controller
    {
        StudentReceiptServices studentReceiptServices = new StudentReceiptServices();
        StudentsServices studentsServices = new StudentsServices();
        // GET: Cities
        public ActionResult Index()
        {
            var model = studentReceiptServices.GetAll();
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
            city.SchoolId = (Guid)TempData["SchoolId"];
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