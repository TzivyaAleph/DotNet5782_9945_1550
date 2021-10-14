//Tzivya Rotlevy 324819945
//Yocheved Ismailoff 211721550

using System;

namespace Targil0
{
     partial class Program
    {
        static void Main(string[] args)
        {
            welcome9945();
            welcome1550();
            Console.ReadKey();
        }

        static partial void welcome1550();
        private static void welcome9945()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.Write("{0}, welcome to my first console application", name);
        }
    }
    
}

