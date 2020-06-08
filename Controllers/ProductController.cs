using Dip_DatabaseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Dip_DatabaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly TestingContext _context;

        public ProductController(TestingContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            return await _context.Product.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            Product product;

            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "GET_PRODUCT_BY_ID";

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("PPRODID", id));

            await cmd.Connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            using (DbDataReader reader = cmd.ExecuteReader())
            {
                reader.Read();

                product = new Product() { Productid = (int)reader["PRODUCTID"], Prodname = (string)reader["PRODNAME"], Buyprice = (decimal)reader["BUYPRICE"], Sellprice = (decimal)reader["SELLPRICE"] };
            }

            await cmd.Connection.CloseAsync();

            return product;
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<int>> PostProduct(Product product)
        {
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "ADD_PRODUCT";

            cmd.CommandType = CommandType.StoredProcedure;

            var idparam = new SqlParameter("PRODUCTID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            cmd.Parameters.Add(new SqlParameter("PPRODNAME", product.Prodname));
            cmd.Parameters.Add(new SqlParameter("PBUYPRICE", product.Buyprice));
            cmd.Parameters.Add(new SqlParameter("PSELLPRICE", product.Sellprice));
            cmd.Parameters.Add(idparam);

            await cmd.Connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await cmd.Connection.CloseAsync();

            return (int)idparam.Value;
        }
    }
}
