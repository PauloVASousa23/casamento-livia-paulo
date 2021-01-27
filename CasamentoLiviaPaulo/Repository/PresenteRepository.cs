using CasamentoLiviaPaulo.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasamentoLiviaPaulo.Repository
{
    public class PresenteRepository
    {
        public string _connectionString { get; set; }

        public PresenteRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public List<Presente> GetPresentes()
        {
            string query = "SELECT * FROM presente";
            var conn = GetConnection();

            List<Presente> listaPresentes = new List<Presente>();
            using (MySqlConnection cnx = GetConnection())
            {
                cnx.Open();
                MySqlCommand cmd = new MySqlCommand(query, cnx);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaPresentes.Add(new Presente()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nome = reader["Nome"].ToString(),
                            Descricao = reader["Descricao"].ToString(),
                            Preco = float.Parse(reader["Preco"].ToString()),
                            Quantidade = Convert.ToInt32(reader["Quantidade"])
                        });
                    }
                }
            }

            return listaPresentes;
        }

    }
}
