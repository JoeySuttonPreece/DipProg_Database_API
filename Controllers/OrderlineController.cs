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
    public class OrderlineController : ControllerBase
    {
        private readonly TestingContext _context;

        public OrderlineController(TestingContext context)
        {
            _context = context;
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Orderline>> PostOrderline(Orderline orderline)
        {
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "ADD_PRODUCT_TO_ORDER";

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("PORDERID", orderline.Orderid));
            cmd.Parameters.Add(new SqlParameter("PPRODIID", orderline.Productid));
            cmd.Parameters.Add(new SqlParameter("PQTY", orderline.Quantity));
            cmd.Parameters.Add(new SqlParameter("DISCOUNT", orderline.Discount));

            await cmd.Connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await cmd.Connection.CloseAsync();

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult<Orderline>> DeleteOrderline(Orderline orderline)
        {
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "REMOVE_PRODUCT_FROM_ORDER";

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("PORDERID", orderline.Orderid));
            cmd.Parameters.Add(new SqlParameter("PPRODIID", orderline.Productid));

            await cmd.Connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await cmd.Connection.CloseAsync();

            return Ok();
        }
    }
}
