using System;
using Parsing;
using Abot;

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

            //string page = "rtujbvgyj < dfadsfas > {dfasdfas}";

            //page = parse.(page);
            //Console.WriteLine(page); 
        }
    }
}
