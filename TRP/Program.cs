using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
//tests
namespace TRP
{
    class Program
    {


        static void Main(string[] args)
        {
            StartingMenu();
            System.Threading.Thread.Sleep(5000);
        }
        #region load objects

        static Item[] Items = { new Weapon("Sword", 4), new Weapon("Spike", 4), new Weapon("Stick", 2), }; //load all game items
        static Monster[] monsters = { new Monster("Wolf", 10, 2), new Monster("Orc", 20, 5), new Monster("Tiger", 40, 6), }; // load all monsters    
        static Player Player1 = new Player("Shoval", 100, 1, (Weapon)Items[0]);
        

        #endregion
        public static void ShowStats(Body body)
        {
            if (body is Fighter)
            {
                Fighter fighter = (Fighter)body;
                Console.WriteLine("Name:" + body.Name + " HP:" + fighter.HitPoints + "\n");
            }
            else
                Console.WriteLine("Name: " + body.Name + "Power: " + body.Power + "\n");

        } //shows a body stats

        public static void Inventory()
        {
            Console.WriteLine("WIP");
            System.Threading.Thread.Sleep(1500);
        } //WIP

        #region Battle methods

        public static Item GenerateItem() //generate a random Item
        {
            int lastCell = Items.Length - 1;
            Random rnd = new Random();
            int randomCell = rnd.Next(0, lastCell);
            Item item = Items[randomCell];
            return item;
        }

        public static Monster GenerateMonster() //generate a random monster
        {
            int lastCell = monsters.Length - 1;
            Random rnd = new Random();
            int randomCell = rnd.Next(0, lastCell);
            Monster enemy = monsters[randomCell];
            return (enemy);
        }

        public static void Battle()
        {
            Monster Enemy = GenerateMonster();
            Console.WriteLine("A Wild " + Enemy.Name + " appeared \n");
            bool endBattle = false;
            while (endBattle == false) //the battle loop
            {
                bool escaped = PlayersTurn(Enemy); //after the player turn checks if he escaped
                if (escaped == true)
                {
                    endBattle = true;
                    Console.WriteLine("You have Escaped!");
                    break;
                }
                if (Enemy.HitPoints <= 0)
                {
                    endBattle = true;
                    RefreshScreen(Enemy);
                    Console.WriteLine("You KILLED the " + Enemy.Name);
                    System.Threading.Thread.Sleep(1000);
                    break;
                }
                RefreshScreen(Enemy);
                System.Threading.Thread.Sleep(1000);
                Attack(Enemy, Player1);
                Console.WriteLine("The " + Enemy.Name + " ATTACKED YOU!");
                System.Threading.Thread.Sleep(1000);
                if (Player1.HitPoints <= 0)
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
                ActionMenu();
            }


        } //the main Battle method

        public static void RefreshScreen(Monster Enemy) //used for battle screen refresh
        {
            Console.Clear();
            ShowStats(Enemy);
            ShowFightMenu();
        }

        public static void Attack(Fighter attacker, Fighter target) //one fighter attacks another
        {
           target.hitPoints -= attacker.AttackPoints;
        }

        public static bool PlayersTurn(Fighter enemy) //handles the player turn , returns true if player escaped
        {
            bool endTurn = false;
            while (endTurn == false)
            {
                Player1.UpdateAP();
                Console.Clear();
                ShowStats(enemy);

                int action = FightMenu();
                if (action == 1) //Attack
                {
                    Attack(Player1, enemy);
                    endTurn = true;
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
                        return true; //played managed to escape
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
            return false;
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
        public static void StartingMenu()
        {
            #region Options
            string[] options =
                { "Start a new Game", };
            for (int i = 0; i < (options.Length); i++)
            {
                Console.WriteLine("[" + (i + 1) + "] " + options[i]);
            }
            #endregion

            Player1.UpdateAP();

            int input;
            input = Convert.ToInt32(Console.ReadLine());

            if (input == 1)
            {
                Console.Clear();
                ActionMenu();
            }
        } //Main Menu

        public static void ActionMenu()
        {
            #region Options
            string[] options =
                { "Search for Trouble","Open Inventory" };
            for (int i = 0; i < (options.Length); i++)
            {
                Console.WriteLine("[" + (i + 1) + "] " + options[i]);
            }

            #endregion

            Console.WriteLine('\n');
            ShowStats(Player1);

            int input;
            input = Convert.ToInt32(Console.ReadLine());

            if (input == 1)
            {
                Console.Clear();
                Battle();
            }
            if (input == 3)
            {
                FightMenu();
            }


        } // idle menu

        public static int FightMenu()
        {
            #region Options
            string[] options =
                { "Attack","Open Inventory","Run!" };
            for (int i = 0; i < (options.Length); i++)
            {
                Console.WriteLine("[" + (i + 1) + "] " + options[i]);
            }
            #endregion

            Console.WriteLine('\n');
            ShowStats(Player1);

            int input;
            input = Convert.ToInt32(Console.ReadLine());
            return input;
        } //fight menu
        public static void ShowFightMenu()
        {
            #region Options
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
