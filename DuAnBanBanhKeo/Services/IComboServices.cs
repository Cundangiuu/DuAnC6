using Microsoft.AspNetCore.Mvc;

namespace DuAnBanBanhKeo.Services
{
    public class IComboServices : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
