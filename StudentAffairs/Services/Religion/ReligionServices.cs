using StudentAffairs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentAffairs.Services.Religion
{
    public class ReligionServices
    {
        public List<Models.Religion> GetAll()
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.Religions.Where(x => x.IsDeleted == false).OrderBy(x => x.CreatedOn).ToList();
                return model;
            }
        }

        public ResultDto<Models.Religion> Create(Models.Religion model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Models.Religion>();
                var Oldmodel = dbContext.Religions.Where(x => x.Name == model.Name && x.IsDeleted == false).FirstOrDefault();
                if (Oldmodel != null)
                {
                    result.Result = Oldmodel;
                    result.IsSuccess = false;
                    result.Message = "هذه الديانة موجودة بالفعل";
                    return result;
                }
                model.CreatedOn = DateTime.UtcNow;
                model.CreatedBy = UserId;
                model.IsDeleted = false;
                dbContext.Religions.Add(model);
                dbContext.SaveChanges();
                result.IsSuccess = true;
                result.Message = "تم حفظ البيانات بنجاح";
                return result;
            }
        }
        public ResultDto<Models.Religion> Edit(Models.Religion model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Models.Religion>();
                var Oldmodel = dbContext.Religions.Find(model.Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "هذه الديانة غير موجودة ";
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
        public ResultDto<Models.Religion> Delete(Guid Id, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Models.Religion>();
                var Oldmodel = dbContext.Religions.Find(Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "هذه الديانة غير موجودة ";
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