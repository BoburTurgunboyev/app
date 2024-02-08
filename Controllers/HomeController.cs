using Microsoft.AspNetCore.Mvc;
using MyProject.Domain.Models;
using MyProject.Infrastructure.Repositories;
using MyProject.Models;
using MyProject.Services.Services;
using System.Diagnostics;

namespace MyProject.Controllers
{
    public class HomeController : Controller
    {
       

        public IActionResult Index()
        {
            return View();
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
