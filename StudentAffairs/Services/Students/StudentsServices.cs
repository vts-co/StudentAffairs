using StudentAffairs.Dtos.Students;
using StudentAffairs.Enums;
using StudentAffairs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentAffairs.Services.Students
{
    public class StudentsServices
    {
        public List<StudentsDto> GetAll(Guid UserId, Guid SchoolId, Guid EmployeeId, Role RoleId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.Students.Where(x => x.IsDeleted == false && x.StudentTypeId == (int)StudentTypes.InSchool && (x.CreatedBy == UserId || x.SchoolId == SchoolId || RoleId == Role.Super_Admin)).OrderBy(x => x.CreatedOn).Select(x => new StudentsDto
                {
                    //بيانات الطالب
                    Id = x.Id,
                    Code = x.Code,
                    SerialNumber = x.SerialNumber,
                    Name = x.Name,
                    Image = x.Image,
                    Phone = x.Phone,
                    LevelId = x.LevelId != null ? x.LevelId : Guid.Empty,
                    LevelName = x.LevelId != null ? x.Level.Name : "",
                    ClassId = x.ClassId != null ? x.ClassId : Guid.Empty,
                    ClassName = x.ClassId != null ? x.Class.Name : "",
                    SeatNumber = x.SeatNumber,
                    ReligionId = x.ReligionId != null ? x.ReligionId : Guid.Empty,
                    ReligionName = x.ReligionId != null ? x.Religion.Name : "",
                    NationalityId = x.NationalityId != null ? x.NationalityId : Guid.Empty,
                    NationalityName = x.NationalityId != null ? x.Nationality.Name : "",
                    ExpenseTypeId = x.ExpenseTypeId != null ? x.ExpenseTypeId : Guid.Empty,
                    ExpenseTypeName = x.ExpenseTypeId != null ? x.ExpenseType.Name : "",
                    ExemptionReasonId = x.ExemptionReasonId != null ? x.ExemptionReasonId : Guid.Empty,
                    ExemptionReasonName = x.ExemptionReasonId != null ? x.ExemptionReason.Name : "",
                    EndYearResultId = x.EndYearResultId != null ? x.EndYearResultId : Guid.Empty,
                    EndYearResultName = x.EndYearResultId != null ? x.EndYearResult.Name : "",
                    SecondRoundResultId = x.SecondRoundResultId != null ? x.SecondRoundResultId : Guid.Empty,
                    SecondRoundResultName = x.SecondRoundResultId != null ? x.SecondRoundResult.Name : "",
                    HealthCondition = x.HealthCondition,
                    FirstSemesterResult = x.FirstSemesterResult,
                    EducationalIntegration = x.EducationalIntegration,
                    TransferredToTheSchool = x.TransferredToTheSchool,
                    Address = x.Address,

                    //بيانات الرقم القومي والقيد
                    NumberId = x.NumberId,
                    BirthDate = x.BirthDate.ToString(),
                    Day = x.Day,
                    Month = x.Month,
                    Year = x.Year,
                    CityId = x.CityId != null ? x.CityId : Guid.Empty,
                    CityName = x.CityId != null ? x.City.Name : "",
                    GenderId = x.GenderId != null ? (int)x.GenderId : 0,
                    GenderName = x.GenderId != null ? ((Gender)x.GenderId).ToString() : "",
                    RegistrationStateId = x.RegistrationStateId != null ? x.RegistrationStateId : Guid.Empty,
                    RegistrationStateName = x.RegistrationStateId != null ? x.RegistrationStatu.Name : "",
                    RegistrationNum = x.RegistrationNum,
                    RegistrationDate = x.RegistrationDate,

                    //بيانات ولي الامر
                    ParentName = x.ParentName,
                    ParentAddress = x.ParentAddress,
                    ParentJob = x.ParentJob,
                    ParentPhone = x.ParentPhone,
                    ParentWhatsApp = x.ParentWhatsApp,
                    MotherName = x.MotherName,
                    MotherPhone = x.MotherPhone,
                    SocialStateId = x.SocialStateId != null ? x.SocialStateId : Guid.Empty,
                    SocialStateName = x.SocialStateId != null ? x.SocialStatu.Name : "",

                    //بيانات خاصة بالثانوي
                    TabletSerialNumber = x.TabletSerialNumber,
                    ScienceDivisionId = x.ScienceDivisionId != null ? x.ScienceDivisionId : Guid.Empty,
                    ScienceDivisionName = x.ScienceDivisionId != null ? x.ScienceDivision.Name : "",
                    IM = x.IM,
                    SecondLanguageIId = x.SecondLanguageId != null ? x.SecondLanguageId : Guid.Empty,
                    SecondLanguageIName = x.SecondLanguageId != null ? x.SecondRoundResult.Name : "",
                    DateOfReceipt = x.DateOfReceipt,
                    InsurancePolicyNumber = x.InsurancePolicyNumber,
                    InsurancePolicyDate = x.InsurancePolicyDate,

                    //لائحة التحفيز والانضباط
                    ProtectionCommittee = x.ProtectionCommittee,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    Duration = x.Duration,
                    Problem = x.Problem,
                    ActionTaken = x.ActionTaken,

                    //المواهب والمسابقات
                    Competitions = x.Competitions,
                    Talents = x.Talents,

                    SchoolId = x.SchoolId,
                    SchoolName = x.SchoolId != null ? x.SchoolInfo.Name : "",
                    Notes = x.Notes
                }).ToList();

                return model;
            }
        }
        public List<StudentsDto> GetAllBySchool(Guid SchoolId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.Students.Where(x => x.IsDeleted == false && x.StudentTypeId == (int)StudentTypes.InSchool &&  x.SchoolId == SchoolId ).OrderBy(x => x.CreatedOn).Select(x => new StudentsDto
                {
                    //بيانات الطالب
                    Id = x.Id,
                    Code = x.Code,
                    SerialNumber = x.SerialNumber,
                    Name = x.Name,
                    Image = x.Image,
                    Phone = x.Phone,
                    LevelId = x.LevelId != null ? x.LevelId : Guid.Empty,
                    LevelName = x.LevelId != null ? x.Level.Name : "",
                    ClassId = x.ClassId != null ? x.ClassId : Guid.Empty,
                    ClassName = x.ClassId != null ? x.Class.Name : "",
                    SeatNumber = x.SeatNumber,
                    ReligionId = x.ReligionId != null ? x.ReligionId : Guid.Empty,
                    ReligionName = x.ReligionId != null ? x.Religion.Name : "",
                    NationalityId = x.NationalityId != null ? x.NationalityId : Guid.Empty,
                    NationalityName = x.NationalityId != null ? x.Nationality.Name : "",
                    ExpenseTypeId = x.ExpenseTypeId != null ? x.ExpenseTypeId : Guid.Empty,
                    ExpenseTypeName = x.ExpenseTypeId != null ? x.ExpenseType.Name : "",
                    ExemptionReasonId = x.ExemptionReasonId != null ? x.ExemptionReasonId : Guid.Empty,
                    ExemptionReasonName = x.ExemptionReasonId != null ? x.ExemptionReason.Name : "",
                    EndYearResultId = x.EndYearResultId != null ? x.EndYearResultId : Guid.Empty,
                    EndYearResultName = x.EndYearResultId != null ? x.EndYearResult.Name : "",
                    SecondRoundResultId = x.SecondRoundResultId != null ? x.SecondRoundResultId : Guid.Empty,
                    SecondRoundResultName = x.SecondRoundResultId != null ? x.SecondRoundResult.Name : "",
                    HealthCondition = x.HealthCondition,
                    FirstSemesterResult = x.FirstSemesterResult,
                    EducationalIntegration = x.EducationalIntegration,
                    TransferredToTheSchool = x.TransferredToTheSchool,
                    Address = x.Address,

                    //بيانات الرقم القومي والقيد
                    NumberId = x.NumberId,
                    BirthDate = x.BirthDate.ToString(),
                    Day = x.Day,
                    Month = x.Month,
                    Year = x.Year,
                    CityId = x.CityId != null ? x.CityId : Guid.Empty,
                    CityName = x.CityId != null ? x.City.Name : "",
                    GenderId = x.GenderId != null ? (int)x.GenderId : 0,
                    GenderName = x.GenderId != null ? ((Gender)x.GenderId).ToString() : "",
                    RegistrationStateId = x.RegistrationStateId != null ? x.RegistrationStateId : Guid.Empty,
                    RegistrationStateName = x.RegistrationStateId != null ? x.RegistrationStatu.Name : "",
                    RegistrationNum = x.RegistrationNum,
                    RegistrationDate = x.RegistrationDate,

                    //بيانات ولي الامر
                    ParentName = x.ParentName,
                    ParentAddress = x.ParentAddress,
                    ParentJob = x.ParentJob,
                    ParentPhone = x.ParentPhone,
                    ParentWhatsApp = x.ParentWhatsApp,
                    MotherName = x.MotherName,
                    MotherPhone = x.MotherPhone,
                    SocialStateId = x.SocialStateId != null ? x.SocialStateId : Guid.Empty,
                    SocialStateName = x.SocialStateId != null ? x.SocialStatu.Name : "",

                    //بيانات خاصة بالثانوي
                    TabletSerialNumber = x.TabletSerialNumber,
                    ScienceDivisionId = x.ScienceDivisionId != null ? x.ScienceDivisionId : Guid.Empty,
                    ScienceDivisionName = x.ScienceDivisionId != null ? x.ScienceDivision.Name : "",
                    IM = x.IM,
                    SecondLanguageIId = x.SecondLanguageId != null ? x.SecondLanguageId : Guid.Empty,
                    SecondLanguageIName = x.SecondLanguageId != null ? x.SecondRoundResult.Name : "",
                    DateOfReceipt = x.DateOfReceipt,
                    InsurancePolicyNumber = x.InsurancePolicyNumber,
                    InsurancePolicyDate = x.InsurancePolicyDate,

                    //لائحة التحفيز والانضباط
                    ProtectionCommittee = x.ProtectionCommittee,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    Duration = x.Duration,
                    Problem = x.Problem,
                    ActionTaken = x.ActionTaken,

                    //المواهب والمسابقات
                    Competitions = x.Competitions,
                    Talents = x.Talents,

                    SchoolId = x.SchoolId,
                    SchoolName = x.SchoolId != null ? x.SchoolInfo.Name : "",
                    Notes = x.Notes
                }).ToList();

                return model;
            }
        }

        public List<StudentsDto> GetAllGraduates(Guid UserId, Guid SchoolId, Guid EmployeeId, Role RoleId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.Students.Where(x => x.IsDeleted == false && x.StudentTypeId == (int)StudentTypes.Graduates && (x.CreatedBy == UserId || x.SchoolId == SchoolId || RoleId == Role.Super_Admin)).OrderBy(x => x.CreatedOn).Select(x => new StudentsDto
                {
                    //بيانات الطالب
                    Id = x.Id,
                    Code = x.Code,
                    SerialNumber = x.SerialNumber,

                    Name = x.Name,
                    Image = x.Image,
                    Phone = x.Phone,
                    LevelId = x.LevelId != null ? x.LevelId : Guid.Empty,
                    LevelName = x.LevelId != null ? x.Level.Name : "",
                    ClassId = x.ClassId != null ? x.ClassId : Guid.Empty,
                    ClassName = x.ClassId != null ? x.Class.Name : "",
                    SeatNumber = x.SeatNumber,
                    ReligionId = x.ReligionId != null ? x.ReligionId : Guid.Empty,
                    ReligionName = x.ReligionId != null ? x.Religion.Name : "",
                    NationalityId = x.NationalityId != null ? x.NationalityId : Guid.Empty,
                    NationalityName = x.NationalityId != null ? x.Nationality.Name : "",
                    ExpenseTypeId = x.ExpenseTypeId != null ? x.ExpenseTypeId : Guid.Empty,
                    ExpenseTypeName = x.ExpenseTypeId != null ? x.ExpenseType.Name : "",
                    ExemptionReasonId = x.ExemptionReasonId != null ? x.ExemptionReasonId : Guid.Empty,
                    ExemptionReasonName = x.ExemptionReasonId != null ? x.ExemptionReason.Name : "",
                    EndYearResultId = x.EndYearResultId != null ? x.EndYearResultId : Guid.Empty,
                    EndYearResultName = x.EndYearResultId != null ? x.EndYearResult.Name : "",
                    SecondRoundResultId = x.SecondRoundResultId != null ? x.SecondRoundResultId : Guid.Empty,
                    SecondRoundResultName = x.SecondRoundResultId != null ? x.SecondRoundResult.Name : "",
                    HealthCondition = x.HealthCondition,
                    FirstSemesterResult = x.FirstSemesterResult,
                    EducationalIntegration = x.EducationalIntegration,
                    TransferredToTheSchool = x.TransferredToTheSchool,
                    Address = x.Address,

                    //بيانات الرقم القومي والقيد
                    NumberId = x.NumberId,
                    BirthDate = x.BirthDate.ToString(),
                    Day = x.Day,
                    Month = x.Month,
                    Year = x.Year,
                    CityId = x.CityId != null ? x.CityId : Guid.Empty,
                    CityName = x.CityId != null ? x.City.Name : "",
                    GenderId = x.GenderId != null ? (int)x.GenderId : 0,
                    RegistrationStateId = x.RegistrationStateId != null ? x.RegistrationStateId : Guid.Empty,
                    RegistrationStateName = x.RegistrationStateId != null ? x.RegistrationStatu.Name : "",
                    RegistrationNum = x.RegistrationNum,
                    RegistrationDate = x.RegistrationDate,

                    //بيانات ولي الامر
                    ParentName = x.ParentName,
                    ParentAddress = x.ParentAddress,
                    ParentJob = x.ParentJob,
                    ParentPhone = x.ParentPhone,
                    ParentWhatsApp = x.ParentWhatsApp,
                    MotherName = x.MotherName,
                    MotherPhone = x.MotherPhone,
                    SocialStateId = x.SocialStateId != null ? x.SocialStateId : Guid.Empty,
                    SocialStateName = x.SocialStateId != null ? x.SocialStatu.Name : "",

                    //بيانات خاصة بالثانوي
                    TabletSerialNumber = x.TabletSerialNumber,
                    ScienceDivisionId = x.ScienceDivisionId != null ? x.ScienceDivisionId : Guid.Empty,
                    ScienceDivisionName = x.ScienceDivisionId != null ? x.ScienceDivision.Name : "",
                    IM = x.IM,
                    SecondLanguageIId = x.SecondLanguageId != null ? x.SecondLanguageId : Guid.Empty,
                    SecondLanguageIName = x.SecondLanguageId != null ? x.SecondRoundResult.Name : "",
                    DateOfReceipt = x.DateOfReceipt,
                    InsurancePolicyNumber = x.InsurancePolicyNumber,
                    InsurancePolicyDate = x.InsurancePolicyDate,

                    //لائحة التحفيز والانضباط
                    ProtectionCommittee = x.ProtectionCommittee,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    Duration = x.Duration,
                    Problem = x.Problem,
                    ActionTaken = x.ActionTaken,

                    //المواهب والمسابقات
                    Competitions = x.Competitions,
                    Talents = x.Talents,

                    SchoolId = x.SchoolId,
                    SchoolName = x.SchoolId != null ? x.SchoolInfo.Name : "",

                    Notes = x.Notes
                }).ToList();

                return model;
            }
        }

        public List<StudentsDto> GetAllTransferFromSchool(Guid UserId, Guid SchoolId, Guid EmployeeId, Role RoleId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.Students.Where(x => x.IsDeleted == false && x.StudentTypeId == (int)StudentTypes.TransferFromSchool && (x.CreatedBy == UserId || x.SchoolId == SchoolId || RoleId == Role.Super_Admin)).OrderBy(x => x.CreatedOn).Select(x => new StudentsDto
                {
                    //بيانات الطالب
                    Id = x.Id,
                    Code = x.Code,
                    SerialNumber = x.SerialNumber,

                    Name = x.Name,
                    Image = x.Image,
                    Phone = x.Phone,
                    LevelId = x.LevelId != null ? x.LevelId : Guid.Empty,
                    LevelName = x.LevelId != null ? x.Level.Name : "",
                    ClassId = x.ClassId != null ? x.ClassId : Guid.Empty,
                    ClassName = x.ClassId != null ? x.Class.Name : "",
                    SeatNumber = x.SeatNumber,
                    ReligionId = x.ReligionId != null ? x.ReligionId : Guid.Empty,
                    ReligionName = x.ReligionId != null ? x.Religion.Name : "",
                    NationalityId = x.NationalityId != null ? x.NationalityId : Guid.Empty,
                    NationalityName = x.NationalityId != null ? x.Nationality.Name : "",
                    ExpenseTypeId = x.ExpenseTypeId != null ? x.ExpenseTypeId : Guid.Empty,
                    ExpenseTypeName = x.ExpenseTypeId != null ? x.ExpenseType.Name : "",
                    ExemptionReasonId = x.ExemptionReasonId != null ? x.ExemptionReasonId : Guid.Empty,
                    ExemptionReasonName = x.ExemptionReasonId != null ? x.ExemptionReason.Name : "",
                    EndYearResultId = x.EndYearResultId != null ? x.EndYearResultId : Guid.Empty,
                    EndYearResultName = x.EndYearResultId != null ? x.EndYearResult.Name : "",
                    SecondRoundResultId = x.SecondRoundResultId != null ? x.SecondRoundResultId : Guid.Empty,
                    SecondRoundResultName = x.SecondRoundResultId != null ? x.SecondRoundResult.Name : "",
                    HealthCondition = x.HealthCondition,
                    FirstSemesterResult = x.FirstSemesterResult,
                    EducationalIntegration = x.EducationalIntegration,
                    TransferredToTheSchool = x.TransferredToTheSchool,
                    Address = x.Address,

                    //بيانات الرقم القومي والقيد
                    NumberId = x.NumberId,
                    BirthDate = x.BirthDate.ToString(),
                    Day = x.Day,
                    Month = x.Month,
                    Year = x.Year,
                    CityId = x.CityId != null ? x.CityId : Guid.Empty,
                    CityName = x.CityId != null ? x.City.Name : "",
                    GenderId = x.GenderId != null ? (int)x.GenderId : 0,
                    RegistrationStateId = x.RegistrationStateId != null ? x.RegistrationStateId : Guid.Empty,
                    RegistrationStateName = x.RegistrationStateId != null ? x.RegistrationStatu.Name : "",
                    RegistrationNum = x.RegistrationNum,
                    RegistrationDate = x.RegistrationDate,

                    //بيانات ولي الامر
                    ParentName = x.ParentName,
                    ParentAddress = x.ParentAddress,
                    ParentJob = x.ParentJob,
                    ParentPhone = x.ParentPhone,
                    ParentWhatsApp = x.ParentWhatsApp,
                    MotherName = x.MotherName,
                    MotherPhone = x.MotherPhone,
                    SocialStateId = x.SocialStateId != null ? x.SocialStateId : Guid.Empty,
                    SocialStateName = x.SocialStateId != null ? x.SocialStatu.Name : "",

                    //بيانات خاصة بالثانوي
                    TabletSerialNumber = x.TabletSerialNumber,
                    ScienceDivisionId = x.ScienceDivisionId != null ? x.ScienceDivisionId : Guid.Empty,
                    ScienceDivisionName = x.ScienceDivisionId != null ? x.ScienceDivision.Name : "",
                    IM = x.IM,
                    SecondLanguageIId = x.SecondLanguageId != null ? x.SecondLanguageId : Guid.Empty,
                    SecondLanguageIName = x.SecondLanguageId != null ? x.SecondRoundResult.Name : "",
                    DateOfReceipt = x.DateOfReceipt,
                    InsurancePolicyNumber = x.InsurancePolicyNumber,
                    InsurancePolicyDate = x.InsurancePolicyDate,

                    //لائحة التحفيز والانضباط
                    ProtectionCommittee = x.ProtectionCommittee,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    Duration = x.Duration,
                    Problem = x.Problem,
                    ActionTaken = x.ActionTaken,

                    //المواهب والمسابقات
                    Competitions = x.Competitions,
                    Talents = x.Talents,

                    SchoolId = x.SchoolId,
                    SchoolName = x.SchoolId != null ? x.SchoolInfo.Name : "",

                    Notes = x.Notes
                }).ToList();

                return model;
            }
        }
        public List<StudentsDto> GetAllTrackConverters(Guid UserId, Guid SchoolId, Guid EmployeeId, Role RoleId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.Students.Where(x => x.IsDeleted == false && x.StudentTypeId == (int)StudentTypes.TrackConverters && (x.CreatedBy == UserId || x.SchoolId == SchoolId || RoleId == Role.Super_Admin)).OrderBy(x => x.CreatedOn).Select(x => new StudentsDto
                {
                    //بيانات الطالب
                    Id = x.Id,
                    Code = x.Code,
                    SerialNumber = x.SerialNumber,

                    Name = x.Name,
                    Image = x.Image,
                    Phone = x.Phone,
                    LevelId = x.LevelId != null ? x.LevelId : Guid.Empty,
                    LevelName = x.LevelId != null ? x.Level.Name : "",
                    ClassId = x.ClassId != null ? x.ClassId : Guid.Empty,
                    ClassName = x.ClassId != null ? x.Class.Name : "",
                    SeatNumber = x.SeatNumber,
                    ReligionId = x.ReligionId != null ? x.ReligionId : Guid.Empty,
                    ReligionName = x.ReligionId != null ? x.Religion.Name : "",
                    NationalityId = x.NationalityId != null ? x.NationalityId : Guid.Empty,
                    NationalityName = x.NationalityId != null ? x.Nationality.Name : "",
                    ExpenseTypeId = x.ExpenseTypeId != null ? x.ExpenseTypeId : Guid.Empty,
                    ExpenseTypeName = x.ExpenseTypeId != null ? x.ExpenseType.Name : "",
                    ExemptionReasonId = x.ExemptionReasonId != null ? x.ExemptionReasonId : Guid.Empty,
                    ExemptionReasonName = x.ExemptionReasonId != null ? x.ExemptionReason.Name : "",
                    EndYearResultId = x.EndYearResultId != null ? x.EndYearResultId : Guid.Empty,
                    EndYearResultName = x.EndYearResultId != null ? x.EndYearResult.Name : "",
                    SecondRoundResultId = x.SecondRoundResultId != null ? x.SecondRoundResultId : Guid.Empty,
                    SecondRoundResultName = x.SecondRoundResultId != null ? x.SecondRoundResult.Name : "",
                    HealthCondition = x.HealthCondition,
                    FirstSemesterResult = x.FirstSemesterResult,
                    EducationalIntegration = x.EducationalIntegration,
                    TransferredToTheSchool = x.TransferredToTheSchool,
                    Address = x.Address,

                    //بيانات الرقم القومي والقيد
                    NumberId = x.NumberId,
                    BirthDate = x.BirthDate.ToString(),
                    Day = x.Day,
                    Month = x.Month,
                    Year = x.Year,
                    CityId = x.CityId != null ? x.CityId : Guid.Empty,
                    CityName = x.CityId != null ? x.City.Name : "",
                    GenderId = x.GenderId != null ? (int)x.GenderId : 0,
                    RegistrationStateId = x.RegistrationStateId != null ? x.RegistrationStateId : Guid.Empty,
                    RegistrationStateName = x.RegistrationStateId != null ? x.RegistrationStatu.Name : "",
                    RegistrationNum = x.RegistrationNum,
                    RegistrationDate = x.RegistrationDate,

                    //بيانات ولي الامر
                    ParentName = x.ParentName,
                    ParentAddress = x.ParentAddress,
                    ParentJob = x.ParentJob,
                    ParentPhone = x.ParentPhone,
                    ParentWhatsApp = x.ParentWhatsApp,
                    MotherName = x.MotherName,
                    MotherPhone = x.MotherPhone,
                    SocialStateId = x.SocialStateId != null ? x.SocialStateId : Guid.Empty,
                    SocialStateName = x.SocialStateId != null ? x.SocialStatu.Name : "",

                    //بيانات خاصة بالثانوي
                    TabletSerialNumber = x.TabletSerialNumber,
                    ScienceDivisionId = x.ScienceDivisionId != null ? x.ScienceDivisionId : Guid.Empty,
                    ScienceDivisionName = x.ScienceDivisionId != null ? x.ScienceDivision.Name : "",
                    IM = x.IM,
                    SecondLanguageIId = x.SecondLanguageId != null ? x.SecondLanguageId : Guid.Empty,
                    SecondLanguageIName = x.SecondLanguageId != null ? x.SecondRoundResult.Name : "",
                    DateOfReceipt = x.DateOfReceipt,
                    InsurancePolicyNumber = x.InsurancePolicyNumber,
                    InsurancePolicyDate = x.InsurancePolicyDate,

                    //لائحة التحفيز والانضباط
                    ProtectionCommittee = x.ProtectionCommittee,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    Duration = x.Duration,
                    Problem = x.Problem,
                    ActionTaken = x.ActionTaken,

                    //المواهب والمسابقات
                    Competitions = x.Competitions,
                    Talents = x.Talents,

                    SchoolId = x.SchoolId,
                    SchoolName = x.SchoolId != null ? x.SchoolInfo.Name : "",

                    Notes = x.Notes
                }).ToList();

                return model;
            }
        }

        public Student Get(Guid Id)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.Students.Where(x => x.IsDeleted == false && x.Id == Id).OrderBy(x => x.CreatedOn).FirstOrDefault();
                return model;
            }
        }
        public StudentsDto GetByDto(Guid Id)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.Students.Where(x => x.IsDeleted == false && x.StudentTypeId == (int)StudentTypes.InSchool && x.Id==Id).OrderBy(x => x.CreatedOn).Select(x => new StudentsDto
                {
                    //بيانات الطالب
                    Id = x.Id,
                    Code = x.Code,
                    SerialNumber = x.SerialNumber,
                    Name = x.Name,
                    Image = x.Image,
                    Phone = x.Phone,
                    LevelId = x.LevelId != null ? x.LevelId : Guid.Empty,
                    LevelName = x.LevelId != null ? x.Level.Name : "",
                    ClassId = x.ClassId != null ? x.ClassId : Guid.Empty,
                    ClassName = x.ClassId != null ? x.Class.Name : "",
                    SeatNumber = x.SeatNumber,
                    ReligionId = x.ReligionId != null ? x.ReligionId : Guid.Empty,
                    ReligionName = x.ReligionId != null ? x.Religion.Name : "",
                    NationalityId = x.NationalityId != null ? x.NationalityId : Guid.Empty,
                    NationalityName = x.NationalityId != null ? x.Nationality.Name : "",
                    ExpenseTypeId = x.ExpenseTypeId != null ? x.ExpenseTypeId : Guid.Empty,
                    ExpenseTypeName = x.ExpenseTypeId != null ? x.ExpenseType.Name : "",
                    ExemptionReasonId = x.ExemptionReasonId != null ? x.ExemptionReasonId : Guid.Empty,
                    ExemptionReasonName = x.ExemptionReasonId != null ? x.ExemptionReason.Name : "",
                    EndYearResultId = x.EndYearResultId != null ? x.EndYearResultId : Guid.Empty,
                    EndYearResultName = x.EndYearResultId != null ? x.EndYearResult.Name : "",
                    SecondRoundResultId = x.SecondRoundResultId != null ? x.SecondRoundResultId : Guid.Empty,
                    SecondRoundResultName = x.SecondRoundResultId != null ? x.SecondRoundResult.Name : "",
                    HealthCondition = x.HealthCondition,
                    FirstSemesterResult = x.FirstSemesterResult,
                    EducationalIntegration = x.EducationalIntegration,
                    TransferredToTheSchool = x.TransferredToTheSchool,
                    Address = x.Address,

                    //بيانات الرقم القومي والقيد
                    NumberId = x.NumberId,
                    BirthDate = x.BirthDate.ToString(),
                    Day = x.Day,
                    Month = x.Month,
                    Year = x.Year,
                    CityId = x.CityId != null ? x.CityId : Guid.Empty,
                    CityName = x.CityId != null ? x.City.Name : "",
                    GenderId = x.GenderId != null ? (int)x.GenderId : 0,
                    GenderName = x.GenderId != null ? ((Gender)x.GenderId).ToString() : "",
                    RegistrationStateId = x.RegistrationStateId != null ? x.RegistrationStateId : Guid.Empty,
                    RegistrationStateName = x.RegistrationStateId != null ? x.RegistrationStatu.Name : "",
                    RegistrationNum = x.RegistrationNum,
                    RegistrationDate = x.RegistrationDate,

                    //بيانات ولي الامر
                    ParentName = x.ParentName,
                    ParentAddress = x.ParentAddress,
                    ParentJob = x.ParentJob,
                    ParentPhone = x.ParentPhone,
                    ParentWhatsApp = x.ParentWhatsApp,
                    MotherName = x.MotherName,
                    MotherPhone = x.MotherPhone,
                    SocialStateId = x.SocialStateId != null ? x.SocialStateId : Guid.Empty,
                    SocialStateName = x.SocialStateId != null ? x.SocialStatu.Name : "",

                    //بيانات خاصة بالثانوي
                    TabletSerialNumber = x.TabletSerialNumber,
                    ScienceDivisionId = x.ScienceDivisionId != null ? x.ScienceDivisionId : Guid.Empty,
                    ScienceDivisionName = x.ScienceDivisionId != null ? x.ScienceDivision.Name : "",
                    IM = x.IM,
                    SecondLanguageIId = x.SecondLanguageId != null ? x.SecondLanguageId : Guid.Empty,
                    SecondLanguageIName = x.SecondLanguageId != null ? x.SecondRoundResult.Name : "",
                    DateOfReceipt = x.DateOfReceipt,
                    InsurancePolicyNumber = x.InsurancePolicyNumber,
                    InsurancePolicyDate = x.InsurancePolicyDate,

                    //لائحة التحفيز والانضباط
                    ProtectionCommittee = x.ProtectionCommittee,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    Duration = x.Duration,
                    Problem = x.Problem,
                    ActionTaken = x.ActionTaken,

                    //المواهب والمسابقات
                    Competitions = x.Competitions,
                    Talents = x.Talents,

                    SchoolId = x.SchoolId,
                    SchoolName = x.SchoolId != null ? x.SchoolInfo.Name : "",
                    Notes = x.Notes
                }).FirstOrDefault();
                return model;
            }
        }
        public StudentsDto GetBySerialNumber(Guid SchoolId, string SerialNumber)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.Students.Where(x => x.IsDeleted == false && x.StudentTypeId == (int)StudentTypes.InSchool && x.SchoolId == SchoolId && x.SerialNumber == SerialNumber).OrderBy(x => x.CreatedOn).Select(x => new StudentsDto
                {
                    //بيانات الطالب
                    Id = x.Id,
                    Code = x.Code,
                    SerialNumber = x.SerialNumber,
                    Name = x.Name,
                    Image = x.Image,

                    LevelId = x.LevelId != null ? x.LevelId : Guid.Empty,
                    LevelName = x.LevelId != null ? x.Level.Name : "",
                    ClassId = x.ClassId != null ? x.ClassId : Guid.Empty,
                    ClassName = x.ClassId != null ? x.Class.Name : "",

                    //بيانات الرقم القومي والقيد
                    NumberId = x.NumberId,

                    SchoolId = x.SchoolId,
                    SchoolName = x.SchoolId != null ? x.SchoolInfo.Name : "",
                    Notes = x.Notes
                }).FirstOrDefault();
                return model;
            }
        }
        public StudentsDto GetByCode(string code, Guid? SchoolId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.Students.Where(x => x.IsDeleted == false && x.Code == code && x.SchoolId == SchoolId).OrderBy(x => x.CreatedOn).Select(x => new StudentsDto
                {
                    //بيانات الطالب
                    Id = x.Id,
                    Code = x.Code,
                    SerialNumber = x.SerialNumber,
                    Name = x.Name,
                    Image = x.Image,
                    Phone = x.Phone,
                    LevelId = x.LevelId != null ? x.LevelId : Guid.Empty,
                    LevelName = x.LevelId != null ? x.Level.Name : "",
                    ClassId = x.ClassId != null ? x.ClassId : Guid.Empty,
                    ClassName = x.ClassId != null ? x.Class.Name : "",
                    SeatNumber = x.SeatNumber,
                    ReligionId = x.ReligionId != null ? x.ReligionId : Guid.Empty,
                    ReligionName = x.ReligionId != null ? x.Religion.Name : "",
                    NationalityId = x.NationalityId != null ? x.NationalityId : Guid.Empty,
                    NationalityName = x.NationalityId != null ? x.Nationality.Name : "",
                    ExpenseTypeId = x.ExpenseTypeId != null ? x.ExpenseTypeId : Guid.Empty,
                    ExpenseTypeName = x.ExpenseTypeId != null ? x.ExpenseType.Name : "",
                    ExemptionReasonId = x.ExemptionReasonId != null ? x.ExemptionReasonId : Guid.Empty,
                    ExemptionReasonName = x.ExemptionReasonId != null ? x.ExemptionReason.Name : "",
                    EndYearResultId = x.EndYearResultId != null ? x.EndYearResultId : Guid.Empty,
                    EndYearResultName = x.EndYearResultId != null ? x.EndYearResult.Name : "",
                    SecondRoundResultId = x.SecondRoundResultId != null ? x.SecondRoundResultId : Guid.Empty,
                    SecondRoundResultName = x.SecondRoundResultId != null ? x.SecondRoundResult.Name : "",
                    HealthCondition = x.HealthCondition,
                    FirstSemesterResult = x.FirstSemesterResult,
                    EducationalIntegration = x.EducationalIntegration,
                    TransferredToTheSchool = x.TransferredToTheSchool,
                    Address = x.Address,

                    //بيانات الرقم القومي والقيد
                    NumberId = x.NumberId,
                    BirthDate = x.BirthDate.ToString(),
                    Day = x.Day,
                    Month = x.Month,
                    Year = x.Year,
                    CityId = x.CityId != null ? x.CityId : Guid.Empty,
                    CityName = x.CityId != null ? x.City.Name : "",
                    GenderId = x.GenderId != null ? (int)x.GenderId : 0,
                    GenderName = x.GenderId != null ? ((Gender)x.GenderId).ToString() : "",
                    RegistrationStateId = x.RegistrationStateId != null ? x.RegistrationStateId : Guid.Empty,
                    RegistrationStateName = x.RegistrationStateId != null ? x.RegistrationStatu.Name : "",
                    RegistrationNum = x.RegistrationNum,
                    RegistrationDate = x.RegistrationDate,

                    //بيانات ولي الامر
                    ParentName = x.ParentName,
                    ParentAddress = x.ParentAddress,
                    ParentJob = x.ParentJob,
                    ParentPhone = x.ParentPhone,
                    ParentWhatsApp = x.ParentWhatsApp,
                    MotherName = x.MotherName,
                    MotherPhone = x.MotherPhone,
                    SocialStateId = x.SocialStateId != null ? x.SocialStateId : Guid.Empty,
                    SocialStateName = x.SocialStateId != null ? x.SocialStatu.Name : "",

                    //بيانات خاصة بالثانوي
                    TabletSerialNumber = x.TabletSerialNumber,
                    ScienceDivisionId = x.ScienceDivisionId != null ? x.ScienceDivisionId : Guid.Empty,
                    ScienceDivisionName = x.ScienceDivisionId != null ? x.ScienceDivision.Name : "",
                    IM = x.IM,
                    SecondLanguageIId = x.SecondLanguageId != null ? x.SecondLanguageId : Guid.Empty,
                    SecondLanguageIName = x.SecondLanguageId != null ? x.SecondRoundResult.Name : "",
                    DateOfReceipt = x.DateOfReceipt,
                    InsurancePolicyNumber = x.InsurancePolicyNumber,
                    InsurancePolicyDate = x.InsurancePolicyDate,

                    //لائحة التحفيز والانضباط
                    ProtectionCommittee = x.ProtectionCommittee,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    Duration = x.Duration,
                    Problem = x.Problem,
                    ActionTaken = x.ActionTaken,

                    //المواهب والمسابقات
                    Competitions = x.Competitions,
                    Talents = x.Talents,

                    SchoolId = x.SchoolId,
                    SchoolName = x.SchoolId != null ? x.SchoolInfo.Name : "",

                    Notes = x.Notes
                }).FirstOrDefault();

                return model;
            }
        }

        public List<StudentsDto> GetByCodeOrNameOrNumberId(Guid UserId, Guid? SchoolId, Guid EmployeeId, Role RoleId, string code, Guid? SchoolId1)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.Students.Where(x => x.IsDeleted == false && (x.CreatedBy == UserId || x.SchoolId == SchoolId || RoleId == Role.Super_Admin) && (x.Code == code || x.Name.Contains(code) || x.NumberId == code) && x.SchoolId == SchoolId1).OrderBy(x => x.CreatedOn).Select(x => new StudentsDto
                {
                    //بيانات الطالب
                    Id = x.Id,
                    Code = x.Code,
                    SerialNumber = x.SerialNumber,

                    Name = x.Name,
                    Image = x.Image,
                    Phone = x.Phone,
                    LevelId = x.LevelId != null ? x.LevelId : Guid.Empty,
                    LevelName = x.LevelId != null ? x.Level.Name : "",
                    ClassId = x.ClassId != null ? x.ClassId : Guid.Empty,
                    ClassName = x.ClassId != null ? x.Class.Name : "",
                    SeatNumber = x.SeatNumber,
                    ReligionId = x.ReligionId != null ? x.ReligionId : Guid.Empty,
                    ReligionName = x.ReligionId != null ? x.Religion.Name : "",
                    NationalityId = x.NationalityId != null ? x.NationalityId : Guid.Empty,
                    NationalityName = x.NationalityId != null ? x.Nationality.Name : "",
                    ExpenseTypeId = x.ExpenseTypeId != null ? x.ExpenseTypeId : Guid.Empty,
                    ExpenseTypeName = x.ExpenseTypeId != null ? x.ExpenseType.Name : "",
                    ExemptionReasonId = x.ExemptionReasonId != null ? x.ExemptionReasonId : Guid.Empty,
                    ExemptionReasonName = x.ExemptionReasonId != null ? x.ExemptionReason.Name : "",
                    EndYearResultId = x.EndYearResultId != null ? x.EndYearResultId : Guid.Empty,
                    EndYearResultName = x.EndYearResultId != null ? x.EndYearResult.Name : "",
                    SecondRoundResultId = x.SecondRoundResultId != null ? x.SecondRoundResultId : Guid.Empty,
                    SecondRoundResultName = x.SecondRoundResultId != null ? x.SecondRoundResult.Name : "",
                    HealthCondition = x.HealthCondition,
                    FirstSemesterResult = x.FirstSemesterResult,
                    EducationalIntegration = x.EducationalIntegration,
                    TransferredToTheSchool = x.TransferredToTheSchool,
                    Address = x.Address,

                    //بيانات الرقم القومي والقيد
                    NumberId = x.NumberId,
                    BirthDate = x.BirthDate.ToString(),
                    Day = x.Day,
                    Month = x.Month,
                    Year = x.Year,
                    CityId = x.CityId != null ? x.CityId : Guid.Empty,
                    CityName = x.CityId != null ? x.City.Name : "",
                    GenderId = x.GenderId != null ? (int)x.GenderId : 0,
                    GenderName = x.GenderId != null ? ((Gender)x.GenderId).ToString() : "",

                    RegistrationStateId = x.RegistrationStateId != null ? x.RegistrationStateId : Guid.Empty,
                    RegistrationStateName = x.RegistrationStateId != null ? x.RegistrationStatu.Name : "",
                    RegistrationNum = x.RegistrationNum,
                    RegistrationDate = x.RegistrationDate,

                    //بيانات ولي الامر
                    ParentName = x.ParentName,
                    ParentAddress = x.ParentAddress,
                    ParentJob = x.ParentJob,
                    ParentPhone = x.ParentPhone,
                    ParentWhatsApp = x.ParentWhatsApp,
                    MotherName = x.MotherName,
                    MotherPhone = x.MotherPhone,
                    SocialStateId = x.SocialStateId != null ? x.SocialStateId : Guid.Empty,
                    SocialStateName = x.SocialStateId != null ? x.SocialStatu.Name : "",

                    //بيانات خاصة بالثانوي
                    TabletSerialNumber = x.TabletSerialNumber,
                    ScienceDivisionId = x.ScienceDivisionId != null ? x.ScienceDivisionId : Guid.Empty,
                    ScienceDivisionName = x.ScienceDivisionId != null ? x.ScienceDivision.Name : "",
                    IM = x.IM,
                    SecondLanguageIId = x.SecondLanguageId != null ? x.SecondLanguageId : Guid.Empty,
                    SecondLanguageIName = x.SecondLanguageId != null ? x.SecondRoundResult.Name : "",
                    DateOfReceipt = x.DateOfReceipt,
                    InsurancePolicyNumber = x.InsurancePolicyNumber,
                    InsurancePolicyDate = x.InsurancePolicyDate,

                    //لائحة التحفيز والانضباط
                    ProtectionCommittee = x.ProtectionCommittee,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    Duration = x.Duration,
                    Problem = x.Problem,
                    ActionTaken = x.ActionTaken,

                    //المواهب والمسابقات
                    Competitions = x.Competitions,
                    Talents = x.Talents,

                    SchoolId = x.SchoolId,
                    SchoolName = x.SchoolId != null ? x.SchoolInfo.Name : "",

                    Notes = x.Notes
                }).ToList();
                return model;
            }
        }

        public ResultDto<Student> Create(Student model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Student>();
                var Oldmodel = dbContext.Students.Where(x => (x.Name == model.Name || x.NumberId == model.NumberId) && x.SchoolId == model.SchoolId && x.IsDeleted == false).FirstOrDefault();
                if (Oldmodel != null)
                {
                    result.Result = Oldmodel;
                    result.IsSuccess = false;
                    result.Message = "هذا الطالب موجود بالفعل";
                    return result;
                }
                var Oldmodel2 = dbContext.Students.Where(x => x.Code == model.Code && x.SchoolId == model.SchoolId && x.IsDeleted == false).FirstOrDefault();
                if (Oldmodel2 != null)
                {
                    result.Result = Oldmodel2;
                    result.IsSuccess = false;
                    result.Message = "هذا الكود موجود لم يمكن استخدامه";
                    return result;
                }
                model.StudentTypeId = (int)StudentTypes.InSchool;
                model.CreatedOn = DateTime.UtcNow;
                model.CreatedBy = UserId;
                model.IsDeleted = false;
                dbContext.Students.Add(model);
                dbContext.SaveChanges();


                result.IsSuccess = true;
                result.Message = "تم حفظ البيانات بنجاح";
                return result;
            }
        }
        public ResultDto<Student> Edit(Student model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Student>();
                var Oldmodel = dbContext.Students.Find(model.Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "هذا الطالب غير موجود ";
                    return result;
                }
                var Oldmodel2 = dbContext.Students.Where(x => x.Code == model.Code && x.IsDeleted == false && x.Id != model.Id).FirstOrDefault();
                if (Oldmodel2 != null)
                {
                    result.Result = model;
                    result.IsSuccess = false;
                    result.Message = "هذا الكود موجود لم يمكن استخدامه";
                    return result;
                }
                Oldmodel.ModifiedOn = DateTime.UtcNow;
                Oldmodel.ModifiedBy = UserId;


                Oldmodel.Code = model.Code;
                Oldmodel.SerialNumber = model.SerialNumber;

                //بيانات الطالب
                Oldmodel.Name = model.Name;
                Oldmodel.Phone = model.Phone;
                Oldmodel.LevelId = model.LevelId;
                Oldmodel.ClassId = model.ClassId;
                Oldmodel.SeatNumber = model.SeatNumber;
                Oldmodel.ReligionId = model.ReligionId;
                Oldmodel.NationalityId = model.NationalityId;
                Oldmodel.ExpenseTypeId = model.ExpenseTypeId;
                Oldmodel.ExemptionReasonId = model.ExemptionReasonId;
                Oldmodel.EndYearResultId = model.EndYearResultId;
                Oldmodel.SecondRoundResultId = model.SecondRoundResultId;
                Oldmodel.HealthCondition = model.HealthCondition;
                Oldmodel.FirstSemesterResult = model.FirstSemesterResult;
                Oldmodel.EducationalIntegration = model.EducationalIntegration;
                Oldmodel.TransferredToTheSchool = model.TransferredToTheSchool;
                Oldmodel.Address = model.Address;

                //بيانات الرقم القومي والقيد
                Oldmodel.NumberId = model.NumberId;
                Oldmodel.BirthDate = model.BirthDate.ToString();
                Oldmodel.Day = model.Day;
                Oldmodel.Month = model.Month;
                Oldmodel.Year = model.Year;
                Oldmodel.CityId = model.CityId;
                Oldmodel.GenderId = (int)model.GenderId;
                Oldmodel.RegistrationStateId = model.RegistrationStateId;
                Oldmodel.RegistrationNum = model.RegistrationNum;
                Oldmodel.RegistrationDate = model.RegistrationDate;

                //بيانات ولي الامر
                Oldmodel.ParentName = model.ParentName;
                Oldmodel.ParentAddress = model.ParentAddress;
                Oldmodel.ParentJob = model.ParentJob;
                Oldmodel.ParentPhone = model.ParentPhone;
                Oldmodel.ParentWhatsApp = model.ParentWhatsApp;
                Oldmodel.MotherName = model.MotherName;
                Oldmodel.MotherPhone = model.MotherPhone;
                Oldmodel.SocialStateId = model.SocialStateId;

                //بيانات خاصة بالثانوي
                Oldmodel.TabletSerialNumber = model.TabletSerialNumber;
                Oldmodel.ScienceDivisionId = model.ScienceDivisionId;
                Oldmodel.IM = model.IM;
                Oldmodel.SecondLanguageId = model.SecondLanguageId;
                Oldmodel.DateOfReceipt = model.DateOfReceipt;
                Oldmodel.InsurancePolicyNumber = model.InsurancePolicyNumber;
                Oldmodel.InsurancePolicyDate = model.InsurancePolicyDate;

                //لائحة التحفيز والانضباط
                Oldmodel.ProtectionCommittee = model.ProtectionCommittee;
                Oldmodel.FromDate = model.FromDate;
                Oldmodel.ToDate = model.ToDate;
                Oldmodel.Duration = model.Duration;
                Oldmodel.Problem = model.Problem;
                Oldmodel.ActionTaken = model.ActionTaken;

                //المواهب والمسابقات
                Oldmodel.Competitions = model.Competitions;
                Oldmodel.Talents = model.Talents;
                Oldmodel.Notes = model.Notes;
                if (model.Image != null)
                {
                    Oldmodel.Image = model.Image;
                }

                Oldmodel.SchoolId = model.SchoolId;

                dbContext.SaveChanges();
                result.IsSuccess = true;
                result.Message = "تم تعديل البيانات بنجاح";
                return result;
            }
        }
        public ResultDto<Student> Delete(Guid Id, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Student>();
                var Oldmodel = dbContext.Students.Find(Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "هذا الطالب غير موجود ";
                    return result;
                }

                Oldmodel.IsDeleted = true;
                Oldmodel.DeletedOn = DateTime.UtcNow;
                Oldmodel.DeletedBy = UserId;
                dbContext.SaveChanges();
                result.IsSuccess = true;
                result.Message = "تم حذف البيانات بنجاح";
                return result;
            }
        }

        public ResultDto<Student> Transfered(Guid Id, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Student>();
                var Oldmodel = dbContext.Students.Find(Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "هذا الطالب غير موجود ";
                    return result;
                }

                Oldmodel.StudentTypeId = (int)StudentTypes.TransferFromSchool;
                Oldmodel.ModifiedOn = DateTime.UtcNow;
                Oldmodel.ModifiedBy = UserId;
                dbContext.SaveChanges();
                result.IsSuccess = true;
                result.Message = "تم حذف البيانات بنجاح";
                return result;
            }
        }
        public ResultDto<Student> TrackConverterd(Guid Id, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Student>();
                var Oldmodel = dbContext.Students.Find(Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "هذا الطالب غير موجود ";
                    return result;
                }

                Oldmodel.StudentTypeId = (int)StudentTypes.TrackConverters;
                Oldmodel.ModifiedOn = DateTime.UtcNow;
                Oldmodel.ModifiedBy = UserId;
                dbContext.SaveChanges();
                result.IsSuccess = true;
                result.Message = "تم حذف البيانات بنجاح";
                return result;
            }
        }

        public ResultDto<Student> Graduated(Guid Id, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Student>();
                var Oldmodel = dbContext.Students.Find(Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "هذا الطالب غير موجود ";
                    return result;
                }

                Oldmodel.StudentTypeId = (int)StudentTypes.Graduates;
                Oldmodel.ModifiedOn = DateTime.UtcNow;
                Oldmodel.ModifiedBy = UserId;
                dbContext.SaveChanges();
                result.IsSuccess = true;
                result.Message = "تم حذف البيانات بنجاح";
                return result;
            }
        }
    }
}