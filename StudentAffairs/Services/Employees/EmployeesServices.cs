using StudentAffairs.Dtos.Employees;
using StudentAffairs.Enums;
using StudentAffairs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentAffairs.Services.Employees
{
    public class EmployeesServices
    {
        public List<EmployeesDto> GetAll(Guid UserId, Guid SchoolId, Guid EmployeeId, Role RoleId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.Employees.Where(x => x.IsDeleted == false && (x.CreatedBy == UserId || x.SchoolId == SchoolId || x.SchoolInfo.Employees.Any(y => !y.IsDeleted && y.Id == EmployeeId) || RoleId == Role.Super_Admin)).OrderBy(x => x.CreatedOn).Select(x=>new EmployeesDto {
                Id=x.Id,
                Name=x.Name,
                JobId=x.JobId!=null?(Guid)x.JobId:Guid.Empty,
                JobName=x.JobId!=null?x.Job.Name:"",
                Phone=x.Phone,
                NumberId=x.NumberId,
                Notes=x.Notes
                }).ToList();
                return model;
            }
        }
        public Employee Get(Guid Id)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var model = dbContext.Employees.Where(x => x.IsDeleted == false && x.Id == Id).OrderBy(x => x.CreatedOn).FirstOrDefault();
                return model;
            }
        }
        
        public ResultDto<Employee> Create(Employee model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Employee>();
                var Oldmodel = dbContext.Employees.Where(x => x.Name == model.Name&&x.SchoolId==model.SchoolId && x.IsDeleted == false).FirstOrDefault();
                if (Oldmodel != null)
                {
                    result.Result = Oldmodel;
                    result.IsSuccess = false;
                    result.Message = "هذا الموظف موجود بالفعل";
                    return result;
                }
              
                model.CreatedOn = DateTime.UtcNow;
                model.CreatedBy = UserId;
                model.IsDeleted = false;
                dbContext.Employees.Add(model);
                dbContext.SaveChanges();
                result.IsSuccess = true;
                result.Message = "تم حفظ البيانات بنجاح";
                return result;
            }
        }
        public ResultDto<Employee> Edit(Employee model, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Employee>();
                var Oldmodel = dbContext.Employees.Find(model.Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "هذا الموظف موجود بالفعل";
                    return result;
                }
             
                Oldmodel.ModifiedOn = DateTime.UtcNow;
                Oldmodel.ModifiedBy = UserId;
               
                Oldmodel.Name = model.Name;
                Oldmodel.Phone = model.Phone;
                Oldmodel.JobId = model.JobId;
                Oldmodel.NumberId = model.NumberId;
               
                Oldmodel.Notes = model.Notes;
                
                dbContext.SaveChanges();
                result.IsSuccess = true;
                result.Message = "تم تعديل البيانات بنجاح";
                return result;
            }
        }
        public ResultDto<Employee> Delete(Guid Id, Guid UserId)
        {
            using (var dbContext = new StudentAffairsEntities())
            {
                var result = new ResultDto<Employee>();
                var Oldmodel = dbContext.Employees.Find(Id);
                if (Oldmodel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "هذا الموظف غير موجود ";
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