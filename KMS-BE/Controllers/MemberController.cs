using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;

        public MemberController(IConfiguration configuration, KioskManagementSystemContext _context)
        {
            _dbcontext = _context;
            _configuration = configuration;
        }

        private DataTable ExecuteRawQuery(string query, SqlParameter[] parameters = null)
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

        [HttpGet]
        [Route("ShowMember")]
        public JsonResult GetMember()
        {
            string query = "select a.id, a.memberId, a.contractId, m.phone, m.department, m.companyName, m.bankName, m.address1, m.isActive, m.dateCreated " +
                "from LAccount a, LMember m " +
                "where a.memberId = m.id";
            DataTable table = ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowMember/{id}")]
        public JsonResult GetMemberById(int id)
        {
            string query = "select a.id, a.memberId, a.contractId, m.phone, m.department, m.companyName, m.bankName, m.address1, m.isActive, m.dateCreated " +
                "from LAccount a, LMember m " +
                "where a.memberId = m.id and a.id=@Id";

            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable table = ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Member not found");
            }
        }

        [HttpPost]
        [Route("AddMember")]
        public JsonResult AddMember([FromBody] Lmember member)
        {
            string query = "INSERT INTO LMember (phone, department, companyName, bankName, address1, isActive, dateCreated) " +
                           "VALUES (@Phone, @Department, @CompanyName, @BankName, @Address1, @IsActive, GETDATE())";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Phone", member.Phone),
                new SqlParameter("@Department", member.Department),
                new SqlParameter("@CompanyName", member.CompanyName),
                new SqlParameter("@BankName", member.BankName),
                new SqlParameter("@Address1", member.Address1),
                new SqlParameter("@IsActive", member.IsActive)
            };

            ExecuteRawQuery(query, parameters);

            return new JsonResult("Member added successfully");
        }

        [HttpPut]
        [Route("UpdateMember/{id}")]
        public JsonResult UpdateMember(int id, [FromBody] Lmember member)
        {
            string query = "UPDATE LMember SET phone=@Phone, department=@Department, companyName=@CompanyName, " +
                           "bankName=@BankName, address1=@Address1, isActive=@IsActive " +
                           "WHERE id=@Id";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Phone", member.Phone),
                new SqlParameter("@Department", member.Department),
                new SqlParameter("@CompanyName", member.CompanyName),
                new SqlParameter("@BankName", member.BankName),
                new SqlParameter("@Address1", member.Address1),
                new SqlParameter("@IsActive", member.IsActive)
            };

            ExecuteRawQuery(query, parameters);

            return new JsonResult("Member updated successfully");
        }

        [HttpDelete]
        [Route("DeleteMember/{id}")]
        public JsonResult DeleteMember(int id)
        {
            string query = "DELETE FROM LMember WHERE id=@Id";

            SqlParameter parameter = new SqlParameter("@Id", id);
            ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult("Member deleted successfully");
        }

        [HttpGet]
        [Route("SearchMember")]
        public JsonResult SearchMember(string searchQuery)
        {
            string query = "SELECT id, phone, department, companyName, bankName, address1, isActive, dateCreated " +
                           "FROM LMember " +
                           "WHERE id LIKE @searchQuery OR " +
                           "phone LIKE @searchQuery OR " +
                           "department LIKE @searchQuery OR " +
                           "companyName LIKE @searchQuery OR " +
                           "bankName LIKE @searchQuery OR " +
                           "address1 LIKE @searchQuery";

            SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
            DataTable table = ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }

    }
}
