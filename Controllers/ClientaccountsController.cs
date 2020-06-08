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
    public class AccountController : ControllerBase
    {
        private readonly TestingContext _context;

        public AccountController(TestingContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clientaccount>>> GetClientaccount()
        {
            return await _context.Clientaccount.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Clientaccount>> GetClientaccount(int id)
        {
            Clientaccount clientaccount;

            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "GET_CLIENT_ACCOUNT_BY_ID";

            cmd.CommandType = CommandType.StoredProcedure;

            var acctnameparam = new SqlParameter("@ACCTNAME", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output };
            var balanceparam = new SqlParameter("@BALANCE", SqlDbType.Money) { Direction = ParameterDirection.Output };
            var creditparam = new SqlParameter("@CREDITLIMIT", SqlDbType.Money) { Direction = ParameterDirection.Output };

            cmd.Parameters.Add(new SqlParameter("PACCOUNTID", id));
            cmd.Parameters.AddRange(new SqlParameter[] { acctnameparam, balanceparam, creditparam });

            await cmd.Connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            using (DbDataReader reader = cmd.ExecuteReader())
            {
                clientaccount = new Clientaccount() { Accountid = id };

                while (reader.Read())
                {
                    clientaccount.Authorisedperson.Add(new Authorisedperson() {
                        Userid = (int)reader["USERID"],
                        Firstname = (string)reader["FIRSTNAME"],
                        Surname = (string)reader["SURNAME"],
                        Email = (string)reader["EMAIL"],
                        Password = (string)reader["PASSWORD"],
                        Accountid = (int)reader["ACCOUNTID"]
                    });
                }
            }

            await cmd.Connection.CloseAsync();

            clientaccount.Acctname = (string)acctnameparam.Value;
            clientaccount.Balance = (decimal)balanceparam.Value;
            clientaccount.Creditlimit = (decimal)creditparam.Value;

            return clientaccount;
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<int>> PostClientaccount(Clientaccount clientaccount)
        {
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "ADD_CLIENT_ACCOUNT";

            cmd.CommandType = CommandType.StoredProcedure;

            var idparam = new SqlParameter("ACCOUNTID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            cmd.Parameters.Add(new SqlParameter("PACCTNAME", clientaccount.Acctname));
            cmd.Parameters.Add(new SqlParameter("PBALANCE", clientaccount.Balance));
            cmd.Parameters.Add(new SqlParameter("PCREDITLIMIT", clientaccount.Creditlimit));
            cmd.Parameters.Add(idparam);

            await cmd.Connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await cmd.Connection.CloseAsync();

            return (int)idparam.Value;
        }
    }
}
