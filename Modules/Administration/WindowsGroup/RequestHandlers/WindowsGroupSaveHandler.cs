using Serenity.Services;
using MyRequest = Serenity.Services.SaveRequest<WinAuthSample.Administration.WindowsGroupRow>;
using MyResponse = Serenity.Services.SaveResponse;
using MyRow = WinAuthSample.Administration.WindowsGroupRow;

namespace WinAuthSample.Administration
{
    public interface IWindowsGroupSaveHandler : ISaveHandler<MyRow, MyRequest, MyResponse> {}

    public class WindowsGroupSaveHandler : SaveRequestHandler<MyRow, MyRequest, MyResponse>, IWindowsGroupSaveHandler
    {
        public WindowsGroupSaveHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}