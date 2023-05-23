using System;
using System.IO;
using System.Net;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace DigDes_1_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            string book = (@"http://az.lib.ru/t/tolstoj_lew_nikolaewich/text_0040.shtml");
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\book.txt";
            Dictionary<string, int> res = new Dictionary<string, int>();

            WebRequest req = WebRequest.Create(book);
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            StreamReader sr = new StreamReader(stream, Encoding.GetEncoding("windows-1251"));
            string text = sr.ReadToEnd();
            

            Type type = typeof(WordCounter.WordCounter);

            MethodInfo methodInfo = type.GetMethod("CountWords", BindingFlags.NonPublic | BindingFlags.Instance);

            if (methodInfo != null)
            {
                WordCounter.WordCounter wc = new WordCounter.WordCounter();
                object[] parametrs = { text };
                res = methodInfo.Invoke(wc, parametrs) as Dictionary<string, int>;
            }
            else
            {
                Console.WriteLine("Метод не найден");
            }

            foreach (var item in res)
            {
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    sw.WriteLine(item);
                }
            }
        }
    }
}
