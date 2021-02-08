using CasamentoLiviaPaulo.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasamentoLiviaPaulo.Repository
{
    public class MercadoPagoRepository
    {
        public string _connectionString { get; set; }

        public MercadoPagoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public bool CadastrarOrdem(MercadoPagoOrdem m)
        {
            string query = "INSERT INTO mercadopagoordem (" +
                                                            "collection_id," +
                                                            " collection_status," +
                                                            " payment_id," +
                                                            " status, external_reference," +
                                                            " payment_type, merchant_order_id," +
                                                            " preference_id, site_id," +
                                                            " processing_mode," +
                                                            " merchant_account_id)" +
                                                            " VALUES " +
                                                            "(@collection_id," +
                                                            " @collection_status," +
                                                            " @payment_id," +
                                                            " @status," +
                                                            " @external_reference," +
                                                            " @payment_type," +
                                                            " @merchant_order_id," +
                                                            " @preference_id," +
                                                            " @site_id," +
                                                            " @processing_mode," +
                                                            " @merchant_account_id)";

            using(MySqlConnection cnx = GetConnection())
            {
                int reader;

                cnx.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, cnx)) {
                    cmd.Parameters.AddWithValue("@collection_id", m.collection_id);
                    cmd.Parameters.AddWithValue("@collection_status", m.collection_status);
                    cmd.Parameters.AddWithValue("@payment_id", m.payment_id);
                    cmd.Parameters.AddWithValue("@status", m.status);
                    cmd.Parameters.AddWithValue("@external_reference", m.external_reference);
                    cmd.Parameters.AddWithValue("@payment_type", m.payment_type);
                    cmd.Parameters.AddWithValue("@merchant_order_id", m.merchant_order_id);
                    cmd.Parameters.AddWithValue("@preference_id", m.preference_id);
                    cmd.Parameters.AddWithValue("@site_id", m.site_id);
                    cmd.Parameters.AddWithValue("@processing_mode", m.processing_mode);
                    cmd.Parameters.AddWithValue("@merchant_account_id", m.merchant_account_id);

                    reader = cmd.ExecuteNonQuery();

                }

                cnx.Close();

                return reader > 0;
            }
        }

    }
}
