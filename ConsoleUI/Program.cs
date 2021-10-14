using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            IDAL.DO.Client clientA = new IDAL.DO.Client
            {
                name = "aaa",
                latitude = 36.22265,



            };
            Console.WriteLine($"{clientA.name}");
        }
    }
}
