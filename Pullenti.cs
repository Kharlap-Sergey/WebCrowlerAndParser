using System;
using EP.Ner;
using EP.Ner.Core;
using EP.Morph;
using System.Diagnostics;

namespace PullentiSDC
{
    class Pullenti
    { 
        static void Main(string[] args)
        {
            /// инициализация - необходимо проводить один раз до обработки текстов
            Console.Write("Initializing ... ");
            /// инициализируются движок и все имеющиеся анализаторы
            EP.Ner.Sdk.Initialize(MorphLang.RU | MorphLang.EN);
            Console.WriteLine("OK, version {0}", ProcessorService.Version);

            /// анализируемый текст
            string txt = "Pullenti предназначено исключительно для разработчиков систем, постоянно имеющих дело с обработкой неструктурированной текстовой информации.";
            Console.WriteLine("Text: {0}", txt);
            /// запускаем обработку на пустом процессоре (без анализаторов NER)
            var are = ProcessorService.EmptyProcessor.Process(new SourceOfAnalysis(txt));
            Console.Write("Noun groups: ");
            /// перебираем токены
            for (Token t = are.FirstToken; t != null; t = t.Next)
            {
                /// выделяем именную группу с текущего токена
                NounPhraseToken npt = NounPhraseHelper.TryParse(t);
                /// не получилось
                if (npt == null) continue;
                /// получилось, выводим в нормализованном виде
                Console.Write("[{0}=>{1}] ", npt.GetSourceText(), npt.GetNormalCaseText(null, true));
                /// указатель на последний токен именной группы
                t = npt.EndToken;
            }
        }

    }
}
