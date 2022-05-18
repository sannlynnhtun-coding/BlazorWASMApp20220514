using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp20220514.Shared
{
    public class SaleModel
    {
        public int saleId { get; set; }
        public string productName { get; set; }
        public int qty { get; set; }
        public decimal price { get; set; }
        public DateTime saleDate { get; set; }
        public string country { get; set; }
        public string staffName { get; set; }
    }

    public class SaleReqModel
    {

    }

    public class SaleResModel : BaseApiResModel
    {
        public SaleModel itemSale { get; set; }
        public List<SaleModel> lstSale { get; set; }
        public int totalPageNo { get; set; }
    }

    public class BaseApiResModel
    {
        public ApiResModel response { get; set; }
    }

    public class ApiResModel
    {
        public string respCode { get; set; }
        public string respDesp { get; set; }
    }
}
