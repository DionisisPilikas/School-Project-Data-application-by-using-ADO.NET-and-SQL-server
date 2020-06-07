using System;
using System.Data.SqlClient;


namespace SERVISES_NS
{
    public class ServiceInput : Connection_String
    {

        public static void InputValuesToStudentTable(string fname, string lname, DateTime? dobirth, int? sumTuitionfees)
        {
            SqlConnection dbcon = new SqlConnection(connectionstring);
            string query = @"insert into STUDENT (FIRSTNAME,LASTNAME,DATEOFBIRTH,SUM_TUITIONFEES,AVG_TOTALMARK) 
            VALUES(@FIRSTNAME, @LASTNAME, @DATEOFBIRTH, @SUM_TUITIONFEES,@AVG_TOTALMARK)";
            SqlCommand cmd = new SqlCommand(query, dbcon);
            cmd.Parameters.AddWithValue("@FIRSTNAME", fname ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@LASTNAME", lname ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@DATEOFBIRTH", dobirth ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@SUM_TUITIONFEES", sumTuitionfees ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@AVG_TOTALMARK", DBNull.Value);

            try
            {
                dbcon.Open();
                int affectedRows = cmd.ExecuteNonQuery();
                Console.WriteLine($"{affectedRows} insert ok!");

                Console.WriteLine("The new Student is :");
                SqlCommand cmd1 = new SqlCommand("select TOP 1 StudentID,FIRSTNAME,LASTNAME,DATEOFBIRTH,SUM_TUITIONFEES,AVG_TOTALMARK from STUDENT ORDER BY StudentID  DESC", dbcon);
                SqlDataReader reader1 = cmd1.ExecuteReader();
                while (reader1.Read())
                {
                    int studentID = Convert.ToInt32(reader1["StudentID"]);
                    string firstname = reader1["FIRSTNAME"] == DBNull.Value ? null : reader1[1].ToString();
                    string lastname = reader1["LASTNAME"] == DBNull.Value ? null : reader1[2].ToString();
                    DateTime? dateofbirth = string.IsNullOrWhiteSpace(reader1["DATEOFBIRTH"].ToString()) ? (DateTime?)null : Convert.ToDateTime(reader1[3]);
                    int? sumtuitionfees = string.IsNullOrWhiteSpace(reader1["SUM_TUITIONFEES"].ToString()) ? (int?)null : Convert.ToInt32(reader1[4].ToString());
                    int? avgtotalmark = string.IsNullOrWhiteSpace(reader1["AVG_TOTALMARK"].ToString()) ? (int?)null : Convert.ToInt32(reader1[reader1.FieldCount - 1]);

                    Console.WriteLine($"StID: {studentID,-3} FIRSTNAME: {firstname,-10} LASTNAME: {lastname,-15}" +
                    $"DATEOFBIRTH: {dateofbirth,-10:dd/MM/yyyy} SUM_TUITIONFEES: {sumtuitionfees,5} " +
                    $"AVG_TOTALMARK: {avgtotalmark,5}");
                }
            }
            catch (SqlException message)
            {
                Console.WriteLine("Error Generated. Details: " + message.ToString());
            }
            finally
            {
                if (dbcon != null) dbcon.Dispose();
            }
        }

        public static void InputValuesToTrainerTable(string firstName, string lastName, string subjectOfTrainer)
        {
            SqlConnection dbcon = new SqlConnection(connectionstring);
            string query = @"insert into TRAINER (FIRSTNAME,LASTNAME,SUBJECTOFTRAINER)
            VALUES(@FIRSTNAME, @LASTNAME, @SUBJECTOFTRAINER)";
            SqlCommand cmd = new SqlCommand(query, dbcon);

            cmd.Parameters.AddWithValue("@FIRSTNAME", firstName);
            cmd.Parameters.AddWithValue("@LASTNAME", lastName);
            cmd.Parameters.AddWithValue("@SUBJECTOFTRAINER", subjectOfTrainer);
            try
            {
                dbcon.Open();
                int affectedRows = cmd.ExecuteNonQuery();
                Console.WriteLine($"{affectedRows} insert ok!");
            }
            catch (SqlException message)
            {
                Console.WriteLine("Error Generated. Details: " + message.ToString());
            }
            finally
            {
                if (dbcon != null) dbcon.Close();
                dbcon.Dispose();
            }
        }
        public static void InputValuesToAssignmentTable(string titleOfAssignment, string description, DateTime? submissionDate)
        {
            SqlConnection dbcon = new SqlConnection(connectionstring);
            string query = @"insert into ASSIGNMENT (TITLEOFASSIGNMENT, DESCRIPTION, SUBMISSIONDATE)
            VALUES(@TITLEOFASSIGNMENT, @DESCRIPTION, @SUBMISSIONDATE)";

            SqlCommand cmd = new SqlCommand(query, dbcon);

            cmd.Parameters.AddWithValue("@TITLEOFASSIGNMENT", titleOfAssignment);
            cmd.Parameters.AddWithValue("@DESCRIPTION", description);
            cmd.Parameters.AddWithValue("@SUBMISSIONDATE", submissionDate ?? (object)DBNull.Value);
            try
            {
                dbcon.Open();
                int affectedRows = cmd.ExecuteNonQuery();
                Console.WriteLine($"{affectedRows} insert ok");

            }
            catch (SqlException message)
            {
                Console.WriteLine("Error Generated. Details: " + message.ToString());
            }
            finally
            {
                if (dbcon != null) dbcon.Close();
                dbcon.Dispose();
            }

        }
        public static void InputValuesToCourseTable(string title, string stream, string type, DateTime? startDate, DateTime? endDate)
        {
            SqlConnection dbcon = new SqlConnection(connectionstring);
            string query = @"insert into COURSE (TITLE,STREAM,TYPE,STARTDATE,ENDDATE)
            VALUES(@TITLE, @STREAM, @TYPE, @STARTDATE, @ENDDATE)";

            SqlCommand cmd = new SqlCommand(query, dbcon);
            cmd.Parameters.AddWithValue("@TITLE", title);
            cmd.Parameters.AddWithValue("@STREAM", stream);
            cmd.Parameters.AddWithValue("@TYPE", type);
            cmd.Parameters.AddWithValue("@STARTDATE", startDate);
            cmd.Parameters.AddWithValue("@ENDDATE", endDate);
            try
            {
                dbcon.Open();
                int affectedRows = cmd.ExecuteNonQuery();
                Console.WriteLine($"{affectedRows} insert ok!");
            }
            catch (SqlException message)
            {
                Console.WriteLine("Error Generated. Details: " + message.ToString());
            }
            finally
            {
                if (dbcon != null) dbcon.Close();
                dbcon.Dispose();
            }
        }
        public static void Assign_StudentPerCoursePerAssignment(int StudentidChoice, int courseidChoise, int AssignmentidChoice)
        {
            SqlConnection dbcon = new SqlConnection(connectionstring);
            string query = @"insert into StudentCourseAssignment (StudentID,CourseID,AssignmentID) 
            VALUES(@StudentID, @CourseID, @AssignmentID)
            select @StudentID where not exists(select * from  StudentCourseAssignment where StudentID = @StudentID); 
            select @CourseID where not exists(select * from  StudentCourseAssignment where CourseID = @CourseID);
            select @AssignmentID where not exists(select * from  StudentCourseAssignment where AssignmentID = @AssignmentID);
            ";
            
            try
            {

                dbcon.Open();
                SqlCommand cmd1 = new SqlCommand(query, dbcon);
                cmd1.Parameters.AddWithValue("@StudentID", StudentidChoice);
                cmd1.Parameters.AddWithValue("@CourseID", courseidChoise);
                cmd1.Parameters.AddWithValue("@AssignmentID", AssignmentidChoice);


                int affectedRows1 = cmd1.ExecuteNonQuery();
                Console.WriteLine($"{affectedRows1} insert ok!");

                Console.Write("choose the second AssignmentNo (every Student have 2 Assignments Per one Course) " +
                " warning! you should type different value now from the first time! : ");
                int AssignmentidChoiceNo2 = Convert.ToInt32(Console.ReadLine());
                SqlCommand cmd2 = new SqlCommand(
                "insert into StudentCourseAssignment values(@StudentID, @CourseID, @AssignmentID)", dbcon);
                cmd2.Parameters.AddWithValue("@StudentID", StudentidChoice);
                cmd2.Parameters.AddWithValue("@CourseID", courseidChoise);
                cmd2.Parameters.AddWithValue("@AssignmentID", AssignmentidChoiceNo2);
                int affectedRows2 = cmd2.ExecuteNonQuery();
                Console.WriteLine($"{affectedRows2} insert ok!");
            }
            catch (SqlException message)
            {
                Console.WriteLine("Error Generated. Details: " + message.ToString());
            }
            finally
            {
                if (dbcon != null) dbcon.Close();
                dbcon.Dispose();
            }
        }
    }
}

