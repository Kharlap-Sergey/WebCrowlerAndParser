using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Data;
using EPull;
using Npgsql;

namespace EPull
{
    public class OperateEnti
    {
        public void OperatePagesContent()
        {
            DataBase dataBase = new DataBase();
            var pagesData = dataBase.GetDataBaseContentFromHTTP();

            for(int i = 0; i < pagesData.Item1.Count; i++)
            {
                string article = pagesData.Item1[i];
                string uri = pagesData.Item2[i];

                ParseEntitiesAndPushToDataBase(article, uri);
            }
        }
        public void ParseEntitiesAndPushToDataBase(string text, string http)
        {
            //Console.WriteLine(http);
            var entities = PullEnti.ExtractEntities(text);

            DataBase dataBase = new DataBase();

            dataBase.UnitEntitiesAndPages(entities, http);
        }
    }
}
