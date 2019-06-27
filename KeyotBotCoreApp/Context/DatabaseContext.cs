using System;
using KeyotBotCoreApp.Context.Entities;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using KeyotBotCoreApp.Services;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Data;

namespace KeyotBotCoreApp.Context
{
    public class DatabaseContext
    {
        private readonly static string ConnectionString = "Server=remotemysql.com;Database=sAzFragJ80;Uid=sAzFragJ80;Pwd=Ye5x0LBDEk;";

        public static void InsertCandidate(ConversationModel conv)
        {

            var query = String.Format(@"
                INSERT INTO Candidate 
                (crew212_college_degree, crew212_email, crew212_firstName, crew212_lastName, crew212_gpa, crew212_grad_year, crew212_leadership, crew212_location, crew212_numInternships, crew212_numOrganizations_expand, crew212_numTechnicalSkills, crew212_relocate, crew212_school_name, crew212_stem_degree, crew212_technicalSkills, crew212_usAuthorized, crew212_work_experience)
            	VALUES
                ('yes', '{0}', '{1}', '{2}', 3.5, 2019, '1', 'yes', '2', '2', '2', 'yes', 'schoolname', 'yes', 'technical', 'yes', '2')", 
                conv.Email, conv.FirstName, conv.LastName);
            ExecuteQuery(query, false);
        }

        public static void SelectCandidate()
        {
            var query = String.Format(@"
                SELECT * 
                FROM Candidate
                WHERE 
            ");
            var result = ExecuteQuery(query, true);
        }

        //INSERT INTO Candidate(candidate_id, crew212_college_degree, crew212_email, crew212_firstName, crew212_lastName, crew212_gpa, crew212_grad_year, crew212_leadership, crew212_location, crew212_numInternships, crew212_numOrganizations_expand, crew212_numTechnicalSkills, crew212_relocate, crew212_school_name, crew212_stem_degree, crew212_technicalSkills, crew212_usAuthorized, crew212_work_experience) VALUES(1, 'yes', 'email@email.com', 'andrew', 'daniels', 3.5, 2019, '1', 'yes', '2', '2', '2', 'yes', 'schoolname', 'yes', 'technical', 'yes', '2')

        private static DataTable ExecuteQuery(string query, bool returnsValue) 
        {
            using (MySqlConnection myConnection = new MySqlConnection(ConnectionString))
            {
                try
                {
                    myConnection.Open();

                    MySqlCommand myCommand = myConnection.CreateCommand();
                    MySqlTransaction myTrans;

                    myTrans = myConnection.BeginTransaction();
                    myCommand.Connection = myConnection;
                    myCommand.Transaction = myTrans;

                    if (!returnsValue)
                    {
                        myCommand.CommandText = query;
                        myCommand.ExecuteNonQuery();
                        myTrans.Commit();
                    }
                    else
                    {
                        MySqlDataReader rdr = myCommand.ExecuteReader();
                        if (rdr.HasRows)
                        {
                            DataTable dataTable = new DataTable();
                            using (MySqlDataAdapter da = new MySqlDataAdapter(myCommand))
                            {
                                da.Fill(dataTable);
                                return dataTable;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return null;
            }
        }
    }
}
