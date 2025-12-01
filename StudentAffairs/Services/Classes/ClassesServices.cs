using StudentAffairs.Dtos.Classes;
using StudentAffairs.Enums;
using StudentAffairs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentAffairs.Services.Classes
{
    public class ClassesServices
    {
        public List<ClassesDto> GetAll(Guid UserId, Guid SchoolId, Guid EmployeeId, Role RoleId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.Classes.Where(x => x.IsDeleted == false && (x.CreatedBy == UserId || x.SchoolId == SchoolId || x.SchoolInfo.Employees.Any(y => !y.IsDeleted && y.Id == EmployeeId) || RoleId == Role.Super_Admin)).OrderBy(x => x.CreatedOn).Select(x => new ClassesDto
                {

                    Id = x.Id,
                    Name = x.Name,
                    LevelId = x.LevelId != null ? (Guid)x.LevelId : Guid.Empty,
                    LevelName = x.LevelId != null ? x.Level.Name : "",
                    SchoolId = x.SchoolId != null ? (Guid)x.SchoolId : Guid.Empty,
                    SchoolName = x.SchoolId != null ? x.SchoolInfo.Name : "",
                    Notes=x.Notes

                }).ToList();
                return model;
            }
        }
        public Class Get(Guid Id)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.Classes.Where(x => x.IsDeleted == false && x.Id==Id).OrderBy(x => x.CreatedOn).FirstOrDefault();
                return model;
            }
        }
        public ResultDto<Class> Create(Class model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Class>();
                var Oldmodel = dbContext.Classes.Where(x => x.Name == model.Name&&x.LevelId==model.LevelId&&x.SchoolId==model.SchoolId && x.IsDeleted == false).FirstOrDefault();
                if (Oldmodel != null)
                {
                    result.Result = Oldmodel;
                    result.IsSuccess = false;
                    result.Message = "هذا الفصل موجود بالفعل";
                    return result;
                }
                model.CreatedOn = DateTime.UtcNow;
                model.CreatedBy = UserId;
                model.IsDeleted = false;
                dbContext.Classes.Add(model);
                dbContext.SaveChanges();
                result.IsSuccess = true;
                result.Message = "تم حفظ البيانات بنجاح";
                return result;
            }
        }
        public ResultDto<Class> Edit(Class model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Class>();
                var Oldmodel = dbContext.Classes.Find(model.Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "هذا الفصل غير موجود ";
                    return result;
                }
                var Oldmodel1 = dbContext.Classes.Where(x => x.Name == model.Name && x.LevelId == model.LevelId && x.SchoolId == model.SchoolId&&x.Id!=model.Id && x.IsDeleted == false).FirstOrDefault();
                if (Oldmodel1 != null)
                {
                    result.Result = Oldmodel1;
                    result.IsSuccess = false;
                    result.Message = "هذا الفصل موجود بالفعل";
                    return result;
                }
                Oldmodel.ModifiedOn = DateTime.UtcNow;
                Oldmodel.ModifiedBy = UserId;
                Oldmodel.Name = model.Name;
                Oldmodel.LevelId = model.LevelId;
                Oldmodel.Notes = model.Notes;

                dbContext.SaveChanges();
                result.IsSuccess = true;
                result.Message = "تم تعديل البيانات بنجاح";
                return result;
            }
        }
        public ResultDto<Class> Delete(Guid Id, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Class>();
                var Oldmodel = dbContext.Classes.Find(Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "هذا الفصل غير موجود ";
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