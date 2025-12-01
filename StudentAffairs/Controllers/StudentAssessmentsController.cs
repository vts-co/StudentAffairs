using StudentAffairs.Authorization;
using StudentAffairs.Dtos.Students;
using StudentAffairs.Enums;
using StudentAffairs.Services.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentAffairs.Controllers
{
    [Authorized(ScreenId = "25")]

    public class StudentAssessmentsController : Controller
    {
        StudentsServices studentsServices = new StudentsServices();

        // GET: StudentAssessments
        public ActionResult Index(Guid? SchoolId)
        {
            if ((Role)TempData["RoleId"] == Role.Super_Admin && SchoolId == null)
            {
                return View(new List<StudentsDto>());
            }
            var model = studentsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            if (SchoolId != null)
                model = model.Where(x => x.SchoolId == SchoolId).ToList();
            return View(model);
        }
    }
}