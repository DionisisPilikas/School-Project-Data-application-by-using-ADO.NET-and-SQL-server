using System;


namespace ENTITIES_NS
{
    public class Assignment
    {
        private int assignmentID; //AssignmentID field in sqlDB is never going to be null
        private string titleofassignment;
        private string description;
        private DateTime? submissionDate;
        public int AssignmentID { get => assignmentID; set => assignmentID = value; }
        public string Titleofassignment { get => titleofassignment; set => titleofassignment = value; }
        public string Description { get => description; set => description = value; }
        public DateTime? SubmissionDate { get => submissionDate; set => submissionDate = value; }
        public Assignment(int assignmentID, string titleofassignment, string description, DateTime? submissionDate)
        {
            AssignmentID = assignmentID;
            Titleofassignment = titleofassignment;
            Description = description;
            SubmissionDate = submissionDate;
        }
        public void Output()
        {
            Console.WriteLine($"AssignmentID: {assignmentID,-3} TITLEofASSIGNMENT: {titleofassignment,-20} " +
            $"DESCRIPTION: {description,-15}" +
            $"SUBMISSIONDATE: {submissionDate,-10:dd/MM/yyyy}");
        }
    }
}
