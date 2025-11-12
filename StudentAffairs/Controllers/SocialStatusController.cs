using StudentAffairs.Authorization;
using StudentAffairs.Enums;
using StudentAffairs.Models;
using StudentAffairs.Services.SocialStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentAffairs.Controllers
{
    [Authorized(ScreenId = "7")]
    public class SocialStatusController : Controller
    {
        SocialStatusServices socialStatusServices = new SocialStatusServices();
        // GET: Cities
        public ActionResult Index(Guid? SchoolId)
        {
            if ((Role)TempData["RoleId"] == Role.Super_Admin && SchoolId == null)
            {
                return View(new List<ExemptionReason>());
            }
            var model = socialStatusServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            if (SchoolId != null)
                model = model.Where(x => x.SchoolId == SchoolId).ToList();
            return View(model);
        }
        public ActionResult Create()
        {
            return View("Upsert", new SocialStatu());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(SocialStatu model)
        {
            model.Id = Guid.NewGuid();
            //model.SchoolId = (Guid)TempData["SchoolId"];

            var result = socialStatusServices.Create(model, (Guid)TempData["UserId"]);
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
            var city = socialStatusServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]).Where(x => x.Id == Id).FirstOrDefault();
            return View("Upsert", city);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(SocialStatu model)
        {

            var result = socialStatusServices.Edit(model, (Guid)TempData["UserId"]);
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
            var result = socialStatusServices.Delete(Id, (Guid)TempData["UserId"]);
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