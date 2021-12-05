using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace furniture
{
    public class Database
    {
        private string connectionString = "Host=localhost;Username=postgres;Password=mkolev;Database=pps_furniture_shop";
        private NpgsqlConnection connection;

        public Database()
        {
            this.connection = new NpgsqlConnection(this.connectionString);
        }

        public void OpenConnection()
        {
            this.connection.Open();
        }

        public void CloseConnection()
        {
            this.connection.Close();
        }

        public NpgsqlConnection getConection()
        {
            return this.connection;
        }
    }
}