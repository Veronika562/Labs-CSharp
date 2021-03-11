using System;
using System.Globalization;

namespace Months
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo French = new CultureInfo("fr-FR");
            for(int i = 1; i <= 12; i++)
            {
                Console.WriteLine(French.DateTimeFormat.GetMonthName(i));
            }
        }
    }
}
