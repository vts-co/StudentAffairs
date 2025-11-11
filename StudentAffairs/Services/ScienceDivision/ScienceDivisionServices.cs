using StudentAffairs.Enums;
using StudentAffairs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentAffairs.Services.ScienceDivision
{
    public class ScienceDivisionServices
    {
        public List<Models.ScienceDivision> GetAll(Guid UserId, Guid SchoolId, Guid EmployeeId, Role RoleId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.ScienceDivisions.Where(x => x.IsDeleted == false && (x.CreatedBy == UserId || x.SchoolId == SchoolId || x.SchoolInfo.Employees.Any(y => !y.IsDeleted && y.Id == EmployeeId) || RoleId == Role.Super_Admin)).OrderBy(x => x.CreatedOn).ToList();
                return model;
            }
        }

        public ResultDto<Models.ScienceDivision> Create(Models.ScienceDivision model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Models.ScienceDivision>();
                var Oldmodel = dbContext.ScienceDivisions.Where(x => x.Name == model.Name && x.IsDeleted == false).FirstOrDefault();
                if (Oldmodel != null)
                {
                    result.Result = Oldmodel;
                    result.IsSuccess = false;
                    result.Message = "هذه الشعبة موجودة بالفعل";
                    return result;
                }
                model.CreatedOn = DateTime.UtcNow;
                model.CreatedBy = UserId;
                model.IsDeleted = false;
                dbContext.ScienceDivisions.Add(model);
                dbContext.SaveChanges();
                result.IsSuccess = true;
                result.Message = "تم حفظ البيانات بنجاح";
                return result;
            }
        }
        public ResultDto<Models.ScienceDivision> Edit(Models.ScienceDivision model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Models.ScienceDivision>();
                var Oldmodel = dbContext.ScienceDivisions.Find(model.Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "هذه الشعبة غير موجودة ";
                    return result;
                }
                Oldmodel.ModifiedOn = DateTime.UtcNow;
                Oldmodel.ModifiedBy = UserId;
                Oldmodel.Name = model.Name;
                Oldmodel.Notes = model.Notes;

                dbContext.SaveChanges();
                result.IsSuccess = true;
                result.Message = "تم تعديل البيانات بنجاح";
                return result;
            }
        }
        public ResultDto<Models.ScienceDivision> Delete(Guid Id, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Models.ScienceDivision>();
                var Oldmodel = dbContext.ScienceDivisions.Find(Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "هذه الشعبة غير موجودة ";
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