using Microsoft.AspNetCore.Mvc;
using Serenity.Web;

namespace WinAuthSample.Membership.Pages 
{

    [PageAuthorize(typeof(MyProfileRow))]
    public class MyProfileController : Controller
    {
        [Route("Membership/MyProfile")]
        public ActionResult Index()
        {
            ////   return View(MVC.Views.Membership.MyProfile.Index); //I would prefer this but can't get it to work.
            return this.GridPage("@/Membership/MyProfile/MyProfilePage", MyProfileRow.Fields.PageTitle());
        }
    }
}