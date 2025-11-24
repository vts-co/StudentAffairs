using StudentAffairs.Controllers;
using StudentAffairs.Dtos.StudentsAttendance;
using StudentAffairs.Enums;
using StudentAffairs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentAffairs.Services.StudentsAttendance
{
    public class StudentsAttendanceServices
    {
        public List<StudentsAttendanceDto> GetAllByFilters(Guid UserId, Guid SchoolId, Guid EmployeeId, Role RoleId, Guid? SchoolId1, DateTime? dateFrom, DateTime? dateTo)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var data1 = dbContext.StudentsAttendances.Where(x => x.IsDeleted == false && x.Student.SchoolId == SchoolId1 && x.AttendDate >= dateFrom && x.AttendDate <= dateTo && (x.CreatedBy == UserId || x.Student.SchoolId == SchoolId || RoleId == Role.Super_Admin)).OrderBy(x => x.CreatedOn)
                    .Select(x => new StudentsAttendanceDto
                    {
                        Id = x.Id,
                        SerialNumber = x.Student.SerialNumber,
                        Date = x.AttendDate != null ? (DateTime)x.AttendDate : DateTime.Now,
                        Code = x.Student.Code,
                        StudentId = x.StudentId,
                        StudentName = x.Student.Name,
                        ClassId = x.ClassId,
                        ClassName = x.Class.Name,
                        LevelName = x.Class.Level.Name,
                        LevelId = x.Class.LevelId,
                        IsAttend = x.AttendOrNo != null ? (bool)x.AttendOrNo : false
                    }).ToList();


                return data1;
            }
        }
        public List<StudentsAttendanceDto> GetAllByStudent(Guid UserId, Guid SchoolId, Guid EmployeeId, Role RoleId, Guid SchoolId1, Guid StudentId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var data1 = dbContext.StudentsAttendances.Where(x => x.IsDeleted == false && x.Student.SchoolId == SchoolId1 && x.StudentId == StudentId && (x.CreatedBy == UserId || x.Student.SchoolId == SchoolId || RoleId == Role.Super_Admin)).OrderBy(x => x.CreatedOn)
                    .Select(x => new StudentsAttendanceDto
                    {

                        Id = x.Id,
                        Date = x.AttendDate != null ? (DateTime)x.AttendDate : DateTime.Now,
                        Code = x.Student.Code,
                        StudentId = x.StudentId,
                        StudentName = x.Student.Name,
                        ClassId = x.ClassId,
                        ClassName = x.Class.Name,
                        LevelName = x.Class.Level.Name,
                        LevelId = x.Class.LevelId,
                        IsAttend = x.AttendOrNo != null ? (bool)x.AttendOrNo : false
                    }).ToList();
                return data1;
            }
        }


        public Models.StudentsAttendance Get(Guid Id)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.StudentsAttendances.Where(x => x.IsDeleted == false && x.Id == Id).OrderBy(x => x.CreatedOn).FirstOrDefault();
                return model;
            }
        }
        public bool GetBySchoolAndDate(Guid SchoolId, DateTime date)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.StudentsAttendances.Any(x => x.IsDeleted == false && x.Student.SchoolId == SchoolId && x.AttendDate == date);
                return model;
            }
        }
        public int Get(DateTime Date, Guid ClassId, Guid StudyYearId, Guid StudyClassId, Guid StudentId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.StudentsAttendances.Where(x => x.IsDeleted == false && x.StudentId == StudentId && x.ClassId == ClassId && x.AttendDate == Date && x.AttendOrNo == true).OrderBy(x => x.CreatedOn).ToList().Count();
                return model;
            }
        }
        public ResultDto<Models.StudentsAttendance> Create(Models.StudentsAttendance model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Models.StudentsAttendance>();

                var student = dbContext.Students.Where(x => x.IsDeleted == false && x.Id == model.StudentId).FirstOrDefault();
                var schoolId = student.SchoolId;
                var test = dbContext.StudentsAttendances.Where(x => x.IsDeleted == false && x.StudentId == model.StudentId && x.AttendDate == model.AttendDate).FirstOrDefault();

                if (test != null)
                {
                    test.AttendOrNo = model.AttendOrNo;
                    dbContext.SaveChanges();
                    result.IsSuccess = true;
                    result.Message = "تم تعديل البيانات بنجاح";
                    return result;
                }
                //if (test.Count() > 0)
                //{
                //    result.IsSuccess = false;
                //    result.Message = "تم اخذ الغياب لهذا اليوم";
                //    return result;
                //}

                var id = Guid.NewGuid();
                model.Id = id;

                model.CreatedOn = DateTime.UtcNow;
                model.CreatedBy = UserId;
                model.IsDeleted = false;

                dbContext.StudentsAttendances.Add(model);
                dbContext.SaveChanges();


                result.IsSuccess = true;
                result.Message = "تم حفظ البيانات بنجاح";
                return result;
            }
        }
        public ResultDto<List<Models.StudentsAttendance>> CreateAll(Guid SchoolId, string AttendDate, string AttendOrNo, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<List<Models.StudentsAttendance>>();
                var students = dbContext.Students.Where(x => x.IsDeleted == false && x.SchoolId == SchoolId).ToList();
                var date = DateTime.Parse(AttendDate);
                var count = 0;
                foreach (var item in students)
                {
                    var test = dbContext.StudentsAttendances.Where(x => x.IsDeleted == false && x.StudentId == item.Id && x.AttendDate == date).FirstOrDefault();

                    if (test == null)
                    {
                        var model = new Models.StudentsAttendance();

                        var id = Guid.NewGuid();
                        model.Id = id;
                        model.StudentId = item.Id;
                        model.LevelId = item.LevelId;
                        model.ClassId = item.ClassId;
                        model.AttendDate = date;
                        if (AttendOrNo == "true")
                            model.AttendOrNo = false;
                        else
                            model.AttendOrNo = true;


                        model.CreatedOn = DateTime.UtcNow;
                        model.CreatedBy = UserId;
                        model.IsDeleted = false;

                        dbContext.StudentsAttendances.Add(model);
                        dbContext.SaveChanges();
                        count += 1;
                    }
                }

                //if (test.Count() > 0)
                //{
                //    result.IsSuccess = false;
                //    result.Message = "تم اخذ الغياب لهذا اليوم";
                //    return result;
                //}



                var att = "حضور";
                if (AttendOrNo == "true")
                    att = "غياب";
                result.IsSuccess = true;
                result.Message = "تم " + att + " عدد " + count + " طلاب بنجاح ";
                return result;
            }
        }
        public ResultDto<Models.StudentsAttendance> Edit(Guid Id, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Models.StudentsAttendance>();
                var Oldmodel = dbContext.StudentsAttendances.Find(Id);

                if (Oldmodel.AttendOrNo == true)
                    Oldmodel.AttendOrNo = false;
                else
                    Oldmodel.AttendOrNo = true;

                Oldmodel.ModifiedOn = DateTime.UtcNow;
                Oldmodel.ModifiedBy = UserId;
                dbContext.SaveChanges();

                result.IsSuccess = true;
                result.Message = "تم تعديل البيانات بنجاح";
                return result;
            }
        }
        public ResultDto<Models.StudentsAttendance> Delete(Guid Id, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Models.StudentsAttendance>();
                var Oldmodel = dbContext.StudentsAttendances.Find(Id);

                Oldmodel.IsDeleted = true;
                Oldmodel.DeletedOn = DateTime.UtcNow;
                Oldmodel.DeletedBy = UserId;
                dbContext.SaveChanges();

                result.IsSuccess = true;
                result.Message = "تم حذف البيانات بنجاح";
                return result;
            }
        }

        public ResultDto<Models.StudentsAttendance> DeleteAll(Guid SchoolId, DateTime DateFrom, DateTime DateTo, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Models.StudentsAttendance>();

                var model = dbContext.StudentsAttendances.Where(x => x.IsDeleted == false && x.Student.SchoolId == SchoolId && x.AttendDate >= DateFrom&&x.AttendDate<= DateTo).ToList();
                foreach (var item in model)
                {
                    item.IsDeleted = true;
                    item.DeletedOn = DateTime.UtcNow;
                    item.DeletedBy = UserId;
                    dbContext.SaveChanges();
                }

                result.IsSuccess = true;
                result.Message = "تم حذف البيانات بنجاح";
                return result;
            }
        }

    }
}