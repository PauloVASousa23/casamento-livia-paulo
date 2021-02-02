using CasamentoLiviaPaulo.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasamentoLiviaPaulo.Repository
{
    public class ImagensRepository
    {
        public string _connectionString { get; set; }

        public ImagensRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public List<Imagens> GetImagens(string timestamp)
        {
            string query = "SELECT * FROM imagens WHERE TimestampPresente = @Timestamp";
            var conn = GetConnection();

            List<Imagens> listaImagens = new List<Imagens>();
            using (MySqlConnection cnx = GetConnection())
            {
                cnx.Open();
                MySqlCommand cmd = new MySqlCommand(query, cnx);
                cmd.Parameters.AddWithValue("@Timestamp", timestamp);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaImagens.Add(new Imagens()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Caminho = reader["Caminho"].ToString(),
                            TimestampPresente = reader["TimestampPresente"].ToString()
                        });
                    }
                }
            }

            return listaImagens;
        }

        public bool CadastrarImagem(Imagens img)
        {
            string query = "INSERT INTO imagens (Caminho, TimestampPresente) VALUES (@Caminho, @TimestampPresente)";

            using (MySqlConnection cnx = GetConnection())
            {
                int reader;

                cnx.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, cnx))
                {
                    cmd.Parameters.AddWithValue("@Caminho", img.Caminho);
                    cmd.Parameters.AddWithValue("@TimestampPresente", img.TimestampPresente);

                    reader = cmd.ExecuteNonQuery();

                }

                cnx.Close();

                return reader > 0;
            }
        }

        public bool DeletarImagem(string caminho)
        {
            string query = "DELETE FROM imagens WHERE Caminho = @Caminho";

            using (MySqlConnection cnx = GetConnection())
            {
                int reader;

                cnx.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, cnx))
                {
                    cmd.Parameters.AddWithValue("@Caminho", caminho);

                    reader = cmd.ExecuteNonQuery();

                }

                cnx.Close();

                return reader > 0;
            }
        }

    }
}
