using Serenity.Services;
using MyRequest = Serenity.Services.DeleteRequest;
using MyResponse = Serenity.Services.DeleteResponse;
using MyRow = WinAuthSample.Administration.WindowsGroupRow;

namespace WinAuthSample.Administration
{ 
    public interface IWindowsGroupDeleteHandler : IDeleteHandler<MyRow, MyRequest, MyResponse> {}

    public class WindowsGroupDeleteHandler : DeleteRequestHandler<MyRow, MyRequest, MyResponse>, IWindowsGroupDeleteHandler
    {
        public WindowsGroupDeleteHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}