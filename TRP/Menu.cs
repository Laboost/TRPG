using System;
using System.Collections.Generic;
using System.Text;

namespace TRP
{
    class Menu
    {
        private string title;
        private List<Option> options;

        public string Title { get { return title; } set { title = value; } }
        public List<Option> Options { get { return options; } set { options = value; } }

        public Menu(string title, List<Option> options)
        {
            this.title = title;
            this.options = options;
        }
        
        public void Show()
        {
            Console.WriteLine(this.title);
            int count = 1;
            foreach (Option option in this.options)
            {
                Console.WriteLine("[{0}] {1}", count, option.Text);
                ++count;
            }

            int selection = 0;
            while (!int.TryParse(Console.ReadLine(), out selection))
            {
                Console.WriteLine("Please enter a valid selection");
            }


        }
    }


    class Option
    {
        private string text;
        private Action action;

        public string Text { get { return text; } set { text = value; } }
        public Action Action { get { return action; } set { action = value; } }

        public Option(string text, Action action)
        {
            this.text = text;
            this.action = action;
        }
    }
}
