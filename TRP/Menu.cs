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

        public Action ChooseAction()
        {
            Console.WriteLine(this.title);
            int count = 1;
            foreach (Option option in this.options)
            {
                Console.WriteLine("[{0}] {1}", count, option.Text);
                ++count;
            }
            Console.WriteLine("\n");
            Program.ShowPlayerStats();

            int selection = 1;
            bool valid_input = false;
            while (!valid_input)
            {
                int.TryParse(Console.ReadLine(), out selection);
                if (selection > 0 && selection <= this.options.Count)
                {
                    valid_input = true;
                    break;
                }
                Console.WriteLine("Please enter a valid selection: ");
            }
            Console.Clear();
            return this.options[selection - 1].Action;

        }
        public int ChooseNum()
        {
            Console.WriteLine(this.title);
            int count = 1;
            foreach (Option option in this.options)
            {
                Console.WriteLine("[{0}] {1}", count, option.Text);
                ++count;
            }
            Console.WriteLine("\n");
            Program.ShowPlayerStats();

            int selection = 1;
            bool valid_input = false;
            while (!valid_input)
            {
                int.TryParse(Console.ReadLine(), out selection);
                if (selection > 0 && selection <= this.options.Count)
                {
                    valid_input = true;
                    break;
                }
                Console.WriteLine("Please enter a valid selection: ");
            }
            Console.Clear();
            Console.WriteLine(this.options[selection - 1].ChoiceNum);
            return this.options[selection - 1].ChoiceNum;

        }
       

    }


    class Option
    {
        private string text;
        private Action action;
        private int choiceNum;

        public string Text { get { return text; } set { text = value; } }
        public Action Action { get { return action; } set { action = value; } }
        public int ChoiceNum { get { return choiceNum; } set { choiceNum = value; } }


        public Option(string text, Action action)
        {
            this.text = text;
            this.action = action;
        }

        public Option (string text ,int choiceNum)
        {
            this.text = text;
            this.choiceNum = choiceNum;
        }
    }
}
