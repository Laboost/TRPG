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

        static Weapon BasicSword = new Weapon("Sword", 10, Rarity.Common, WieldAttribute.OffHand);
        static List<Item> Items = new List<Item> {
            new Weapon("Sword", 10,WieldAttribute.MainHand)
            , new Weapon("Spike", 20,WieldAttribute.TwoHanded)
            , new Weapon("dagger", 5,WieldAttribute.OffHand) }; //load all game items

        static Player Player1 = new Player("Player1", 100, 1, BasicSword); //Player
        static List<Monster> Monsters = new List<Monster> {
            new Monster("Wolf", 10, 2),
            new Monster("Orc", 5, 5),
            new Monster("Tiger", 10, 6) }; // load all monsters    


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

        #region Item Methods

        public static void Inventory() //Handles The inventory UI
        {
            int eqWeaponCount;
            Console.Clear();
            Console.WriteLine("Choose A Weapon to equip.\n");
            for (eqWeaponCount = 0; eqWeaponCount < Player1.EquippedWeapons.Length - 1; eqWeaponCount++) // Show All equiped Weapons        
            {
                Console.WriteLine("[" + (eqWeaponCount + 1) + "]" + "[Equipped]" + "[" + Player1.EquippedWeapons[eqWeaponCount].Name + " - " + Player1.EquippedWeapons[eqWeaponCount].Rarity + "]");
            }
            int WeaponCount = eqWeaponCount;
            int inventoryWeaponCount;
            for (inventoryWeaponCount = 0; inventoryWeaponCount < Player1.Inventory.Count; inventoryWeaponCount++) //shows all items in inventory
            {
                Console.WriteLine("[" + (WeaponCount + 1) + "]" + "[" + Player1.Inventory[inventoryWeaponCount].Name + " - " + Player1.Inventory[inventoryWeaponCount].Rarity + "]"); //show Inventory
                WeaponCount++;
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
            int chosenWeaponSlot = input - eqWeaponCount - 1;
            if (input > 0 && input > eqWeaponCount) // if player chose a weapon from inventory
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

        #region Generators

        public static Item GenerateItem() //generate a random Item
        {
            int randomCell = RandomCellFromList(Items);
            Item item = ObjectCloner.Clone((Weapon)Items[randomCell]);
            Random rnd2 = new Random();
            item.Rarity = RandomEnumValue<Rarity>();
            item.UpdateStats();

            return item;
        }

        public static Monster GenerateMonster() //generate a random monster
        {
            int randomCell = RandomCellFromList(Monsters);
            Monster enemy = ObjectCloner.Clone(Monsters[randomCell]);
            Item item = GenerateItem();
            enemy.Inventory.Add(item); //add loot to the monster

            return (enemy);
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
                Console.WriteLine("Name:" + body.Name + " HP:" + player.HitPoints + " Weapon:" + Player1.EquippedWeapons[0] + "\n");
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
        public static void ExitMenu()
        {
            return;
        }
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


        #endregion


    }

    public static class ObjectCloner
    {
        public static T Clone<T>(T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }
    }
}