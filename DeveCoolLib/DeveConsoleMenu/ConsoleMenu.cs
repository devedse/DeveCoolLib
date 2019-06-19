using System;
using System.Collections.Generic;

namespace DeveCoolLib.DeveConsoleMenu
{
    public class ConsoleMenu
    {
        public ConsoleMenuType ConsoleMenuType { get; set; }
        public int StartNumber { get; set; }
        public List<ConsoleMenuOption> MenuOptions { get; set; } = new List<ConsoleMenuOption>();

        public ConsoleMenu(ConsoleMenuType consoleMenuType, int startNumber = 1)
        {
            ConsoleMenuType = consoleMenuType;
            StartNumber = startNumber;
        }

        public void RenderMenu()
        {
            Console.WriteLine("Choose any option:");

            int maxLength = MenuOptions.Count.ToString().Length;

            for (int i = 0; i < MenuOptions.Count; i++)
            {
                var option = MenuOptions[i];
                Console.WriteLine($"  {$"{i + StartNumber}:".PadRight(maxLength, ' ')}  {option.Text}");
            }
        }

        public void WaitForResult()
        {
            while (true)
            {
                Console.WriteLine();
                Console.Write("Choose an option: ");

                string input = "";
                if (ConsoleMenuType == ConsoleMenuType.KeyPress)
                {
                    input = Console.ReadKey().KeyChar.ToString();
                    Console.WriteLine();
                }
                else
                {
                    input = Console.ReadLine();
                }

                if (int.TryParse(input, out int result))
                {
                    int actualChoice = result - StartNumber;
                    if (actualChoice >= 0 && actualChoice < MenuOptions.Count)
                    {
                        ExecuteOption(actualChoice);
                        return;
                    }
                }

                Console.WriteLine("Input is not valid, please choose any of the provided options");
            }
        }

        private void ExecuteOption(int id)
        {
            var option = MenuOptions[id];
            option.ActionToExecute();
        }
    }
}
