using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimchaSite.web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SimchaSite.data;

namespace SimchaSite.web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress; Initial Catalog=SimchasTable;Integrated Security=true;";
        public IActionResult Index()
        {
            DbManager db = new DbManager(_connectionString);
            SimchosViewModel vm = new SimchosViewModel();
            List<Simcha> list  = db.GetAllSimchos();
            vm.Simchos = list;
            vm.TotalContributers = db.GetTotalContributers();
            return View(vm);
        }
     
    }
}
