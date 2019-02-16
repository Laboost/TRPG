using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace TRP
{
    class Program
    {
        #region load objects

        static List<Item> Items =  new List<Item> {new Weapon("Sword", 4), new Weapon("Spike", 8), new Weapon("Stick", 2), }; //load all game items
        static Monster[] monsters = { new Monster("Wolf", 10, 2), new Monster("Orc", 20, 5), new Monster("Tiger", 40, 6 )}; // load all monsters    
        static Player Player1 = new Player("Axel", 100, 1,(Weapon)Items[0]); //Player

        static Menu InventoryMenu = new Menu("Inventory", new List<Option> { });
        static void ShowInventoryMenu()
        {
            InventoryMenu.ChooseAction();
        }

        static Menu FightMenu = new Menu("Fight Menu", new List<Option> {
            new Option("Attack", 1),
            new Option("Open Inventory",2),
            new Option("Run!",3)
        }); //Battle Menu
        static int ShowFightMenu()
        {
           return FightMenu.ChooseNum();
        } //return fight menu option number

        static Menu ActionMenu = new Menu("Action Menu", new List<Option> {
            new Option("Search for Trouble", Battle),
            new Option("Open Inventory", ActionMenuInventory)
        }); // Idle Menu
        static void ShowActionMenu()
        {
            ActionMenu.ChooseAction()();
        } //return action menu action

        static Menu StartingMenu = new Menu("Main Menu", new List<Option> { new Option("Start a new Game",ShowActionMenu)}); //Main Menu
        static void ShowStartMenu()
        {
            StartingMenu.ChooseAction()();
        } //return starting menu action


        #endregion

        static void Main(string[] args)
        {

            test();
            ShowStartMenu();

          System.Threading.Thread.Sleep(5000);
        }

        public static void test() //Test
        {
            
        }


        public static void ShowStats(Body body)
        {
            if (body is Player)
            {
                Player player = (Player)body;
                Console.WriteLine("Name:" + body.Name + " HP:" + player.HitPoints + " Weapon:" + player.EquippedWeapon.Name + "\n");
            }
            else if (body is Fighter)
            {
                Fighter fighter = (Fighter)body;
                Console.WriteLine("Name:" + body.Name + " HP:" + fighter.HitPoints + "\n");
            }
            else
                Console.WriteLine("Name: " + body.Name + "Power: " + body.Power + "\n");

        } //shows a body stats

        #region Item Methods
        public static void NewInventory()
        {
            Console.Clear();
            Console.WriteLine("[Equipped]" + "[" + Player1.EquippedWeapon.Name + "]");
            for (int i = 0; i < Player1.Inventory.Count; i++)
            {
                InventoryMenu.Options[i].Text = Player1.Inventory[i].Name;
                //InventoryMenu.Options[i].Action = Player1.EquipWeapon((Weapon)(Player1.Inventory[i]));
            }
            ShowInventoryMenu();

        }

        public static void Inventory() //Handles The inventory UI
        {
            if (Player1.Inventory.Count > 0)
            {
                Console.Clear();
                Console.WriteLine("[Equipped]" + "[" + Player1.EquippedWeapon.Name + "]");
                for (int i = 0; i <= Player1.Inventory.Count - 1; i++)
                {
                    Console.WriteLine("[" + (i + 1) + "]" + "[" + Player1.Inventory[i].Name + "]"); //show Inventory               
                }
                int input;
                input = Convert.ToInt32(Console.ReadLine());
                Player1.EquipWeapon((Weapon)Player1.Inventory[input - 1]);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Inventory is empty");
                System.Threading.Thread.Sleep(1000);
            }
            Console.Clear();
        }

        public static void ActionMenuInventory() //Handles The inventory UI
        {
            if (Player1.Inventory.Count > 0)
            {
                Console.Clear();
                Console.WriteLine("[Equipped]" + "[" + Player1.EquippedWeapon.Name + "]");
                for (int i = 0; i <= Player1.Inventory.Count - 1; i++)
                {
                    Console.WriteLine("[" + (i + 1) + "]" + "[" + Player1.Inventory[i].Name + "]"); //show Inventory               
                }
                int input;
                input = Convert.ToInt32(Console.ReadLine());
                Player1.EquipWeapon((Weapon)Player1.Inventory[input - 1]);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Inventory is empty");
                System.Threading.Thread.Sleep(1000);
            }
            Console.Clear();
            ShowActionMenu();
        }

        public static void LootMonster(Monster monster, Player player) //transfer Monster item to the player
        {
            Item loot = monster.Inventory[monster.Inventory.Count - 1];
            player.Inventory.Add(loot);
        } 

        #endregion
           
        #region Battle methods

        public static Item GenerateItem() //generate a random Item
        {
            int lastCell = Items.Count - 1;
            Random rnd = new Random();
            int randomCell = rnd.Next(0, lastCell + 1);
            Item item = Items[randomCell];
            return item;
        }

        public static Monster GenerateMonster() //generate a random monster
        {
            int lastCell = monsters.Length - 1;
            Random rnd = new Random();          //pick random number
            int randomCell = rnd.Next(0, lastCell + 1);

            Monster enemy = new Monster("null",0,0);
            enemy.Name = monsters[randomCell].Name;
            enemy.AttackPoints = monsters[randomCell].AttackPoints; //create empty monster and dupe a monster from array
            enemy.HitPoints = monsters[randomCell].HitPoints;

            Item item = GenerateItem();
            enemy.Inventory.Add(item); //add loot to the monster

            return (enemy);
        }

        public static void Battle()
        {
            Player1.UpdateAP();
            Monster Enemy = GenerateMonster();
            Console.WriteLine("A Wild " + Enemy.Name + " appeared \n");
            bool endBattle = false;
            while (endBattle == false) //the battle loop
            {
                string playerAction = PlayersTurn(Enemy);  //players turn
                if (playerAction == "Escaped") //if player escape
                {
                    endBattle = true;
                    Console.WriteLine("You have Escaped!");
                    break;
                }
                if (Enemy.HitPoints <= 0) //if enemy died
                {
                    endBattle = true;
                    RefreshScreen(Enemy);
                    Console.WriteLine("You KILLED the " + Enemy.Name);
                    LootMonster(Enemy, Player1);

                    System.Threading.Thread.Sleep(1000);
                    break;
                }
                RefreshScreen(Enemy);
                if (playerAction == "Attacked") //if player attacked
                {
                    Console.WriteLine("You have attacked the " + Enemy.Name + ".");
                }
                System.Threading.Thread.Sleep(800);

                Attack(Enemy, Player1); //enemy turn

                Console.WriteLine("The " + Enemy.Name + " ATTACKED YOU!");
                System.Threading.Thread.Sleep(800);
                if (Player1.HitPoints <= 0) //if player died
                {
                    endBattle = true;
                    RefreshScreen(Enemy);
                    Console.WriteLine("You have DIED");
                    break;
                }

                RefreshScreen(Enemy);
            }
            if (Player1.HitPoints == 0)
            {
                Console.Clear();
                Console.WriteLine("GAME OVER.");

            }
            else
            {
                Console.Clear();
                ShowActionMenu();
            }


        } //the main Battle method

        public static void RefreshScreen(Monster Enemy) //used for battle screen refresh
        {
            Console.Clear();
            ShowStats(Enemy);
            OnlyShowFightMenu();
        }

        public static void Attack(Fighter attacker, Fighter target) //one fighter attacks another
        {
           target.HitPoints -= attacker.AttackPoints;
        }

        public static string PlayersTurn(Fighter enemy) //handles the player turn 
        {
            bool endTurn = false;
            while (endTurn == false)
            {
                Console.Clear();
                ShowStats(enemy);

                int action = ShowFightMenu();
                if (action == 1) //Attack
                {
                    Attack(Player1, enemy);
                    return "Attacked";
                }
                else if (action == 2) //Inventory
                {
                    Inventory();
                }
                else if (action == 3) //run!
                {
                    bool escape = Escape();
                    if (escape == true)
                    {
                        return "Escaped"; //played managed to escape
                    }
                    else
                    {
                        endTurn = true; //player failed to escape
                    }
                }
                else
                {
                    Console.WriteLine("invalid input.");
                }
            }
            return "invalid";
        }

        public static bool Escape()
        {
            Random rnd = new Random();
            int escape = rnd.Next(1, 6); // a chance of 2/6
            if (escape >= 5)
            {
                return true;
            }
            else
            {
                return false;
            }

        } //Player trying to escape
        #endregion

        #region Menus
        public static void OnlyShowFightMenu()
        {
            #region Options
            Console.WriteLine("Fight Menu");
            string[] options =
                { "Attack","Open Inventory","Run!" };
            for (int i = 0; i < (options.Length); i++)
            {
                Console.WriteLine("[" + (i + 1) + "] " + options[i]);
            }
            #endregion

            Console.WriteLine('\n');
            ShowStats(Player1);
        } //Only SHOWS fight menu

        #endregion

    }
}
