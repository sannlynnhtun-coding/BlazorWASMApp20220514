using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp20220514.Client.Pages.Components;

namespace WebApp20220514.Client.Services
{
    public class InjectService : IInjectService
    {
        private readonly IJSRuntime jSRuntime;
        private readonly NavigationManager NavigationManager;
        private readonly IDialogService DialogService;

        public InjectService(IJSRuntime jSRuntime, NavigationManager NavigationManager, IDialogService DialogService)
        {
            this.jSRuntime = jSRuntime;
            this.NavigationManager = NavigationManager;
            this.DialogService = DialogService;
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
    }
}
