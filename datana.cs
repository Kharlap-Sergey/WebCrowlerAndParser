using System;
using System.Data.Common;
using Npgsql;

namespace Data
{

    public class DataBase
    {
        //create new table
        //CREATE TABLE SITELIST(
        //       http varchar(1000) NOT NULL,
        //       fullpage varchar(200000) NOT NULL,
        //       article varchar(50000) NOT NULL,
        //       datePublished varchar(100) NOT NULL,
        //       PRIMARY KEY(http)
        //);

        //insert data
        //INSERT INTO sitelist(http , fullpage, article, datepublished)
        //VALUES('http..', '...', '....', '...')

        public string FindPage(string text)
        {
            string result = "The page wasn't found!";

       
            return result;
            
        }
        string connactionParametrs = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=123456;Database=AbotProject;";
        public void WriteLineToConsole_color(string info, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(info);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void PushData(string http, string nameart, string article, string fullpage, string datePublished)
        { 
            NpgsqlConnection SQLConnaction = new NpgsqlConnection(connactionParametrs);
            SQLConnaction.Open();
            WriteLineToConsole_color("Conaction to DataBase is sucsess", ConsoleColor.Green);

            string command = String.Format("INSERT INTO sitelist(http , fullpage, article, datepublished) VALUES($${0}$$, $${3}$$, $${1}$$, $${2}$$)", 
                http, article, datePublished, fullpage);
            string com = String.Format("select 1 from sitelist where http = $${0}$$", http);
            NpgsqlCommand newComm = new NpgsqlCommand(com, SQLConnaction);

            var ans = newComm.ExecuteReaderAsync();
            //Console.WriteLine(ans.Depth);
            NpgsqlCommand newCommand = new NpgsqlCommand(command, SQLConnaction);
            newCommand.ExecuteNonQuery();

            SQLConnaction.Close();
        }

        void GetData()
        {
            NpgsqlConnection SQLConnaction = new NpgsqlConnection(connactionParametrs);

            SQLConnaction.Open();
            WriteLineToConsole_color("Conaction to DataBase is sucsess", ConsoleColor.Green);
            string command = "SELECT * FROM example";

            NpgsqlCommand newCommand = new NpgsqlCommand(command, SQLConnaction);
            NpgsqlDataReader npgSqlDataReader = newCommand.ExecuteReader();

            foreach (DbDataRecord dbDataRecord in npgSqlDataReader)
                Console.WriteLine(dbDataRecord["uri"] + "   " + dbDataRecord["article"]);

            SQLConnaction.Close();
        }
        public void NAME()
        {
            //string conn_param = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=123456;Database=usersdb;"; //Например: "Server=127.0.0.1;Port=5432;User Id=postgres;Password=mypass;Database=mybase;"
            //string sql = "CREATE TABLE customers;";
            //NpgsqlConnection conn = new NpgsqlConnection(conn_param);
            //NpgsqlCommand comm = new NpgsqlCommand(sql, conn);
            //conn.Open(); //Открываем соединение.
            // var result = comm.ExecuteScalar().ToString(); //Выполняем нашу команду.
            //conn.Close(); //Закрываем соединение.

            NpgsqlConnection SQLConnaction = new NpgsqlConnection(connactionParametrs);

            SQLConnaction.Open();
            WriteLineToConsole_color("Conaction to DataBase is sucsess", ConsoleColor.Green);
            string command = "SELECT * FROM example";

            NpgsqlCommand newCommand = new NpgsqlCommand(command, SQLConnaction);
            NpgsqlDataReader npgSqlDataReader = newCommand.ExecuteReader();

            foreach (DbDataRecord dbDataRecord in npgSqlDataReader)
                Console.WriteLine(dbDataRecord["id"] + "   " + dbDataRecord["value"]);

            SQLConnaction.Close();

        }
    }
}
