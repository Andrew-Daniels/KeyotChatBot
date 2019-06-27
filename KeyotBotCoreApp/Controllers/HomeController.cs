using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KeyotBotCoreApp.Models;
using KeyotBotCoreApp.Services;
using KeyotBotCoreApp.Context;
using KeyotBotCoreApp.Context.Entities;
using MySql.Data.MySqlClient;

namespace KeyotBotCoreApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly CandidateContext _context;

        public HomeController(CandidateContext context)
        {
            _context = context;
        }

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
