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
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Data;
using ExcelDataReader;
using System.Drawing;
using System.Drawing.Imaging;
using QRCoder;

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
        [Authorized(ScreenId = "14")]

        public ActionResult Index(string code, Guid? SchoolId, Guid? Id)
        {
            if (Id != null && Id != Guid.Empty)
            {
                var student = studentsServices.GetByDto(Id.Value);
                TempData["StudentId"] = student.Id;
                TempData["StudentImage"] = student.Image;
                TempData["StudentName"] = "الاسم : " + student.Name;
                TempData["LevelName"] = "الصف : " + student.LevelName;
                TempData["ClassName"] = "الفصل : " + student.ClassName;
                TempData["QrCodeUri"] = QrPage(Id.Value);
            }

            if ((Role)TempData["RoleId"] == Role.Super_Admin && SchoolId == null)
            {
                return View(new List<StudentsDto>());
            }
            if (code == null)
            {
                var model = studentsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                if (SchoolId != null)
                    model = model.Where(x => x.SchoolId == SchoolId).ToList();
                return View(model);
            }
            else
            {
                var model = studentsServices.GetByCodeOrNameOrNumberId((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"], code, SchoolId);

                return View(model);
            }
        }
        [Authorized(ScreenId = "13")]

        public ActionResult Create()
        {
            if ((Role)TempData["RoleId"] == Role.Super_Admin)
            {
                ViewBag.LevelId = new SelectList("");
                ViewBag.ExemptionReasonId = new SelectList("");
                ViewBag.SocialStateId = new SelectList("");
                ViewBag.ScienceDivisionId = new SelectList("");
                ViewBag.SecondLanguageId = new SelectList("");
            }
            else
            {
                var Levels = levelsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                ViewBag.LevelId = new SelectList(Levels, "Id", "Name");

                var ExemptionReasons = exemptionReasonsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                ViewBag.ExemptionReasonId = new SelectList(ExemptionReasons, "Id", "Name");

                var SocialStatus = socialStatusServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                ViewBag.SocialStateId = new SelectList(SocialStatus, "Id", "Name");

                var ScienceDivision = scienceDivisionServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                ViewBag.ScienceDivisionId = new SelectList(ScienceDivision, "Id", "Name");

                var SecondLanguage = secondLanguageServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                ViewBag.SecondLanguageId = new SelectList(SecondLanguage, "Id", "Name");

            }

            //var Classes = classesServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            ViewBag.ClassId = new SelectList("");

            var Religion = religionServices.GetAll();
            ViewBag.ReligionId = new SelectList(Religion, "Id", "Name");

            var Nationalities = nationalitiesServices.GetAll();
            ViewBag.NationalityId = new SelectList(Nationalities, "Id", "Name");

            var ExpenseTypes = expenseTypesServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            ViewBag.ExpenseTypeId = new SelectList(ExpenseTypes, "Id", "Name");


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


            ViewBag.InsurancePolicyDate = DateTime.Now.ToString("yyyy-MM-dd");

            ViewBag.ProtectionCommittee = new SelectList(ProtectionCommittee(), "Value", "Text");

            ViewBag.FromDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.ToDate = DateTime.Now.ToString("yyyy-MM-dd");



            return View("Upsert", new Student());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Student student, HttpPostedFileBase Image1)
        {
            student.Id = Guid.NewGuid();

            var studentsCount = studentsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]).Where(x => x.SchoolId == student.SchoolId).Count();
            studentsCount += 100;
            student.SerialNumber = studentsCount.ToString();

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
        public FileResult DownloadExcel()
        {
            string path = "/Uploads/Excel/StudentsFormat.xlsx";
            return File(path, "application/vnd.ms-excel", "StudentsFormat.xlsx");
        }

        #region Import MCQ Question Excel

        public ActionResult ImportExcel(HttpPostedFileBase upload, Guid SchoolId)
        {
            if (ModelState.IsValid)
            {

                if (upload != null && upload.ContentLength > 0)
                {
                    // ExcelDataReader works with the binary Excel file, so it needs a FileStream
                    // to get started. This is how we avoid dependencies on ACE or Interop:
                    Stream stream = upload.InputStream;

                    IExcelDataReader reader = null;


                    if (upload.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (upload.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        TempData["warning"] = "البيانات المدخلة غير صحيحة";
                        return RedirectToAction("Index");
                    }
                    int fieldcount = reader.FieldCount;
                    int rowcount = reader.RowCount;
                    DataTable model = new DataTable();
                    DataRow row;
                    DataTable dt_ = new DataTable();
                    try
                    {
                        dt_ = reader.AsDataSet().Tables[0];
                        for (int i = 0; i < dt_.Columns.Count; i++)
                        {
                            var ss = dt_.Rows[0][i].ToString();
                            model.Columns.Add(dt_.Rows[0][i].ToString());
                        }
                        int rowcounter = 0;
                        for (int row_ = 1; row_ < rowcount; row_++)
                        {
                            row = model.NewRow();
                            for (int col = 0; col < fieldcount; col++)
                            {
                                row[col] = dt_.Rows[row_][col].ToString();
                                var sss = dt_.Rows[row_][col].ToString();

                                rowcounter++;
                            }
                            model.Rows.Add(row);

                        }

                    }
                    catch (Exception ex)
                    {
                        TempData["warning"] = "البيانات المدخلة غير صحيحة";
                        return RedirectToAction("Index");
                    }

                    reader.Close();
                    reader.Dispose();
                    var j = studentsServices.GetAllBySchool(SchoolId).Count() + 100;
                    for (int i = 0; i < model.Rows.Count; i++)
                    {
                        Student student = new Student();
                        student.Id = Guid.NewGuid();
                        student.SchoolId = SchoolId;

                        if (model.Rows[i][0].ToString() == "" || model.Rows[i][0].ToString() == null)
                            student.SerialNumber = j.ToString();
                        else
                            student.SerialNumber = model.Rows[i][0].ToString();

                        if (model.Rows[i][1].ToString() == "" || model.Rows[i][1].ToString() == null)
                            student.Code = j.ToString();
                        else
                            student.Code = model.Rows[i][1].ToString();

                        student.Name = model.Rows[i][2].ToString();

                        if (model.Rows[i][3].ToString() == Enums.TransferredToTheSchool.محول.ToString())
                            student.TransferredToTheSchool = (int)Enums.TransferredToTheSchool.محول;
                        else if (model.Rows[i][3].ToString() == Enums.TransferredToTheSchool.لا.ToString())
                            student.TransferredToTheSchool = (int)Enums.TransferredToTheSchool.لا;

                        student.NumberId = model.Rows[i][4].ToString();

                        var num = student.NumberId.Select(digit => int.Parse(digit.ToString())).ToList();

                        if (num.Count == 14 && num[0].ToString() == "2")
                        {
                            var year = "19" + num[1].ToString() + num[2].ToString();
                            var month = num[3].ToString() + num[4].ToString();
                            var day = num[5].ToString() + num[6].ToString();

                            var yearnow = DateTime.Now.Year;
                            var date1 = DateTime.Parse(year + "-" + month + "-" + day);
                            var date2 = DateTime.Parse(yearnow + "-" + "10" + "-" + "01");

                            if (date1 > date2)
                            {
                                var temp = date1;
                                date1 = date2;
                                date2 = temp;
                            }

                            var years = date2.Year - date1.Year;
                            var months = date2.Month - date1.Month;
                            var days = date2.Day - date1.Day;

                            // تعديل الأيام
                            if (days < 0)
                            {
                                months--;
                                var prevMonth = DateTime.DaysInMonth(date1.Year, date1.Month) - 1;
                                days += prevMonth;
                            }

                            // تعديل الشهور
                            if (months < 0)
                            {
                                years--;
                                months += 12;
                            }
                            if (model.Rows[i][13].ToString() == null || model.Rows[i][13].ToString() == "")
                                model.Rows[i][13] = days.ToString();
                            if (model.Rows[i][14].ToString() == null || model.Rows[i][14].ToString() == "")
                                model.Rows[i][14] = months.ToString();
                            if (model.Rows[i][15].ToString() == null || model.Rows[i][15].ToString() == "")
                                model.Rows[i][15] = years.ToString();

                            var gender = int.Parse(num[12].ToString());
                            if (gender % 2 == 0)
                            {
                                model.Rows[i][16] = "ذكر";
                            }
                            else
                            {
                                model.Rows[i][16] = "انثي";
                            }

                            var cityCode = num[7].ToString() + num[8].ToString();
                            var city = citiesServices.GetByCode(cityCode);
                            if (city != null)
                                model.Rows[i][36] = city.Id.ToString();


                        }
                        else if (num.Count == 14 && num[0].ToString() == "3")
                        {
                            var year = "20" + num[1].ToString() + num[2].ToString();
                            var month = num[3].ToString() + num[4].ToString();
                            var day = num[5].ToString() + num[6].ToString();

                            var yearnow = DateTime.Now.Year;
                            var date1 = DateTime.Parse(year + "-" + month + "-" + day);
                            var date2 = DateTime.Parse(yearnow + "-" + "10" + "-" + "01");

                            if (date1 > date2)
                            {
                                var temp = date1;
                                date1 = date2;
                                date2 = temp;
                            }

                            var years = date2.Year - date1.Year;
                            var months = date2.Month - date1.Month;
                            var days = date2.Day - date1.Day;

                            // تعديل الأيام
                            if (days < 0)
                            {
                                months--;
                                var prevMonth = DateTime.DaysInMonth(date1.Year, date1.Month) - 1;
                                days += prevMonth;
                            }

                            // تعديل الشهور
                            if (months < 0)
                            {
                                years--;
                                months += 12;
                            }
                            if (model.Rows[i][13].ToString() == null || model.Rows[i][13].ToString() == "")
                                model.Rows[i][13] = days.ToString();
                            if (model.Rows[i][14].ToString() == null || model.Rows[i][14].ToString() == "")
                                model.Rows[i][14] = months.ToString();
                            if (model.Rows[i][15].ToString() == null || model.Rows[i][15].ToString() == "")
                                model.Rows[i][15] = years.ToString();

                            var gender = int.Parse(num[12].ToString());
                            if (gender % 2 == 0)
                            {
                                model.Rows[i][16] = "ذكر";
                            }
                            else
                            {
                                model.Rows[i][16] = "انثي";
                            }

                            var cityCode = num[7].ToString() + num[8].ToString();
                            var city = citiesServices.GetByCode(cityCode);
                            if (city != null)
                                model.Rows[i][36] = city.Id.ToString();
                        }

                        if (model.Rows[i][5].ToString() == "" || model.Rows[i][5].ToString() == null)
                            student.LevelId = null;
                        else
                            student.LevelId = Guid.Parse(model.Rows[i][5].ToString());

                        if (model.Rows[i][6].ToString() == "" || model.Rows[i][6].ToString() == null)
                            student.ClassId = null;
                        else
                            student.ClassId = Guid.Parse(model.Rows[i][6].ToString());

                        //if (model.Rows[i][7].ToString() == "" || model.Rows[i][7].ToString() == null)
                        //    student.ExpenseTypeId = null;
                        //else
                        //    student.ExpenseTypeId = Guid.Parse(model.Rows[i][7].ToString());

                        if (model.Rows[i][7].ToString() == "معفي")
                            student.ExpenseTypeId = Guid.Parse("2036014E-CF81-425A-B30E-854FD7F85F7C");
                        else if (model.Rows[i][7].ToString() == "سدد")
                            student.ExpenseTypeId = Guid.Parse("2036014E-CF81-425A-B30E-854FD7F85F7C");
                        else if (model.Rows[i][7].ToString() == "لم يسدد")
                            student.ExpenseTypeId = Guid.Parse("2036014E-CF81-425A-B30E-854FD7F85F7C");
                        else
                            student.ExpenseTypeId = null;

                        student.Address = model.Rows[i][8].ToString();

                        if (model.Rows[i][9].ToString() == "مستجد")
                            student.RegistrationStateId = Guid.Parse("4741296C-C863-474E-A395-26B92574F971");
                        else if (model.Rows[i][9].ToString() == "باق")
                            student.RegistrationStateId = Guid.Parse("4741296C-C863-474E-A395-26B93574F971");
                        else
                            student.RegistrationStateId = null;

                        student.RegistrationNum = model.Rows[i][10].ToString();
                        if (model.Rows[i][11].ToString() == null || model.Rows[i][11].ToString() == "")
                            student.RegistrationDate = null;
                        else
                            student.RegistrationDate = DateTime.Parse(model.Rows[i][11].ToString());

                        student.BirthDate = model.Rows[i][12].ToString();

                        if (model.Rows[i][13].ToString() == null || model.Rows[i][13].ToString() == "")
                            student.Day = null;
                        else
                            student.Day = int.Parse(model.Rows[i][13].ToString());

                        if (model.Rows[i][14].ToString() == null || model.Rows[i][14].ToString() == "")
                            student.Month = null;
                        else
                            student.Month = int.Parse(model.Rows[i][14].ToString());

                        if (model.Rows[i][15].ToString() == null || model.Rows[i][15].ToString() == "")
                            student.Year = null;
                        else
                            student.Year = int.Parse(model.Rows[i][15].ToString());

                        if (model.Rows[i][16].ToString() == "ذكر")
                            student.GenderId = (int)Gender.ذكر;
                        else if (model.Rows[i][16].ToString() == "انثي")
                            student.GenderId = (int)Gender.انثي;

                        student.SeatNumber = model.Rows[i][17].ToString();
                        student.FirstSemesterResult = model.Rows[i][18].ToString();
                        student.ParentName = model.Rows[i][19].ToString();
                        student.ParentJob = model.Rows[i][20].ToString();
                        student.ParentPhone = model.Rows[i][21].ToString();
                        student.ParentAddress = model.Rows[i][22].ToString();
                        student.HealthCondition = model.Rows[i][23].ToString();
                        student.Talents = model.Rows[i][24].ToString();
                        student.Competitions = model.Rows[i][25].ToString();
                        student.Problem = model.Rows[i][26].ToString();
                        student.ActionTaken = model.Rows[i][27].ToString();

                        if (model.Rows[i][28].ToString() == null || model.Rows[i][28].ToString() == "")
                            student.FromDate = null;
                        else
                            student.FromDate = DateTime.Parse(model.Rows[i][28].ToString());

                        if (model.Rows[i][29].ToString() == null || model.Rows[i][29].ToString() == "")
                            student.ToDate = null;
                        else
                            student.ToDate = DateTime.Parse(model.Rows[i][29].ToString());

                        student.Duration = model.Rows[i][30].ToString();
                        student.ParentWhatsApp = model.Rows[i][31].ToString();
                        student.MotherName = model.Rows[i][32].ToString();
                        student.Phone = model.Rows[i][33].ToString();

                        if (model.Rows[i][34].ToString() == "مسيحي")
                            student.ReligionId = Guid.Parse("4741296C-C863-474E-A395-26B93574F971");
                        else if (model.Rows[i][34].ToString() == "مسلم")
                            student.ReligionId = Guid.Parse("4741296C-C863-474E-A395-26B92574F971");
                        else
                            student.ReligionId = null;

                        if (model.Rows[i][35].ToString() == "مصري")
                            student.NationalityId = Guid.Parse("4741296C-C863-474E-A395-26B92574F971");
                        else if (model.Rows[i][35].ToString() == "وافد")
                            student.NationalityId = Guid.Parse("4741296C-C863-474E-A395-26B93574F971");
                        else
                            student.NationalityId = null;

                        if (model.Rows[i][36].ToString() == null || model.Rows[i][36].ToString() == "")
                            student.CityId = null;
                        else
                            student.CityId = Guid.Parse(model.Rows[i][36].ToString());

                        if (model.Rows[i][37].ToString() == "يوجد")
                            student.ProtectionCommittee = Enums.ProtectionCommittee.يوجد.ToString();
                        else if (model.Rows[i][37].ToString() == "لا يوجد")
                            student.ProtectionCommittee = Enums.ProtectionCommittee.لا_يوجد.ToString();
                        else
                            student.ProtectionCommittee = model.Rows[i][37].ToString();

                        if (model.Rows[i][38].ToString() == "دمج")
                            student.EducationalIntegration = (int)Enums.EducationalIntegration.دمج;
                        else if (model.Rows[i][38].ToString() == "لا يوجد")
                            student.EducationalIntegration = (int)Enums.EducationalIntegration.لا_يوجد;

                        if (model.Rows[i][39].ToString() == "ناجح")
                            student.SecondRoundResultId = Guid.Parse("4741296C-C863-474E-A395-26B92574F971");
                        else if (model.Rows[i][39].ToString() == "راسب وله حق الاعادة")
                            student.SecondRoundResultId = Guid.Parse("4741296C-C863-474E-A395-26B93574F971");
                        else if (model.Rows[i][39].ToString() == "راسب وليس له حق الاعادة")
                            student.SecondRoundResultId = Guid.Parse("4741296C-C863-474E-A395-26C93574F971");
                        else
                            student.SecondRoundResultId = null;

                        if (model.Rows[i][40].ToString() == null || model.Rows[i][40].ToString() == "")
                            student.SocialStateId = null;
                        else
                            student.SocialStateId = Guid.Parse(model.Rows[i][40].ToString());

                        student.TabletSerialNumber = model.Rows[i][41].ToString();
                        student.IM = model.Rows[i][42].ToString();

                        if (model.Rows[i][43].ToString() == null || model.Rows[i][43].ToString() == "")
                            student.DateOfReceipt = null;
                        else
                            student.DateOfReceipt = DateTime.Parse(model.Rows[i][43].ToString());

                        if (model.Rows[i][44].ToString() == "ناجح")
                            student.EndYearResultId = Guid.Parse("4741296C-C863-474E-A395-26B92574F971");
                        else if (model.Rows[i][44].ToString() == "راسب")
                            student.EndYearResultId = Guid.Parse("4741296C-C863-474E-A395-26B93574F971");
                        else
                            student.EndYearResultId = null;

                        student.MotherPhone = model.Rows[i][45].ToString();
                        if (model.Rows[i][46].ToString() == null || model.Rows[i][46].ToString() == "")
                            student.SecondLanguageId = null;
                        else
                            student.SecondLanguageId = Guid.Parse(model.Rows[i][46].ToString());

                        if (model.Rows[i][47].ToString() == null || model.Rows[i][47].ToString() == "")
                            student.ScienceDivisionId = null;
                        else
                            student.ScienceDivisionId = Guid.Parse(model.Rows[i][47].ToString());

                        if (model.Rows[i][48].ToString() == null || model.Rows[i][48].ToString() == "")
                            student.ExemptionReasonId = null;
                        else
                            student.ExemptionReasonId = Guid.Parse(model.Rows[i][48].ToString());

                        student.InsurancePolicyNumber = model.Rows[i][49].ToString();
                        if (model.Rows[i][50].ToString() == null || model.Rows[i][50].ToString() == "")
                            student.InsurancePolicyDate = null;
                        else
                            student.InsurancePolicyDate = DateTime.Parse(model.Rows[i][50].ToString());

                        if (student.Name != null && student.Name != "" && student.LevelId != null && student.NumberId != "" && student.NumberId != null)
                            studentsServices.Create(student, (Guid)TempData["UserId"]);
                        else
                        {
                            if (student.Name == null || student.Name == "")
                            {
                                TempData["warning"] = "ادخل اسماء الطلاب الفارغة";
                                return RedirectToAction("Index");
                            }
                            else if (student.LevelId != null)
                            {
                                TempData["warning"] = "ادخل الصفوف الفارغة";
                                return RedirectToAction("Index");
                            }
                            else if (student.NumberId == "" || student.NumberId == null)
                            {
                                TempData["warning"] = "ادخل الرقم القومي الفارغ";
                                return RedirectToAction("Index");
                            }
                        }
                        j++;
                    }
                    TempData["success"] = "تم حفظ البيانات بنجاح";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["warning"] = "البيانات المدخلة غير صحيحة";
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        #endregion

        [Authorized(ScreenId = "14")]

        public ActionResult Edit(Guid Id)
        {
            var student = studentsServices.Get(Id);

            var Levels = levelsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            Levels = Levels.Where(x => x.SchoolId == student.SchoolId).ToList();
            ViewBag.LevelId = new SelectList(Levels, "Id", "Name", student.LevelId);

            var Classes = classesServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]).Where(x => x.LevelId == student.LevelId).ToList();
            Classes = Classes.Where(x => x.SchoolId == student.SchoolId).ToList();
            ViewBag.ClassId = new SelectList(Classes, "Id", "Name", student.ClassId);

            var Religion = religionServices.GetAll();
            ViewBag.ReligionId = new SelectList(Religion, "Id", "Name", student.ReligionId);

            var Nationalities = nationalitiesServices.GetAll();
            ViewBag.NationalityId = new SelectList(Nationalities, "Id", "Name", student.NationalityId);

            var ExpenseTypes = expenseTypesServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            ViewBag.ExpenseTypeId = new SelectList(ExpenseTypes, "Id", "Name", student.ExpenseTypeId);

            var ExemptionReasons = exemptionReasonsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            ExemptionReasons = ExemptionReasons.Where(x => x.SchoolId == student.SchoolId).ToList();
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
            SocialStatus = SocialStatus.Where(x => x.SchoolId == student.SchoolId).ToList();
            ViewBag.SocialStateId = new SelectList(SocialStatus, "Id", "Name", student.SocialStateId);

            var ScienceDivision = scienceDivisionServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            ScienceDivision = ScienceDivision.Where(x => x.SchoolId == student.SchoolId).ToList();
            ViewBag.ScienceDivisionId = new SelectList(ScienceDivision, "Id", "Name", student.ScienceDivisionId);

            var SecondLanguage = secondLanguageServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            SecondLanguage = SecondLanguage.Where(x => x.SchoolId == student.SchoolId).ToList();

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
            if (student.SerialNumber == "" || student.SerialNumber == null)
            {
                var studentsCount = studentsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]).Where(x => x.SchoolId == student.SchoolId).Count();
                studentsCount += 100;
                student.SerialNumber = studentsCount.ToString();
            }
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
                Levels = Levels.Where(x => x.SchoolId == student.SchoolId).ToList();
                ViewBag.LevelId = new SelectList(Levels, "Id", "Name", student.LevelId);

                var Classes = classesServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]).Where(x => x.LevelId == student.LevelId).ToList();
                Classes = Classes.Where(x => x.SchoolId == student.SchoolId).ToList();
                ViewBag.ClassId = new SelectList(Classes, "Id", "Name", student.ClassId);

                var Religion = religionServices.GetAll();
                ViewBag.ReligionId = new SelectList(Religion, "Id", "Name", student.ReligionId);

                var Nationalities = nationalitiesServices.GetAll();
                ViewBag.NationalityId = new SelectList(Nationalities, "Id", "Name", student.NationalityId);

                var ExpenseTypes = expenseTypesServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                ViewBag.ExpenseTypeId = new SelectList(ExpenseTypes, "Id", "Name", student.ExpenseTypeId);

                var ExemptionReasons = exemptionReasonsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                ExemptionReasons = ExemptionReasons.Where(x => x.SchoolId == student.SchoolId).ToList();
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
                SocialStatus = SocialStatus.Where(x => x.SchoolId == student.SchoolId).ToList();
                ViewBag.SocialStateId = new SelectList(SocialStatus, "Id", "Name", student.SocialStateId);

                var ScienceDivision = scienceDivisionServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                ScienceDivision = ScienceDivision.Where(x => x.SchoolId == student.SchoolId).ToList();
                ViewBag.ScienceDivisionId = new SelectList(ScienceDivision, "Id", "Name", student.ScienceDivisionId);

                var SecondLanguage = secondLanguageServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                SecondLanguage = SecondLanguage.Where(x => x.SchoolId == student.SchoolId).ToList();

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

        public ActionResult Details(Guid Id)
        {
            var student = studentsServices.Get(Id);

            var Levels = levelsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            Levels = Levels.Where(x => x.SchoolId == student.SchoolId).ToList();
            ViewBag.LevelId = new SelectList(Levels, "Id", "Name", student.LevelId);

            var Classes = classesServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]).Where(x => x.LevelId == student.LevelId).ToList();
            Classes = Classes.Where(x => x.SchoolId == student.SchoolId).ToList();
            ViewBag.ClassId = new SelectList(Classes, "Id", "Name", student.ClassId);

            var Religion = religionServices.GetAll();
            ViewBag.ReligionId = new SelectList(Religion, "Id", "Name", student.ReligionId);

            var Nationalities = nationalitiesServices.GetAll();
            ViewBag.NationalityId = new SelectList(Nationalities, "Id", "Name", student.NationalityId);

            var ExpenseTypes = expenseTypesServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            ViewBag.ExpenseTypeId = new SelectList(ExpenseTypes, "Id", "Name", student.ExpenseTypeId);

            var ExemptionReasons = exemptionReasonsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            ExemptionReasons = ExemptionReasons.Where(x => x.SchoolId == student.SchoolId).ToList();
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
            SocialStatus = SocialStatus.Where(x => x.SchoolId == student.SchoolId).ToList();
            ViewBag.SocialStateId = new SelectList(SocialStatus, "Id", "Name", student.SocialStateId);

            var ScienceDivision = scienceDivisionServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            ScienceDivision = ScienceDivision.Where(x => x.SchoolId == student.SchoolId).ToList();
            ViewBag.ScienceDivisionId = new SelectList(ScienceDivision, "Id", "Name", student.ScienceDivisionId);

            var SecondLanguage = secondLanguageServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            SecondLanguage = SecondLanguage.Where(x => x.SchoolId == student.SchoolId).ToList();

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

            return View(student);
        }


        public ActionResult QrPage(Guid Id)
        {
            var student = studentsServices.GetByDto(Id);
            ViewBag.url2 = "https://studentaffairs.system-clouds.com/";

            var baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority;
            string qrString = student.SerialNumber;
            var QrCodeUri = QRCode(qrString)[0];
            //return RedirectToAction("Index");
            return Json(new { StudentImage = student.Image, StudentName = "الاسم : " + student.Name, LevelName = "الصف : " + student.LevelName, QrCodeUri = QrCodeUri }, JsonRequestBehavior.AllowGet);
        }
        public string[] QRCode(string qRCode)
        {
            var file = Server.MapPath("~/Uploads") + "/" + Guid.NewGuid();
            QRCodeGenerator QrGenerator = new QRCodeGenerator();
            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(qRCode, QRCodeGenerator.ECCLevel.Q);
            QRCode QrCode = new QRCode(QrCodeInfo);
            Bitmap QrBitmap = QrCode.GetGraphic(60);
            byte[] BitmapArray = QrBitmap.BitmapToByteArray();
            string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
            QrCode.GetGraphic(60).Save(file);
            string[] qr = { "", "" };

            qr[0] = QrUri;
            qr[1] = file;
            return qr;
        }


        public ActionResult getLevels(Guid? Id)
        {
            var result = levelsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]).Where(x => x.SchoolId == Id).Select(x => new { x.Id, x.Name }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult getExemptionReasons(Guid? Id)
        {
            var result = exemptionReasonsServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]).Where(x => x.SchoolId == Id).Select(x => new { x.Id, x.Name }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult getSocialStates(Guid? Id)
        {
            var result = socialStatusServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]).Where(x => x.SchoolId == Id).Select(x => new { x.Id, x.Name }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult getScienceDivision(Guid? Id)
        {
            var result = scienceDivisionServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]).Where(x => x.SchoolId == Id).Select(x => new { x.Id, x.Name }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult getSecondLanguage(Guid? Id)
        {
            var result = secondLanguageServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]).Where(x => x.SchoolId == Id).Select(x => new { x.Id, x.Name }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult getClasses(Guid? Id)
        {
            var result = classesServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]).Where(x => x.LevelId == Id).Select(x => new { x.Id, x.Name }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [Authorized(ScreenId = "15")]

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

        [Authorized(ScreenId = "17")]

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

        [Authorized(ScreenId = "16")]

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
            var result = citiesServices.GetAll().Select(x => new { x.Id, x.Name, x.Code }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);

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

    public static class BitmapExtension
    {
        public static byte[] BitmapToByteArray(this Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }

}