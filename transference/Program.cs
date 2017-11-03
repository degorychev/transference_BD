using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace transference
{
    public class aud
    {
        public int KORPUS;
        public string AUDITORIA;
        public aud(int korp, string aud)
        {
            AUDITORIA = aud;
            KORPUS = korp;
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
            comm.CommandText = "SELECT `korpus`, `auditoria` from auditorii_original;";
            MySqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                auditorii.Add(new aud(int.Parse(reader[0].ToString()), reader[1].ToString()));
            }
            reader.Close();
            conn1.Close();            
            //----------------------------------------------------------------------//

            conn2.Open();
            comm = conn2.CreateCommand();
            comm.CommandText = "INSERT INTO auditorii_original (korpus, auditoria) VALUES (?korp, ?kab);";
            for (int i=0; i<auditorii.Count; i++)
            {
                comm.Parameters.Add("?korp", MySqlDbType.Int32).Value = auditorii[i].KORPUS;
                comm.Parameters.Add("?kab", MySqlDbType.VarChar).Value = auditorii[i].AUDITORIA;
                comm.ExecuteNonQuery();
                comm.Parameters.Clear();
                Console.WriteLine("Вставил " + auditorii[i].AUDITORIA);
            }
            conn2.Close();
            Console.ReadKey();
        }
    }
}
