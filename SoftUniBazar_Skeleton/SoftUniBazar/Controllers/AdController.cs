using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SoftUniBazar.Controllers
{
    public class AdController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
