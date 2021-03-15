using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimchaSite.data;
namespace SimchaSite.web.Models
{
    public class SimchosViewModel
    {
        public List<Simcha> Simchos { get; set; }
        public int TotalContributers { get; set; }
    }
    public class ContributerViewModel
    {
        public List<Contributer> Contributers { get; set; }
        public decimal TotalAmount { get; set; }
    }
    public class HistoryViewModel
    {
        public Contributer Contributer { get; set; }
        public List<Simcha> Simcha { get; set; }
        public List<Transaction> Contributions { get; set; }
    }
    public class ContributionsViewModel
    {
        public Simcha Simcha { get; set; }
        public List<Contributer> Contributers { get; set; }
       public List<Transaction> Contribs { get; set; }
       
    }
}
