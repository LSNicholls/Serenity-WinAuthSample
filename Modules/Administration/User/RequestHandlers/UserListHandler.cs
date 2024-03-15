using MyRow = WinAuthSample.Administration.UserRow;
using MyRequest = WinAuthSample.Administration.UserListRequest;
using MyResponse = Serenity.Services.ListResponse<WinAuthSample.Administration.UserRow>;

namespace WinAuthSample.Administration;

public interface IUserListHandler : IListHandler<MyRow, MyRequest, MyResponse> { }

public class UserListHandler : ListRequestHandler<MyRow, MyRequest, MyResponse>, IUserListHandler
{
    public UserListHandler(IRequestContext context)
         : base(context)
    {
    }
}