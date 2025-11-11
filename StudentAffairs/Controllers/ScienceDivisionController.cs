using StudentAffairs.Authorization;
using StudentAffairs.Enums;
using StudentAffairs.Models;
using StudentAffairs.Services.ScienceDivision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentAffairs.Controllers
{
    [Authorized(ScreenId = "8")]
    public class ScienceDivisionController : Controller
    {

        ScienceDivisionServices scienceDivisionServices = new ScienceDivisionServices();
        // GET: Cities
        public ActionResult Index()
        {
            var model = scienceDivisionServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            return View(model);
        }
        public ActionResult Create()
        {
            return View("Upsert", new ScienceDivision());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(ScienceDivision model)
        {
            model.Id = Guid.NewGuid();
            model.SchoolId = (Guid)TempData["SchoolId"];

            var result = scienceDivisionServices.Create(model, (Guid)TempData["UserId"]);
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
            var city = scienceDivisionServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]).Where(x => x.Id == Id).FirstOrDefault();
            return View("Upsert", city);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(ScienceDivision model)
        {

            var result = scienceDivisionServices.Edit(model, (Guid)TempData["UserId"]);
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
            var result = scienceDivisionServices.Delete(Id, (Guid)TempData["UserId"]);
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