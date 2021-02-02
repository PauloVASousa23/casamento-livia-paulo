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

        public Presente GetPresenteTimestamp(string timestamp)
        {
            string query = "SELECT * FROM presente WHERE Timestamp = @Timestamp";
            var conn = GetConnection();

            Presente Presente = new Presente();
            using (MySqlConnection cnx = GetConnection())
            {
                cnx.Open();
                MySqlCommand cmd = new MySqlCommand(query, cnx);
                cmd.Parameters.AddWithValue("@Timestamp", timestamp);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Presente.Id = Convert.ToInt32(reader["Id"]);
                        Presente.Nome = reader["Nome"].ToString();
                        Presente.Descricao = reader["Descricao"].ToString();
                        Presente.Preco = float.Parse(reader["Preco"].ToString());
                        Presente.Quantidade = Convert.ToInt32(reader["Quantidade"]);
                        Presente.Timestamp = reader["Timestamp"].ToString();
                    }
                }
            }

            return Presente;
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

        public bool CadastrarPresente(Presente p)
        {
            string query = "INSERT INTO presente (Nome, Descricao, Preco, Quantidade, Timestamp) VALUES (@Nome, @Descricao, @Preco, @Quantidade, @Timestamp)";

            using(MySqlConnection cnx = GetConnection())
            {
                int reader;

                cnx.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, cnx)) {
                    cmd.Parameters.AddWithValue("@Nome", p.Nome);
                    cmd.Parameters.AddWithValue("@Descricao", p.Descricao);
                    cmd.Parameters.AddWithValue("@Preco", p.Preco);
                    cmd.Parameters.AddWithValue("@Quantidade", p.Quantidade);
                    cmd.Parameters.AddWithValue("@Timestamp", p.Timestamp);

                    reader = cmd.ExecuteNonQuery();

                }

                cnx.Close();

                return reader > 0;
            }
        }

    }
}
