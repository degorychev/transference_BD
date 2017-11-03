using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace aud
{
    public class aud
    {
        public int KORPUS;
        public string AUDITORIA;
        public string ORIG;
        public int origID;
        public aud(int korp, string aud, string orig)
        {
            AUDITORIA = aud;
            KORPUS = korp;
            ORIG = orig;
        }
    }

    class Program
    {
        public static string GetConnectionString1()
        {
            MySqlConnectionStringBuilder mysqlCSB;
            mysqlCSB = new MySqlConnectionStringBuilder();
            mysqlCSB.Server = "localhost";
            mysqlCSB.Database = "testbase";
            mysqlCSB.UserID = "root";
            mysqlCSB.CharacterSet = "utf8";
            return mysqlCSB.ConnectionString;
        }
        public static string GetConnectionString2()
        {
            MySqlConnectionStringBuilder mysqlCSB;
            mysqlCSB = new MySqlConnectionStringBuilder();
            mysqlCSB.Server = "localhost";
            mysqlCSB.Database = "raspisanie";
            mysqlCSB.UserID = "root";
            mysqlCSB.CharacterSet = "utf8";
            return mysqlCSB.ConnectionString;
        }

        static void Main(string[] args)
        {
            List<aud> auditorii = new List<aud>();
            MySqlConnection conn1 = new MySqlConnection(GetConnectionString1());
            MySqlConnection conn2 = new MySqlConnection(GetConnectionString2());


            conn1.Open();
            MySqlCommand comm = conn1.CreateCommand();
            comm.CommandText = "SELECT auditorii.korpus, auditorii.auditoria, auditorii_original.auditoria AS 'orig' FROM auditorii LEFT join auditorii_original on auditorii_original.ID = auditorii.Original_kab";
            MySqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                auditorii.Add(new aud(int.Parse(reader[0].ToString()), reader[1].ToString(), reader[2].ToString()));
            }
            reader.Close();
            conn1.Close();
            //----------------------------------------------------------------------//
            conn2.Open();
            comm = conn2.CreateCommand();
            comm.CommandText = "SELECT ID FROM auditorii_original WHERE auditoria=?value1";
            for (int i = 0; i < auditorii.Count; i++)
            {
                comm.Parameters.Add("?value1", MySqlDbType.VarChar).Value = auditorii[i].ORIG;
                MySqlDataReader DR = comm.ExecuteReader();
                if (DR.Read())
                    auditorii[i].origID = int.Parse(DR[0].ToString());
                else
                    Console.WriteLine("Какая то хня");
                    
                comm.Parameters.Clear();
                DR.Close();
                Console.WriteLine("Получил " + auditorii[i].origID);
            }

            comm.CommandText = "INSERT INTO auditorii (korpus, auditoria, original_kab) VALUES (?value1, ?value2, ?value3);";
            for (int i = 0; i < auditorii.Count; i++)
            {
                comm.Parameters.Add("?value1", MySqlDbType.Int32).Value = auditorii[i].KORPUS;
                comm.Parameters.Add("?value2", MySqlDbType.VarChar).Value = auditorii[i].AUDITORIA;
                comm.Parameters.Add("?value3", MySqlDbType.Int32).Value = auditorii[i].origID;


                comm.ExecuteNonQuery();
                comm.Parameters.Clear();
                Console.WriteLine("Вставил " + auditorii[i].AUDITORIA);
            }
            conn2.Close();
            Console.ReadKey();
        }
    }
}
