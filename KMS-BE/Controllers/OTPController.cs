using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using KMS.Tools;
using System.Data;
using Microsoft.Data.SqlClient;
using Twilio.Rest.Api.V2010.Account;
using Twilio;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OTPController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public OTPController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        

        [HttpPost("SendText")]
        public ActionResult SendText(string phoneNumber)
        {
            string accountSid = "AC1f43d0e6021680961add2f212b319f74";
            string authToken = "61b3ba665de84b301a75540ee16b5711";
            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: "Hi",
                from: new Twilio.Types.PhoneNumber("+19302031874"),
                to: new Twilio.Types.PhoneNumber(phoneNumber)
            );




            return StatusCode(200, new { message = message.Sid });
        }


    }
}