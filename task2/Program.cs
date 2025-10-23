using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;

namespace task2
{
    internal class Program
    {
        class ComplexNumber
        {
            private double real;
            private double imaginary;
            public double Real
            {
                get { return real; }
                set { real = value; }
            }
            public double Imaginary
            {
                get { return imaginary; }
                set { imaginary = value; }
            }
            public ComplexNumber(double real, double imaginary)
            {
                this.real = real;
                this.imaginary = imaginary;
            }
            static public ComplexNumber Sum(ComplexNumber complexNumber1, ComplexNumber complexNumber2)
            {
                double sumReal = complexNumber1.Real + complexNumber2.Real;
                double sumImaginary = complexNumber1.Imaginary + complexNumber2.Imaginary;
                return new ComplexNumber(sumReal, sumImaginary);
            }
            static public ComplexNumber Sub(ComplexNumber complexNumber1, ComplexNumber complexNumber2)
            {
                double subReal = complexNumber1.Real - complexNumber2.Real;
                double subImaginary = complexNumber1.Imaginary - complexNumber2.Imaginary;
                return new ComplexNumber(subReal, subImaginary);
            }
            static public ComplexNumber Mult(ComplexNumber complexNumber1, ComplexNumber complexNumber2)
            {
                double multReal = complexNumber1.Real * complexNumber2.Real - 
                    complexNumber1.Imaginary * complexNumber2.Imaginary;
                double multImaginary = complexNumber1.Imaginary * complexNumber2.Real +
                    complexNumber1.Real * complexNumber2.Imaginary;
                return new ComplexNumber(multReal, multImaginary);
            }
            static public ComplexNumber Div(ComplexNumber complexNumber1, ComplexNumber complexNumber2)
            {
                double divReal = (complexNumber1.Real * complexNumber2.Real +
                    complexNumber1.Imaginary * complexNumber2.Imaginary) /
                    (Math.Pow(complexNumber2.Real, 2) + Math.Pow(complexNumber2.Imaginary, 2));
                double divImaginary = (complexNumber2.Real * complexNumber1.Imaginary -
                    complexNumber1.Real * complexNumber2.Imaginary) /
                    (Math.Pow(complexNumber2.Real, 2) + Math.Pow(complexNumber2.Imaginary, 2));
                return new ComplexNumber(divReal, divImaginary);
            }
            public double Module()
            {
                return Math.Sqrt(real * real + imaginary * imaginary);
            }
            public double Argument()
            {
                if (real > 0)
                {
                    return Math.Atan(imaginary / real);
                }

                if (real < 0 && imaginary >= 0)
                {
                    return Math.Atan(imaginary / real) + Math.PI;
                }

                if (real < 0 && imaginary < 0)
                {
                    return Math.Atan(imaginary / real) - Math.PI;
                }

                if (real == 0 && imaginary > 0)
                {
                    return Math.PI / 2;
                }

                if (real == 0 && imaginary < 0)
                {
                    return -Math.PI / 2;
                }
                return 0;
            }
            public void Output()
            {
                Console.WriteLine($"({real}; {imaginary})");
            }
        }
        static void Main(string[] args)
        {
            ComplexNumber complexNumber = new ComplexNumber(0, 0);

            Console.WriteLine("Меню");
            Console.WriteLine("1. Ввести число");
            Console.WriteLine("2. Сложить");
            Console.WriteLine("3. Вычесть");
            Console.WriteLine("4. Умножить");
            Console.WriteLine("5. Поделить");
            Console.WriteLine("6. Вычислить модуль");
            Console.WriteLine("7. Вычислить аргумент");
            Console.WriteLine("q. Выход");
            string action  = Console.ReadLine();

            while (action != "q" && action != "Q")
            {
                switch (action)
                {
                    case "1":
                        Console.WriteLine("Введите вещественную и мнимую части через пробел");
                        string[] number = Console.ReadLine().Split(' ');
                        double real = double.Parse(number[0]);
                        double imaginary = double.Parse(number[1]);
                        complexNumber.Real = real;
                        complexNumber.Imaginary = imaginary;
                        break;

                    case "2":
                        Console.WriteLine("Введите второе число");
                        number = Console.ReadLine().Split(' ');
                        real = double.Parse(number[0]);
                        imaginary = double.Parse(number[1]);
                        ComplexNumber complexNumber2 = new ComplexNumber(real, imaginary);
                        ComplexNumber.Sum(complexNumber, complexNumber2).Output();
                        break;

                    case "3":
                        Console.WriteLine("Введите второе число");
                        number = Console.ReadLine().Split(' ');
                        real = double.Parse(number[0]);
                        imaginary = double.Parse(number[1]);
                        complexNumber2 = new ComplexNumber(real, imaginary);
                        ComplexNumber.Sub(complexNumber, complexNumber2).Output();
                        break;

                    case "4":
                        Console.WriteLine("Введите второе число");
                        number = Console.ReadLine().Split(' ');
                        real = double.Parse(number[0]);
                        imaginary = double.Parse(number[1]);
                        complexNumber2 = new ComplexNumber(real, imaginary);
                        ComplexNumber.Mult(complexNumber, complexNumber2).Output();
                        break;

                    case "5":
                        Console.WriteLine("Введите второе число");
                        number = Console.ReadLine().Split(' ');
                        real = double.Parse(number[0]);
                        imaginary = double.Parse(number[1]);
                        complexNumber2 = new ComplexNumber(real, imaginary);
                        ComplexNumber.Div(complexNumber, complexNumber2).Output();
                        break;

                    case "6":
                        Console.WriteLine(complexNumber.Module());
                        break;

                    case "7":
                        Console.WriteLine(complexNumber.Argument());
                        break;

                    default:
                        Console.WriteLine("Неизвестная команда");
                        break;
                }

                Console.WriteLine("Меню");
                Console.WriteLine("1. Ввести число");
                Console.WriteLine("2. Сложить");
                Console.WriteLine("3. Вычесть");
                Console.WriteLine("4. Умножить");
                Console.WriteLine("5. Поделить");
                Console.WriteLine("6. Вычислить модуль");
                Console.WriteLine("7. Вычислить аргумент");
                Console.WriteLine("q. Выход");
                action = Console.ReadLine();
            }
        }
    }
}
