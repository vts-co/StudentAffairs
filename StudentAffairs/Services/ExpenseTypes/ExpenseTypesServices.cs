using StudentAffairs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentAffairs.Services.ExpenseTypes
{
    public class ExpenseTypesServices
    {
        public List<ExpenseType> GetAll()
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.ExpenseTypes.Where(x => x.IsDeleted == false).OrderBy(x => x.CreatedOn).ToList();
                return model;
            }
        }

        public ResultDto<ExpenseType> Create(ExpenseType model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<ExpenseType>();
                var Oldmodel = dbContext.ExpenseTypes.Where(x => x.Name == model.Name && x.IsDeleted == false).FirstOrDefault();
                if (Oldmodel != null)
                {
                    result.Result = Oldmodel;
                    result.IsSuccess = false;
                    result.Message = "نوع المصروف موجود بالفعل";
                    return result;
                }
                model.CreatedOn = DateTime.UtcNow;
                model.CreatedBy = UserId;
                model.IsDeleted = false;
                dbContext.ExpenseTypes.Add(model);
                dbContext.SaveChanges();
                result.IsSuccess = true;
                result.Message = "تم حفظ البيانات بنجاح";
                return result;
            }
        }
        public ResultDto<ExpenseType> Edit(Level model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<ExpenseType>();
                var Oldmodel = dbContext.ExpenseTypes.Find(model.Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "نوع المصروف غير موجود ";
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
        public ResultDto<ExpenseType> Delete(Guid Id, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<ExpenseType>();
                var Oldmodel = dbContext.ExpenseTypes.Find(Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "نوع المصروف غير موجود ";
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