using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace prepod_or
{
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
            List<string> values = new List<string>();
            MySqlConnection conn1 = new MySqlConnection(GetConnectionString1());
            MySqlConnection conn2 = new MySqlConnection(GetConnectionString2());


            conn1.Open();
            MySqlCommand comm = conn1.CreateCommand();
            comm.CommandText = "SELECT `FIO` from prepodavatel_original GROUP BY `FIO`;";
            MySqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                values.Add(reader[0].ToString());
            }
            reader.Close();
            conn1.Close();
            //----------------------------------------------------------------------//

            conn2.Open();
            comm = conn2.CreateCommand();
            comm.CommandText = "INSERT INTO prepodavatel_original (FIO) VALUES (?value1);";
            for (int i = 0; i < values.Count; i++)
            {
                comm.Parameters.Add("?value1", MySqlDbType.VarChar).Value = values[i];
                comm.ExecuteNonQuery();
                comm.Parameters.Clear();
                Console.WriteLine("Вставил " + values[i]);
            }
            conn2.Close();
            Console.ReadKey();
        }
    }
}
