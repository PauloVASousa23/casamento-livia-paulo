using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace CasamentoLiviaPaulo.Repository
{
    public class DAO
    {
        public string _connectionString { get; set; }

        public DAO()
        {

        }

        public DAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

    }
}
