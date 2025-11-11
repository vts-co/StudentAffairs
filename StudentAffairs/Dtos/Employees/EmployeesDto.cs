using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentAffairs.Dtos.Employees
{
    public class EmployeesDto
    {
        public Guid Id { get; set; }
    
        public Guid JobId { get; set; }
        public string JobName { get; set; }
        public string Name { get; set; }  
        public string Phone { get; set; }
        public string NumberId { get; set; }
    
        public string Notes { get; set; }
    }
}