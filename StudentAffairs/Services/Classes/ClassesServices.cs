using StudentAffairs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentAffairs.Services.Classes
{
    public class ClassesServices
    {
        public List<Class> GetAll()
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.Classes.Where(x => x.IsDeleted == false).OrderBy(x => x.CreatedOn).ToList();
                return model;
            }
        }

        public ResultDto<Class> Create(Class model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Class>();
                var Oldmodel = dbContext.Classes.Where(x => x.Name == model.Name && x.IsDeleted == false).FirstOrDefault();
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