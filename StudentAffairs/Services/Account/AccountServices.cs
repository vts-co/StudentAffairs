using StudentAffairs.Authorization;
using StudentAffairs.Enums;
using StudentAffairs.Models;
using StudentAffairs.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentAffairs.Services
{
    public class AccountServices
    {
        public ResultDto<UserInfo> Login(string userName, string password)
        {
            var result = new ResultDto<UserInfo>();
            using (var dbContext = new StudentAffairsEntities())
            {
                if (userName == null || password == null)
                {
                    result.IsSuccess = false;
                    result.Message = "اسم المستخدم او كلمة المرور غير صحيحة";
                    return result;
                }
                var pass = Security.Encrypt(password);
                var user = dbContext.Users.Where(x => x.Username == userName && x.Password == pass && x.IsDeleted == false).FirstOrDefault();
                if (user == null)
                {
                    result.IsSuccess = false;
                    result.Message = "اسم المستخدم او كلمة المرور غير صحيحة";
                    return result;
                }

                result.Message = "تم تسجيل الدخول بنجاح";
                result.IsSuccess = true;
                result.Result = new UserInfo()
                {
                    RoleId = (Role)user.RoleId,
                    UserId = user.Id,
                    SchoolId= user.SchoolId!=null?(Guid)user.SchoolId:Guid.Empty,
                    SchoolName= user.SchoolId != null ? user.SchoolInfo.Name:"",
                    //EmployeeId = user.EmployeeId != null ? user.EmployeeId.Value : Guid.Empty,
                    //EmployeeName = user.EmployeeId != null ? user.Employee.Name : "Admin",

                    UserName = user.Username,
                    UserScreens = user.UserScreens
                };
            }
            return result;
        }
    }
}