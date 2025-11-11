using StudentAffairs.Enums;
using StudentAffairs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentAffairs.Services.Jobs
{
    public class JobsServices
    {
        public List<Job> GetAll(Guid UserId, Guid SchoolId, Guid EmployeeId, Role RoleId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.Jobs.Where(x => x.IsDeleted == false && (x.CreatedBy == UserId || x.SchoolId == SchoolId || x.SchoolInfo.Employees.Any(y => !y.IsDeleted && y.Id == EmployeeId) || RoleId == Role.Super_Admin)).OrderBy(x => x.CreatedOn).ToList();
                return model;
            }
        }
        public Job Get(Guid Id)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.Jobs.Where(x => x.IsDeleted == false && x.Id == Id).OrderBy(x => x.CreatedOn).FirstOrDefault();
                return model;
            }
        }
        public ResultDto<Job> Create(Job model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Job>();
                var Oldmodel = dbContext.Jobs.Where(x => x.Name == model.Name && x.SchoolId == model.SchoolId && x.IsDeleted == false).FirstOrDefault();
                if (Oldmodel != null)
                {
                    result.Result = Oldmodel;
                    result.IsSuccess = false;
                    result.Message = "هذه الوظيفة موجودة بالفعل";
                    return result;
                }
                model.CreatedOn = DateTime.UtcNow;
                model.CreatedBy = UserId;
                model.IsDeleted = false;
                dbContext.Jobs.Add(model);
                dbContext.SaveChanges();
                result.IsSuccess = true;
                result.Message = "تم حفظ البيانات بنجاح";
                return result;
            }
        }
        public ResultDto<Job> Edit(Job model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Job>();
                var Oldmodel = dbContext.Jobs.Find(model.Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "هذه الوظيفة موجودة بالفعل";
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
        public ResultDto<Job> Delete(Guid Id, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Job>();
                var Oldmodel = dbContext.Jobs.Find(Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "هذه الوظيفة غير موجودة ";
                    return result;
                }

                if (Oldmodel.Employees.Any(y => y.IsDeleted == false))
                {
                    result.IsSuccess = false;
                    result.Message = "هذه الوظيفة بيها موظفين لا يمكن حذفها ";
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