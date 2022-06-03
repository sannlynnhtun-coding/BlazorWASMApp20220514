using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApp20220514.Client.Pages.Components;

namespace WebApp20220514.Client.Services
{
    public class InjectService : IInjectService
    {
        private readonly IJSRuntime jSRuntime;
        private readonly NavigationManager NavigationManager;
        private readonly IDialogService DialogService;
        private readonly HttpClient HttpClient;

        public InjectService(IJSRuntime jSRuntime, NavigationManager NavigationManager,
            IDialogService DialogService,
            HttpClient HttpClient
            )
        {
            this.jSRuntime = jSRuntime;
            this.NavigationManager = NavigationManager;
            this.DialogService = DialogService;
            this.HttpClient = HttpClient;
        }

        public async Task DatePicker(string id)
        {
            await jSRuntime.InvokeVoidAsync("datePicker2", id);
        }

        public void Go(string url, bool forceReload = false)
        {
            NavigationManager.NavigateTo(url, forceReload);
        }

        public void ShowMsg(string msg, string url = null)
        {
            DialogParameters parameters = new DialogParameters();
            parameters.Add("msg", msg);
            parameters.Add("url", url);
            DialogService.Show<MessageBox>("", parameters);
        }

        public async Task<T> ExecuteApiAsync<T>(string url, object param = null, EnumHttpMethod httpMethod = EnumHttpMethod.Post)
        {
            Log("ExecuteApiAsync Started!");
            HttpResponseMessage httpResponseMessage;
            T model = default(T);
            try
            {
                switch (httpMethod)
                {
                    case EnumHttpMethod.Get:
                        Log("ExecuteApiAsync Get!");
                        model = await HttpClient.GetFromJsonAsync<T>(url);
                        break;
                    case EnumHttpMethod.Delete:
                        Log("ExecuteApiAsync Delete!");
                        httpResponseMessage = await HttpClient.DeleteAsync(url);
                        Log("IsSucess => " + httpResponseMessage.IsSuccessStatusCode);
                        if (httpResponseMessage.IsSuccessStatusCode)
                        {
                            string data = await httpResponseMessage.Content.ReadAsStringAsync();
                            Log($"data => {data}");
                            model = JsonSerializer.Deserialize<T>(data);
                        }
                        break;
                    default:
                    case EnumHttpMethod.Post:
                        Log("ExecuteApiAsync Post!");
                        StringContent reqJson = new StringContent(JsonSerializer.Serialize(param), Encoding.UTF8, "application/json");
                        httpResponseMessage = await HttpClient.PostAsync(url, reqJson);
                        Log("IsSucess => " + httpResponseMessage.IsSuccessStatusCode);
                        if (httpResponseMessage.IsSuccessStatusCode)
                        {
                            string data = await httpResponseMessage.Content.ReadAsStringAsync();
                            Log($"data => {data}");
                            model = JsonSerializer.Deserialize<T>(data);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Log($"ExecuteApiAsync! => {ex.Message} {ex.StackTrace}");
            }
            Log("ExecuteApiAsync Ended!");
            Log(JsonSerializer.Serialize(model));
            return model;
        }

        private void Log(string msg)
        {
            Console.WriteLine(msg);
        }

        public async Task<bool> ConfirmMessage(string title, string message, EnumConfirmButton confirmButton = EnumConfirmButton.Success)
        {
            #region Check Color
            Color c = Color.Success;
            switch (confirmButton)
            {
                case EnumConfirmButton.Info:
                    c = Color.Info;
                    break;
                case EnumConfirmButton.Warning:
                    c = Color.Warning;
                    break;
                case EnumConfirmButton.Delete:
                    c = Color.Error;
                    break;
                default:
                case EnumConfirmButton.Success:
                    c = Color.Success;
                    break;
            }
            #endregion

            var parameters = new DialogParameters();
            parameters.Add("Message", message);
            parameters.Add("BtnColor", c);
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

            var dialog = DialogService.Show<ConfirmMessageBox>(title, parameters, options);
            var result = await dialog.Result;
            return !result.Cancelled;
        }
    }
}

public enum EnumHttpMethod
{
    Get,
    Post,
    Delete
}

public enum EnumConfirmButton
{
    Success,
    Info,
    Warning,
    Delete
}
