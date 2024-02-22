using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Text;

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
    }
}
