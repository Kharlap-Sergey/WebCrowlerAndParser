using System;
using System.Data.Common;
using Npgsql;

namespace Data
{

    public class DataBase
    {
       // CREATE TABLE SITELIST(
       //id serial Primary key,
       //http varchar(1000) NOT NULL,
       //fullpage varchar(200000) NOT NULL,
       //article varchar(50000) NOT NULL,
       //datePublished varchar(100) NOT NULL
       // );

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

        public async void PushDataAsync(string http, string nameart, string article, string fullpage, string datePublished)
        {
            await using var conn = new NpgsqlConnection(connactionParametrs);
            await conn.OpenAsync();

            // Insert some data
            string command = String.Format("INSERT INTO sitelist(http , fullpage, article, datepublished) VALUES($${0}$$, $${3}$$, $${1}$$, $${2}$$)",
                http, article, datePublished, fullpage);
            string com = String.Format("select http from sitelist where http = $${0}$$", http);

            bool isPresent = false;
            await using (var cmd = new NpgsqlCommand(com, conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                    isPresent = true;

            if (isPresent) return;
            await using (var cmd = new NpgsqlCommand(command, conn))
            {
                await cmd.ExecuteNonQueryAsync();
            }

            // Retrieve all rows
            
        }
        public void PushData(string http, string nameart, string article, string fullpage, string datePublished)
        { 
            NpgsqlConnection SQLConnaction = new NpgsqlConnection(connactionParametrs);
            SQLConnaction.Open();
            WriteLineToConsole_color("Conaction to DataBase is sucsess", ConsoleColor.Green);

            string command = String.Format("INSERT INTO sitelist(http , fullpage, article, datepublished) VALUES($${0}$$, $${3}$$, $${1}$$, $${2}$$)", 
                http, article, datePublished, fullpage);
            string com = String.Format("select http from sitelist where http = $${0}$$", http);
            NpgsqlCommand newComm = new NpgsqlCommand(com, SQLConnaction);

            //var ans = newComm.ExecuteReader();
            //?dsafa
            //Console.WriteLine(ans.Depth);
            NpgsqlCommand newCommand = new NpgsqlCommand(command, SQLConnaction);
            newCommand.ExecuteNonQuery();

            SQLConnaction.Close();
        }

        public void GetData()
        {
            NpgsqlConnection SQLConnaction = new NpgsqlConnection(connactionParametrs);

            SQLConnaction.Open();
            WriteLineToConsole_color("Conaction to DataBase is sucsess", ConsoleColor.Green);
            string command = "SELECT * FROM sitelist";

            NpgsqlCommand newCommand = new NpgsqlCommand(command, SQLConnaction);
            NpgsqlDataReader npgSqlDataReader = newCommand.ExecuteReader();

            foreach (DbDataRecord dbDataRecord in npgSqlDataReader)
                Console.WriteLine(dbDataRecord["http"] + "\n" + dbDataRecord["article"]);

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
