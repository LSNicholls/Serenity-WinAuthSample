using Serenity.Services;
using MyRequest = Serenity.Services.SaveRequest<WinAuthSample.Membership.MyProfileRow>;
using MyResponse = Serenity.Services.SaveResponse;
using MyRow = WinAuthSample.Membership.MyProfileRow;

namespace WinAuthSample.Membership
{
    public interface IMyProfileSaveHandler : ISaveHandler<MyRow, MyRequest, MyResponse> {}

    public class MyProfileSaveHandler : SaveRequestHandler<MyRow, MyRequest, MyResponse>, IMyProfileSaveHandler
    {
        public MyProfileSaveHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}