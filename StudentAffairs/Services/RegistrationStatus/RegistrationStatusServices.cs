using StudentAffairs.Enums;
using StudentAffairs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentAffairs.Services.RegistrationStatus
{
    public class RegistrationStatusServices
    {
        public List<Models.RegistrationStatu> GetAll(Guid UserId, Guid SchoolId, Guid EmployeeId, Role RoleId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.RegistrationStatus.Where(x => x.IsDeleted == false /*&& (x.CreatedBy == UserId || x.SchoolId == SchoolId || x.SchoolInfo.Employees.Any(y => !y.IsDeleted && y.Id == EmployeeId) || RoleId == Role.Super_Admin)*/).OrderBy(x => x.CreatedOn).ToList();
                return model;
            }
        }

        public ResultDto<Models.RegistrationStatu> Create(Models.RegistrationStatu model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Models.RegistrationStatu>();
                var Oldmodel = dbContext.RegistrationStatus.Where(x => x.Name == model.Name && x.IsDeleted == false).FirstOrDefault();
                if (Oldmodel != null)
                {
                    result.Result = Oldmodel;
                    result.IsSuccess = false;
                    result.Message = "حالة القيد موجودة بالفعل";
                    return result;
                }
                model.CreatedOn = DateTime.UtcNow;
                model.CreatedBy = UserId;
                model.IsDeleted = false;
                dbContext.RegistrationStatus.Add(model);
                dbContext.SaveChanges();
                result.IsSuccess = true;
                result.Message = "تم حفظ البيانات بنجاح";
                return result;
            }
        }
        public ResultDto<Models.RegistrationStatu> Edit(Models.RegistrationStatu model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Models.RegistrationStatu>();
                var Oldmodel = dbContext.RegistrationStatus.Find(model.Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "حالة القيد غير موجودة ";
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
        public ResultDto<Models.RegistrationStatu> Delete(Guid Id, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Models.RegistrationStatu>();
                var Oldmodel = dbContext.RegistrationStatus.Find(Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "حالة القيد غير موجودة ";
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