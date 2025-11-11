using StudentAffairs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentAffairs.Services.EndYearResults
{
    public class EndYearResultsServices
    {
        public List<Models.EndYearResult> GetAll()
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.EndYearResults.Where(x => x.IsDeleted == false).OrderBy(x => x.CreatedOn).ToList();
                return model;
            }
        }

        public ResultDto<Models.EndYearResult> Create(Models.EndYearResult model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Models.EndYearResult>();
                var Oldmodel = dbContext.EndYearResults.Where(x => x.Name == model.Name && x.IsDeleted == false).FirstOrDefault();
                if (Oldmodel != null)
                {
                    result.Result = Oldmodel;
                    result.IsSuccess = false;
                    result.Message = "هذه النتيجة موجودة بالفعل";
                    return result;
                }
                model.CreatedOn = DateTime.UtcNow;
                model.CreatedBy = UserId;
                model.IsDeleted = false;
                dbContext.EndYearResults.Add(model);
                dbContext.SaveChanges();
                result.IsSuccess = true;
                result.Message = "تم حفظ البيانات بنجاح";
                return result;
            }
        }
        public ResultDto<Models.EndYearResult> Edit(Models.EndYearResult model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Models.EndYearResult>();
                var Oldmodel = dbContext.EndYearResults.Find(model.Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "هذه النتيجة غير موجودة ";
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
        public ResultDto<Models.EndYearResult> Delete(Guid Id, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Models.EndYearResult>();
                var Oldmodel = dbContext.EndYearResults.Find(Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "هذه النتيجة غير موجودة ";
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