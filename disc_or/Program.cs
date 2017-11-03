using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace disc_or
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
            List<string> auditorii = new List<string>();
            MySqlConnection conn1 = new MySqlConnection(GetConnectionString1());
            MySqlConnection conn2 = new MySqlConnection(GetConnectionString2());


            conn1.Open();
            MySqlCommand comm = conn1.CreateCommand();
            comm.CommandText = "SELECT `naimenovanie` from zaniatie_original GROUP BY `naimenovanie`;";
            MySqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                auditorii.Add(reader[0].ToString());
            }
            reader.Close();
            conn1.Close();
            //----------------------------------------------------------------------//

            conn2.Open();
            comm = conn2.CreateCommand();
            comm.CommandText = "INSERT INTO disciplines_original (naimenovanie) VALUES (?naimenovanie);";
            for (int i = 0; i < auditorii.Count; i++)
            {
                comm.Parameters.Add("?naimenovanie", MySqlDbType.VarChar).Value = auditorii[i];
                comm.ExecuteNonQuery();
                comm.Parameters.Clear();
                Console.WriteLine("Вставил " + auditorii[i]);
            }
            conn2.Close();
            Console.ReadKey();
        }
    }
}