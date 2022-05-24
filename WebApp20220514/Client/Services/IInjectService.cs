using System.Threading.Tasks;

namespace WebApp20220514.Client.Services
{
    public interface IInjectService
    {
        Task DatePicker(string id);
        void Go(string url, bool forceReload = false);
        void ShowMsg(string msg, string url = null);
    }
}