using Serenity.Services;
using MyRequest = Serenity.Services.RetrieveRequest;
using MyResponse = Serenity.Services.RetrieveResponse<WinAuthSample.Administration.WindowsGroupRow>;
using MyRow = WinAuthSample.Administration.WindowsGroupRow;

namespace WinAuthSample.Administration
{
    public interface IWindowsGroupRetrieveHandler : IRetrieveHandler<MyRow, MyRequest, MyResponse> {}

    public class WindowsGroupRetrieveHandler : RetrieveRequestHandler<MyRow, MyRequest, MyResponse>, IWindowsGroupRetrieveHandler
    {
        public WindowsGroupRetrieveHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}