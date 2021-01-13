using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasamentoLiviaPaulo.Models
{
    public class TesteContext
    {
        public string ConnectionString { get; set; }

        public TesteContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<Teste> GetAllAlbums()
        {
            List<Teste> list = new List<Teste>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Teste", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Teste()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Usuario = reader["Usuario"].ToString(),
                            Senha = reader["Senha"].ToString(),
                        });
                    }
                }
            }
            return list;
        }
    }
}
