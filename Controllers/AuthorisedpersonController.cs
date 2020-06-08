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
    public class AuthorisedpersonController : ControllerBase
    {
        private readonly TestingContext _context;

        public AuthorisedpersonController(TestingContext context)
        {
            _context = context;
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<int>> PostAuthorisedperson(Authorisedperson authorisedperson)
        {
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "ADD_AUTHORISED_PERSON";

            cmd.CommandType = CommandType.StoredProcedure;

            var idparam = new SqlParameter("USERID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            cmd.Parameters.Add(new SqlParameter("PFIRSTNAME", authorisedperson.Firstname));
            cmd.Parameters.Add(new SqlParameter("PSURNAME", authorisedperson.Surname));
            cmd.Parameters.Add(new SqlParameter("PEMAIL", authorisedperson.Email));
            cmd.Parameters.Add(new SqlParameter("PPASSWORD", authorisedperson.Password));
            cmd.Parameters.Add(new SqlParameter("PACCOUNTID", authorisedperson.Accountid));
            
            cmd.Parameters.Add(idparam);

            await cmd.Connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await cmd.Connection.CloseAsync();

            return (int)idparam.Value;
        }
    }
}
