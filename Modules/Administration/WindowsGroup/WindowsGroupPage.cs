using Microsoft.AspNetCore.Mvc;
using Serenity.Web;

namespace WinAuthSample.Administration.Pages 
{

    [PageAuthorize(typeof(WindowsGroupRow))]
    public class WindowsGroupPage : Controller
    {
        [Route("Membership/WindowsGroup")]
        public ActionResult Index()
        {
            return this.GridPage("@/Administration/WindowsGroup/WindowsGroupPage",
                WindowsGroupRow.Fields.PageTitle());
        }
    }
}