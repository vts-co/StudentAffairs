using StudentAffairs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentAffairs.Services.Nationalities
{
    public class NationalitiesServices
    {
        public List<Models.Nationality> GetAll()
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.Nationalities.Where(x => x.IsDeleted == false).OrderBy(x => x.CreatedOn).ToList();
                return model;
            }
        }

        public ResultDto<Models.Nationality> Create(Models.Nationality model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Models.Nationality>();
                var Oldmodel = dbContext.Nationalities.Where(x => x.Name == model.Name && x.IsDeleted == false).FirstOrDefault();
                if (Oldmodel != null)
                {
                    result.Result = Oldmodel;
                    result.IsSuccess = false;
                    result.Message = "هذه الجنسية موجودة بالفعل";
                    return result;
                }
                model.CreatedOn = DateTime.UtcNow;
                model.CreatedBy = UserId;
                model.IsDeleted = false;
                dbContext.Nationalities.Add(model);
                dbContext.SaveChanges();
                result.IsSuccess = true;
                result.Message = "تم حفظ البيانات بنجاح";
                return result;
            }
        }
        public ResultDto<Models.Nationality> Edit(Models.Nationality model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Models.Nationality>();
                var Oldmodel = dbContext.Nationalities.Find(model.Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "هذه الجنسية غير موجودة ";
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
        public ResultDto<Models.Nationality> Delete(Guid Id, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Models.Nationality>();
                var Oldmodel = dbContext.Nationalities.Find(Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "هذه الجنسية غير موجودة ";
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