using Notes.Models;
using System.Web.Mvc;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Configuration;
using System.Threading.Tasks;

namespace Заметки.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

    }
}