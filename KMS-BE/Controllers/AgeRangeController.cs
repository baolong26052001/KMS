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
    public class AgeRangeController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public AgeRangeController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("CheckAgeRange/{id}")] // id của member
        public JsonResult CheckAgeRange(int id)
        {
            string query = "SELECT lm.id, lm.fullName, DATEDIFF(DAY, lm.birthday, GETDATE()) / 365.25 AS age, " +
                "ar.id AS ageRangeId, N'Từ ' + CAST(ar.startAge AS NVARCHAR) + N' đến ' + CAST(ar.endAge AS NVARCHAR) + N' tuổi' AS description " +
                "FROM LMember lm " +
                "JOIN AgeRange ar ON ar.startAge <= (DATEDIFF(DAY, lm.birthday, GETDATE()) / 365.25) " +
                "and DATEDIFF(DAY, lm.birthday, GETDATE()) / 365.25 < ar.endAge + 1 and lm.id=@Id";

            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Member not found, cannot check age range");
            }
        }

        [HttpGet]
        [Route("ShowAgeRange")]
        public JsonResult GetAgeRange()
        {
            string query = "SELECT * FROM AgeRange";

            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowAgeRange/{id}")]
        public JsonResult GetAgeRangeById(int id)
        {
            string query = "SELECT * FROM AgeRange where id=@Id";

            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Age Range ID not found");
            }
        }

        [HttpPost]
        [Route("AddAgeRange")]
        public JsonResult AddAgeRange([FromBody] AgeRange ageRange)
        {
            string query = "INSERT INTO AgeRange (startAge, endAge, dateCreated, dateModified) " +
                           "VALUES (@StartAge, @EndAge, GETDATE(), GETDATE())";

            SqlParameter[] parameters =
            {
                new SqlParameter("@StartAge", ageRange.StartAge),
                new SqlParameter("@EndAge", ageRange.EndAge),
            };

            _exQuery.ExecuteRawQuery(query, parameters);

            return new JsonResult("Age Range added successfully");
        }

        [HttpPut]
        [Route("EditAgeRange/{id}")]
        public JsonResult EditAgeRange(int id, [FromBody] AgeRange ageRange)
        {
            string query = "UPDATE AgeRange " +
                           "SET startAge = @StartAge, endAge = @EndAge, dateModified = GETDATE() " +
                           "WHERE id = @Id";
            
            SqlParameter[] parameters =
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@StartAge", ageRange.StartAge),
                new SqlParameter("@EndAge", ageRange.EndAge),
            };
            

            _exQuery.ExecuteRawQuery(query, parameters);
            

            return new JsonResult("Age Range updated successfully");
        }

        [HttpDelete]
        [Route("DeleteAgeRange")]
        public JsonResult DeleteAgeRange([FromBody] List<int> ageRangeIds)
        {
            if (ageRangeIds == null || ageRangeIds.Count == 0)
            {
                return new JsonResult("No age range IDs provided for deletion");
            }

            StringBuilder deleteQuery = new StringBuilder("DELETE FROM AgeRange WHERE id IN (");
            string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Delete', 'AgeRange', GETDATE(), GETDATE(), 1)";

            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter[] parameters2 = { };
            for (int i = 0; i < ageRangeIds.Count; i++)
            {
                string parameterName = "@AgeRangeId" + i;
                deleteQuery.Append(parameterName);

                if (i < ageRangeIds.Count - 1)
                {
                    deleteQuery.Append(", ");
                }

                parameters.Add(new SqlParameter(parameterName, ageRangeIds[i]));
            }

            deleteQuery.Append(");");

            _exQuery.ExecuteRawQuery(deleteQuery.ToString(), parameters.ToArray());
            _exQuery.ExecuteRawQuery(query2, parameters2);

            return new JsonResult("Age Range deleted successfully");
        }

    }
}
