using System;

namespace Targil0
{
    partial class Program
    {
        private static void Main(string[] args)
        {
            Welcome3602();
            Welcome2068();
            Console.ReadKey();


        }
        static partial void Welcome2068();
        

        private static void Welcome3602()
        {
            Console.WriteLine("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0},welcome to my first console application", name);
        }

    }
}