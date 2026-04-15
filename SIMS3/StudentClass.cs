using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace SIMS3
{
    internal class StudentClass
    {
        // call connectdb
            DBConnect connect = new DBConnect();

            // create a function to add a new students to the database
            public bool insertStudent(string fname, string mname, string lname, string suffix, DateTime bdate, string gender, string phone, string address, byte[] img)
            {
                MySqlCommand command = new MySqlCommand("INSERT INTO `student`(`FirstName`, `MiddleName`, `LastName`, `Suffix`, `Birthdate`, `Gender`, `Phone`, `Address`, `Photo`) VALUES (@fn, @mn, @ln, @sfx, @bd, @gd, @ph, @adr, @img)", connect.GetConnection());

                //@fn, @ln, @bd, @gd, @ph, @adr, @img
                command.Parameters.Add("@fn", MySqlDbType.VarChar).Value = fname;
                command.Parameters.Add("@mn", MySqlDbType.VarChar).Value = mname;   // Added Middle Name
                command.Parameters.Add("@ln", MySqlDbType.VarChar).Value = lname;
                command.Parameters.Add("@sfx", MySqlDbType.VarChar).Value = suffix; // Added Suffix
                command.Parameters.Add("@bd", MySqlDbType.Date).Value = bdate;
                command.Parameters.Add("@gd", MySqlDbType.VarChar).Value = gender;
                command.Parameters.Add("@ph", MySqlDbType.VarChar).Value = phone;
                command.Parameters.Add("@adr", MySqlDbType.VarChar).Value = address;
                command.Parameters.Add("@img", MySqlDbType.Blob).Value = img;

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

            public DataTable getStudentlist()
            {
                // Fix 1: Added () to GetConnection()
                MySqlCommand command = new MySqlCommand("SELECT * FROM `student`", connect.GetConnection());

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);

                // Fix 2: Created the empty 'table' before trying to fill it
                DataTable table = new DataTable();

                adapter.Fill(table);
                return table;

            }
            //function to execute the count
            public string exeCount(string query)
            {
                MySqlCommand command = new MySqlCommand(query, connect.GetConnection());
                connect.openConnect();
                string count = command.ExecuteScalar().ToString();
                connect.closeConnect();
                return count;
            }

            // Removed (string query)
            public string totalStudents()
            {
                return exeCount("SELECT COUNT(*) FROM student");
            }

            // Removed (string query) and fixed 'Gender' quotes
            public string maleStudents()
            {
                // Do not put quotes around the column name Gender
                return exeCount("SELECT COUNT(*) FROM student WHERE Gender='Male'");
            }

            // Removed (string query), fixed quotes, and fixed "Femaale" typo
            public string femaleStudents()
            {
                return exeCount("SELECT COUNT(*) FROM student WHERE Gender='Female'");
            }

            // function for student seach

            public DataTable searchStudent(string searchdata)
            {
                // Fix: We combined the string properly, used backticks for columns, and used SELECT *
                string query = "SELECT * FROM `student` WHERE CONCAT(`FirstName`, `MiddleName`, `LastName`, `Suffix`) LIKE '%" + searchdata + "%'";

                // Fix: Added () to GetConnection
                MySqlCommand command = new MySqlCommand(query, connect.GetConnection());

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;

            }


            //function for edit ayoko na 

            // Method to update student data
            public bool updateStudent(int id, string fname, string mname, string lname, string suffix, DateTime bdate, string gender, string phone, string address, byte[] img)
            {
                // Make sure column names match your phpMyAdmin exactly
                string query = "UPDATE `student` SET `FirstName`=@fn, `MiddleName`=@mn, `LastName`=@ln, `Suffix`=@sfx, `Birthdate`=@bd, `Gender`=@gd, `Phone`=@ph, `Address`=@adr, `Photo`=@img WHERE `Student ID`=@id";

                MySqlCommand command = new MySqlCommand(query, connect.GetConnection());

                // Adding parameters to prevent SQL injection
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@fn", MySqlDbType.VarChar).Value = fname;
                command.Parameters.Add("@mn", MySqlDbType.VarChar).Value = mname; // Your modification
                command.Parameters.Add("@ln", MySqlDbType.VarChar).Value = lname;
                command.Parameters.Add("@sfx", MySqlDbType.VarChar).Value = suffix; // Your modification
                command.Parameters.Add("@bd", MySqlDbType.Date).Value = bdate;
                command.Parameters.Add("@gd", MySqlDbType.VarChar).Value = gender;
                command.Parameters.Add("@ph", MySqlDbType.VarChar).Value = phone;
                command.Parameters.Add("@adr", MySqlDbType.VarChar).Value = address;
                command.Parameters.Add("@img", MySqlDbType.Blob).Value = img;

                connect.openConnect();

                // Execute the query
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

            //fucntion for any command in students
            public DataTable getlist(MySqlCommand command)
            {
                command.Connection = connect.GetConnection();

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;

            }
        }
    }

