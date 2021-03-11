using System;

namespace ReverseWordsOrder
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = Console.ReadLine();
            int i = input.Length - 1;
            for ( ; i >= 0; i--)
            {
                if (input[i] == ' ')
                {
                    Console.Write(input[(i + 1)..input.Length] + ' ');
                    break;
                }
                else if (i == 0)
                {
                    Console.Write(input[(i)..input.Length] + ' ');
                    break;
                }
            }
            if (i != 0)
            {
                int EndOfWord = i;
                for (; i > 0; i--)
                {
                    if (input[i] == ' ')
                    {
                        EndOfWord = i--;
                        while (i > 0)
                        {
                            if (input[i] == ' ')
                            {
                                Console.Write(input[(i + 1)..EndOfWord] + ' ');
                                EndOfWord = i;
                            }
                            i--;
                        }
                    }
                }
                Console.Write(input[..EndOfWord]);
            }
        }
    }
}
