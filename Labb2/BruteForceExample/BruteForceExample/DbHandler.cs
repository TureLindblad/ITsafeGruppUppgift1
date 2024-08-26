using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Collections;

namespace BruteForceExample
{
    public static class DbHandler
    {
        private static Dictionary<string, int> Logs = new Dictionary<string, int>();
        private static readonly SqliteConnection Connection = new SqliteConnection("Data Source=crypto.db");

        public static void InitDB()
        {
            using (Connection)
            {
                Connection.Open();
                
                string dropTable = "DROP TABLE IF EXISTS Passwords;";

                using (SqliteCommand dropCommand = Connection.CreateCommand())
                {
                    dropCommand.CommandText = dropTable;
                    dropCommand.ExecuteNonQuery();
                }

                string createTable = @"CREATE TABLE IF NOT EXISTS Passwords (
                                        Id INTEGER PRIMARY KEY,
                                        Password TEXT NOT NULL
                                     );";

                using (SqliteCommand command = Connection.CreateCommand())
                {
                    command.CommandText = createTable;
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void InsertDB(string password)
        {
            
            string query =$"INSERT INTO Passwords VALUES ('1', '{password}');";

            using (Connection)
            {
                Connection.Open();

                using (SqliteCommand command = Connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("@password", password);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static bool CheckPasswordInDB(string password, string identifier)
        {
            if (Logs.ContainsKey(identifier)) Logs[identifier]++;
            else Logs[identifier] = 1;

            if (Logs[identifier] >= 10) Thread.Sleep(1000);

            Connection.Open();

            string checkPassword = @"SELECT * FROM Passwords WHERE Id = '1';";

            DataTable dataTable = new DataTable();

            using (Connection)
            {
                Connection.Open();

                using (SqliteCommand command = Connection.CreateCommand())
                {
                    command.CommandText = checkPassword;
                    using (var reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
            }

            if (dataTable.Rows[0]["Password"].ToString() == password) return true;
            return false;
        }
    }
}
