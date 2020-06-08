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
    public class OrderController : ControllerBase
    {
        private readonly TestingContext _context;

        public OrderController(TestingContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
        {
            List<Order> openOrders = new List<Order>();

            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "GET_OPEN_ORDERS";

            cmd.CommandType = CommandType.StoredProcedure;

            await cmd.Connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            using (DbDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    openOrders.Add(new Order()
                    {
                        Orderid = (int)reader["ORDERID"],
                        Shippingaddress = (string)reader["SHIPPINGADDRESS"],
                        Datetimecreated = (DateTime)reader["DATETIMECREATED"],
                        Total = (decimal)reader["TOTAL"],
                        Userid = (int)reader["USERID"]
                    });
                }
            }

            await cmd.Connection.CloseAsync();

            return openOrders;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            Order order;

            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "GET_ORDER_BY_ID";

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("PORDERID", id));

            await cmd.Connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            using (DbDataReader reader = cmd.ExecuteReader())
            {
                await reader.ReadAsync();

                order = new Order() {
                    Orderid = (int)reader["ORDERID"],
                    Shippingaddress = (string)reader["SHIPPINGADDRESS"],
                    Datetimecreated = (DateTime)reader["DATETIMECREATED"],
                    Datetimedispatched = (DateTime)reader["DATETIMEDISPATCHED"],
                    Total = (decimal)reader["TOTAL"],
                    Userid = (int)reader["USERID"]
                };

                await reader.NextResultAsync();

                while (reader.Read())
                {
                    order.Orderline.Add(new Orderline()
                    {
                        Orderid = (int)reader["ORDERID"],
                        Productid = (int)reader["PRODUCTID"],
                        Quantity = (int)reader["QUANTITY"],
                        Discount = (decimal)reader["DISCOUNT"],
                        Subtotal = (decimal)reader["SUBTOTAL"]
                    });
                }
            }

            await cmd.Connection.CloseAsync();

            return order;
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<int>> PostOrder(Order order)
        {
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "CREATE_ORDER";

            cmd.CommandType = CommandType.StoredProcedure;

            var idparam = new SqlParameter("ORDERID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            cmd.Parameters.Add(new SqlParameter("PSHIPPINGADDRESS", order.Shippingaddress));
            cmd.Parameters.Add(new SqlParameter("PUSERID", order.Userid));
            cmd.Parameters.Add(idparam);

            await cmd.Connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await cmd.Connection.CloseAsync();

            return (int)idparam.Value;
        }

        [HttpPut]
        public async Task<ActionResult<Order>> PutOrder(Order order)
        {
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "FULLFILL_ORDER";

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("PORDERID", order.Orderid));

            await cmd.Connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await cmd.Connection.CloseAsync();

            return Ok();
        }
    }
}
