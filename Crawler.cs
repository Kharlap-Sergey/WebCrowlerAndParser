using System;
using Parsing;
using Abot;
using Data;
namespace Crawler
{
    class Crawler
    {
        
        static void Main(string[] args)
        {
            AbotC abot = new AbotC();
            Parse parse = new Parse();

            //abot.Start("https://belaruspartisan.by/life/491542/");
            abot.Start("https://belaruspartisan.by/politic/491685/");

            DataBase database = new DataBase();
            database.GetData();
        }
    }
}
