using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using KMS.Models;
using System.Security.Cryptography;
using System.Security.AccessControl;
using System.Text;
using Microsoft.Office.Interop.Word;
using Microsoft.AspNetCore.Authorization;
using DataTable = System.Data.DataTable;
using Microsoft.AspNetCore.Mvc;

namespace KMS.Tools
{
    public class ExecuteQuery
    {
        private readonly IConfiguration _configuration; 

        public ExecuteQuery(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DataTable ExecuteRawQuery(string query, SqlParameter[] parameters = null)
        {
            DataTable table = new DataTable();

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                myCon.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    if (parameters != null)
                    {
                        myCommand.Parameters.AddRange(parameters);
                    }

                    SqlDataReader myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
            }

            return table;
        }

        public T ExecuteSingle<T>(string query, SqlParameter[] parameters)
        {
            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                myCon.Open();

                using (SqlCommand command = new SqlCommand(query, myCon))
                {
                    // Add parameters to the command if any
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    // Execute the query and return the result
                    object result = command.ExecuteScalar();

                    // Convert the result to the specified type T
                    if (result != null && result != DBNull.Value)
                    {
                        return (T)Convert.ChangeType(result, typeof(T));
                    }
                    else
                    {
                        return default(T);
                    }
                }
            }
        }

        public T ExecuteScalar<T>(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        return (T)Convert.ChangeType(result, typeof(T));
                    }
                    else
                    {
                        return default(T);
                    }
                }
            }
        }

        public List<string> ExecuteAndGetGroupNames(string query)
        {
            List<string> groupNames = new List<string>();

            DataTable result = ExecuteRawQuery(query);

            foreach (DataRow row in result.Rows)
            {
                string groupName = row["groupName"].ToString();
                groupNames.Add(groupName);
            }

            return groupNames;
        }

        public List<string> GetAuthorizedGroupNames(string siteName, string abilityName)
        {
            string query = @$"SELECT b.groupName
                    FROM TAccessRule a
                    INNER JOIN TUserGroup b ON a.groupId = b.id
                    WHERE a.site = '{siteName}' AND a.{abilityName} = 1";

            List<string> authorizedGroupNames = ExecuteAndGetGroupNames(query);
            authorizedGroupNames.Add("Admin");
            authorizedGroupNames.Add("Administrator");

            return authorizedGroupNames;
        }

        public bool IsUserAuthorized(List<string> authorizedGroupNames, IEnumerable<string> userRoles)
        {
            foreach (var groupName in authorizedGroupNames)
            {
                if (userRoles.Contains(groupName))
                {
                    return true;
                }
            }
            return false;
        }

    }

    //public class WordApi
    //{
    //    public void InsertText(string filePath, string text)
    //    {
    //        Application wordApp = new Application();
    //        Document wordDoc = wordApp.Documents.Open(filePath);

    //        // Insert text at the end of the document
    //        wordDoc.Content.InsertAfter(text);

    //        // Save and close the document
    //        wordDoc.Save();
    //        wordDoc.Close();

    //        // Quit Word application
    //        wordApp.Quit();
    //    }
    //}

}
