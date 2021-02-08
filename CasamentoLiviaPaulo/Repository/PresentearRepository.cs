using CasamentoLiviaPaulo.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasamentoLiviaPaulo.Repository
{
    public class PresentearRepository
    {
        public string _connectionString { get; set; }

        public PresentearRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public bool CadastrarPresenteou(Presenteou p)
        {
            string query = "INSERT INTO presenteou (Pessoa, Data, Mensagem, Presente_Id) VALUES (@Pessoa, @Data, @Mensagem, @Presente_Id)";

            using(MySqlConnection cnx = GetConnection())
            {
                int reader;

                cnx.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, cnx)) {
                    cmd.Parameters.AddWithValue("@Pessoa", p.Pessoa);
                    cmd.Parameters.AddWithValue("@Data", p.Data);
                    cmd.Parameters.AddWithValue("@Mensagem", p.Mensagem);
                    cmd.Parameters.AddWithValue("@Presente_Id", p.Presente_Id);

                    reader = cmd.ExecuteNonQuery();

                }

                cnx.Close();

                return reader > 0;
            }
        }

    }
}
