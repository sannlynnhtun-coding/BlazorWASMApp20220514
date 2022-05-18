using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApp20220514.Shared;

namespace WebApp20220514.Server.Controllers
{
    [Route("api/salemvc")]
    public class SaleMVCController : Controller
    {
        private readonly IConfiguration _configuration;
        public SaleMVCController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // List
        [HttpGet]
        public SaleResModel SaleList()
        {
            SaleResModel model = new SaleResModel();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DbStr")))
                {
                    string query = "select * from TblSale with (nolock)";
                    List<SaleModel> lst = db.Query<SaleModel>(query).ToList();
                    model.lstSale = lst;
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
