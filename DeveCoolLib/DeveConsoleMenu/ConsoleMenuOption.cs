using System;
using System.Collections.Generic;
using System.Text;

namespace DeveCoolLib.DeveConsoleMenu
{
    public class ConsoleMenuOption
    {
        private readonly string _text;
        private readonly Action _actionToExecute;

        public ConsoleMenuOption(string text, Action actionToExecute)
        {
            _text = text;
            _actionToExecute = actionToExecute;
        }
    }
}
