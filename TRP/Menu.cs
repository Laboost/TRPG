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
        public static int ActionMenu<T>(List<T> list, string description)
        {
            Console.Clear();
            Console.WriteLine(description);
            int itemCount;
            for (itemCount = 0; itemCount < list.Count; itemCount++)
            {
                DescribeItem(list[itemCount],itemCount);

            }
            Console.WriteLine("\n[0] Quit");

            int input = 1;
            bool valid_input = false;
            while (!valid_input)
            {
                int.TryParse(Console.ReadLine(), out input);
                if (input <= itemCount && input >= 0)
                {
                    valid_input = true;
                    break;
                }
                Console.WriteLine("Please enter a valid selection: ");
            }

            int chosenItemSlot = input - 1;
            Console.Clear();
            if (input > 0 && input <= itemCount)
            {
                return input;
                
            }
            return 0; 
        }
        private static void DescribeItem(object item, int count)
        {
            if (item is Item)
            {
                Item Item = item as Item;
                if (Item.Rarity == Rarity.Rare)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                }
                if (Item.Rarity == Rarity.Legendary)
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                }
                if (Item.Rarity == Rarity.Divine)
                {
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                }
            }
            if (item is Consumable)
            {
                Consumable Item = item as Consumable;
                Console.WriteLine("[" + (count + 1) + "]" + "[" + Item.Name + " - " + Item.Rarity + " - " +Item.Description + " - " + " Power: " + Item.Power + " Armor: " + Item.Armor + "]");
            }
            if (item is Equipment)
            {
                Equipment Item = item as Equipment;
                Console.WriteLine("[" + (count + 1) + "]" + "[" + Item.Name + " - " + Item.Rarity + " - " + " Power: " + Item.Power + " Armor: " + Item.Armor + "]");
            }
            if (item is Skill)
            {
                Skill Item = item as Skill;
                Console.WriteLine("[" + (count + 1) + "]" + "[" + Item.Name + " - " + (Item.Damage * 100) + "% " + "]");
            }
            Console.ResetColor();
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