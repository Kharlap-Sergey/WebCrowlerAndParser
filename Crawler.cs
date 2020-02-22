using System;
using Parsing;
using Abot;
using Data;
using EPull;
namespace Crawler
{
    class Crawler
    {
        
        static void Main(string[] args)
        {
            AbotC abot = new AbotC();
            Parse parse = new Parse();
            DataBase database = new DataBase();
            OperateEnti pullEnti = new OperateEnti();
            pullEnti.ParseEntitiesAndPushToDataBase("", "fasdf");
            //abot.Start("https://belaruspartisan.by/life/491542/");
            //abot.Start("https://belaruspartisan.by/politic/491685/");
            //database.GetData();


        }
    }
}
