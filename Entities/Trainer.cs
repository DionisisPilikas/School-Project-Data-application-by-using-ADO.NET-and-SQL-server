using System;


namespace ENTITIES_NS
{
    public class Trainer
    {
        private int trainerID; //TrainerID field in sqlDB is never going to be null
        private string firstName;
        private string lastName;
        private string subjectOfTrainer;
        public int TrainerID { get => trainerID; set => trainerID = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string SubjectOfTrainer { get => subjectOfTrainer; set => subjectOfTrainer = value; }
        public Trainer(int trainerID, string firstName, string lastName, string subjectOfTrainer)
        {
            TrainerID = trainerID;
            FirstName = firstName;
            LastName = lastName;
            SubjectOfTrainer = subjectOfTrainer;
        }
        public void Output()
        {
            Console.WriteLine($"TrID: {trainerID,-3} FIRSTNAME: {firstName,-10} LASTNAME: {lastName,-15}" +
            $"SUBJECTOFTRAINER: {subjectOfTrainer,-10}");
        }
    }
}
