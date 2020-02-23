using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Data;
using EPull;
using Npgsql;

namespace Epull
{
    public class OperateEnti
    {
        public void OperatePagesContent()
        {
            DataBase dataBase = new DataBase();
            NpgsqlDataReader pagesData = dataBase.GetDataBaseContentFromHTTP();

            foreach (DbDataRecord pageConstent in pagesData)
            {
                string article = pageConstent["article"].ToString();
                string uri = pageConstent["http"].ToString();
                ParseEntitiesAndPushToDataBase(article, uri);
            }
        }
        public void ParseEntitiesAndPushToDataBase(string text, string http)
        {
            PullEnti.ExtractEntities(text);

        }
    }
}
