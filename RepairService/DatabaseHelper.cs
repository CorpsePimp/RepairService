using Npgsql;
using System;
using System.Configuration;

namespace RepairService
{
    public static class DatabaseHelper
    {
        private static string ConnectionString = "Host=localhost;Port=5432;Database=RepairService;Username=Steve;Password=1";

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        public static void TestConnection()
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    throw new Exception("Database connection failed: " + ex.Message);
                }
            }
        }
    }
}