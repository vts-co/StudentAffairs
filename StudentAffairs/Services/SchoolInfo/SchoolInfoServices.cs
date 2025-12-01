using StudentAffairs.Dtos.SchoolInfo;
using StudentAffairs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentAffairs.Services.SchoolInfo
{
    public class SchoolInfoServices
    {
        public List<SchoolInfoDto> GetAll()
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.SchoolInfoes.Where(x => x.IsDeleted == false)
                    .OrderBy(x => x.CreatedOn).Select(x => new SchoolInfoDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        CityId = x.CityId != null ? (Guid)x.CityId : Guid.Empty,
                        CityName = x.CityId != null ? x.City.Name : "",
                        CityDepartmentId = x.DepartmentId != null ? (Guid)x.DepartmentId : Guid.Empty,
                        CityDepartmentName = x.DepartmentId != null ? x.CityDepartment.Name : "",

                        Image = x.Image,
                        SchoolPrincipal = x.SchoolPrincipal,
                        Administration = x.Administration,
                        StudentAffairsOfficer = x.StudentAffairsOfficer,
                        StudyYear = x.StudyYear,
                        SocialWorker = x.SocialWorker,
                        Notes = x.Notes
                    }).ToList();
                return model;
            }
        }
        public SchoolInfoDto Get(Guid Id)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.SchoolInfoes.Where(x => x.IsDeleted == false && x.Id == Id)
                    .OrderBy(x => x.CreatedOn).Select(x => new SchoolInfoDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        CityId = x.CityId != null ? (Guid)x.CityId : Guid.Empty,
                        CityName = x.CityId != null ? x.City.Name : "",
                        CityDepartmentId = x.DepartmentId != null ? (Guid)x.DepartmentId : Guid.Empty,
                        CityDepartmentName = x.DepartmentId != null ? x.CityDepartment.Name : "",

                        Image = x.Image,
                        SchoolPrincipal = x.SchoolPrincipal,
                        Administration = x.Administration,
                        StudentAffairsOfficer = x.StudentAffairsOfficer,
                        SocialWorker = x.SocialWorker,
                        StudyYear = x.StudyYear,
                        Notes = x.Notes
                    }).FirstOrDefault();
                return model;
            }
        }

        public ResultDto<SchoolInfoDto> Create(SchoolInfoDto model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<SchoolInfoDto>();
                var Oldmodel = dbContext.SchoolInfoes.Where(x => x.Name == model.Name && x.IsDeleted == false).FirstOrDefault();
                if (Oldmodel != null)
                {
                    result.Result = model;
                    result.IsSuccess = false;
                    result.Message = "يوجد بيانات مدرسة بالفعل";
                    return result;
                }

                var result1 = new StudentAffairs.Models.SchoolInfo();
                result1.Id = model.Id;
                result1.Name = model.Name;
                if (model.CityId == Guid.Empty)
                    result1.CityId = null;
                else
                    result1.CityId = model.CityId;

                if (model.CityDepartmentId == Guid.Empty)
                    result1.DepartmentId = null;
                else
                    result1.DepartmentId = model.CityDepartmentId;

               
                result1.Image = model.Image;
                result1.SchoolPrincipal = model.SchoolPrincipal;
                result1.Administration = model.Administration;
                result1.StudentAffairsOfficer = model.StudentAffairsOfficer;
                result1.StudyYear = model.StudyYear;
                result1.SocialWorker = model.SocialWorker;
                result1.Notes = model.Notes;

                result1.CreatedOn = DateTime.UtcNow;
                result1.CreatedBy = UserId;
                result1.IsDeleted = false;
                dbContext.SchoolInfoes.Add(result1);
                dbContext.SaveChanges();
                result.IsSuccess = true;
                result.Message = "تم حفظ البيانات بنجاح";
                return result;
            }
        }
        public ResultDto<SchoolInfoDto> Edit(SchoolInfoDto model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<SchoolInfoDto>();
                var Oldmodel = dbContext.SchoolInfoes.Find(model.Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "بيانات المدرسة غير موجودة ";
                    return result;
                }

                Oldmodel.Id = model.Id;
                Oldmodel.Name = model.Name;
                if (model.CityId == Guid.Empty)
                    Oldmodel.CityId = null;
                else
                    Oldmodel.CityId = model.CityId;

                if (model.CityDepartmentId == Guid.Empty)
                    Oldmodel.DepartmentId = null;
                else
                    Oldmodel.DepartmentId = model.CityDepartmentId;

                Oldmodel.Image = model.Image;
                Oldmodel.SchoolPrincipal = model.SchoolPrincipal;
                Oldmodel.Administration = model.Administration;
                Oldmodel.StudentAffairsOfficer = model.StudentAffairsOfficer;
                Oldmodel.StudyYear = model.StudyYear;
                Oldmodel.SocialWorker = model.SocialWorker;
                Oldmodel.Notes = model.Notes;

                Oldmodel.ModifiedOn = DateTime.UtcNow;
                Oldmodel.ModifiedBy = UserId;


                dbContext.SaveChanges();
                result.IsSuccess = true;
                result.Message = "تم تعديل البيانات بنجاح";
                return result;
            }
        }
        public ResultDto<SchoolInfoDto> Delete(Guid Id, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<SchoolInfoDto>();
                var Oldmodel = dbContext.SchoolInfoes.Find(Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "بيانات المدرسة غير موجودة ";
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
    }
}