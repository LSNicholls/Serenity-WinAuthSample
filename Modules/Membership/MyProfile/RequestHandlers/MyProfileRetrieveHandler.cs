using Serenity.Data;
using Serenity.Services;
using MyRequest = Serenity.Services.RetrieveRequest;
using MyResponse = Serenity.Services.RetrieveResponse<WinAuthSample.Membership.MyProfileRow>;
using MyRow = WinAuthSample.Membership.MyProfileRow;

namespace WinAuthSample.Membership
{
    public interface IMyProfileRetrieveHandler : IRetrieveHandler<MyRow, MyRequest, MyResponse> {}

    public class MyProfileRetrieveHandler : RetrieveRequestHandler<MyRow, MyRequest, MyResponse>, IMyProfileRetrieveHandler
    {
        public MyProfileRetrieveHandler(IRequestContext context)
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