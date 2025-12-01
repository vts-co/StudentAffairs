using StudentAffairs.Authorization;
using StudentAffairs.Enums;
using StudentAffairs.Services.Levels;
using StudentAffairs.Models;
using StudentAffairs.Services.Classes;
using StudentAffairs.Services.Students;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentAffairs.Services.StudentsAttendance;
using StudentAffairs.Services;

namespace StudentAffairs.Controllers
{
    [Authorized(ScreenId = "21")]

    public class StudentsAttendanceController : Controller
    {

        LevelsServices levelsServices = new LevelsServices();
        ClassesServices classesServices = new ClassesServices();
        StudentsServices studentsServices = new StudentsServices();

        StudentsAttendanceServices studentsAttendanceServices = new StudentsAttendanceServices();
        // GET: Destricts
        public ActionResult Index(Guid? SchoolId, Guid? LevelId, Guid? ClassId, bool? AttendOrNo, DateTime? dateFrom, DateTime? dateTo, string Search)
        {
            var model = studentsAttendanceServices.GetAllByFilters((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"], SchoolId, dateFrom, dateTo);
            if (LevelId != null)
                model = model.Where(x => x.LevelId == LevelId).ToList();
            if (ClassId != null)
                model = model.Where(x => x.ClassId == ClassId).ToList();
            if (AttendOrNo != null)
                model = model.Where(x => x.IsAttend == AttendOrNo).ToList();
            if (Search != null && Search != "")
                model = model.Where(x => x.StudentName.Contains(Search)).ToList();


            if (SchoolId != null&& SchoolId!=Guid.Empty)
            {
                var levels = levelsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]).Where(x => x.SchoolId == SchoolId).ToList();
                ViewBag.LevelId = new SelectList(levels, "Id", "Name");
            }
            else if ((Guid)TempData["SchoolId"] != null&& (Guid)TempData["SchoolId"] != Guid.Empty)
            {
                var levels = levelsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                ViewBag.LevelId = new SelectList(levels, "Id", "Name");
            }
            else
                ViewBag.LevelId = new SelectList("");

            if (dateFrom != null)
                ViewBag.DateFrom = dateFrom.Value.ToString("yyyy-MM-dd");
            if (dateTo != null)
                ViewBag.DateTo = dateTo.Value.ToString("yyyy-MM-dd");
            ViewBag.ClassId = new SelectList("");
            return View(model);
        }

        public ActionResult Create()
        {
            return View("Upsert", new StudentsAttendance() { });
        }
        public ActionResult Create2(Guid? SchoolId, DateTime? AttendDate, string AttendOrNo)
        {
            ViewBag.SchoolId = SchoolId;
            ViewBag.AttendOrNo = AttendOrNo;
            ViewBag.Date = AttendDate.Value.ToString("yyyy-MM-dd");
            return View("Upsert", new StudentsAttendance() { });
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(StudentsAttendance Class, List<Attend> IsAttend)
        {
            var result = studentsAttendanceServices.Create(Class, (Guid)TempData["UserId"]);
            if (result.IsSuccess)
            {
                TempData["success"] = result.Message;
                return RedirectToAction("Index");
            }
            else
            {


                TempData["warning"] = result.Message;
                return View("Upsert", Class);
            }
        }
        public ActionResult Edit(Guid Id)
        {
            var result = studentsAttendanceServices.Edit(Id, (Guid)TempData["UserId"]);
            if (result.IsSuccess)
            {
                TempData["success"] = result.Message;
                var student = studentsServices.Get(result.Result.StudentId.Value);
                return RedirectToAction("Index",new {SchoolId= student.SchoolId, dateFrom = result.Result.AttendDate, dateTo = result.Result.AttendDate });
            }
            else
            {
                TempData["warning"] = result.Message;
                return RedirectToAction("Index");
            }
        }
        public ActionResult Delete(Guid Id)
        {
            var result = studentsAttendanceServices.Delete(Id, (Guid)TempData["UserId"]);
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
        public ActionResult DeleteAll(Guid SchoolId,DateTime DateFrom, DateTime DateTo,Guid? LevelId,Guid? ClassId)
        {
            var result = studentsAttendanceServices.DeleteAll(SchoolId, DateFrom, DateTo, LevelId, ClassId, (Guid)TempData["UserId"]);
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
        public ActionResult getLevels(Guid Id)
        {
            var model = levelsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]).Where(x => x.SchoolId == Id).Select(x => new { x.Id, Name = x.Name }).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getClasses(Guid Id)
        {
            var model = classesServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]).Where(x => x.LevelId == Id).Select(x => new { x.Id, Name = x.Name }).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getStudentAttendans(string Date, Guid ClassId, Guid StudyYearId, Guid StudyClassId, Guid StudentId)
        {
            var date1 = DateTime.Parse(Date);
            var model = studentsAttendanceServices.Get(date1, ClassId, StudyYearId, StudyClassId, StudentId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getStudent(Guid SchoolId, string SerialNumber, string AttendDate, string AttendOrNo)
        {
            var model = studentsServices.GetBySerialNumber(SchoolId, SerialNumber);
            var date = DateTime.Parse(AttendDate);
            var model1 = studentsAttendanceServices.GetBySchoolAndDate(SchoolId, date);
            if (model1)
            {
                model = new Dtos.Students.StudentsDto() { Name = "جلسة قديمة" };
                return Json(new { data = model, message = "تم اخذ غياب هذا اليوم لا يمكن فتح جلسة جديدة" }, JsonRequestBehavior.AllowGet);
            }
            var mode = new ResultDto<Models.StudentsAttendance>() { Message = "" };
            if (model != null)
            {
                var mod = new StudentsAttendance();
                mod.StudentId = model.Id;
                mod.LevelId = model.LevelId;
                if (model.ClassId == Guid.Empty)
                    mod.ClassId = null;
                else
                    mod.ClassId = model.ClassId;
                mod.AttendDate = DateTime.Parse(AttendDate);
                if (AttendOrNo == "true")
                    mod.AttendOrNo = true;
                else
                    mod.AttendOrNo = false;

                mode = studentsAttendanceServices.Create(mod, (Guid)TempData["UserId"]);
            }

            if (model == null)
                model = new Dtos.Students.StudentsDto() { Name = "" };
            else
            {
                if (model.Image == null || model.Image == "")
                    model.Image = "";
            }

            return Json(new { data = model, message = mode.Message }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CheckGalsa(Guid SchoolId, string AttendDate)
        {
            var date = DateTime.Parse(AttendDate);
            var model1 = studentsAttendanceServices.GetBySchoolAndDate(SchoolId, date);
            if (model1)
            {
                var model = new Dtos.Students.StudentsDto() { Name = "جلسة قديمة" };
                return Json(new { data = model, message = "تم اخذ غياب هذا اليوم لا يمكن فتح جلسة جديدة" }, JsonRequestBehavior.AllowGet);
            }
            var model2 = new Dtos.Students.StudentsDto() { Name = "" };

            return Json(new { data = model2, message = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckGalsa2(Guid SchoolId, string AttendDate)
        {
            var date = DateTime.Parse(AttendDate);
            var model1 = studentsAttendanceServices.GetBySchoolAndDate(SchoolId, date);
            if (!model1)
            {
                var model = new Dtos.Students.StudentsDto() { Name = "جلسة قديمة" };
                return Json(new { data = model, message = "لا يوجد جلسة قديمة لهذا اليوم" }, JsonRequestBehavior.AllowGet);
            }
            var model2 = new Dtos.Students.StudentsDto() { Name = "" };

            return Json(new { data = model2, message = "" }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult StudentAllAttend(Guid SchoolId, string AttendDate, string AttendOrNo)
        {
            //var date = DateTime.Parse(AttendDate);
            //var model1 = studentsAttendanceServices.GetBySchoolAndDate(SchoolId, date);
            //if (model1)
            //{
            //    var model = new Dtos.Students.StudentsDto() { Name = "جلسة قديمة" };
            //    return Json(new { data = model,flag="danger", message = "تم اخذ غياب هذا اليوم لا يمكن فتح جلسة جديدة" }, JsonRequestBehavior.AllowGet);
            //}
            var mode = studentsAttendanceServices.CreateAll(SchoolId, AttendDate, AttendOrNo, (Guid)TempData["UserId"]);
            return Json(new { message = mode.Message,flag= "success" }, JsonRequestBehavior.AllowGet);
        }


    }
    public class Attend
    {
        public Guid Id { get; set; }
        public string Att { get; set; }

    }
}
