using System;
using MENU_NS;

namespace SchoolProjectB_NS
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu1 Bootcamp = new Menu1();
            Bootcamp.RunMainMenu();
            Console.ReadKey();
        }
    }
}
