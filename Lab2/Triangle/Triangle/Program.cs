using System;

namespace Triangle
{
    class Program
    {
        static void Main(string[] args)
        {
            double SideAB, SideBC, SideAC;
            while (true)
            {
                Console.Write("Side AB: ");
                SideAB = Convert.ToDouble(Console.ReadLine());
                Console.Write("Side BC: ");
                SideBC = Convert.ToDouble(Console.ReadLine());
                Console.Write("Side AC: ");
                SideAC = Convert.ToDouble(Console.ReadLine());
                if (SideAB < SideAC + SideBC && SideAB > Math.Abs(SideAC - SideBC))
                {
                    break;
                }
                Console.WriteLine("Not a triangle.");
            }
            double P = SideAB + SideBC + SideAC;
            double SemiP = P / 2;
            double Square = Math.Sqrt(SemiP * (SemiP - SideAB) * (SemiP - SideBC) * (SemiP - SideAC));
            double AngleA, AngleB, AngleC;
            double Inradius, Circumradius;
            Inradius = Square / SemiP;
            Circumradius = SideAB * SideBC * SideAC / 4 / Square; 
            AngleA = Math.Asin(Square * 2 / (SideAB * SideAC));
            AngleB = Math.Asin(Square * 2 / (SideAB * SideBC));
            AngleC = Math.Asin(Square * 2 / (SideBC * SideAC));
            Console.WriteLine("Perimeter: {0:F4}", P);
            Console.WriteLine("Squre: {0:F4}", Square);
            Console.WriteLine("Angles: A = {0:F4}, B = {1:F4}, C = {2:F4}",  AngleA, AngleB, AngleC);
            Console.WriteLine("Inradius: {0:F4}", Inradius);
            Console.WriteLine("Circumradius: {0:F4}", Circumradius);
        }
    }
}
