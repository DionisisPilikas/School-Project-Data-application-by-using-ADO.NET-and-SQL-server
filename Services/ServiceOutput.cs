using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using ENTITIES_NS;

namespace SERVISES_NS
{
    public delegate bool Mydelegate(Trainer tr);
    public class ServiceOutput : Connection_String
    {
        public static List<Student> GetAllStudents()
        {
            SqlConnection dbconnection = new SqlConnection(connectionstring);
            dbconnection.Open();
            Console.WriteLine("ALL STUDENTS OF BOOTCAMP : ");
            List<Student> students = new List<Student>();
            try
            {
                using (dbconnection)
                {
                    SqlCommand cmd = new SqlCommand("select * from STUDENT", dbconnection);
                    using (cmd)
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Student S1 = new Student
                                (studentID: Convert.ToInt32(reader["StudentID"]), //StudentID field in sqlDB is never going to be null
                                firstName: reader["FIRSTNAME"] == DBNull.Value ? null : reader[1].ToString(),
                                 lastName: reader["LASTNAME"] == DBNull.Value ? null : reader[2].ToString(),
                                 dateOfBirth: string.IsNullOrWhiteSpace(reader["DATEOFBIRTH"].ToString()) ? (DateTime?)null : Convert.ToDateTime(reader[3]),
                                sum_TuisionFees: string.IsNullOrWhiteSpace(reader["SUM_TUITIONFEES"].ToString()) ? (int?)null : Convert.ToInt32(reader[4]),
                                avg_TotalMark: string.IsNullOrWhiteSpace(reader["AVG_TOTALMARK"].ToString()) ? (int?)null : Convert.ToInt32(reader[5]));

                            students.Add(S1);
                        }
                        reader.Close();
                        Console.WriteLine();
                    }
                }
            }
            catch (SqlException message)
            {
                Console.WriteLine("Error Generated. Details: " + message.ToString());
            }
            finally { }
            return students;
        }
         public static List<Trainer> GetAllTrainers(Mydelegate checkTrainer)
        {
            SqlConnection dbconnection = new SqlConnection(connectionstring);
            dbconnection.Open();
            Console.WriteLine("ALL TRAINERS OF BOOTCAMP : ");
            List<Trainer> trainers = new List<Trainer>();
            try
            {
                using (dbconnection)
                {
                    SqlCommand cmd = new SqlCommand("select * from TRAINER", dbconnection);
                    using (cmd)
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Trainer T1 = new Trainer
                                (trainerID: Convert.ToInt32(reader["TrainerID"]),//TrainerID field in sqlDB is never going to be null
                                firstName: reader["FIRSTNAME"] == DBNull.Value ? null : reader[1].ToString(),
                                lastName: reader["LASTNAME"] == DBNull.Value ? null : reader[2].ToString(),
                                subjectOfTrainer: reader["SUBJECTOFTRAINER"] == DBNull.Value ? "NULL" : reader[3].ToString());
                           
                            if (checkTrainer(T1))
                            {
                                trainers.Add(T1);
                            }
                        }
                        reader.Close();
                        Console.WriteLine();
                    }
                }
            }
            catch (SqlException message)
            {
                Console.WriteLine("Error Generated. Details: " + message.ToString());
            }
            finally { }
            return trainers;
        }
        public static List<Course> GetAllCourses(Predicate<Course> Condition)
        {
            SqlConnection dbconnection = new SqlConnection(connectionstring);
            dbconnection.Open();
            Console.WriteLine("ALL COURSES OF BOOTCAMP : ");
            List<Course> courses = new List<Course>();
            try
            {
                using (dbconnection)
                {
                    SqlCommand cmd = new SqlCommand("select * from COURSE", dbconnection);
                    using (cmd)
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Course C1 = new Course
                                (courseID: Convert.ToInt32(reader["CourseID"]),//field CourseID in sqlDB is never going to be null
                                title: reader["TITLE"] == DBNull.Value ? null : reader[1].ToString(),
                                stream: reader["STREAM"] == DBNull.Value ? "NULL" : reader[2].ToString(),
                                type: reader["TYPE"] == DBNull.Value ? "NULL" : reader[3].ToString(),
                                startdate: reader["STARTDATE"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader[4]),
                                enddate: reader["ENDDATE"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader[5]));
                           
                            if (Condition(C1))
                            {
                                courses.Add(C1);
                            }
                        }
                        reader.Close();
                        Console.WriteLine();
                    }
                }
            }
            catch (SqlException message)
            {
                Console.WriteLine("Error Generated. Details: " + message.ToString());
            }
            finally { }
            return courses;
        }
        public static List<Assignment> GetAllAssignments()
        {
            SqlConnection dbconnection = new SqlConnection(connectionstring);
            dbconnection.Open();
            Console.WriteLine("ALL ASSIGNMENTS OF BOOTCAMP : ");
            List<Assignment> assignments = new List<Assignment>();
            try
            {
                using (dbconnection)
                {
                    SqlCommand cmd = new SqlCommand("select * from ASSIGNMENT", dbconnection);
                    using (cmd)
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Assignment A1 = new Assignment
                                (assignmentID: Convert.ToInt32(reader["AssignmentID"]),//field Assignment in sqlDB is never going to be null
                                titleofassignment: reader["TITLEOFASSIGNMENT"] == DBNull.Value ? "NULL" : reader[1].ToString(),
                                description: reader["DESCRIPTION"] == DBNull.Value ? "NULL" : reader[2].ToString(),
                                submissionDate: reader["SUBMISSIONDATE"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader[3]));
                            assignments.Add(A1);
                        }
                        reader.Close();
                        Console.WriteLine();
                    }
                }
            }
            catch (SqlException message)
            {
                Console.WriteLine("Error Generated. Details: " + message.ToString());
            }
            finally { }
            return assignments;
        }
        public static void GetAllStudentsPerCourse()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ALL THE STUDENTSS PER COURSE");
            Console.ForegroundColor = ConsoleColor.White;
            SqlConnection dbconnection = new SqlConnection(connectionstring);
            SqlDataReader reader = null;
            try
            {
                dbconnection.Open();
                SqlCommand cmd1 = new SqlCommand(@"DROP PROCEDURE IF EXISTS GET_STUDENTS_PER_COURSE", dbconnection);
                cmd1.ExecuteNonQuery();
                SqlCommand cmd2 = new SqlCommand(
                @"CREATE PROCEDURE GET_STUDENTS_PER_COURSE @number INT
                AS
                BEGIN
                select DISTINCT a.StudentID, a.FIRSTNAME,a.LASTNAME,a.DATEOFBIRTH,a.SUM_TUITIONFEES,a.AVG_TOTALMARK from STUDENT a
                inner join StudentCourseAssignment b ON a.StudentID = b.STudentID
                inner join COURSE c ON b.CourseID = c.CourseID
                WHERE c.CourseID = @number
                END", dbconnection);
                cmd2.ExecuteNonQuery();
                for (int CourseID = 1; CourseID <= 4; CourseID++)
                {
                    Console.WriteLine("ALL THE STUDENTS PER COURSE :" + CourseID);
                    SqlCommand cmd3 = new SqlCommand(@"GET_STUDENTS_PER_COURSE", dbconnection);
                    cmd3.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@number", CourseID);
                    reader = cmd3.ExecuteReader();
                    while (reader.Read())
                    {
                        int studentID = Convert.ToInt32(reader["StudentID"]); //StudentID field in sqlDB is never going to be null
                        string firstname = reader["FIRSTNAME"] == DBNull.Value ? "NULL" : reader[1].ToString();
                        string lastname = reader["LASTNAME"] == DBNull.Value ? "NULL" : reader[2].ToString();
                        DateTime? dateofbirth = reader["DATEOFBIRTH"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader[3]);
                        int sumtuitionfees = reader["SUM_TUITIONFEES"] == DBNull.Value ? 0 : Convert.ToInt32(reader[4]);
                        int avgtotalmark = reader["AVG_TOTALMARK"] == DBNull.Value ? 0 : Convert.ToInt16(reader[reader.FieldCount - 1]);

                        Console.WriteLine($"StID: {studentID,-3} FIRSTNAME: {firstname,-10} LASTNAME: {lastname,-15} " +
                        $"DATEOFBIRTH: {dateofbirth,-10:dd/MM/yyyy} SUM_TUITIONFEES: {sumtuitionfees,5} " +
                        $"AVG_TOTALMARK: {avgtotalmark,5}");
                    }
                    reader.Close();
                    Console.WriteLine();
                }

            }
            catch (SqlException message)
            {
                Console.WriteLine("Error Generated. Details: " + message.ToString());
            }
            finally
            {
                if (reader != null) reader.Close();
                if (dbconnection != null) dbconnection.Close();
                dbconnection.Dispose();
            }
        }
        public static void GetAllTrainersPerCourse()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ALL THE TRAINERS PER COURSE");
            Console.ForegroundColor = ConsoleColor.White;
            SqlConnection dbconnection = new SqlConnection(connectionstring);
            SqlDataReader reader = null;
            try
            {
                dbconnection.Open();
                SqlCommand cmd1 = new SqlCommand(@"DROP PROCEDURE IF EXISTS GET_TRAINERS_PER_COURSE", dbconnection);
                cmd1.ExecuteNonQuery();
                SqlCommand cmd2 = new SqlCommand(
                @"CREATE PROCEDURE GET_TRAINERS_PER_COURSE @number INT
                    AS
                    BEGIN
                    select DISTINCT a.TrainerID, a.FIRSTNAME,a.LASTNAME,a.SUBJECTOFTRAINER from TRAINER a
                    inner join TRAINERCOURSE b ON a.TrainerID = b.TrainerID
                    inner join COURSE c ON b.CourseID = c.CourseID
                    WHERE c.CourseID = @number
                    END", dbconnection);
                cmd2.ExecuteNonQuery();
                for (int CourseID = 1; CourseID <= 4; CourseID++)
                {
                    Console.WriteLine("ALL THE TRAINERS PER COURSE :" + CourseID);
                    SqlCommand cmd3 = new SqlCommand(@"GET_TRAINERS_PER_COURSE", dbconnection);
                    cmd3.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@number", CourseID);
                    reader = cmd3.ExecuteReader();
                    while (reader.Read())
                    {
                        int trainerID = Convert.ToInt32(reader["TrainerID"]);//TrainerID field in sqlDB is never going to be null
                        string firstname = reader["FIRSTNAME"] == DBNull.Value ? "NULL" : reader[1].ToString(); ;
                        string lastname = reader["LASTNAME"] == DBNull.Value ? "NULL" : reader[2].ToString();
                        string subjectoftrainer = reader["SUBJECTOFTRAINER"] == DBNull.Value ? "NULL" : reader[reader.FieldCount - 1].ToString(); ;

                        Console.WriteLine($"TrID: {trainerID,-3} FIRSTNAME: {firstname,-10} LASTNAME: {lastname,-15}" +
                        $"SUBJECTOFTRAINER: {subjectoftrainer,-10}");
                    }
                    reader.Close();
                    Console.WriteLine();
                }
            }
            catch (SqlException message)
            {
                Console.WriteLine("Error Generated. Details: " + message.ToString());
            }
            finally
            {
                if (reader != null) reader.Close();
                if (dbconnection != null) dbconnection.Close();
                dbconnection.Dispose();
            }
        }
        /// <summary>
        /// GET AND PRINT ALL ASSIGNMENTS PER COURSE (4 COURSES) ** DATASET WAY OF SOLUTION **
        /// </summary>
        public static void GetAllAssignmentsPerCourse()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ALL THE ASSIGNMENT PER COURSE");
            Console.ForegroundColor = ConsoleColor.White;
            SqlConnection dbconnection = new SqlConnection(connectionstring);

            SqlDataAdapter adapter1 = null;
            SqlDataAdapter adapter2 = null;
            SqlDataAdapter adapter3 = null;
            DataSet dataset1 = null;
            DataSet dataset2 = null;
            DataSet dataset3 = null;
            
            //************SOS**** By THIS METHOD DOESN'T NEED TO OPEN THE CONNECTION WITH DATABASE ********************

            try
            {
                adapter1 = new SqlDataAdapter(@"DROP PROCEDURE IF EXISTS GET_ASSIGNMENTS_PER_COURSE", dbconnection);
                dataset1 = new DataSet();
                adapter1.Fill(dataset1);

                adapter2 = new SqlDataAdapter(@"CREATE PROCEDURE GET_ASSIGNMENTS_PER_COURSE @number INT
                    AS
                    BEGIN
                    select DISTINCT a.AssignmentID,a.TITLEofASSIGNMENT,a.DESCRIPTION,a.SUBMISSIONDATE from Assignment a
                    inner join studentcourseassignment b on b.AssignmentID = a.AssignmentID
                    inner join course c on b.CourseID = c.CourseID
                    WHERE c.CourseID = @number
                    END", dbconnection);
                dataset2 = new DataSet();
                adapter2.Fill(dataset2);

                for (int CourseID = 1; CourseID <= 4; CourseID++)
                {

                    Console.WriteLine("ALL THE ASSIGNMENTS PER COURSE : " + CourseID);
                    SqlCommand command1 = new SqlCommand(@"GET_ASSIGNMENTS_PER_COURSE", dbconnection);

                    command1.CommandType = System.Data.CommandType.StoredProcedure;
                    command1.Parameters.AddWithValue("@number", CourseID);
                    adapter3 = new SqlDataAdapter(command1);
                    adapter3.UpdateCommand = command1;

                    dataset3 = new DataSet();
                    adapter3.Fill(dataset3);

                    DataTable table = dataset3.Tables[0];
                    for (int j = 0; j < table.Rows.Count; j++)
                    {
                        DataRow row = table.Rows[j];
                        int assignmentID = Convert.ToInt32(row[0]); //AssignmentID field in sqlDB is never going to be null
                        string titleofAssignment = row[1] == DBNull.Value ? null : row[1].ToString();
                        string description = row[2] == DBNull.Value ? null : row[2].ToString();
                        DateTime? submissionDate = row[3] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row[3]);

                        Console.WriteLine($"AssignmentID : {assignmentID,-2} TitleOfAssignment : {titleofAssignment,-20} " +
                        $"Description : {description,-15} SubmissionDate: {submissionDate,-10:dd/MM/yyyy}");
                    }
                    Console.WriteLine();
                }
            }
            catch (SqlException message)
            {
                Console.WriteLine("Error Generated. Details: " + message.ToString());
            }
            finally
            {
                if (dataset1 != null) dataset1.Clear();
                dataset1.Dispose();
                if (dataset2 != null) dataset2.Clear();
                dataset2.Dispose();
                if (dataset3 != null) dataset3.Clear();
                dataset3.Dispose();
                
                if (adapter1 != null) adapter1.Dispose();
                if (adapter2 != null) adapter2.Dispose();
                if (adapter3 != null) adapter3.Dispose();
                
            }
        }
        /// <summary>
        /// Get Assignments Per Course Per Student - Dataset way
        /// </summary>
        public static void GetAssignmentsPerStudentPerCourse()
        {
            for (int CourseID = 1; CourseID <= 4; CourseID++)
            {
                SqlConnection dbconnection = new SqlConnection(connectionstring);

                DataSet dataset1 = null;  //require using System.Data
                DataSet dataset2 = null;  //require using System.Data
                SqlDataAdapter adapter1 = null;
                try
                {
                    dbconnection.Open();
                    SqlCommand cmd = new SqlCommand(@"DROP PROCEDURE IF EXISTS GET_STUDENTS_PER_COURSE", dbconnection);
                    cmd.ExecuteNonQuery();
                    SqlCommand cmd0 = new SqlCommand(
                    @"CREATE PROCEDURE GET_STUDENTS_PER_COURSE @number INT
                       AS
                       BEGIN
                       select DISTINCT a.StudentID, a.FIRSTNAME,a.LASTNAME,a.DATEOFBIRTH,a.SUM_TUITIONFEES,a.AVG_TOTALMARK from STUDENT a
                       inner join StudentCourseAssignment b ON a.StudentID = b.STudentID
                       inner join COURSE c ON b.CourseID = c.CourseID
                       WHERE c.CourseID = @number
                       END", dbconnection);
                    cmd0.ExecuteNonQuery();
                    SqlCommand cmd1 = new SqlCommand("GET_STUDENTS_PER_COURSE", dbconnection);
                    cmd1.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@number", CourseID);
                    adapter1 = new SqlDataAdapter(cmd1);
                    dataset1 = new DataSet();
                    adapter1.Fill(dataset1);
                    adapter1.Dispose();
                    SqlDataAdapter adapter2 = null;
                    SqlCommand cmd2 = new SqlCommand(@"DROP PROCEDURE IF EXISTS GET_ASSIGNMENTS_PER_COURSE", dbconnection);
                    cmd2.ExecuteNonQuery();
                    SqlCommand cmd3 = new SqlCommand(
                    @"CREATE PROCEDURE GET_ASSIGNMENTS_PER_COURSE @number INT
                       AS
                       BEGIN
                       select DISTINCT a.AssignmentID,a.TITLEofASSIGNMENT,a.DESCRIPTION,a.SUBMISSIONDATE from Assignment a
                       inner join studentcourseassignment b on b.AssignmentID = a.AssignmentID
                       inner join course c on b.CourseID = c.CourseID
                       WHERE c.CourseID = @number
                       END", dbconnection);
                    cmd3.ExecuteNonQuery();
                    SqlCommand cmd4 = new SqlCommand(@"GET_ASSIGNMENTS_PER_COURSE", dbconnection);
                    cmd4.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd4.Parameters.AddWithValue("@number", CourseID);
                    //dataset way of solution
                    adapter2 = new SqlDataAdapter(cmd4);
                    dataset2 = new DataSet();
                    adapter2.Fill(dataset2);
                    adapter2.Dispose();
                    DataTable table1 = dataset1.Tables[0];
                    DataTable table2 = dataset2.Tables[0];
                    for (int i = 0; i < table1.Rows.Count; i++)
                    {
                        for (int j = 0; j < table2.Rows.Count; j++)
                        {

                            DataRow row1 = table1.Rows[i]; ;
                            int studentID = Convert.ToInt32(row1[0]);
                            string firstname = row1[1] == DBNull.Value ? "NULL" : row1[1].ToString(); ;
                            string lastaname = row1[2] == DBNull.Value ? "NULL" : row1[2].ToString();
                            DateTime? dateofbirth = row1[3] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row1[3]);
                            int sumtuitionfees = row1[4] == DBNull.Value ? 0 : Convert.ToInt32(row1[4]);
                            int avgtotalmark = row1[5] == DBNull.Value ? 0 : Convert.ToInt32(row1[5]);
                            Console.WriteLine("The Course with CourseID : {0}", CourseID);
                            Console.WriteLine("THE STUDENT WITH:");
                            Console.WriteLine($"StudentID : {studentID,-2} firstname : {firstname,-10} " +
                            $"lastname : {lastaname,-12} SubmissionDate: {dateofbirth,-12:dd/MM/yyyy}" +
                            $"sumtuitionfees:{sumtuitionfees,-10}avgtotalmark:{avgtotalmark,-10}");
                            DataRow row2 = table2.Rows[j];
                            int assignmentID = Convert.ToInt32(row2[0]);
                            string titleofAssignment = row2[1] == DBNull.Value ? "NULL" : row2[1].ToString();
                            string description = row2[2] == DBNull.Value ? "NULL" : row2[2].ToString(); ;
                            DateTime? submissionDate = row2[2] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row2[3]);
                            Console.WriteLine("HAS THE ASSIGNMENT :");
                            Console.WriteLine($"AssignmentID : {assignmentID,-2} TitleOfAssignment : {titleofAssignment,-20} " +
                            $"Description : {description,-15} SubmissionDate: {submissionDate,-10:dd/MM/yyyy}");
                        }
                    }
                }
                catch (SqlException message)
                {
                    Console.WriteLine("Error Generated. Details: " + message.ToString());
                }
                finally
                {
                    dataset1.Dispose();
                    dataset2.Dispose();
                    dbconnection.Close();
                    dbconnection.Dispose();
                }
            }
        }
        /// <summary>
        /// GET AND PRINT the list of students that belong to more than one courses
        /// (the number of courses per student > 1)
        /// </summary>
        public static void GetAllSTudentsBelongMoreThanOneCourse()
        {
            SqlConnection dbconnection = new SqlConnection(connectionstring);
            SqlDataReader reader = null;
            try
            {
                dbconnection.Open();
                SqlCommand cmd = new SqlCommand(
                @"select DISTINCT a.StudentID, a.FIRSTNAME,a.LASTNAME,a.DATEOFBIRTH,a.SUM_TUITIONFEES,a.AVG_TOTALMARK from STUDENT a
                   inner join studentcourseAssignment b on a.StudentID = b.StudentID
                   inner join course c on b.CourseID = c.CourseID
                   group by a.StudentID,a.firstname,a.lastname,a.DATEOFBIRTH,a.SUM_TUITIONFEES,a.AVG_TOTALMARK
                   having count(DISTINCT b.CourseID)>1", dbconnection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int studentID = Convert.ToInt32(reader["StudentID"]);
                    string firstname = reader["FIRSTNAME"] == DBNull.Value ? "NULL" : reader[1].ToString();
                    string lastname = reader["LASTNAME"] == DBNull.Value ? "NULL" : reader[2].ToString();
                    DateTime? dateofbirth = reader["DATEOFBIRTH"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader[3]);
                    int sumtuitionfees = reader["SUM_TUITIONFEES"] == DBNull.Value ? 0 : Convert.ToInt32(reader[4]);
                    int avgtotalmark = reader["AVG_TOTALMARK"] == DBNull.Value ? 0 : Convert.ToInt16(reader[reader.FieldCount - 1]);

                    Console.WriteLine($"StID: {studentID,-3} FIRSTNAME: {firstname,-10} LASTNAME: {lastname,-15} " +
                    $"DATEOFBIRTH: {dateofbirth,-10:dd/MM/yyyy} SUM_TUITIONFEES: {sumtuitionfees,5} " +
                    $"AVG_TOTALMARK: {avgtotalmark,5}");
                }
                reader.Close();
                Console.WriteLine();
            }
            catch (SqlException message)
            {
                Console.WriteLine("Error Generated. Details: " + message.ToString());
            }
            finally
            {
                dbconnection.Close();
                dbconnection.Dispose();
            }
        }
    }
}

