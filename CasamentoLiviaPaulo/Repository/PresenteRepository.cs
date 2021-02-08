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

        public Presente GetPresenteId(int id)
        {
            string query = "SELECT * FROM presente WHERE Id = @Id";
            var conn = GetConnection();

            Presente Presente = new Presente();
            using (MySqlConnection cnx = GetConnection())
            {
                cnx.Open();
                MySqlCommand cmd = new MySqlCommand(query, cnx);
                cmd.Parameters.AddWithValue("@Id", id);

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
                            Quantidade = Convert.ToInt32(reader["Quantidade"]),
                            Timestamp = reader["Timestamp"].ToString()
                        });
                    }
                }
            }

            return listaPresentes;
        }

        public List<Presente> GetPresentes(int pagina)
        {
            int inicio = pagina * 20;
            int fim = inicio + 20;
            string query = "SELECT * FROM presente ORDER BY Preco LIMIT " + inicio + "," + fim;
            string queryCount = "SELECT count(*) as Registros FROM presente";
            var conn = GetConnection();

            List<Presente> listaPresentes = new List<Presente>();
            using (MySqlConnection cnx = GetConnection())
            {
                cnx.Open();
                MySqlCommand cmd = new MySqlCommand(query, cnx);
                MySqlCommand cmd2 = new MySqlCommand(queryCount, cnx);
                int count = 0;
                using (var reader = cmd2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        count = Convert.ToInt32(reader["Registros"]);
                    }
                }
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
                            Quantidade = Convert.ToInt32(reader["Quantidade"]),
                            Timestamp = reader["Timestamp"].ToString(),
                            Registros = count/20
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

        public bool AtualizarPresente(Presente p)
        {
            string query = "UPDATE presente SET Nome = @Nome, Descricao = @Descricao, Preco = @Preco, Quantidade = @Quantidade, Timestamp = @Timestamp WHERE Timestamp = @Timestamp";

            using (MySqlConnection cnx = GetConnection())
            {
                int reader;

                cnx.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, cnx))
                {
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

        public bool AtualizarQuantidadePresente(int id, int quantidade)
        {
            string query = "UPDATE presente SET Quantidade = @Quantidade WHERE Id = @Id";

            using (MySqlConnection cnx = GetConnection())
            {
                int reader;

                cnx.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, cnx))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Quantidade", quantidade);

                    reader = cmd.ExecuteNonQuery();

                }

                cnx.Close();

                return reader > 0;
            }
        }

        public bool DeletarPresente(int id)
        {
            string query = "DELETE FROM presente WHERE Id = @Id";
            var conn = GetConnection();

            using (MySqlConnection cnx = GetConnection())
            {
                cnx.Open();
                MySqlCommand cmd = new MySqlCommand(query, cnx);
                cmd.Parameters.AddWithValue("@Id", id);

                var retorno = cmd.ExecuteNonQuery();

                return retorno > 0;
            }

            return false;
        }

    }
}
