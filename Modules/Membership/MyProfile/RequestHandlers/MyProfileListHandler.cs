using Serenity.Data;
using Serenity.Services;
using MyRequest = Serenity.Services.ListRequest;
using MyResponse = Serenity.Services.ListResponse<WinAuthSample.Membership.MyProfileRow>;
using MyRow = WinAuthSample.Membership.MyProfileRow;

namespace WinAuthSample.Membership
{
    public interface IMyProfileListHandler : IListHandler<MyRow, MyRequest, MyResponse> {}

    public class MyProfileListHandler : ListRequestHandler<MyRow, MyRequest, MyResponse>, IMyProfileListHandler
    {
        public MyProfileListHandler(IRequestContext context)
             : base(context)
        {
        }

        protected override void PrepareQuery(SqlQuery query)
        {
            var flds = MyRow.Fields;

            base.PrepareQuery(query);

            query.Where(new Criteria(flds.Username) == Context.User.Identity.Name);
        }
    }
}