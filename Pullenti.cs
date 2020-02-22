using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using EP.Ner;
using EP.Morph;
using EP.Ner.Core;
using EP.Ner.Keyword;
using EP.Ner.Geo;
using EP.Ner.Person;
using EP.Ner.Org;

namespace EPull
{
    public class OperateEnti
    {
        public void ParseEntitiesAndPushToDataBase(string text, string uri)
        {
            PullEnti.ExtractEntities(text);
        }
    }

    public class PullEnti
    {     
       private static AnalysisResult ExtractPerson(string text)
        {
            Stopwatch sw = Stopwatch.StartNew();
            ProcessorService.Initialize(MorphLang.RU | MorphLang.EN);
            PersonAnalyzer.Initialize();

            sw.Stop();

            using (Processor proc = ProcessorService.CreateProcessor())
            {
                AnalysisResult ar = proc.Process(new SourceOfAnalysis(text));
                return ar;
            }
        }

        private static AnalysisResult ExtractPGeo(string text)
        {
            Stopwatch sw = Stopwatch.StartNew();
            ProcessorService.Initialize(MorphLang.RU | MorphLang.EN);
            GeoAnalyzer.Initialize();

            sw.Stop();

            using (Processor proc = ProcessorService.CreateProcessor())
            {
                AnalysisResult ar = proc.Process(new SourceOfAnalysis(text));
                return ar;
            }
        }
        private static AnalysisResult ExtractOrganization(string text)
        {
            Stopwatch sw = Stopwatch.StartNew();
            ProcessorService.Initialize(MorphLang.RU | MorphLang.EN);
            OrganizationAnalyzer.Initialize();

            sw.Stop();

            using (Processor proc = ProcessorService.CreateProcessor())
            {
                AnalysisResult ar = proc.Process(new SourceOfAnalysis(text));
                return ar;
            }
        }

        public static void ExtractEntities(string text)
        {
            AnalysisResult persons = ExtractPerson(text);
            AnalysisResult geos = ExtractPGeo(text);
            AnalysisResult organis = ExtractOrganization(text);

            foreach (var person in persons.Entities)
                Console.WriteLine(person);
            foreach (var geo in geos.Entities)
                Console.WriteLine(geo);
            foreach (var organ in organis.Entities)
                Console.WriteLine(organ);
        }
        void AnalizeExempl()
        {
            Console.WriteLine("Enered");
            Stopwatch sw = Stopwatch.StartNew();

            // инициализация - необходимо проводить один раз до обработки текстов
            Console.Write("Initializing ... ");

            // инициализируются движок
            ProcessorService.Initialize(MorphLang.RU | MorphLang.EN);

           // GeoAnalyzer.Initialize();
           // OrganizationAnalyzer.Initialize();
            PersonAnalyzer.Initialize();

            sw.Stop();
            Console.WriteLine("OK (by {0} ms), version {1}", (int)sw.ElapsedMilliseconds, ProcessorService.Version);

            // анализируемый текст
            string txt = "Единственным конкурентом «Трансмаша» на этом дебильном тендере было ООО «Плассер Алека Рейл Сервис», основным владельцем которого является австрийская компания «СТЦ-Холдинг ГМБХ». До конца 2011 г. эта же фирма была совладельцем «Трансмаша» вместе с «Тако» Краснова. Зато совладельцем «Плассера», также до конца 2011 г., был тот самый Карл Контрус, который имеет четверть акций «Трансмаша». ";

            // создаём экземпляр обычного процессора с типовым набором анализаторов
            using (Processor proc = ProcessorService.CreateProcessor())
            {
                // анализируем текст

                //AnalysisResult analRes = proc.Process(new SourceOfAnalysis(txt));
                AnalysisResult ar = proc.Process(new SourceOfAnalysis(txt));

                // результирующие сущности
                Console.WriteLine("Entities: ");
                foreach (var e in ar.Entities)
                    Console.WriteLine(e);

                // пример выделения именных групп
                Console.WriteLine("\r\n==========================================\r\nNoun groups: ");
                for (Token t = ar.FirstToken; t != null; t = t.Next)
                {
                    if (t.GetReferent() != null) continue; // токены с сущностями игнорируем
                    // пробуем создать именную группу
                    NounPhraseToken npt = NounPhraseHelper.TryParse(t, NounPhraseParseAttr.AdjectiveCanBeLast);
                    if (npt == null) continue; // не получилось
                    Console.WriteLine(npt);
                    t = npt.EndToken; // указатель на последний токен группы
                }
            }

            // создаём экземпляр спец. процессора для выделения ключевых комбинаций
            using (Processor proc = ProcessorService.CreateSpecificProcessor(KeywordAnalyzer.ANALYZER_NAME))
            {
                AnalysisResult ar = proc.Process(new SourceOfAnalysis(txt));
                Console.WriteLine("\r\n==========================================\r\nKeywords: ");
                foreach (var e in ar.Entities)
                    if (e is KeywordReferent)
                        Console.WriteLine(e);
            }
            Console.WriteLine("Over!");
            Console.Read();
        }
    }
}
