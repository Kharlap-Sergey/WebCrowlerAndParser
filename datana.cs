using System;
using System.Collections.Generic;
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

        public int GetEntityiD(string entity)
        {
            string connactionParametrs = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=123456;Database=AbotProject;";
            var conn = new NpgsqlConnection(connactionParametrs);
            conn.Open();

            //Console.WriteLine(-1);
            // Insert some data
            string com = String.Format("select id from entities where entity = $${0}$$", entity);

            int id = -1;
            var cmd = new NpgsqlCommand(com, conn);
            var reader = cmd.ExecuteReader();
            foreach (DbDataRecord dbDataRecord in reader)
                //Console.WriteLine(dbDataRecord["id"]);
                id = int.Parse(dbDataRecord["id"].ToString());

            conn.Close();
            return id;
        }
        public int GetHttpID(string http)
        {
            string connactionParametrs = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=123456;Database=AbotProject;";
            var conn = new NpgsqlConnection(connactionParametrs);
            conn.Open();

            // Insert some data
            string com = String.Format("select id from sitelist where http = $${0}$$", http);

            int id = -1;
            var cmd = new NpgsqlCommand(com, conn);
            var reader = cmd.ExecuteReader();
            foreach (DbDataRecord dbDataRecord in reader)
                id = int.Parse(dbDataRecord["id"].ToString());

            conn.Close();
            return id;

            // Retrieve all rows
        }
        public void UnitEntitiesAndPages(List<string> entities, string http)
        {
            
            var httpId = GetHttpID(http); 
            if (httpId == -1) return;

            foreach(var entity in entities)
            {
                AddEntitiAndPageTodDataBase(entity, httpId);
            }
        }

        async private void UniteTablesColumns(int E_ID, int P_ID)
        {
            string connactionParametrs = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=123456;Database=AbotProject;";
            await using var conn = new NpgsqlConnection(connactionParametrs);
            await conn.OpenAsync();

            // Insert some data
            string command = String.Format("INSERT INTO EntiSite(enti_id, site_id) VALUES($${0}$$, $${1}$$)", E_ID, P_ID);
            string com = String.Format("select  *from EntiSite where enti_id = $${0}$$ and site_id = $${1}$$", E_ID, P_ID);

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
        }
        private void AddEntitiAndPageTodDataBase(string entity, int httpID)
        {
            PushEntitiesAsync(entity);
            int entityID = GetEntityiD(entity);

            UniteTablesColumns(entityID, httpID);
           // Console.WriteLine(entityID.ToString() + httpID.ToString());
        }
        public void WriteLineToConsole_color(string info, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(info);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public async void PushEntitiesAsync(string entity)
        {
            string connactionParametrs = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=123456;Database=AbotProject;";
            await using var conn = new NpgsqlConnection(connactionParametrs);
            await conn.OpenAsync();

            // Insert some data
            string command = String.Format("INSERT INTO entities(entity, fullpage) VALUES($${0}$$, $$re$$)", entity);
            string com = String.Format("select entity from entities where entity = $${0}$$", entity);

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
        }
        public async void PushDataAsync(string http, string nameart, string article, string fullpage, string datePublished)
        {
            string connactionParametrs = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=123456;Database=AbotProject;";
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
            string connactionParametrs = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=123456;Database=AbotProject;";
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

        public Tuple<List<string>, List<string>> GetDataBaseContentFromHTTP()
        {
            string connactionParametrs = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=123456;Database=AbotProject;";
            NpgsqlConnection SQLConnaction = new NpgsqlConnection(connactionParametrs);

            SQLConnaction.Open();
            WriteLineToConsole_color("Conaction to DataBase is sucsess", ConsoleColor.Green);
            string command = "SELECT * FROM sitelist";

            NpgsqlCommand newCommand = new NpgsqlCommand(command, SQLConnaction);
            NpgsqlDataReader npgSqlDataReader = newCommand.ExecuteReader();

            List<string> articles = new List<string>();
            List<string> uries = new List<string>();
            foreach(DbDataRecord pageConstent in npgSqlDataReader)
            {
                string article = pageConstent["article"].ToString();
                string uri = pageConstent["http"].ToString();

                articles.Add(article);
                uries.Add(uri);
            }

            SQLConnaction.Close();

            return Tuple.Create(articles, uries);
        }
        public void GetData()
        {
            string connactionParametrs = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=123456;Database=AbotProject;";
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
            string connactionParametrs = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=123456;Database=AbotProject;";

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
