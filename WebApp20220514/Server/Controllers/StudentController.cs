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

        [HttpGet("{id:int}")]
        public async Task<StudentModel> StudentEdit(int id)
        {
            using (var connection = new SqlConnection(configuration.GetConnectionString("DbStr")))
            {
                var item = await connection.QueryAsync<StudentModel>("select * from tblstudent where StudentId = @StudentId", new { StudentId = id });
                return item.FirstOrDefault();
            }
        }

        [HttpPost]
        public async Task<ResponseModel> StudentAdd([FromBody] StudentModel studentModel)
        {
            using (var connection = new SqlConnection(configuration.GetConnectionString("DbStr")))
            {
                string query = @"INSERT INTO [dbo].[TblStudent]
           ([StudentCode]
           ,[StudentName])
     VALUES
           (@StudentCode
           ,@StudentName)";
                var res = await connection.ExecuteAsync(query, new
                {
                    StudentName = studentModel.studentName,
                    StudentCode = studentModel.studentCode
                });
                ResponseModel model = new ResponseModel();
                model.respCode = res == 1 ? EnumRespCode.success : EnumRespCode.error;
                model.respDesp = model.respCode.ToString();
                return model;
            }
        }

        [HttpPut("{id=int}")]
        public async Task<ResponseModel> StudentUpdate(int id, [FromBody] StudentModel studentModel)
        {
            using (var connection = new SqlConnection(configuration.GetConnectionString("DbStr")))
            {
                string query = @"UPDATE [dbo].[TblStudent]
   SET [StudentCode] = @StudentCode
      ,[StudentName] =@StudentName
 WHERE StudentId = @StudentId";
                var res = await connection.ExecuteAsync(query, new
                {
                    StudentId = studentModel.studentId,
                    StudentCode = studentModel.studentCode,
                    StudentName = studentModel.studentName
                });
                ResponseModel model = new ResponseModel();
                model.respCode = res == 1 ? EnumRespCode.success : EnumRespCode.error;
                model.respDesp = model.respCode.ToString();
                return model;
            }
        }

        [HttpDelete("{id=int}")]
        public bool StudentDelete(int id)
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
