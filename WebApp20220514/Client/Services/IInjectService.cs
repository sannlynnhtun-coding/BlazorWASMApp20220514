using System.Threading.Tasks;

namespace WebApp20220514.Client.Services
{
    public interface IInjectService
    {
        Task<bool> ConfirmMessage(string title, string message, EnumConfirmButton confirmButton = EnumConfirmButton.Success);
        Task DatePicker(string id);
        Task<T> ExecuteApiAsync<T>(string url, object param = null, EnumHttpMethod httpMethod = EnumHttpMethod.Post);
        void Go(string url, bool forceReload = false);
        void ShowMsg(string msg, string url = null);
    }
}