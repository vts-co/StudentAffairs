using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentAffairs.Dtos.SchoolInfo
{
    public class SchoolInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public Guid CityId { get; set; }
        public string CityName { get; set; }
        public string SchoolPrincipal { get; set; }
        public string Administration { get; set; }
        public string StudentAffairsOfficer { get; set; }
        public string StudyYear { get; set; }
        public string Notes { get; set; }
    }
}