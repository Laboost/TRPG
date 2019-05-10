﻿using System;
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
            Program.ShowUI();

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
            Program.ShowUI();

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
        public static int ActionMenu<T>(List<T> list, string description,bool showUI = false, int startingCount = 0, bool clearConsole = true, bool showShopItem = false)
        {
            if (clearConsole)
            {
                Console.Clear();
            }
            Console.WriteLine(description);
            int itemCount;
            for (itemCount = startingCount; itemCount < list.Count; itemCount++)
            {
                DescribeItem(list[itemCount],true, itemCount,showShopItem:showShopItem);

            }
            Console.WriteLine("\n[0] Quit");

            if (showUI == true)
            {
                Program.ShowUI();
            }

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
        public static Item ItemMenu(List<Item> list, string description,bool showUI = false, int startingCount = 0, bool clearConsole = true)
        {
            if (clearConsole)
            {
                Console.Clear();
            }
            Console.WriteLine(description);
            int itemCount;
            for (itemCount = startingCount; itemCount < list.Count; itemCount++)
            {
                DescribeItem(list[itemCount], true, itemCount);
            }
            Console.WriteLine("\n[0] Quit");

            if (showUI == true)
            {
                Program.ShowUI();
            }

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
                return list[chosenItemSlot];
            }
            return null;
        }
        public static void DescribeItem(object item, bool countItem ,int count = 0,bool showShopItem = false)
        {
            bool printed = false;
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
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Cyan;
                }
            }
            if (countItem == true)
            {
                Console.Write("[" + (count + 1) + "]");
            }

            if (item is Consumable && printed == false)
            {
                Consumable Item = item as Consumable;
                if (showShopItem)
                {
                    Console.WriteLine("[" + Item.Name + " - " + Item.Rarity + " - " + Item.Description + " - " + " Power: " + Item.Power + " Armor: " + Item.Armor + " Buy Price: " + Item.BuyPrice + "]");
                }
                else
                {
                    Console.WriteLine("[" + Item.Name + " - " + Item.Rarity + " - " + Item.Description + " - " + " Power: " + Item.Power + " Armor: " + Item.Armor + " Sell Price: " + Item.SellPrice + "]");
                }
                printed = true;
            }
            if (item is Weapon && printed == false)
            {
                Weapon Item = item as Weapon;
                if (showShopItem)
                {
                    Console.WriteLine("[" + Item.Name + " - " + Item.Rarity + " - " + " Power: " + Item.Power + " Buy Price: " + Item.BuyPrice + "]");
                }
                else
                {
                    Console.WriteLine("[" + Item.Name + " - " + Item.Rarity + " - " + " Power: " + Item.Power + " Sell Price: " + Item.SellPrice + "]");
                }
                printed = true;
            }
            if (item is Equipment && printed == false)
            {
                Equipment Item = item as Equipment;
                if (showShopItem)
                {
                  Console.WriteLine("[" + Item.Name + " - " + Item.Rarity + " - " + " Power: " + Item.Power + " Armor: " + Item.Armor + " Buy Price: " + Item.BuyPrice + "]");
                }
                else
                {
                    Console.WriteLine("[" + Item.Name + " - " + Item.Rarity + " - " + " Power: " + Item.Power + " Armor: " + Item.Armor + " Sell Price: " + Item.SellPrice + "]");
                }
                printed = true;
            }
            if (item is Skill && printed == false)
            {
                Skill Item = item as Skill;
                Console.WriteLine("[" + Item.Name + " - " + (Item.Damage * 100) + "% " + "]");       
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