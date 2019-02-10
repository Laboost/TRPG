using System;
using System.Collections.Generic;
using System.Text;

namespace TRP
{
    class Menu
    {

    class Option
    {
        public string text;
        public Action action;
        
        public Option(string text, Action action)
        {
            this.text = text;
            this.action = action;
        }
    }
}
