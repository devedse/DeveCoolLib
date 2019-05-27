using System;

namespace DeveCoolLib.DeveConsoleMenu
{
    public class ConsoleMenuOption
    {
        public string Text { get; }
        public Action ActionToExecute { get; }

        public ConsoleMenuOption(string text, Action actionToExecute)
        {
            Text = text;
            ActionToExecute = actionToExecute;
        }
    }
}
