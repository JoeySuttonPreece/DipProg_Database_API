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
    public class PurchaseController : ControllerBase
    {
        private readonly TestingContext _context;

        public PurchaseController(TestingContext context)
        {
            _context = context;
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Object>> PostPurchaseorder(Purchaseorder purchaseorder)
        {
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "PURCHASE_STOCK";

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("PPRODID", purchaseorder.Productid));
            cmd.Parameters.Add(new SqlParameter("PLOCID", purchaseorder.Locationid));
            cmd.Parameters.Add(new SqlParameter("PQTY", purchaseorder.Quantity));

            await cmd.Connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await cmd.Connection.CloseAsync();

            return Ok();
        }
    }
}
