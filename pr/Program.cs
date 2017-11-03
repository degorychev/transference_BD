using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace pr
{
    public class trans
    {
        public string name;
        public string origNAME;
        public int origID;
        public trans(string _name, string _origNAME)
        {
            name = _name;
            origNAME = _origNAME;
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
            List<trans> transes = new List<trans>();
            MySqlConnection conn1 = new MySqlConnection(GetConnectionString1());
            MySqlConnection conn2 = new MySqlConnection(GetConnectionString2());


            conn1.Open();
            MySqlCommand comm = conn1.CreateCommand();
            comm.CommandText = "SELECT prepodovatel.FIO, prepodavatel_original.FIO AS 'orig' FROM prepodovatel LEFT join prepodavatel_original on prepodavatel_original.ID = prepodovatel.original_FIO";
            MySqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                transes.Add(new trans(reader[0].ToString(), reader[1].ToString()));
            }
            reader.Close();
            conn1.Close();
            //----------------------------------------------------------------------//
            conn2.Open();
            comm = conn2.CreateCommand();
            comm.CommandText = "SELECT ID FROM prepodavatel_original WHERE FIO=?value1";
            for (int i = 0; i < transes.Count; i++)
            {
                comm.Parameters.Add("?value1", MySqlDbType.VarChar).Value = transes[i].origNAME;
                MySqlDataReader DR = comm.ExecuteReader();
                if (DR.Read())
                    transes[i].origID = int.Parse(DR[0].ToString());
                else
                    Console.WriteLine("Какая то хня");

                comm.Parameters.Clear();
                DR.Close();
                Console.WriteLine("Получил " + transes[i].origID);
            }

            comm.CommandText = "INSERT INTO prepodovatel (FIO, original_FIO) VALUES (?value1, ?value2);";
            for (int i = 0; i < transes.Count; i++)
            {
                comm.Parameters.Add("?value1", MySqlDbType.VarChar).Value = transes[i].name;
                comm.Parameters.Add("?value2", MySqlDbType.Int32).Value = transes[i].origID;


                comm.ExecuteNonQuery();
                comm.Parameters.Clear();
                Console.WriteLine("Вставил " + transes[i].name);
            }
            conn2.Close();
            Console.ReadKey();
        }
    }
}
