using System;
using System.Collections.Generic;
using System.Text;

namespace SimchaSite.data
{
    public class Simcha
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Total { get; set; }
        public List<Transaction> Contributions { get; set; }
    }
    public class Contributer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cell { get; set; }
        public DateTime CreatedDate { get; set; }   
        public bool AlwaysInclude { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
    public class Transaction
    {
        public int Id { get; set; }
        public int ContributerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? Date { get; set; }
        public int SimchaId { get; set; }
        public bool Include { get; set; }
    }
}
