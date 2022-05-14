using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApp20220514.Shared;
using Dapper;

namespace WebApp20220514.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public StudentController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<StudentModel> StudentList()
        {
            using (var connection = new SqlConnection(configuration.GetConnectionString("DbStr")))
            {
                var lst = connection.Query<StudentModel>("select * from tblstudent");
                return lst;
            }
        }

        [HttpDelete("{id=int}")]
        public bool DeleteById(int id)
        {
            bool res = false;
            try
            {
                using (var connection = new SqlConnection(configuration.GetConnectionString("DbStr")))
                {
                    var count = connection.Execute("delete from tblstudent where studentid = @StudentId", new { StudentId = id });
                    res = count > 0;
                }
            }
            catch
            {

            }
            return res;
        }
    }
}
