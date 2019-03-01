using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace TRP
{
    class Program
    {
        #region Load Objects

        static Weapon BasicSword = new Weapon("Sword", 10, Rarity.Common, WieldAttribute.MainHand);
        static List<Weapon> Weapons = new List<Weapon> {
            new Weapon("Sword", 20,WieldAttribute.MainHand,10)
            , new Weapon("Spike", 40,WieldAttribute.TwoHanded,10)
            , new Weapon("dagger", 10,WieldAttribute.OneHanded,10) }; //load all game items

        static Player Player1 = new Player("Player1", 100, 1, BasicSword); //Player
        static List<Monster> Monsters = new List<Monster> {
            new Monster("Wolf", 10, 50,75),
            new Monster("Orc", 20, 8,25),
            new Monster("Tiger", 30, 15,5) }; // load all monsters    


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

            void StartGame()
            {
                Player1 = new Player("Player1", 100, 1, BasicSword);
                Console.WriteLine("Choose your Name: ");
                string name = Console.ReadLine();
                Player1.Name = name;
                Console.Clear();
                for (int i = 0; i < 20; i++)
                {
                    Player1.Inventory.Add(GenerateWeapon());
                }
                ShowActionMenu();
            } //init a new game    

            Menu ActionMenu = new Menu("Action Menu", new List<Option> {
            new Option("Search for Trouble", (Action)Battle),
            new Option("Open Inventory\n", (Action)Inventory),
            new Option("Quit Game",(Action)EndGame)
        }); // Idle Menu
            void ShowActionMenu()
            {
                ActionMenu.ChooseAction()();
                if (Player1.HitPoints <= 0)
                {
                    return;
                }
                ShowActionMenu();
            } //return action menu action

            Menu StartingMenu = new Menu("Main Menu", new List<Option> { new Option("Start a new Game", (Action)StartGame) }); //Main Menu
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



        #region Item Methods

        public static void Inventory() //Handles The inventory UI
        {
            Console.Clear();
            Console.WriteLine("Choose A Weapon to equip.\n");
            Console.WriteLine("[MainHand]" + "[" + Player1.EquippedWeapons[0].Name + " - " + Player1.EquippedWeapons[0].Rarity + " - " + Player1.EquippedWeapons[0].Power + "]");
            if (Player1.EquippedWeapons[1] != null)
            {
                Console.WriteLine("[OffHand]" + "[" + Player1.EquippedWeapons[1].Name + " - " + Player1.EquippedWeapons[1].Rarity + " - " + Player1.EquippedWeapons[1].Power + "]");
            }
            else
            {
                Console.WriteLine("[OffHand]");
            }

            Console.WriteLine("\n"); //end of equipped Weapons

            int WeaponCount;
            for (WeaponCount = 0; WeaponCount < Player1.Inventory.Count; WeaponCount++) //shows all items in inventory
            {
                Console.WriteLine("[" + (WeaponCount + 1) + "]" + "[" + Player1.Inventory[WeaponCount].Name + " - " + Player1.Inventory[WeaponCount].Rarity + " - " + Player1.Inventory[WeaponCount].Power + "]"); //show Inventory
            }
            Console.WriteLine("\n[0] Quit");


            int input = 1;
            bool valid_input = false;
            while (!valid_input)
            {
                int.TryParse(Console.ReadLine(), out input);
                if (input <= WeaponCount && input >= 0)
                {
                    valid_input = true;
                    break;
                }
                Console.WriteLine("Please enter a valid selection: ");
            }
            int chosenWeaponSlot = input - 1;
            if (input > 0 && input <= WeaponCount) // if player chose a weapon from inventory
            {
                Player1.EquipWeapon((Weapon)Player1.Inventory[chosenWeaponSlot], chosenWeaponSlot);
            }
            Console.Clear();
        }

        public static void LootMonster(Monster monster, Player player) //transfer Monster item to the player
        {
            Item loot = monster.Inventory[monster.Inventory.Count - 1];
            player.AddToInventory(loot);
        }

        #endregion

        #region Battle Methods


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
                    System.Threading.Thread.Sleep(2000);
                    break;
                }

                RefreshScreen(Enemy);
            }
            if (Player1.HitPoints <= 0)
            {
                Console.Clear();
                Console.WriteLine("GAME OVER.");
                System.Threading.Thread.Sleep(5000);
                Console.Clear();

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
            OnlyShowFightMenu();
            ShowPlayerStats();
        }

        public static double Attack(Fighter attacker, Fighter target) //one fighter attacks another
        {
            target.HitPoints -= attacker.AttackPoints;
            return target.HitPoints;
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

        #region Generators


        public static Weapon GenerateWeapon()
        {
            Weapon X = RandomWeaponDrop(Weapons);
            Weapon item = CopyWeapon(X);
            Rarity randomRarity = RandomRarityDrop();
            item.Rarity = randomRarity;
            item.UpdateStats();

            return item;

        }

        public static Monster GenerateMonster() //generate a random monster
        {
            Monster X = RandomMonsterSpawn(Monsters);
            Monster enemy = CopyMonster(X);
            Item item = GenerateWeapon();
            enemy.Inventory.Add(item); //add loot to the monster

            return (enemy);
        }

        public static Weapon RandomWeaponDrop(List<Weapon> items) //generate weapon by Drop chance
        {
            int maxRoll = 0;
            foreach (Weapon item in items)
            {
                maxRoll += item.DropChance;
            }
            int roll = new Random().Next(0, maxRoll);
            int weightSum = 0;
            foreach (Weapon item in items)
            {
                weightSum += item.DropChance;
                if (roll < weightSum)
                {
                    return item;
                }
            }
            return null;
        }

        public static Monster RandomMonsterSpawn(List<Monster> monsters) // generate Monster by Drop Chance
        {
            double maxRoll = 0;
            foreach (Monster monster in monsters)
            {
                maxRoll += monster.DropChance;
            }
            int roll = new Random().Next(0, (int)maxRoll + 1);
            double weightSum = 0;
            foreach (Monster monster in monsters)
            {
                weightSum += monster.DropChance;
                if (roll < weightSum)
                {
                    return monster;
                }
            }
            return null;
        }

        public static Rarity RandomRarityDrop() //Generate Random Item Rarity
        {
            int roll = new Random().Next(0, 101);
            if (roll <= 50)
            {
                return Rarity.Common;
            }
            else if (roll > 50 && roll <= 80)
            {
                return Rarity.Rare;
            }
            else if (roll > 80 && roll < 95)
            {
                return Rarity.Legendary;
            }
            else
            {
                return Rarity.Divine;
            }

        }


        #endregion

        #region UI
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
        public static void ShowStats(Body body)
        {
            if (body is Player)
            {
                Player player = (Player)body;
                if (Player1.EquippedWeapons[1] != null)
                {
                    Console.WriteLine("Name: " + body.Name + "\nHP: " + player.HitPoints + "\nLevel:" + Player1.Power + "\nMain Hand: " + "Level:" + Player1.Power + Player1.EquippedWeapons[0].Name + "\nOff Hand: " + Player1.EquippedWeapons[1].Name + "\n");
                }
                else
                {
                    Console.WriteLine("Name: " + body.Name + "\nHP: " + player.HitPoints + "\nLevel:" + Player1.Power + "\nMain Hand: " +  Player1.EquippedWeapons[0].Name + "\n");
                }
                
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
        } // Shows the player stats
        public static void ExitMenu()
        {
            return;
        } // General return
        public static void EndGame()
        {
            Console.WriteLine("Are you sure you want to quit?");
            Console.WriteLine("\n[1] Yes");
            Console.WriteLine("[2] No");
            int selection = 1;
            bool valid_input = false;
            while (!valid_input)
            {
                int.TryParse(Console.ReadLine(), out selection);
                if (selection > 0 && selection <= 2)
                {
                    valid_input = true;
                    break;
                }
                Console.WriteLine("Please enter a valid selection: ");
            }
            Console.Clear();
            if (selection == 1)
            {
                Player1.HitPoints = 0;
            }
            if (selection == 2)
            {
                return;
            }
        } // Exit to Main Menu
        
        #endregion

        #region Utility

        static public T RandomEnumValue<T>()
        {
            var list = Enum.GetValues(typeof(T));
            return (T)list.GetValue(new Random().Next(list.Length));
        }

        static public int RandomCellFromList<T>(List<T> list)
        {
            int lastCell = list.Count - 1;
            Random rnd = new Random();          //pick random number
            int randomCell = rnd.Next(0, lastCell + 1);
            return randomCell;
        }

        static public Weapon CopyWeapon(Weapon original)
        {
            Weapon weapon = new Weapon(null, 0, WieldAttribute.MainHand, 0);
            weapon.Name = original.Name;
            weapon.Power = original.Power;
            weapon.WieldAttribute = original.WieldAttribute;

            return weapon;
        }

        static public Monster CopyMonster(Monster original)
        {
            Monster monster = new Monster(null, 0, 0,0);
            monster.Name = original.Name;
            monster.HitPoints = original.HitPoints;
            monster.AttackPoints = original.AttackPoints;

            return monster;
        }
        #endregion

    }
    

}