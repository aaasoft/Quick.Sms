using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Quick.Sms.BlazorServer.Pages
{
    public partial class Index
    {
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            var imageInfo = Agent.Instance.ContainerInfo?.Image;
            if (imageInfo == null)
                await setTitle("短信工具 Debug version");
            else
                await setTitle($"{imageInfo.Name} v{imageInfo.Version}");
        }

        private async Task setTitle(string title)
        {
            await JSRuntime.InvokeVoidAsync("setTitle", title);
        }
    }
}
