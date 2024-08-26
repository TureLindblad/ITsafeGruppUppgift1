using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lektion2Grupp
{
    public static class DbHandler
    {
        private static readonly SqliteConnection Connection = new SqliteConnection("Data Source=crypto.db");
        public static void InitDB()
        {
            using (Connection)
            {
                Connection.Open();

                string createTable = @"CREATE TABLE IF NOT EXISTS EncryptedStrings (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Encryption TEXT NOT NULL,
                                        Salt INTEGER NOT NULL
                                     );";
                ExecuteQuery(createTable);
            }
        }
        public static void ExecuteQuery(string query)
        {
            Connection.Open();

            using (SqliteCommand command = Connection.CreateCommand())
            {
                command.CommandText = query;
                command.ExecuteNonQuery();
            }
        }

        public static DataTable GetTable(string query)
        {
            DataTable dataTable = new DataTable();

            using (Connection)
            {
                Connection.Open();

                using (SqliteCommand command = Connection.CreateCommand())
                {
                    command.CommandText = query;
                    using (var reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
            }

            return dataTable;
        }
    }
}
