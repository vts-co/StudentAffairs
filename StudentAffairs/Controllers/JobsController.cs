using StudentAffairs.Authorization;
using StudentAffairs.Enums;
using StudentAffairs.Models;
using StudentAffairs.Services.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentAffairs.Controllers
{
    [Authorized(ScreenId = "18")]

    public class JobsController : Controller
    {
        JobsServices jobsServices = new JobsServices();
        // GET: Cities
        public ActionResult Index()
        {
            var model = jobsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            return View(model);
        }
        public ActionResult Create()
        {
            return View("Upsert", new Job());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Job model)
        {
            model.Id = Guid.NewGuid();
            model.SchoolId = (Guid)TempData["SchoolId"];

            var result = jobsServices.Create(model, (Guid)TempData["UserId"]);
            if (result.IsSuccess)
            {
                TempData["success"] = result.Message;
                return RedirectToAction("Index");
            }
            else
            {
                model.Id = Guid.Empty;

                TempData["warning"] = result.Message;
                return View("Upsert", model);
            }
        }
        public ActionResult Edit(Guid Id)
        {
            var city = jobsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]).Where(x => x.Id == Id).FirstOrDefault();
            return View("Upsert", city);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Job model)
        {

            var result = jobsServices.Edit(model, (Guid)TempData["UserId"]);
            if (result.IsSuccess)
            {
                TempData["success"] = result.Message;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["warning"] = result.Message;
                return View("Upsert", model);
            }
        }
        public ActionResult Delete(Guid Id)
        {
            var result = jobsServices.Delete(Id, (Guid)TempData["UserId"]);
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
    }
}