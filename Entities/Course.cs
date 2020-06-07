using System;


namespace ENTITIES_NS
{
    public class Course
    {
        private int courseID;
        private string title;
        private string stream;
        private string type;
        private DateTime? startdate;
        private DateTime? enddate;
        public int CourseID { get => courseID; set => courseID = value; }
        public string Title { get => title; set => title = value; }
        public string Stream { get => stream; set => stream = value; }
        public string Type { get => type; set => type = value; }
        public DateTime? Startdate { get => startdate; set => startdate = value; }
        public DateTime? Enddate { get => enddate; set => enddate = value; }


        public Course(int courseID, string title, string stream, string type, DateTime? startdate, DateTime? enddate)
        {
            CourseID = courseID;
            Title = title;
            Stream = stream;
            Type = type;
            Startdate = startdate;
            Enddate = enddate;
        }
        public void Output()
        {
            Console.WriteLine($"CourseID: {courseID,-3} TITLE: {Title,-10} STREAM: {stream,-10}" +
            $"TYPE: {type,-10} STARTDATE: {startdate,-10:dd/MM/yyyy}" +
            $"ENDDATE: {enddate,-10:dd/MM/yyyy}");
        }
    }
}
