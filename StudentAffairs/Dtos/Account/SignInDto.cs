using StudentAffairs.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentAffairs.Dtos.Account
{
    public class SignInDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Role RoleId { get; set; } = Role.SystemAdmin;
    }
}