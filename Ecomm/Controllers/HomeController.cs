using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ecomm.Models;
using Ecomm.DataAccess;
using System.Threading.Tasks;

namespace Ecomm.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOrderDetailsProvider _orderDetailsProvider;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IOrderDetailsProvider orderDetailsProvider, ILogger<HomeController> logger)
        {
            _orderDetailsProvider = orderDetailsProvider;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var orderDetails = await _orderDetailsProvider.Get();
            return View(orderDetails);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
