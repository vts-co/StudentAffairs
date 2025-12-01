using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentAffairs.Dtos.Classes
{
    public class ClassesDto
    {
        public Guid Id { get; set; }
        public Guid SchoolId { get; set; }
        public string SchoolName { get; set; }
        public Guid LevelId { get; set; }
        public string LevelName { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
    }
}