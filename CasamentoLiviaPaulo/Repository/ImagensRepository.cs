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
    }
}
