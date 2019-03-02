using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace TRP //Version 0.1
{
    class Program
    {
        #region Load Objects

        static Weapon BasicSword = new Weapon("Basic Sword",10,WieldAttribute.MainHand,0);
        static List<Weapon> Weapons = new List<Weapon> {
            new Weapon("Sword", 20,WieldAttribute.MainHand,400)
            , new Weapon("Spike", 40,WieldAttribute.TwoHanded,150)
            , new Weapon("dagger", 10,WieldAttribute.OneHanded,150) }; //load all game weapons
        static List<Consumable> Items = new List<Consumable> // load all game consumables
        {
            new Consumable("HP Potion",10,600,ConsumableType.HealthPotion,"Heals the Consumer"),
        }; //load all game items
        static List<Equipment> Equipment = new List<Equipment> {
            new Equipment("Iron Chest",0,40,EquipmentSlot.Chest,150),
            new Equipment("Iron Head",0,30,EquipmentSlot.Head,150),
            new Equipment("Iron Legs",0,20,EquipmentSlot.Legs,150),
            new Equipment("Iron Wrists",0,10,EquipmentSlot.Wrists,200),
            new Equipment("Iron Hands",0,20,EquipmentSlot.Hands,150),
            new Equipment("Iron Feet",0,10,EquipmentSlot.Feet,200)
        }; //load all game Equipment

        static Player Player1 = new Player("Player1", 100, BasicSword); //Player

        static List<Monster> Monsters = new List<Monster> {
            new Monster("Wolf", 25, 30,75,40),
            new Monster("Orc", 40, 30,25,40),
            new Monster("Tiger", 80, 30,5,40) }; // load all monsters    


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
                Player1 = new Player("Player1", 100, BasicSword);
                Console.WriteLine("Choose your Name: ");
                string name = Console.ReadLine();
                Player1.Name = name;
                Console.Clear();
                ShowActionMenu();
            } //init a new game    

            Menu ActionMenu = new Menu("Action Menu", new List<Option> {
            new Option("Search for Trouble", (Action)Battle),
            new Option("Open Weapon Inventory", (Action)WeaponInventory),
            new Option("Open Item Inventory\n", (Action)ItemInventory),
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

        public static void WeaponInventory() //Handles The inventory UI
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
            for (WeaponCount = 0; WeaponCount < Player1.WeaponInventory.Count; WeaponCount++) //shows all items in inventory
            {
                Console.WriteLine("[" + (WeaponCount + 1) + "]" + "[" + Player1.WeaponInventory[WeaponCount].Name + " - " + Player1.WeaponInventory[WeaponCount].Rarity + " - " + Player1.WeaponInventory[WeaponCount].Power + "]"); //show Inventory
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
                Player1.EquipWeapon((Weapon)Player1.WeaponInventory[chosenWeaponSlot], chosenWeaponSlot);
            }
            Console.Clear();
        }

        public static void ItemInventory()
        {
            Console.Clear();
            Console.WriteLine("Choose Item to use.");
            int itemCount;
            for (itemCount = 0; itemCount < Player1.ItemInventory.Count; itemCount++)
            {
                if (Player1.ItemInventory[itemCount].Description != null)
                {
                    Console.WriteLine("[" + (itemCount + 1) + "]" + "[" + Player1.ItemInventory[itemCount].Name + " - " + Player1.ItemInventory[itemCount].Rarity + " - " + Player1.ItemInventory[itemCount].Description + " - " + " Power: " + Player1.ItemInventory[itemCount].Power + " Armor: " + Player1.ItemInventory[itemCount].Armor + "]");
                }
                Console.WriteLine("[" + (itemCount + 1) + "]" + "[" + Player1.ItemInventory[itemCount].Name + " - " + Player1.ItemInventory[itemCount].Rarity + " - "  + " Power: "  + Player1.ItemInventory[itemCount].Power + " Armor: " + Player1.ItemInventory[itemCount].Armor + "]");
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
            if (input > 0 && input <= itemCount)
            {
                if (Player1.ItemInventory[chosenItemSlot].Armor != 0)
                {
                    Player1.Equip((Equipment)Player1.ItemInventory[chosenItemSlot], chosenItemSlot);
                }
                Player1.Use(Player1.ItemInventory[chosenItemSlot], chosenItemSlot);
            }
            Console.Clear();
        }

        public static void LootMonster(Monster monster, Player player) //transfer Monster item to the player
        {
            foreach (Item item in monster.ItemInventory)
            {
                Item loot = item;
                if (loot != null)
                {
                    if (loot is Weapon)
                    {
                        player.AddToWeaponInventory(loot);
                    }
                    else
                    {
                        player.AddToItemInventory(loot);
                    }
                }
            }

        }

        #endregion

        #region Battle Methods


        public static void Battle()
        {
            Player1.UpdateStats();
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
                    Player1.AddExp(Enemy.Exp);
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
            double damageDelt = attacker.AttackPoints;

            if (target.Armor < damageDelt)
            {
                target.HitPoints = target.HitPoints + target.Armor - damageDelt;
            }
            else
            {
                damageDelt = 0;
            }
           
            return damageDelt;
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
                    double damageDelt = Attack(Player1, enemy);
                    return "Attacked";
                }
                else if (action == 2) //Inventory
                {
                    WeaponInventory();
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


        public static Weapon GenerateWeapon(List<Weapon> weapons)
        {
            Weapon X = RandomWeaponDrop(Weapons);
            if (X == null)
            {
                return X;
            }
            Weapon item = Cloner.CloneJson(X);
            Rarity randomRarity = RandomRarityDrop();
            item.Rarity = randomRarity;
            item.UpdateStats();

            return item;

        }
        public static Weapon RandomWeaponDrop(List<Weapon> items) //generate weapon by Drop chance
        {
            int roll = new Random().Next(0, 1000);
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

        public static Consumable GenerateConsumable(List<Consumable> items)
        {
            Consumable X = RandomConsumableDrop(items);
            if (X == null)
            {
                return X;
            }
            Consumable item = Cloner.CloneJson(X);
            Rarity randomRarity = RandomRarityDrop();
            item.Rarity = randomRarity;
            item.UpdateStats();

            return item; 
        }
        public static Consumable RandomConsumableDrop(List<Consumable> items) //generate Item by Drop Chance
        {
            int roll = new Random().Next(0, 1000);
            int weightSum = 0;
            foreach (Consumable item in items)
            {
                weightSum += item.DropChance;
                if (roll < weightSum)
                {
                    return item;
                }
            }
            return null;
        }

        public static Equipment GenerateEquipment(List<Equipment> items)
        {
            Equipment X = RandomEquipmentDrop(items);
            if (X == null)
            {
                return X;
            }
            Equipment item = Cloner.CloneJson(X);
            Rarity randomRarity = RandomRarityDrop();
            item.Rarity = randomRarity;
            item.UpdateStats();

            return item;

        }
        public static Equipment RandomEquipmentDrop(List<Equipment> items) //generate Equipment by Drop chance
        {
            int roll = new Random().Next(0, 1000);
            int weightSum = 0;
            foreach (Equipment item in items)
            {
                weightSum += item.DropChance;
                if (roll < weightSum)
                {
                    return item;
                }
            }
            return null;
        }

        public static Monster GenerateMonster() //generate a random monster
        {
            Monster X = RandomMonsterSpawn(Monsters);
            Monster enemy = Cloner.CloneJson(X);
            Item weapon = GenerateWeapon(Weapons);
            Item item = GenerateConsumable(Items);
            Item equipment = GenerateEquipment(Equipment);

            enemy.ItemInventory.Add(equipment);
            enemy.ItemInventory.Add(item);
            enemy.ItemInventory.Add(weapon); //add loot to the monster

            return (enemy);
        }
        public static Monster RandomMonsterSpawn(List<Monster> monsters) // generate Monster by Drop Chance
        {
            double maxRoll = 0;
            foreach (Monster monster in monsters)
            {
                maxRoll += monster.DropChance;
            }
            int roll = new Random().Next(0, (int)maxRoll);
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
            if (roll <= 70)
            {
                return Rarity.Common;
            }
            else if (roll > 70 && roll <= 95)
            {
                return Rarity.Rare;
            }
            else if (roll > 95 && roll < 98)
            {
                return Rarity.Legendary;
            }
            else
            {
                return Rarity.Divine;
            }

        }

        public static Item GenerateItem(List<Item> items)
        {
            Item X = RandomItemDrop(items);
            if (X == null)
            {
                return X;
            }
            Item item = Cloner.CloneJson(X);
            Rarity randomRarity = RandomRarityDrop();
            item.Rarity = randomRarity;
            item.UpdateStats();

            return item;
        }
        public static Item RandomItemDrop(List<Item> items)
        {
            int roll = new Random().Next(0, 1000);
            int weightSum = 0;
            foreach (Item item in items)
            {
                weightSum += item.DropChance;
                if (roll < weightSum)
                {
                    return item;
                }
            }
            return null;
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
                    Console.WriteLine("Name: " + body.Name + "\nLevel:" + Player1.Level + "\nArmor: " + Player1.Armor + "\nHP: " + player.HitPoints + " \\ " + Player1.MaxHitPoints  + "\nExp: " + Player1.Exp + " \\ " + Player1.MaxExp + "\n\nMain Hand: " + Player1.EquippedWeapons[0].Name + "\nOff Hand: " + Player1.EquippedWeapons[1].Name + "\n");
                }
                else
                {
                    Console.WriteLine("Name: " + body.Name + "\nLevel:" + Player1.Level + "\nArmor: " + Player1.Armor + "\nHP: " + player.HitPoints + " \\ " + Player1.MaxHitPoints  + "\nExp: " + Player1.Exp + " \\ " + Player1.MaxExp + "\n\nMain Hand: " + Player1.EquippedWeapons[0].Name + "\n");
                }
                for (int i = 0; i < Player1.BodySlots.Length; i++)
                {
                    if (Player1.BodySlots[i].Name != null)
                    {
                      Console.WriteLine("[" + Player1.BodySlots[i].Name + " - " + Player1.BodySlots[i].Rarity + " - " + Player1.BodySlots[i].Armor + "]");
                    }
                    
                }
                Console.WriteLine("\n");

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

        /*
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
            Monster monster = new Monster();
            monster.Name = original.Name;
            monster.HitPoints = original.HitPoints;
            monster.AttackPoints = original.AttackPoints;
            monster.Exp = original.Exp;

            return monster;
        }

        static public Consumeable CopyConsumeable(Consumeable original)
        {
            {
                Consumeable item = new Consumeable(null, 0, Rarity.Common);
                item.Name = original.Name;
                item.Power = original.Power;
                item.Description = original.Description;

                return item;
            }
        }
        */

        #endregion

    }

    public static class Cloner
    {
        public static T CloneJson<T>(this T source)
        {
            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            // initialize inner objects individually
            // for example in default constructor some list property initialized with some values,
            // but in 'source' these items are cleaned -
            // without ObjectCreationHandling.Replace default constructor values will be added to result
            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
        }
    }
}