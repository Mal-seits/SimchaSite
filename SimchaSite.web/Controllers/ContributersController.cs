using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimchaSite.web.Models;
using SimchaSite.data;

namespace SimchaSite.web.Controllers
{
    public class ContributersController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress; Initial Catalog=SimchasTable;Integrated Security=true;";

        public IActionResult Contributers()
        {
            DbManager db = new DbManager(_connectionString);
            List<Contributer> contributers = db.GetContributers();
            decimal total = 0;
            foreach (Contributer c in contributers)
            {
                total += c.Balance;
            }
            ContributerViewModel vm = new ContributerViewModel();
            vm.Contributers = contributers;
            vm.TotalAmount = total;
            return View(vm);
        }
        [HttpPost]
        public IActionResult NewContributer(string firstName, string lastName, string cellNumber, decimal initialDeposit, DateTime createdDate, bool alwaysInclude)
        {
            DbManager db = new DbManager(_connectionString);
            db.AddContributer(firstName, lastName, cellNumber, initialDeposit, createdDate, alwaysInclude);
            return Redirect("/contributers/contributers");
        }
        [HttpPost]
        public IActionResult Deposit(decimal amount, DateTime date, int contributerId)
        {
            DbManager db = new DbManager(_connectionString);
            db.AddDeposit(amount, date, contributerId);
            return Redirect("/contributers/contributers");
        }
        public IActionResult Edit(string firstName, string lastName, string cellNumber, DateTime createdDate, bool alwaysInclude, int contributerId)
        {
            DbManager db = new DbManager(_connectionString);
            db.EditContributer(firstName, lastName, cellNumber, createdDate, alwaysInclude, contributerId);
            return Redirect("/contributers/contributers");
        }
        public IActionResult History(int contributerId)
        {
            HistoryViewModel vm = new HistoryViewModel();
            DbManager db = new DbManager(_connectionString);
            List<Contributer> contributerList = db.GetContributers();
            Contributer currentContributer = contributerList.FirstOrDefault(c => c.Id == contributerId);
            vm.Contributer = currentContributer;
            List<Transaction> contribs = db.GetContributions();

            List<Transaction> currents = contribs.Where(c => c.ContributerId == contributerId).ToList();

            vm.Simcha = db.GetAllSimchos();
           
            vm.Contributions = currents;
            return View(vm);
        }
    }
}
