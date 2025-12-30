using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;

namespace task_11
{
    internal class Program
    {
        static MyVector<string> ExtractIP(string line)
        {
            MyVector<string> IPs = new MyVector<string>();

            for (int i = 0; i < line.Length; i++)
            {
                for (int j = i + 7; j <= Math.Min(i + 15, line.Length); j++)
                {
                    string substring = line.Substring(i, j - i);

                    if (IsCorrectIP(substring))
                    {
                        bool isValid = true;

                        if (i > 0)
                        {
                            if (char.IsDigit(line[i - 1]))
                            {
                                isValid = false;
                            }
                        }

                        if (i > 1)
                        {
                            if (line[i - 1] == '.' && char.IsDigit(line[i - 2]))
                            {
                                isValid = false;
                            }
                        }

                        if (j < line.Length)
                        {
                            if (char.IsDigit(line[j]))
                            {
                                isValid = false;
                            }
                        }

                        if (j < line.Length - 1)
                        {
                            if (line[j] == '.' && char.IsDigit(line[j + 1]))
                            {
                                isValid = false;
                            }
                        }

                        if (isValid)
                        {
                            IPs.Add(substring);
                        }
                    }
                }
            }

            return IPs;
        }

        static bool IsCorrectIP(string text)
        {
            int dotCount = 0;

            foreach (char c in text)
            {
                if (c == '.') dotCount++;
            }
            if (dotCount != 3) return false;

            string[] parts = text.Split('.');
            if (parts.Length != 4) return false;

            foreach (string part in parts)
            {
                if (string.IsNullOrEmpty(part)) return false;
                if (part.Length > 1 && part[0] == '0') return false;

                foreach (char c in part)
                {
                    if (!char.IsDigit(c)) return false;
                }

                int number = int.Parse(part);
                if (number < 0 || number > 255) return false;
            }
            return true;
        }

        static void Main(string[] args)
        {
            MyVector<string> strings = new MyVector<string>();
            MyVector<string> IPs = new MyVector<string>();

            try
            {
                using (StreamReader sr = new StreamReader("input.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        strings.Add(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            for (int i = 0; i < strings.Size(); i++)
            {
                MyVector<string> IPinString = ExtractIP(strings.Get(i));

                if (IPinString.Size() > 0)
                {
                    IPs.AddAll(IPinString.ToArray());
                }
            }

            try
            {
                using (StreamWriter sr = new StreamWriter("output.txt", true))
                {
                    for (int i = 0; i < IPs.Size(); i++)
                    {
                        sr.WriteLine(IPs.Get(i));
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
