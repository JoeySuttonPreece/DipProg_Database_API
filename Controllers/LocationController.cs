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
using Newtonsoft.Json.Linq;

namespace Dip_DatabaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly TestingContext _context;

        public LocationController(TestingContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocation()
        {
            return await _context.Location.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocation(string id)
        {
            Location location;

            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "GET_LOCATION_BY_ID";

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("PLOCID", id));

            await cmd.Connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            using (DbDataReader reader = cmd.ExecuteReader())
            {
                reader.Read();

                location = new Location() { Locationid = (string)reader["LOCATIONID"], Locname = (string)reader["LOCNAME"], Address = (string)reader["ADDRESS"], Manager = (string)reader["MANAGER"] };
            }

            await cmd.Connection.CloseAsync();

            return location;
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<string>> PostLocation(Location location)
        {
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "ADD_LOCATION";

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("PLOCID", location.Locationid));
            cmd.Parameters.Add(new SqlParameter("PLOCNAME", location.Locname));
            cmd.Parameters.Add(new SqlParameter("PLOCADDRESS", location.Address));
            cmd.Parameters.Add(new SqlParameter("PMANAGER", location.Manager));

            await cmd.Connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await cmd.Connection.CloseAsync();

            return location.Locationid;
        }
    }
}
