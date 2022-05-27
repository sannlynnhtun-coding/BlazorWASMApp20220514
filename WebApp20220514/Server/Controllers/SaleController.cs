using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApp20220514.Shared;

namespace WebApp20220514.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ILogger<SaleController> _logger;
        private readonly IConfiguration _configuration;
        public SaleController(IConfiguration configuration, ILogger<SaleController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        // List
        [HttpGet("{pageNo:int}/{rowPerPage:int}")]
        public SaleResModel SaleList(int pageNo = 1, int rowPerPage = 10)
        {
            SaleResModel model = new SaleResModel();
            try
            {
                _logger.LogInformation("First Line");
                if (pageNo == 0)
                    pageNo = 1;
                _logger.LogInformation($"Get Page No {pageNo}");
                //int rowPerPage = 3;
                using (var db = new SqlConnection(_configuration.GetConnectionString("DbStr")))
                {
                    _logger.LogInformation($"using sql connection {_configuration.GetConnectionString("DbStr")}");

                    string query = "select * from TblSale with (nolock) order by SaleId desc";

                    _logger.LogInformation($"assign query {query}");

                    #region Get Total Page
                    int totalRowCount = db.Query<SaleModel>(query).Count();
                    int totalPage = 0;
                    var res = totalRowCount / rowPerPage;
                    var pageCount = totalRowCount / rowPerPage;
                    var pageCount2 = totalRowCount % rowPerPage;
                    if (pageCount2 > 0)
                        pageCount += 1;
                    totalPage = pageCount;
                    #endregion

                    List<SaleModel> lst = db.Query<SaleModel>(query).Skip(pageNo * rowPerPage - rowPerPage).Take(rowPerPage).ToList();
                    model.lstSale = lst;
                    model.totalPageNo = totalPage;
                    model.response = new ApiResModel
                    {
                        respCode = "000",
                        respDesp = "success"
                    };
                }
            }
            catch (Exception ex)
            {
                model.response = getError(ex);
            }
            string log = JsonConvert.SerializeObject(model, Formatting.Indented);
            _logger.LogInformation($"return data {log}");
            //Log.Information(log);
            return model;
        }

        // Get By Id
        [HttpGet("{id:int}")]
        public SaleResModel SaleEdit(int id)
        {
            SaleResModel model = new SaleResModel();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DbStr")))
                {
                    string editQuery = @"select * from TblSale with (nolock) where SaleId = @SaleId";
                    var item = db.Query<SaleModel>(editQuery, new { SaleId = id }).FirstOrDefault();
                    model.response = item != null ? getSuccess() : getError("no record found!");
                    model.itemSale = item;
                }
            }
            catch (Exception ex)
            {
                model.response = getError(ex);
            }
            return model;
        }

        // Insert
        [HttpPost]
        public async Task<SaleResModel> SaleAdd([FromBody] SaleModel reqModel)
        {
            SaleResModel model = new SaleResModel();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DbStr")))
                {
                    string insertQuery = @"INSERT INTO [dbo].[TblSale]
           ([ProductName]
           ,[Qty]
           ,[Price]
           ,[SaleDate]
           ,[Country]
           ,[StaffName])
     VALUES
           (@ProductName
           ,@Qty
           ,@Price
           ,@SaleDate
           ,@Country
           ,@StaffName)";
                    var count = await db.ExecuteAsync(insertQuery, new
                    {
                        ProductName = reqModel.productName,
                        Qty = reqModel.qty,
                        Price = reqModel.price,
                        SaleDate = reqModel.saleDate,
                        Country = reqModel.country,
                        StaffName = reqModel.staffName
                    });
                    model.response = count == 1 ? getSuccess() : getError("no record affect!");
                }
            }
            catch (Exception ex)
            {
                model.response = getError(ex);
            }
            return model;
        }

        // Update
        [HttpPut]
        public async Task<SaleResModel> SaleUpdate([FromBody] SaleModel reqModel)
        {
            SaleResModel model = new SaleResModel();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DbStr")))
                {
                    string insertQuery = @"UPDATE [dbo].[TblSale]
   SET [ProductName] = @ProductName
      ,[Qty] = @Qty
      ,[Price] = @Price
      ,[SaleDate] = @SaleDate
      ,[Country] = @Country
      ,[StaffName] = @StaffName
 WHERE SaleId = @SaleId";
                    var count = await db.ExecuteAsync(insertQuery, new
                    {
                        ProductName = reqModel.productName,
                        Qty = reqModel.qty,
                        Price = reqModel.price,
                        SaleDate = reqModel.saleDate,
                        Country = reqModel.country,
                        StaffName = reqModel.staffName,
                        SaleId = reqModel.saleId
                    });
                    model.response = count == 1 ? getSuccess("updated!") : getError("no record affect!");
                }
            }
            catch (Exception ex)
            {
                model.response = getError(ex);
            }
            return model;
        }

        // Delete
        [HttpDelete("{id:int}")]
        public async Task<SaleResModel> SaleDelete(int id)
        {
            SaleResModel model = new SaleResModel();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DbStr")))
                {
                    string insertQuery = @"delete from TblSale where SaleId = @SaleId";
                    var count = await db.ExecuteAsync(insertQuery, new
                    {
                        SaleId = id
                    });
                    model.response = count == 1 ? getSuccess("deleted!") : getError("no record affect!");
                }
            }
            catch (Exception ex)
            {
                model.response = getError(ex);
            }
            return model;
        }

        private ApiResModel getError(Exception ex)
        {
            ApiResModel model = new ApiResModel();
            string msg = ex.Message + ex.StackTrace;
            model.respCode = "999";
            model.respDesp = msg;
            return model;
        }

        private ApiResModel getError(string msg)
        {
            ApiResModel model = new ApiResModel();
            model.respCode = "999";
            model.respDesp = msg;
            return model;
        }

        private ApiResModel getSuccess(string msg = "success")
        {
            ApiResModel model = new ApiResModel();
            model.respCode = "000";
            model.respDesp = msg;
            return model;
        }
    }
}