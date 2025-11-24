using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentAffairs.Dtos.StudentsAttendance
{
    public class StudentsAttendanceDto
    {
        public Guid Id { get; set; }
       
        public Guid? LevelId { get; set; }
        public string LevelName { get; set; }
       
        public Guid? ClassId { get; set; }
        public string ClassName { get; set; }
        public Guid? StudentId { get; set; }
        public string Code { get; set; }
        public string SerialNumber { get; set; }

        public string StudentName { get; set; }
        public bool IsAttend { get; set; }
        public string NumAttend { get; set; }
        public string NumAllAttend { get; set; }
        public DateTime Date { get; set; }
        //public List<StudentsAttendanceDaysDto> Days { get; set; }

    }
    public class StudentsAttendanceDaysDto
    {
        public string Date { get; set; }
        public bool IsAttend { get; set; }

    }
}