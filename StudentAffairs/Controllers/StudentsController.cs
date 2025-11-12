using StudentAffairs.Authorization;
using StudentAffairs.Dtos.Students;
using StudentAffairs.Enums;
using StudentAffairs.Models;
using StudentAffairs.Services.Cities;
using StudentAffairs.Services.Classes;
using StudentAffairs.Services.EndYearResults;
using StudentAffairs.Services.ExemptionReasons;
using StudentAffairs.Services.ExpenseTypes;
using StudentAffairs.Services.Levels;
using StudentAffairs.Services.Nationalities;
using StudentAffairs.Services.RegistrationStatus;
using StudentAffairs.Services.Religion;
using StudentAffairs.Services.ScienceDivision;
using StudentAffairs.Services.SecondLanguage;
using StudentAffairs.Services.SecondRoundResults;
using StudentAffairs.Services.SocialStatus;
using StudentAffairs.Services.Students;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace StudentAffairs.Controllers
{
    [Authorized(ScreenId = "11")]
    public class StudentsController : Controller
    {
        LevelsServices levelsServices = new LevelsServices();
        ClassesServices classesServices = new ClassesServices();
        ReligionServices religionServices = new ReligionServices();
        NationalitiesServices nationalitiesServices = new NationalitiesServices();
        ExpenseTypesServices expenseTypesServices = new ExpenseTypesServices();
        ExemptionReasonsServices exemptionReasonsServices = new ExemptionReasonsServices();
        EndYearResultsServices endYearResultsServices = new EndYearResultsServices();
        SecondRoundResultsServices secondRoundResultsServices = new SecondRoundResultsServices();

        CitiesServices citiesServices = new CitiesServices();
        RegistrationStatusServices registrationStatusServices = new RegistrationStatusServices();

        SocialStatusServices socialStatusServices = new SocialStatusServices();

        ScienceDivisionServices scienceDivisionServices = new ScienceDivisionServices();
        SecondLanguageServices secondLanguageServices = new SecondLanguageServices();

        StudentsServices studentsServices = new StudentsServices();
        [Authorized(ScreenId = "12")]

        public ActionResult Index(string code,Guid? SchoolId)
        {
            if ((Role)TempData["RoleId"] == Role.Super_Admin && SchoolId == null)
            {
                return View(new List<StudentsDto>());
            }
            if (code==null)
            {
                var model = studentsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                if (SchoolId != null)
                    model = model.Where(x => x.SchoolId == SchoolId).ToList();
                return View(model);
            }
            else
            {
                var model = studentsServices.GetByCodeOrNameOrNumberId((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"],code,SchoolId);
               
                return View(model);
            }
        }
        [Authorized(ScreenId = "11")]

        public ActionResult Create()
        {
            var Levels = levelsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            ViewBag.LevelId = new SelectList(Levels, "Id", "Name");

            //var Classes = classesServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            ViewBag.ClassId = new SelectList("");

            var Religion = religionServices.GetAll();
            ViewBag.ReligionId = new SelectList(Religion, "Id", "Name");

            var Nationalities = nationalitiesServices.GetAll();
            ViewBag.NationalityId = new SelectList(Nationalities, "Id", "Name");

            var ExpenseTypes = expenseTypesServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            ViewBag.ExpenseTypeId = new SelectList(ExpenseTypes, "Id", "Name");

            var ExemptionReasons = exemptionReasonsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            ViewBag.ExemptionReasonId = new SelectList(ExemptionReasons, "Id", "Name");

            var EndYearResults = endYearResultsServices.GetAll();
            ViewBag.EndYearResultId = new SelectList(EndYearResults, "Id", "Name");

            var SecondRoundResults = secondRoundResultsServices.GetAll();
            ViewBag.SecondRoundResultId = new SelectList(SecondRoundResults, "Id", "Name");

            ViewBag.EducationalIntegration = new SelectList(EducationalIntegration(), "Value", "Text");
            ViewBag.TransferredToTheSchool = new SelectList(TransferredToTheSchool(), "Value", "Text");
            ViewBag.BirthDate = DateTime.Now.ToString("yyyy-MM-dd");

            var Cities = citiesServices.GetAll();
            ViewBag.CityId = new SelectList(Cities, "Id", "Name");

            ViewBag.GenderId = new SelectList(Genders(), "Value", "Text");

            var RegistrationStatus = registrationStatusServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            ViewBag.RegistrationStateId = new SelectList(RegistrationStatus, "Id", "Name");

            ViewBag.RegistrationDate = DateTime.Now.ToString("yyyy-MM-dd");

            var SocialStatus = socialStatusServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            ViewBag.SocialStateId = new SelectList(SocialStatus, "Id", "Name");

            var ScienceDivision = scienceDivisionServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            ViewBag.ScienceDivisionId = new SelectList(ScienceDivision, "Id", "Name");

            var SecondLanguage = secondLanguageServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            ViewBag.SecondLanguageId = new SelectList(SecondLanguage, "Id", "Name");

            ViewBag.InsurancePolicyDate = DateTime.Now.ToString("yyyy-MM-dd");

            ViewBag.ProtectionCommittee = new SelectList(ProtectionCommittee(), "Value", "Text");

            ViewBag.FromDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.ToDate = DateTime.Now.ToString("yyyy-MM-dd");


            var studentsCount = studentsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]).Count();
            studentsCount += 100;
            return View("Upsert", new Student() { Code = studentsCount.ToString() });
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Student student, HttpPostedFileBase Image1)
        {

            student.Id = Guid.NewGuid();
            if (Image1 != null)
            {
                student.Image = "/Uploads/Studentes/";

                if (!Directory.Exists(Server.MapPath("~" + student.Image + student.Id)))
                    Directory.CreateDirectory(Server.MapPath("~" + student.Image + student.Id));
                student.Image = student.Image + student.Id + "/" + student.Id + ".jpg";
                Image1.SaveAs(Server.MapPath("~" + student.Image));
            }
            var result = studentsServices.Create(student, (Guid)TempData["UserId"]);
            if (result.IsSuccess)
            {
                TempData["success"] = result.Message;
                return RedirectToAction("Index");
            }
            else
            {
                student.Id = Guid.Empty;

                var Levels = levelsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                ViewBag.LevelId = new SelectList(Levels, "Id", "Name", student.LevelId);

                var Classes = classesServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]).Where(x=>x.LevelId== student.LevelId).ToList();
                ViewBag.ClassId = new SelectList(Classes, "Id", "Name", student.ClassId);

                var Religion = religionServices.GetAll();
                ViewBag.ReligionId = new SelectList(Religion, "Id", "Name", student.ReligionId);

                var Nationalities = nationalitiesServices.GetAll();
                ViewBag.NationalityId = new SelectList(Nationalities, "Id", "Name", student.NationalityId);

                var ExpenseTypes = expenseTypesServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                ViewBag.ExpenseTypeId = new SelectList(ExpenseTypes, "Id", "Name", student.ExpenseTypeId);

                var ExemptionReasons = exemptionReasonsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                ViewBag.ExemptionReasonId = new SelectList(ExemptionReasons, "Id", "Name", student.ExemptionReasonId);

                var EndYearResults = endYearResultsServices.GetAll();
                ViewBag.EndYearResultId = new SelectList(EndYearResults, "Id", "Name", student.EndYearResultId);

                var SecondRoundResults = secondRoundResultsServices.GetAll();
                ViewBag.SecondRoundResultId = new SelectList(SecondRoundResults, "Id", "Name", student.SecondRoundResultId);

                ViewBag.TransferredToTheSchool = new SelectList(TransferredToTheSchool(), "Value", "Text", student.TransferredToTheSchool);
                ViewBag.EducationalIntegration = new SelectList(EducationalIntegration(), "Value", "Text", student.EducationalIntegration);
                if (student.BirthDate != null)
                    ViewBag.BirthDate = student.BirthDate;
                else
                    ViewBag.BirthDate = DateTime.Now.ToString("yyyy-MM-dd");

                var Cities = citiesServices.GetAll();
                ViewBag.CityId = new SelectList(Cities, "Id", "Name", student.CityId);

                ViewBag.GenderId = new SelectList(Genders(), "Value", "Text", student.GenderId);

                var RegistrationStatus = registrationStatusServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                ViewBag.RegistrationStateId = new SelectList(RegistrationStatus, "Id", "Name", student.RegistrationStateId);

                if (student.RegistrationDate != null)
                    ViewBag.RegistrationDate = student.RegistrationDate.Value.ToString("yyyy-MM-dd");
                else
                    ViewBag.RegistrationDate = DateTime.Now.ToString("yyyy-MM-dd");


                var SocialStatus = socialStatusServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                ViewBag.SocialStateId = new SelectList(SocialStatus, "Id", "Name", student.SocialStateId);

                var ScienceDivision = scienceDivisionServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                ViewBag.ScienceDivisionId = new SelectList(ScienceDivision, "Id", "Name", student.ScienceDivisionId);

                var SecondLanguage = secondLanguageServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                ViewBag.SecondLanguageId = new SelectList(SecondLanguage, "Id", "Name", student.SecondLanguageId);

                if (student.InsurancePolicyDate != null)
                    ViewBag.InsurancePolicyDate = student.InsurancePolicyDate.Value.ToString("yyyy-MM-dd");
                else
                    ViewBag.InsurancePolicyDate = DateTime.Now.ToString("yyyy-MM-dd");


                ViewBag.ProtectionCommittee = new SelectList(ProtectionCommittee(), "Value", "Text", student.ProtectionCommittee);

                if (student.FromDate != null)
                    ViewBag.FromDate = student.FromDate.Value.ToString("yyyy-MM-dd");
                else
                    ViewBag.FromDate = DateTime.Now.ToString("yyyy-MM-dd");

                if (student.ToDate != null)
                    ViewBag.ToDate = student.ToDate.Value.ToString("yyyy-MM-dd");
                else
                    ViewBag.ToDate = DateTime.Now.ToString("yyyy-MM-dd");

                
                TempData["warning"] = result.Message;
                return View("Upsert", student);
            }
        }
        [Authorized(ScreenId = "12")]

        public ActionResult Edit(Guid Id)
        {
            var student = studentsServices.Get(Id);

            var Levels = levelsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            ViewBag.LevelId = new SelectList(Levels, "Id", "Name", student.LevelId);

            var Classes = classesServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]).Where(x => x.LevelId == student.LevelId).ToList();
            ViewBag.ClassId = new SelectList(Classes, "Id", "Name", student.ClassId);

            var Religion = religionServices.GetAll();
            ViewBag.ReligionId = new SelectList(Religion, "Id", "Name", student.ReligionId);

            var Nationalities = nationalitiesServices.GetAll();
            ViewBag.NationalityId = new SelectList(Nationalities, "Id", "Name", student.NationalityId);

            var ExpenseTypes = expenseTypesServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            ViewBag.ExpenseTypeId = new SelectList(ExpenseTypes, "Id", "Name", student.ExpenseTypeId);

            var ExemptionReasons = exemptionReasonsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            ViewBag.ExemptionReasonId = new SelectList(ExemptionReasons, "Id", "Name", student.ExemptionReasonId);

            var EndYearResults = endYearResultsServices.GetAll();
            ViewBag.EndYearResultId = new SelectList(EndYearResults, "Id", "Name", student.EndYearResultId);

            var SecondRoundResults = secondRoundResultsServices.GetAll();
            ViewBag.SecondRoundResultId = new SelectList(SecondRoundResults, "Id", "Name", student.SecondRoundResultId);

            ViewBag.TransferredToTheSchool = new SelectList(TransferredToTheSchool(), "Value", "Text", student.TransferredToTheSchool);
            ViewBag.EducationalIntegration = new SelectList(EducationalIntegration(), "Value", "Text", student.EducationalIntegration);
            if (student.BirthDate != null)
                ViewBag.BirthDate = student.BirthDate;
            else
                ViewBag.BirthDate = DateTime.Now.ToString("yyyy-MM-dd");

            var Cities = citiesServices.GetAll();
            ViewBag.CityId = new SelectList(Cities, "Id", "Name", student.CityId);

            ViewBag.GenderId = new SelectList(Genders(), "Value", "Text", student.GenderId);

            var RegistrationStatus = registrationStatusServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            ViewBag.RegistrationStateId = new SelectList(RegistrationStatus, "Id", "Name", student.RegistrationStateId);

            if (student.RegistrationDate != null)
                ViewBag.RegistrationDate = student.RegistrationDate.Value.ToString("yyyy-MM-dd");
            else
                ViewBag.RegistrationDate = DateTime.Now.ToString("yyyy-MM-dd");


            var SocialStatus = socialStatusServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            ViewBag.SocialStateId = new SelectList(SocialStatus, "Id", "Name", student.SocialStateId);

            var ScienceDivision = scienceDivisionServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            ViewBag.ScienceDivisionId = new SelectList(ScienceDivision, "Id", "Name", student.ScienceDivisionId);

            var SecondLanguage = secondLanguageServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            ViewBag.SecondLanguageId = new SelectList(SecondLanguage, "Id", "Name", student.SecondLanguageId);

            if (student.InsurancePolicyDate != null)
                ViewBag.InsurancePolicyDate = student.InsurancePolicyDate.Value.ToString("yyyy-MM-dd");
            else
                ViewBag.InsurancePolicyDate = DateTime.Now.ToString("yyyy-MM-dd");


            ViewBag.ProtectionCommittee = new SelectList(ProtectionCommittee(), "Value", "Text", student.ProtectionCommittee);

            if (student.FromDate != null)
                ViewBag.FromDate = student.FromDate.Value.ToString("yyyy-MM-dd");
            else
                ViewBag.FromDate = DateTime.Now.ToString("yyyy-MM-dd");

            if (student.ToDate != null)
                ViewBag.ToDate = student.ToDate.Value.ToString("yyyy-MM-dd");
            else
                ViewBag.ToDate = DateTime.Now.ToString("yyyy-MM-dd");

            return View("Upsert", student);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Student student, HttpPostedFileBase Image1)
        {
            if (Image1 != null)
            {
                student.Image = "/Uploads/Studentes/";

                if (!Directory.Exists(Server.MapPath("~" + student.Image + student.Id)))
                    Directory.CreateDirectory(Server.MapPath("~" + student.Image + student.Id));
                student.Image = student.Image + student.Id + "/" + student.Id + ".jpg";
                Image1.SaveAs(Server.MapPath("~" + student.Image));
            }

            var result = studentsServices.Edit(student, (Guid)TempData["UserId"]);
            if (result.IsSuccess)
            {
                TempData["success"] = result.Message;
                return RedirectToAction("Index");
            }
            else
            {
                var Levels = levelsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                ViewBag.LevelId = new SelectList(Levels, "Id", "Name", student.LevelId);

                var Classes = classesServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]).Where(x => x.LevelId == student.LevelId).ToList();
                ViewBag.ClassId = new SelectList(Classes, "Id", "Name", student.ClassId);

                var Religion = religionServices.GetAll();
                ViewBag.ReligionId = new SelectList(Religion, "Id", "Name", student.ReligionId);

                var Nationalities = nationalitiesServices.GetAll();
                ViewBag.NationalityId = new SelectList(Nationalities, "Id", "Name", student.NationalityId);

                var ExpenseTypes = expenseTypesServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                ViewBag.ExpenseTypeId = new SelectList(ExpenseTypes, "Id", "Name", student.ExpenseTypeId);

                var ExemptionReasons = exemptionReasonsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                ViewBag.ExemptionReasonId = new SelectList(ExemptionReasons, "Id", "Name", student.ExemptionReasonId);

                var EndYearResults = endYearResultsServices.GetAll();
                ViewBag.EndYearResultId = new SelectList(EndYearResults, "Id", "Name", student.EndYearResultId);

                var SecondRoundResults = secondRoundResultsServices.GetAll();
                ViewBag.SecondRoundResultId = new SelectList(SecondRoundResults, "Id", "Name", student.SecondRoundResultId);

                ViewBag.TransferredToTheSchool = new SelectList(TransferredToTheSchool(), "Value", "Text", student.TransferredToTheSchool);
                ViewBag.EducationalIntegration = new SelectList(EducationalIntegration(), "Value", "Text", student.EducationalIntegration);
                if (student.BirthDate != null)
                    ViewBag.BirthDate = student.BirthDate;
                else
                    ViewBag.BirthDate = DateTime.Now.ToString("yyyy-MM-dd");

                var Cities = citiesServices.GetAll();
                ViewBag.CityId = new SelectList(Cities, "Id", "Name", student.CityId);

                ViewBag.GenderId = new SelectList(Genders(), "Value", "Text", student.GenderId);

                var RegistrationStatus = registrationStatusServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                ViewBag.RegistrationStateId = new SelectList(RegistrationStatus, "Id", "Name", student.RegistrationStateId);

                if (student.RegistrationDate != null)
                    ViewBag.RegistrationDate = student.RegistrationDate.Value.ToString("yyyy-MM-dd");
                else
                    ViewBag.RegistrationDate = DateTime.Now.ToString("yyyy-MM-dd");


                var SocialStatus = socialStatusServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                ViewBag.SocialStateId = new SelectList(SocialStatus, "Id", "Name", student.SocialStateId);

                var ScienceDivision = scienceDivisionServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                ViewBag.ScienceDivisionId = new SelectList(ScienceDivision, "Id", "Name", student.ScienceDivisionId);

                var SecondLanguage = secondLanguageServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                ViewBag.SecondLanguageId = new SelectList(SecondLanguage, "Id", "Name", student.SecondLanguageId);

                if (student.InsurancePolicyDate != null)
                    ViewBag.InsurancePolicyDate = student.InsurancePolicyDate.Value.ToString("yyyy-MM-dd");
                else
                    ViewBag.InsurancePolicyDate = DateTime.Now.ToString("yyyy-MM-dd");


                ViewBag.ProtectionCommittee = new SelectList(ProtectionCommittee(), "Value", "Text", student.ProtectionCommittee);

                if (student.FromDate != null)
                    ViewBag.FromDate = student.FromDate.Value.ToString("yyyy-MM-dd");
                else
                    ViewBag.FromDate = DateTime.Now.ToString("yyyy-MM-dd");

                if (student.ToDate != null)
                    ViewBag.ToDate = student.ToDate.Value.ToString("yyyy-MM-dd");
                else
                    ViewBag.ToDate = DateTime.Now.ToString("yyyy-MM-dd");

                TempData["warning"] = result.Message;
                return View("Upsert", student);
            }
        }
        public ActionResult Delete(Guid Id)
        {
            var result = studentsServices.Delete(Id, (Guid)TempData["UserId"]);
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

        public ActionResult getClasses(Guid? Id)
        {
            var result = classesServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]).Where(x=>x.LevelId==Id).Select(x => new { x.Id, x.Name }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [Authorized(ScreenId = "13")]

        public ActionResult Graduates(Guid? SchoolId)
        {
            if ((Role)TempData["RoleId"] == Role.Super_Admin && SchoolId == null)
            {
                return View(new List<StudentsDto>());
            }
            var model = studentsServices.GetAllGraduates((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            if (SchoolId != null)
                model = model.Where(x => x.SchoolId == SchoolId).ToList();
            return View(model);
        }
        public ActionResult Graduated(Guid Id)
        {
            var result = studentsServices.Graduated(Id, (Guid)TempData["UserId"]);
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

        [Authorized(ScreenId = "14")]

        public ActionResult TransferFromSchool(Guid? SchoolId)
        {
            if ((Role)TempData["RoleId"] == Role.Super_Admin && SchoolId == null)
            {
                return View(new List<StudentsDto>());
            }
            var model = studentsServices.GetAllTransferFromSchool((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            if (SchoolId != null)
                model = model.Where(x => x.SchoolId == SchoolId).ToList();
            return View(model);
        }
        public ActionResult Transfered(Guid Id)
        {
            var result = studentsServices.Transfered(Id, (Guid)TempData["UserId"]);
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

        [Authorized(ScreenId = "18")]

        public ActionResult TrackConverters(Guid? SchoolId)
        {
            if ((Role)TempData["RoleId"] == Role.Super_Admin && SchoolId == null)
            {
                return View(new List<StudentsDto>());
            }
            var model = studentsServices.GetAllTrackConverters((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            if (SchoolId != null)
                model = model.Where(x => x.SchoolId == SchoolId).ToList();
            return View(model);
        }
        public ActionResult TrackConverterd(Guid Id)
        {
            var result = studentsServices.TrackConverterd(Id, (Guid)TempData["UserId"]);
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

        [Authorized(ScreenId = "0")]
        public ActionResult getCities(Guid? Id)
        {
            var result = citiesServices.GetAll().Select(x=>new { x.Id,x.Name,x.Code}).ToList();
           
                return Json(result,JsonRequestBehavior.AllowGet);
            
        }
        List<ListItem> Genders()
        {
            Array values = Enum.GetValues(typeof(Gender));
            List<ListItem> items = new List<ListItem>(values.Length);
            var count = 1;
            foreach (var i in values)
            {
                items.Add(new ListItem
                {
                    Text = Enum.GetName(typeof(Gender), i),
                    Value = count.ToString()
                });
                count++;
            }
            return items;
        }
        List<ListItem> TransferredToTheSchool()
        {
            Array values = Enum.GetValues(typeof(TransferredToTheSchool));
            List<ListItem> items = new List<ListItem>(values.Length);
            var count = 1;
            foreach (var i in values)
            {
                items.Add(new ListItem
                {
                    Text = Enum.GetName(typeof(TransferredToTheSchool), i),
                    Value = count.ToString()
                });
                count++;
            }
            return items;
        }
        List<ListItem> EducationalIntegration()
        {
            Array values = Enum.GetValues(typeof(EducationalIntegration));
            List<ListItem> items = new List<ListItem>(values.Length);
            var count = 1;
            foreach (var i in values)
            {
                items.Add(new ListItem
                {
                    Text = Enum.GetName(typeof(EducationalIntegration), i),
                    Value = count.ToString()
                });
                count++;
            }
            return items;
        }
        List<ListItem> ProtectionCommittee()
        {
            Array values = Enum.GetValues(typeof(ProtectionCommittee));
            List<ListItem> items = new List<ListItem>(values.Length);
            var count = 1;
            foreach (var i in values)
            {
                items.Add(new ListItem
                {
                    Text = Enum.GetName(typeof(ProtectionCommittee), i),
                    Value = count.ToString()
                });
                count++;
            }
            return items;
        }
    }
}