using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dip_DatabaseAPI;
using Dip_DatabaseAPI.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace Dip_DatabaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly TestingContext _context;

        public PaymentController(TestingContext context)
        {
            _context = context;
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Accountpayment>> PostAccountpayment(Accountpayment accountpayment)
        {
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "MAKE_ACCOUNT_PAYMENT";

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("PACCOUNTID", accountpayment.Accountid));
            cmd.Parameters.Add(new SqlParameter("PAMOUNT", accountpayment.Amount));

            await cmd.Connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await cmd.Connection.CloseAsync();

            return Ok();
        }
    }
}
