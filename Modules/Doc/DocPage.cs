
using Microsoft.AspNetCore.Hosting;

namespace WinAuthSample;

[Route("Doc/[action]")]
public class DocPage : Controller
{
 
    [PageAuthorize, HttpGet, Route("~/Doc")]
    public ActionResult Index()
    {
        return View(MVC.Views.Doc.DocPage);
    }
}
