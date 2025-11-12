using StudentAffairs.Authorization;
using StudentAffairs.Enums;
using StudentAffairs.Models;
using StudentAffairs.Services.Cities;
using StudentAffairs.Services.CityDepartments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentAffairs.Controllers
{
    [Authorized(ScreenId = "2")]

    public class CityDepartmentsController : Controller
    {
        CitiesServices citiesServices = new CitiesServices();
        CityDepartmentsServices cityDepartmentsServices = new CityDepartmentsServices();
        // GET: Cities
        public ActionResult Index()
        {
            var model = cityDepartmentsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            return View(model);
        }
        public ActionResult Create()
        {
            var cities = citiesServices.GetAll().Select(x=>new { x.Id,x.Name}).ToList();
            ViewBag.CityId = new SelectList(cities, "Id", "Name");
            return View("Upsert", new CityDepartment());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(CityDepartment model)
        {
            model.Id = Guid.NewGuid();
            //model.SchoolId = (Guid)TempData["SchoolId"];

            var result = cityDepartmentsServices.Create(model, (Guid)TempData["UserId"]);
            if (result.IsSuccess)
            {
                TempData["success"] = result.Message;
                return RedirectToAction("Index");
            }
            else
            {
                model.Id = Guid.Empty;
                var cities = citiesServices.GetAll().Select(x => new { x.Id, x.Name }).ToList();
                ViewBag.CityId = new SelectList(cities, "Id", "Name", model.CityId);
                TempData["warning"] = result.Message;
                return View("Upsert", model);
            }
        }
        public ActionResult Edit(Guid Id)
        {
            var city = cityDepartmentsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]).Where(x => x.Id == Id).FirstOrDefault();

            var cities = citiesServices.GetAll().Select(x => new { x.Id, x.Name }).ToList();
            ViewBag.CityId = new SelectList(cities, "Id", "Name", city.CityId);
            return View("Upsert", city);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(CityDepartment model)
        {

            var result = cityDepartmentsServices.Edit(model, (Guid)TempData["UserId"]);
            if (result.IsSuccess)
            {
                TempData["success"] = result.Message;
                return RedirectToAction("Index");
            }
            else
            {
                var cities = citiesServices.GetAll().Select(x => new { x.Id, x.Name }).ToList();
                ViewBag.CityId = new SelectList(cities, "Id", "Name", model.CityId);

                TempData["warning"] = result.Message;
                return View("Upsert", model);
            }
        }
        public ActionResult Delete(Guid Id)
        {
            var result = cityDepartmentsServices.Delete(Id, (Guid)TempData["UserId"]);
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