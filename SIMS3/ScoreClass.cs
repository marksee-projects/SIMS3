using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SIMS3
{
    class ScoreClass
    {
        DBConnect connect = new DBConnect();

        // Create a function to insert course
        // Create a function to insert grade
        public bool insertCourse(int stdId, string cName, double grade, string desc)
        {

            MySqlCommand command = new MySqlCommand("INSERT INTO `grade`(`Student ID`, `CourseName`, `Grade`, `Description`) VALUES (@stdId, @cName, @grade, @desc)", connect.GetConnection());

            command.Parameters.Add("@stdId", MySqlDbType.Int32).Value = stdId;
            command.Parameters.Add("@cName", MySqlDbType.VarChar).Value = cName;
            command.Parameters.Add("@grade", MySqlDbType.Double).Value = grade;
            command.Parameters.Add("@desc", MySqlDbType.VarChar).Value = desc;

            connect.openConnect();


            if (command.ExecuteNonQuery() == 1)
            {
                connect.closeConnect();
                return true;
            }
            else
            {
                connect.closeConnect();
                return false;
            }
        }


        //function to get grade list
        public DataTable getlist(MySqlCommand command)
        {
            command.Connection = connect.GetConnection();

            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;

        }

        // Create a function to check if the grade already exists for a given student and course
        public bool checkScore(int stdId, string cName)
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM `grade` WHERE `Student ID` = @stdId AND `CourseName` = @cName", connect.GetConnection());


            command.Parameters.Add("@stdId", MySqlDbType.Int32).Value = stdId;
            command.Parameters.Add("@cName", MySqlDbType.VarChar).Value = cName;

            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);


            if (table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        // Create a function to update the grade for a specific student and course
        public bool updateScore(int stdId, string cName, double grade, string desc)
        {

            string query = "UPDATE `grade` SET `Grade` = @grade, `Description` = @desc WHERE `Student ID` = @stdId AND `CourseName` = @cName";

            MySqlCommand command = new MySqlCommand(query, connect.GetConnection());


            command.Parameters.Add("@stdId", MySqlDbType.Int32).Value = stdId;
            command.Parameters.Add("@cName", MySqlDbType.VarChar).Value = cName;
            command.Parameters.Add("@grade", MySqlDbType.Double).Value = grade;
            command.Parameters.Add("@desc", MySqlDbType.VarChar).Value = desc;

            connect.openConnect();


            if (command.ExecuteNonQuery() == 1)
            {
                connect.closeConnect();
                return true;
            }
            else
            {
                connect.closeConnect();
                return false;
            }
        }

        // Create a function to "delete" (deactivate) a course by setting IsActive to 0
        public bool deleteScore(int stdId, string cName)
        {
            string query = "UPDATE `grade` SET `IsActive` = 0 WHERE `Student ID` = @stdId AND `CourseName` = @cName";

            MySqlCommand command = new MySqlCommand(query, connect.GetConnection());

            command.Parameters.Add("@stdId", MySqlDbType.Int32).Value = stdId;
            command.Parameters.Add("@cName", MySqlDbType.VarChar).Value = cName;

            connect.openConnect();

            if (command.ExecuteNonQuery() == 1)
            {
                connect.closeConnect();
                return true;
            }
            else
            {
                connect.closeConnect();
                return false;
            }
        }

        // This is the same as SearchCourse but it also checks if IsActive = 1 to hide deleted courses
        public DataTable SearchScore(string searchdata, int statusFilter)
        {
            string query = "SELECT grade.`Student ID` AS `Student ID`, student.FirstName AS `First Name`, student.LastName AS `Last Name`, grade.CourseName AS `CourseName`, grade.Grade, grade.Description, grade.IsActive FROM grade INNER JOIN student ON grade.`Student ID` = student.`Student ID` WHERE (student.FirstName LIKE @search OR student.LastName LIKE @search OR grade.CourseName LIKE @search OR grade.`Student ID` LIKE @search)";

            if (statusFilter == 1)
            {
                query += " AND grade.IsActive = 1";
            }
            else if (statusFilter == 0)
            {
                query += " AND grade.IsActive = 0";
            }

            query += " ORDER BY grade.`Student ID` DESC";

            MySqlCommand command = new MySqlCommand(query, connect.GetConnection());
            command.Parameters.AddWithValue("@search", "%" + searchdata + "%");

            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            return table;


        }
    }
}

