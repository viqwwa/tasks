using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace task22
{
    internal class Program
    {
        static string NormalizeTeg(string tag)
        {
            string content = tag.Trim('<', '>').TrimStart('/');
            return content.ToLowerInvariant();
        }

        static void Main(string[] args)
        {
            MyHashMap<string, int> tags = new MyHashMap<string, int>();

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
                            string normTag = NormalizeTeg(match.Value);
                            
                            if (!tags.ContainsKey(normTag))
                            {
                                tags.Put(normTag, 1);
                            }
                            else
                            {
                                int prevCount = (int)tags.Get(normTag);
                                tags.Put(normTag, prevCount + 1);
                            }
                        }
                    }

                    HashSet<Entry<string, int>> tagsHashSet = tags.EntrySet();

                    foreach (var tag in tagsHashSet)
                    {
                        Console.WriteLine($"{tag.Key}: {tag.Value}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
