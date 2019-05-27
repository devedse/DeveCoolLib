using DeveCoolLib.DeveConsoleMenu;
using System;

namespace DeveCoolLib.ConsoleApp
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            TestConsoleMenu();
        }

        private static void TestConsoleMenu()
        {
            var running = true;
            var menu = new ConsoleMenu(ConsoleMenuType.KeyPress);

            menu.MenuOptions.Add(new ConsoleMenuOption("Do something", () => Console.WriteLine("This is the first option")));
            menu.MenuOptions.Add(new ConsoleMenuOption("Do something else", () => Console.WriteLine("This is the second option")));
            menu.MenuOptions.Add(new ConsoleMenuOption("Do something 3", () => Console.WriteLine("This is the third option")));
            menu.MenuOptions.Add(new ConsoleMenuOption("Exit", () => running = false));

            while (running)
            {
                menu.RenderMenu();
                menu.WaitForResult();
                Console.WriteLine();
                Console.WriteLine();

                if (menu.ConsoleMenuType == ConsoleMenuType.KeyPress)
                {
                    menu.ConsoleMenuType = ConsoleMenuType.StringInput;
                }
                else
                {
                    menu.ConsoleMenuType = ConsoleMenuType.KeyPress;
                }
            }
        }
    }
}
