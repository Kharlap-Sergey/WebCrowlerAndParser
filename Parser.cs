using System;
using Data;
namespace Parsing
{
    public class Parse
    {       
        public static void WriteLineToConsole_color(string info, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(info);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void OperatePage(string page, string url)
        {
            //Console.WriteLine(page.Length);
            string article = ExtractArticle(page);
            string articleName= ExtractArticleName(page);
            string date = GetDate(article);
            DataBase data = new DataBase();
            data.PushData(url, articleName, article, page, date);

            WriteLineToConsole_color(articleName, ConsoleColor.Green);
            WriteLineToConsole_color(date, ConsoleColor.Yellow);
            WriteLineToConsole_color(url, ConsoleColor.DarkMagenta);
            WriteLineToConsole_color(article, ConsoleColor.Blue);
        }

        private string GetDate(string text)
        {
            if (text == "NON") return "0";
            return text.Substring(text.Length - 20, 18);
        }
        string Trim(string page, string clasName, string openTeg, string closeTeg)
        {
            page = TrimHead(page, clasName, openTeg);
            page = TrimTail(page, 0, 0, openTeg, closeTeg);
            return page;
        }
        string TrimHead(string page, string className, string teg)
        {
            string aticle = className;
            var ind = page.IndexOf(aticle);
            if(ind == -1)
            {
                return "NON";
            }
            while (page[ind] != teg[0] || page[ind + 1] != teg[1])
            {
                ind--;
            }
            page = page.Remove(0, ind);
            //Console.WriteLine(page);
            return page;
        }
        string DeletScripts(string page)
        {
            int indexBegin, indexEnd;
            while ((indexBegin = page.IndexOf("<script>")) != -1)
            {
                indexEnd = page.IndexOf("</script>");
                if (indexEnd == -1 || indexEnd+19 >= page.Length || indexEnd < indexBegin) break;
                page = page.Remove(indexBegin, indexEnd + 10);
            }
            return page;
        }
        string ExtractByClass(string page, string className, string openTeg, string closeTeg)
        {
            page = Trim(page, className, openTeg, closeTeg);
            page = DeletScripts(page);
            string separeters = " \n\t\a";
            string handels = "<[{";
            string OposHandels = ">]}";
            for (int i = 0; i < handels.Length; i++)
            {
                page = DeletTegs(page, handels[i], OposHandels[i]);
            }
            page = DeletSpaces(page, separeters);
            return page;
        }
        string TrimTail(string page, int opened, int closed, string openTeg, string closeTeg)
        {
            int opdiv = page.IndexOf(openTeg);
            int clodiv = page.IndexOf(closeTeg);
            if (opdiv == -1 && clodiv == -1) return "NON";
            if(opdiv < clodiv && opdiv != -1)
            {
                page = page.Remove(opdiv + 1, 3);
                opened++;
                page = TrimTail(page, opened, closed, openTeg, closeTeg);
            }
            else
            {
                closed++;
                if (closed >= opened) return page.Remove(clodiv);

                page = page.Remove(clodiv + 1, 3);
                page = TrimTail(page, opened, closed, openTeg, closeTeg);
            }
            
            return page;
        }
        private string ExtractArticleName(string page)
        {
            string className = "class=\"name\"";
            string tegO = "<h1";
            string tegC = "</h1";
            return ExtractByClass(page, className, tegO, tegC);
        }
        private string ExtractArticle(string page)
        {
            string className = "class=\"pw article\"";
            string tegO = "<div";
            string tegC = "</div";
            return ExtractByClass(page, className, tegO, tegC);
        }
        static string DeletSpaces(string str, string character)
        {
            var flag = false;
            for (int i = 0; i < str.Length; i++)
            {
                if (character.IndexOf(str[i]) != -1)
                {
                    var counter = 1;
                    for (int j = i + 1; j < str.Length; j++)
                    {
                        if (character.IndexOf(str[j]) == -1)
                        {

                            if (counter > 1)
                            {
                                //DataBase.WriteLineToConsole_color(counter.ToString(), ConsoleColor.Green);
                                flag = true;
                                str = str.Remove(i, counter - 1);
                            }
                            break;
                        }
                        counter++;
                    }
                }
                if (flag) break;
            }

            if (flag) return DeletSpaces(str, character);

            return str;
        }

        public static string DeletTegs(string HTMLTxt, char charecter, char oposCharacter)
        { 
            bool flag = false;
            for (int i = 0; i < HTMLTxt.Length; i++)
            {
                
                if (HTMLTxt[i] == charecter)
                { 
                    for (int j = i + 1; j < HTMLTxt.Length; j++)
                    {
                        if (HTMLTxt[j] == oposCharacter)
                        {

                            flag = true;
                            HTMLTxt = HTMLTxt.Remove(i, j - i + 1);
                            break;
                        }
                    }

                    if (flag)
                    {
                        break;
                    }
                }
            }
            if (flag) return DeletTegs(HTMLTxt, charecter, oposCharacter);

            return HTMLTxt;
        }
    }
}
