using Serenity.Services;
using MyRequest = Serenity.Services.ListRequest;
using MyResponse = Serenity.Services.ListResponse<WinAuthSample.Administration.WindowsGroupRow>;
using MyRow = WinAuthSample.Administration.WindowsGroupRow;

namespace WinAuthSample.Administration
{
    public interface IWindowsGroupListHandler : IListHandler<MyRow, MyRequest, MyResponse> {}

    public class WindowsGroupListHandler : ListRequestHandler<MyRow, MyRequest, MyResponse>, IWindowsGroupListHandler
    {
        public WindowsGroupListHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}