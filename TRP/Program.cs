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
        static Player Player1 = new Player("Axel", 100, 1,(Weapon)Items[0]); //Player
        static List<Monster> Monsters = new List<Monster> { new Monster("Wolf", 10, 2), new Monster("Orc", 20, 5), new Monster("Tiger", 40, 6) }; // load all monsters    

        static Menu FightMenu = new Menu("Fight Menu", new List<Option> {
            new Option("Attack", 1),
            new Option("Open Inventory",2),
            new Option("Run!",3)
        }); //Battle Menu
        static int ShowFightMenu()
        {
            return FightMenu.ChooseNum();
        } //return fight menu option number

        #endregion

        static void Main(string[] args)
        {
            #region load menus

            Menu ActionMenu = new Menu("Action Menu", new List<Option> {
            new Option("Search for Trouble", (Action)Battle),
            new Option("Open Inventory", (Action)Inventory),
            new Option("Show Stats", (Action)ShowPlayerStats)
        }); // Idle Menu
            void ShowActionMenu()
            {
                ActionMenu.ChooseAction()();
                ShowActionMenu();
            } //return action menu action



            Menu StartingMenu = new Menu("Main Menu", new List<Option> { new Option("Start a new Game", (Action)ShowActionMenu) }); //Main Menu
            void ShowStartMenu()
            {
                StartingMenu.ChooseAction()();
                ShowStartMenu();
            } //return starting menu action


            #endregion
            test();
            ShowStartMenu();


          System.Threading.Thread.Sleep(5000);
        }

        public static void test() //Test
        {
            
        }

        public static void ExitMenu()
        {
            return;
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
        public static void ShowPlayerStats()
        {
            ShowStats(Player1);
        }

        #region Item Methods

            public static void Inventory() //Handles The inventory UI
            {
                Console.Clear();
                Console.WriteLine("Enter a Weapon's number to equip it.\n");
                Console.WriteLine("[Equipped]" + "[" + Player1.EquippedWeapon.Name + "]");
                for (int i = 0; i <= Player1.Inventory.Count - 1; i++)
                {
                    Console.WriteLine("[" + (i + 1) + "]" + "[" + Player1.Inventory[i].Name + "]"); //show Inventory  
                }
                Console.WriteLine("\n[0] Quit");
                int input = 1;
                bool valid_input = false;
                while (!valid_input)
                {
                    int.TryParse(Console.ReadLine(), out input);
                    if (input <= Player1.Inventory.Count)
                    {
                        valid_input = true;
                        break;
                    }
                    Console.WriteLine("Please enter a valid selection: ");
                }
                if (input > 0)
                {
                    Player1.EquipWeapon((Weapon)Player1.Inventory[input - 1], (input - 1));
                }
             Console.Clear();
            }

            

        public static void LootMonster(Monster monster, Player player) //transfer Monster item to the player
        {
            Item loot = monster.Inventory[monster.Inventory.Count - 1];
            player.AddToInventory(loot);
        } 

        #endregion

        #region Battle methods

        public static Item GenerateItem() //generate a random Item
        {
            int randomCell = RandomCellFromList(Monsters);
            Item item = Items[randomCell];
            Random rnd2 = new Random();
            item.Rarity = RandomEnumValue<Rarity>();
            return item;
            Console.WriteLine(item.Rarity);
        }

        public static Monster GenerateMonster() //generate a random monster
        {
            int randomCell = RandomCellFromList(Items);
            Monster enemy = new Monster("null", 0, 0);
            enemy.Name = Monsters[randomCell].Name;
            enemy.AttackPoints = Monsters[randomCell].AttackPoints; //create empty monster and dupe a monster from array
            enemy.HitPoints = Monsters[randomCell].HitPoints;

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
            }


        } //the main Battle method

        public static void RefreshScreen(Monster Enemy) //used for battle screen refresh
        {
            Console.Clear();
            ShowStats(Enemy);     
            ShowStats(Player1);
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
                ShowStats(Player1);

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
        } //Only SHOWS fight menu

        #endregion

        #region Utility

        static public T RandomEnumValue<T>()
        {
            var list = Enum.GetValues(typeof (T));
            return (T) list.GetValue(new Random().Next(list.Length));
        }

    static public int RandomCellFromList<T>(List<T> list)
        {
            int lastCell = list.Count - 1;
            Random rnd = new Random();          //pick random number
            int randomCell = rnd.Next(0, lastCell + 1);
            return randomCell;
        }
        #endregion
    }
}
