using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SimchaSite.data
{
    public class DbManager
    {
        string _connectionString;
        public DbManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Simcha> GetAllSimchos()
        {
            using (var connnection = new SqlConnection(_connectionString))
            using (var command = connnection.CreateCommand())
            {
                command.CommandText = @"SELECT * FROM Simchos";
                connnection.Open();
                var reader = command.ExecuteReader();
                List<Simcha> list = new List<Simcha>();
                while (reader.Read())
                {
                    Simcha s = new Simcha();
                    s.Id = (int)reader["id"];
                    s.Name = (string)reader["Name"];
                    s.Date = (DateTime)reader["date"];
                    s.Total = GetTotal(s.Id);
                    s.Contributions = GetContributions(s.Id);
                    list.Add(s);

                }
                return list;
            }


        }
        private List<Transaction> GetContributions(int simchaId)
        {

            using (var connnection = new SqlConnection(_connectionString))
            using (var command = connnection.CreateCommand())
            {
                command.CommandText = @"SELECT * FROM Contribs WHERE SimchaId = @simchaId";
                command.Parameters.AddWithValue("@simchaId", simchaId);
                connnection.Open();
                var reader = command.ExecuteReader();
                List<Transaction> list = new List<Transaction>();
                if (!reader.Read())
                {
                    return null;
                }
                while (reader.Read())
                {
                    Transaction t = new Transaction();
                    t.Id = (int)reader["id"];
                    t.Date = (DateTime)reader["Date"];
                    t.ContributerId = (int)reader["contributorId"];
                    t.Amount = (decimal)reader["amount"];
                    list.Add(t);
                }
                return list;
               
            }
        }
        private int GetTotal(int Id)
        {

            using (var connnection = new SqlConnection(_connectionString))
            using (var command = connnection.CreateCommand())
            {
                command.CommandText = @"SELECT ISNULL(SUM(Amount), 0) from Contribs where simchaId = @id";
                command.Parameters.AddWithValue("@id", Id);
                connnection.Open();
                return (int)(decimal)command.ExecuteScalar();
              
            }

        }
        public int GetTotalContributers()
        {
            using (var connnection = new SqlConnection(_connectionString))
            using (var command = connnection.CreateCommand())
            {
                command.CommandText = @"SELECT COUNT(*) FROM Contributers";
                connnection.Open();
                int total = (int)command.ExecuteScalar();
                return total;
            }

        }
        public void AddSimcha(string name, DateTime date)
        {
            using (var connnection = new SqlConnection(_connectionString))
            using (var command = connnection.CreateCommand())
            {
                command.CommandText = @"INSERT INTO Simchos (Name, Date)
                                        VALUES(@name, @date)";
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@date", date);
                connnection.Open();
                command.ExecuteNonQuery();
            }
        }
        public List<Contributer> GetContributers()
        {

            using (var connnection = new SqlConnection(_connectionString))
            using (var command = connnection.CreateCommand())
            {
                command.CommandText = @"select * from Contributers";
                connnection.Open();
                List<Contributer> list = new List<Contributer>();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Contributer c = new Contributer();

                    c.Id = (int)reader["id"];
                    c.FirstName = (string)reader["firstName"];
                    c.LastName = (string)reader["lastName"];
                    c.Cell = (string)reader["cell"];
                    c.AlwaysInclude = (bool)reader["alwaysInclude"];
                    c.CreatedDate = (DateTime)reader["createdDate"];
                    list.Add(c);
                    c.Balance = GetBalance(c.Id);
                    c.Transactions = GetTransactions(c.Id);
                }
                return list;
            }
        }
        private List<Transaction> GetTransactions(int contributorId)
        {

            using (var connnection = new SqlConnection(_connectionString))
            using (var command = connnection.CreateCommand())
            {
                command.CommandText = @"SELECT * FROM Deposits WHERE contributerId = @id";
                command.Parameters.AddWithValue("@id", contributorId);
                connnection.Open();
                var reader = command.ExecuteReader();
                List<Transaction> transactions = new List<Transaction>();
                while (reader.Read())
                {
                    transactions.Add(new Transaction
                    {
                        Id = (int)reader["DId"],
                        Amount = (decimal)reader["amount"],
                        Date = (DateTime)reader["date"],
                        ContributerId = contributorId

                    });
                }
                command.Parameters.Clear();
                reader.Close();
                command.CommandText = @"SELECT * FROM Deposits WHERE contributerId = @id";
                command.Parameters.AddWithValue("@id", contributorId);
                var CReader = command.ExecuteReader();
                while (CReader.Read())
                {

                    Transaction t = new Transaction();
                    t.Id = (int)CReader["DId"];
                    t.Amount = (decimal)CReader["amount"];
                    t.Date = (DateTime)CReader["date"];
                    t.ContributerId = contributorId;

                    transactions.Add(t);
                }
                return transactions;
            }
        }
        private Decimal GetBalance(int id)
        {

            using (var connnection = new SqlConnection(_connectionString))
            using (var command = connnection.CreateCommand())
            {
                command.CommandText = @"SELECT(SELECT ISNULL(SUM(d.Amount), '0') From Contributers c " +
                              "JOIN Deposits d ON c.Id = d.ContributerId WHERE c.Id = @Id) " +
                              "-(SELECT ISNULL(SUM(cs.Amount), '0') From Contributers c " +
                              "JOIN Contribs cs ON c.Id = cs.ContributorId WHERE c.Id = @Id) as 'Balance'";
                command.Parameters.AddWithValue("@id", id);
                connnection.Open();
                return (int)(decimal)command.ExecuteScalar();
            }
        }
        public void AddContributer(string firstName, string lastName, string cellNumber, decimal initialDeposit, DateTime createdDate, bool alwaysInclude)
        {
            using (var connnection = new SqlConnection(_connectionString))
            using (var command = connnection.CreateCommand())
            {
                command.CommandText = @"INSERT INTO Contributers(firstName, lastName, cell, createdDate, alwaysInclude)
                                        VALUES (@firstName, @lastName, @cell, @createdDate, @alwaysInclude) SELECT SCOPE_IDENTITY()";
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@cell", cellNumber);
                command.Parameters.AddWithValue("@createdDate", createdDate);
                command.Parameters.AddWithValue("@alwaysInclude", alwaysInclude);
                connnection.Open();
                int id = (int)(decimal)command.ExecuteScalar();
                command.Parameters.Clear();

                AddDeposit(initialDeposit, createdDate, id);

            }
        }
        public void EditContributer(string firstName, string lastName, string cellNumber, DateTime createdDate, bool alwaysInclude, int contributerId)
        {
            using (var connnection = new SqlConnection(_connectionString))
            using (var command = connnection.CreateCommand())
            {
                command.CommandText = @"UPDATE Contributers SET firstName = @firstName, lastName = @lastName, cell = @cell, createdDate = @createdDate, alwaysInclude = @alwaysInclude WHERE id = @conId";
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@cell", cellNumber);
                command.Parameters.AddWithValue("@createdDate", createdDate);
                command.Parameters.AddWithValue("@alwaysInclude", alwaysInclude);
                command.Parameters.AddWithValue("@conId", contributerId);
                connnection.Open();
                command.ExecuteNonQuery();
            }
        }
        
        public void AddDeposit(decimal amount, DateTime date, int contributerId)
        {

            using (var connnection = new SqlConnection(_connectionString))
            using (var command = connnection.CreateCommand())
            {
                command.CommandText = @"INSERT INTO DEPOSITS(amount, date, contributerId)
                                        VALUES (@amount, @date, @contributerId)";
                command.Parameters.AddWithValue("@amount", amount);
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@contributerId", contributerId);
                connnection.Open();
                command.ExecuteNonQuery();


            }
        }
    
        public void ClearAndReplace(int simchaId, List<Transaction> list)
        {
            using (var connnection = new SqlConnection(_connectionString))
            using (var command = connnection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Contribs WHERE simchaId = @id";
                command.Parameters.AddWithValue("@id", simchaId);
                connnection.Open();
                command.ExecuteNonQuery();
                foreach(Transaction t in list)
                {
                    command.Parameters.Clear();
                    command.CommandText = @"INSERT INTO Contribs(SimchaId, ContributorId, Amount, Date)
                                            VALUES(@simchaId, @contributerId, @amount, @date)";
                    command.Parameters.AddWithValue("@simchaId", simchaId);
                    command.Parameters.AddWithValue("@contributerId", t.ContributerId);
                    command.Parameters.AddWithValue("@amount", t.Amount);
                    command.Parameters.AddWithValue("@date", t.Date);
                    
                    command.ExecuteNonQuery();
                }

            }
        } 
        public List<Transaction> GetContributions()
        {
            using (var connnection = new SqlConnection(_connectionString))
            using (var command = connnection.CreateCommand())
            {
                command.CommandText = @"SELECT * FROM Contribs";
                connnection.Open();
                var reader = command.ExecuteReader();
                List<Transaction> list = new List<Transaction>();
                while (reader.Read())
                {
                    list.Add(new Transaction
                    {
                        Id = (int)reader["id"],
                        Date = (DateTime)reader["date"],
                        ContributerId = (int)reader["contributorId"],
                        Amount = (decimal)reader["amount"],
                        SimchaId = (int)reader["simchaId"]
                    });
                }
                return list;

            }
        }
    }


}
