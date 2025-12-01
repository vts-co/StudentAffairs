using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentAffairs.Dtos.Students
{
    public class StudentsDto
    {
        public System.Guid Id { get; set; }
        public string Code { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Phone { get; set; }
        public Nullable<System.Guid> LevelId { get; set; }
        public string LevelName { get; set; }
        public Nullable<System.Guid> ClassId { get; set; }
        public string ClassName { get; set; }
        public string SeatNumber { get; set; }
        public Nullable<System.Guid> ReligionId { get; set; }
        public string ReligionName { get; set; }
        public Nullable<System.Guid> NationalityId { get; set; }
        public string NationalityName { get; set; }
        public Nullable<System.Guid> ExpenseTypeId { get; set; }
        public string ExpenseTypeName { get; set; }
        public Nullable<System.Guid> ExemptionReasonId { get; set; }
        public string ExemptionReasonName { get; set; }
        public Nullable<System.Guid> EndYearResultId { get; set; }
        public string EndYearResultName { get; set; }
        public Nullable<System.Guid> SecondRoundResultId { get; set; }
        public string SecondRoundResultName { get; set; }
        public string HealthCondition { get; set; }
        public string FirstSemesterResult { get; set; }
        public Nullable<int> EducationalIntegration { get; set; }
        public Nullable<int> TransferredToTheSchool { get; set; }
        public string Address { get; set; }
        public string NumberId { get; set; }
        public string BirthDate { get; set; }
        public Nullable<int> Day { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<System.Guid> CityId { get; set; }
        public string CityName { get; set; }
        public Nullable<int> GenderId { get; set; }
        public string GenderName { get; set; }
        public Nullable<System.Guid> RegistrationStateId { get; set; }
        public string RegistrationStateName { get; set; }
        public string RegistrationNum { get; set; }
        public Nullable<System.DateTime> RegistrationDate { get; set; }
        public string ParentName { get; set; }
        public string ParentAddress { get; set; }
        public string ParentJob { get; set; }
        public string ParentPhone { get; set; }
        public string ParentWhatsApp { get; set; }
        public string MotherName { get; set; }
        public string MotherPhone { get; set; }
        public Nullable<System.Guid> SocialStateId { get; set; }
        public string SocialStateName { get; set; }
        public string TabletSerialNumber { get; set; }
        public Nullable<System.Guid> ScienceDivisionId { get; set; }
        public string ScienceDivisionName { get; set; }
        public string IM { get; set; }
        public Nullable<System.Guid> SecondLanguageIId { get; set; }
        public string SecondLanguageIName { get; set; }
        public Nullable<System.DateTime> DateOfReceipt { get; set; }
        public string InsurancePolicyNumber { get; set; }
        public Nullable<System.DateTime> InsurancePolicyDate { get; set; }
        public string ProtectionCommittee { get; set; }
        public string Duration { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string Problem { get; set; }
        public string ActionTaken { get; set; }
        public string Talents { get; set; }
        public string Competitions { get; set; }

        public Nullable<System.Guid> SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string AttendOrNot { get; set; }
        public string Notes { get; set; }
    }

    public class StudentFilesDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
      
        public string Name { get; set; }
     
        public string Notes { get; set; }
    }

}