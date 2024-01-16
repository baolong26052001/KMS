using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using KMS.Tools;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TermController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public TermController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowTerm")]
        public JsonResult GetTerm()
        {
            string query = "SELECT * FROM Term";

            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowTerm/{id}")]
        public JsonResult GetTermById(int id)
        {
            string query = "SELECT * FROM Term where id=@Id";

            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Term ID not found");
            }
        }

        [HttpPost]
        [Route("AddTerm")]
        public JsonResult AddTerm([FromBody] Term term)
        {
            string query = "INSERT INTO Term (content, dateCreated, dateModified) " +
                           "VALUES (@Content, GETDATE(), GETDATE())";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Content", term.Content),
            };

            _exQuery.ExecuteRawQuery(query, parameters);

            return new JsonResult("Term added successfully");
        }

        [HttpPut]
        [Route("EditTerm/{id}")]
        public JsonResult EditTerm(int id, [FromBody] Term term)
        {
            string query = "UPDATE Term " +
                           "SET content = @Content, dateModified = GETDATE() " +
                           "WHERE id = @Id";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Content", term.Content),

            };


            _exQuery.ExecuteRawQuery(query, parameters);


            return new JsonResult("Term updated successfully");
        }

        [HttpDelete]
        [Route("DeleteTerm")]
        public JsonResult DeleteTerm([FromBody] List<int> termIds)
        {
            if (termIds == null || termIds.Count == 0)
            {
                return new JsonResult("No term IDs provided for deletion");
            }

            StringBuilder deleteQuery = new StringBuilder("DELETE FROM Term WHERE id IN (");
            string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Delete', 'Term', GETDATE(), GETDATE(), 1)";

            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter[] parameters2 = { };
            for (int i = 0; i < termIds.Count; i++)
            {
                string parameterName = "@TermId" + i;
                deleteQuery.Append(parameterName);

                if (i < termIds.Count - 1)
                {
                    deleteQuery.Append(", ");
                }

                parameters.Add(new SqlParameter(parameterName, termIds[i]));
            }

            deleteQuery.Append(");");

            _exQuery.ExecuteRawQuery(deleteQuery.ToString(), parameters.ToArray());
            _exQuery.ExecuteRawQuery(query2, parameters2);

            return new JsonResult("Term deleted successfully");
        }


    }
}
