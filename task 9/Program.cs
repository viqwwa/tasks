using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace task_9
{
    internal class Program
    {
        static MyArrayList<string> ReadData()
        {
            MyArrayList<string> tags = new MyArrayList<string>();

            try
            {
                using (StreamReader sr = new StreamReader("input.txt"))
                {
                    string line;
                    string pattern = @"<\/?[a-zA-Z][a-zA-Z0-9]*>";
                    while ((line = sr.ReadLine()) != null)
                    {
                        MatchCollection matches = Regex.Matches(line, pattern);

                        foreach (Match match in matches)
                        {
                            tags.Add(match.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return tags;
        }

        static string NormalizeTeg(string tag)
        {
            string content = tag.Trim('<', '>').TrimStart('/');
            return content.ToLowerInvariant();
        }

        static void DeleteDublicates(MyArrayList<string> tags)
        {
            MyArrayList<string> uniqueTags = new MyArrayList<string>();

            for (int i = 0; i < tags.Size(); i++)
            {
                string normTag = NormalizeTeg(tags.Get(i));

                if (!uniqueTags.Contains(normTag))
                {
                    uniqueTags.Add(normTag);
                }
                else
                {
                    tags.Remove(i);
                    i--;
                }
            }
        }

        static void Print(MyArrayList<string> tags)
        {
            for (int i = 0; i < tags.Size(); i++)
            {
                Console.Write(tags.Get(i) + " ");
            }
        }

        static void Main(string[] args)
        {
            MyArrayList<string> tags = ReadData();
            DeleteDublicates(tags);
            Print(tags);
        }
    }
}
