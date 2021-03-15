using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimchaSite.data;
using SimchaSite.web.Models;

namespace SimchaSite.web.Controllers
{
    public class SimchosController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress; Initial Catalog=SimchasTable;Integrated Security=true;";
        [HttpPost]
        public IActionResult AddSimcha(string name, DateTime date)
        {
            DbManager db = new DbManager(_connectionString);
            db.AddSimcha(name, date);
            return Redirect("/home/index");

        }
        public IActionResult ViewContributions(int simchaId)
        {
            ContributionsViewModel vm = new ContributionsViewModel();
            DbManager db = new DbManager(_connectionString);
            List<Simcha> simchaList = db.GetAllSimchos();
            Simcha simcha = simchaList.FirstOrDefault(s => s.Id == simchaId);
            vm.Simcha = simcha;
         
                List<Contributer> contributerList = db.GetContributers();
                vm.Contributers = contributerList;
            List<Transaction> contribs = db.GetContributions();
            vm.Contribs = contribs.Where(c => c.SimchaId == simchaId).ToList();
            return View(vm);
        }
        public IActionResult UpdateContributions(int simchaId, List<Transaction> transactions)
        {
            DbManager db = new DbManager(_connectionString);
            List<Transaction> currents = transactions.Where(t => t.Include).ToList();
            db.ClearAndReplace(simchaId, currents);
            return Redirect("/home/index");
        }
    }
}
