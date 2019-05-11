using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using System.Linq;

namespace TRP //Version 0.1
{
    class Program
    {
        #region Load Objects

        #region Skills
        static Skill BasicAttack = new Skill("Basic Attack", 1);
        static Skill SwordSlash = new Skill("Sword Slash", 1.2);
        static Skill SpikeDash = new Skill("Spike Dash", 1.2);
        static Skill DaggerStab = new Skill("Dagger Stab", 1.2);

        #region  Skill set for types of weapons

        static Skill[] SwordSkillSet = { BasicAttack, SwordSlash };
        static Skill[] SpikeSkillSet = { BasicAttack, SpikeDash };
        static Skill[] DaggerSkillSet = { BasicAttack, DaggerStab };

        #endregion
        #endregion

        static Weapon BasicSword = new Weapon("Basic Sword", 10, WieldAttribute.MainHand, 0, SwordSkillSet, 0, 5);
        static List<Weapon> Weapons = new List<Weapon> {
            new Weapon("Sword", 20,WieldAttribute.MainHand,400,SwordSkillSet,10,7)
            , new Weapon("Spike", 40,WieldAttribute.TwoHanded,300,SpikeSkillSet,50,10)
            , new Weapon("dagger", 10,WieldAttribute.OneHanded,300,DaggerSkillSet,30,7) };//load all game weapons

        static List<Consumable> Consumables = new List<Consumable> // load all game consumables
        {
            new Consumable("HP Potion",10,1000,ConsumableType.HealthPotion,"Heals the Consumer",30,10),
        }; //load all game items
        static List<Equipment> Equipment = new List<Equipment> {
            new Equipment("Iron Chest",0,40,EquipmentSlot.Chest,150,40,8),
            new Equipment("Iron Head",0,30,EquipmentSlot.Head,150,40,8),
            new Equipment("Iron Legs",0,20,EquipmentSlot.Legs,150,40,8),
            new Equipment("Iron Wrists",0,10,EquipmentSlot.Wrists,200,40,8),
            new Equipment("Iron Hands",0,20,EquipmentSlot.Hands,150,40,8),
            new Equipment("Iron Feet",0,10,EquipmentSlot.Feet,200,40,8)
        }; //load all game Equipment

        static Map Map;
        static Player Player1 = new Player("Player1", 100, BasicSword, 0); //Player

        static List<Monster> Monsters = new List<Monster> {
            new Monster("Wolf", 25, 30,75,40),
            new Monster("Orc", 40, 50,25,40),
            new Monster("Tiger", 80, 60,5,40) }; // load all monsters    

        static Shop CurrentShop = new Shop();



        #endregion

        #region load menus

        static Menu InventoryMenu = new Menu("Tabs", new List<Option>
            {
                new Option("Weapons",(Action)WeaponInventory),
                new Option("Items",(Action)ItemInventory)
            }); //inventory Menu UI
        public static void ShowInventoryMenu()
        {
            InventoryMenu.ChooseAction()();
        }

        static Menu ActionMenu = new Menu("Action Menu", new List<Option> {
            new Option("Move forward!", (Action)MoveForward),
            new Option("Open Inventory", (Action)ShowInventoryMenu),
            new Option("Quit Game",(Action)EndGame)
        });// Tile Menu with no shop
        static Menu ActionMenuWithShop = new Menu("Action Menu", new List<Option> {
            new Option("Shop", (Action)EnterShop),
            new Option("Move forward!", (Action)MoveForward),
            new Option("Open Inventory", (Action)ShowInventoryMenu),
            new Option("Quit Game",(Action)EndGame)
        }); // Tile Menu UI

        static void ShowActionMenu()
        {
            if (Map.CurrentTile.Type == TileType.Shop)
            {
                ActionMenuWithShop.ChooseAction()();
            }
            else
            {
                ActionMenu.ChooseAction()();
            }
            if (Player1.HitPoints <= 0)
            {
                return;
            }
        } //return action menu action

        static Menu StartingMenu = new Menu("Main Menu", new List<Option> { new Option("Start a new Game", (Action)GameManager) }); //Main Menu
        static void ShowStartMenu()
        {
            StartingMenu.ChooseAction()();
            ShowStartMenu();
        } //return starting menu action

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

        public static void GameManager() //handles a game instance
        {
            StartGame();
            while (Player1.HitPoints > 0)
            {
                InitTileEvent();
            }
        }

        static void Main(string[] args)
        {
            ShowStartMenu();
            System.Threading.Thread.Sleep(5000);
        }

        public static void test() //Test
        {
            for (int i = 0; i < 20; i++)
            {
                Player1.AddToWeaponInventory(GenerateItem(Weapons, true));
            }
        }

        #region Game Methods

        public static void StartGame()
        {
            Player1 = new Player("Player1", 100, BasicSword, 500);
            Console.WriteLine("Choose your Name: ");
            string name = Console.ReadLine();
            Player1.Name = name;
            Map = GenerateMap();
            Console.Clear();
            test();
        } //init a new game
        public static void InitTileEvent() // init the current tile event
        {
            Tile tile = Map.CurrentTile;
            TileType type = tile.Type;

            if (type == TileType.Battle)
            {
                if (!(tile.EventIsDone))
                {
                    Battle();
                    tile.EventIsDone = true;
                }
                ShowActionMenu();
            }
            if (type == TileType.Shop)
            {
                ShowActionMenu();
            }
            if (type == TileType.Boss)
            {
                //Initiate Boss Fight
            }
        }
        public static void MoveForward() // move the player one tile forward 
        {
            Map.MoveForward();
        }

        #endregion

        #region Item Methods

        public static void EnterShop()
        {
            if (CurrentShop.TileName != Map.CurrentTile.Name)
            {
                GenerateShop();
            }
            ViewShop();
        } //Enter Shop Action

        public static void WeaponInventory() //Handles The inventory UI
        {
            Console.Clear();
            Console.WriteLine("Choose A Weapon to equip.\n");
            Console.Write("[MainHand]");
            Menu.DescribeItem(Player1.EquippedWeapons[0], false);
            if (Player1.EquippedWeapons[1] != null)
            {
                Console.WriteLine("[OffHand]");
                Menu.DescribeItem(Player1.EquippedWeapons[1], false);
            }
            else
            {
                Console.WriteLine("[OffHand]");
            }

            Console.WriteLine("\n"); //end of equipped Weapons

            int WeaponCount;
            for (WeaponCount = 0; WeaponCount < Player1.WeaponInventory.Count; WeaponCount++) //shows all items in inventory
            {
                Menu.DescribeItem(Player1.WeaponInventory[WeaponCount], true, WeaponCount);//show Inventory
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
                Player1.EquipWeapon((Weapon)Player1.WeaponInventory[chosenWeaponSlot]);
            }
            Console.Clear();
        }

        public static void ItemInventory()
        {
            int input = Menu.ActionMenu(Player1.ItemInventory, "Choose Item to use.");
            int chosenItemSlot = input - 1;
            if (input == 0)
            {
                return;
            }
            if (Player1.ItemInventory[chosenItemSlot].Armor != 0)
            {
                Player1.Equip((Equipment)Player1.ItemInventory[chosenItemSlot]);
            }
            Player1.Use(Player1.ItemInventory[chosenItemSlot]);
            Console.Clear();
        } //Handles Item inventory UI

        public static void LootMonster(Monster monster, Player player) //transfer Monster item to the player
        {
            player.Gold += monster.Gold;
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
            PrintInColor("A Wild " + Enemy.Name + " appeared \n", ConsoleColor.DarkRed);
            bool endBattle = false;
            while (endBattle == false) //the battle loop
            {
                string playerAction = PlayersTurn(Enemy);  //players turn
                if (playerAction == "Escaped") //if player escape
                {
                    endBattle = true;
                    Console.Clear();
                    PrintInColor("You have Escaped!", ConsoleColor.Blue);
                    System.Threading.Thread.Sleep(3000);
                    break;
                }
                RefreshScreen(Enemy);
                if (playerAction.Contains("You hit")) //if player attacked
                {
                    Console.WriteLine(playerAction);
                    System.Threading.Thread.Sleep(1000);
                }
                if (Enemy.HitPoints <= 0) //if enemy died
                {
                    endBattle = true;
                    PrintInColor("You KILLED the " + Enemy.Name, ConsoleColor.Yellow);
                    LootMonster(Enemy, Player1);
                    Player1.AddExp(Enemy.Exp);
                    System.Threading.Thread.Sleep(1000);
                    break;
                }
                System.Threading.Thread.Sleep(800);

                double damageDealt = Attack(Enemy.AttackPoints, Player1); //enemy turn

                Console.Write(Enemy.Name + " hit you with ");
                PrintInColor(damageDealt.ToString(), ConsoleColor.Red);
                Console.Write(" Damage");
                System.Threading.Thread.Sleep(1000);
                if (Player1.HitPoints <= 0) //if player died
                {
                    endBattle = true;
                    RefreshScreen(Enemy);
                    Console.Write("You have ");
                    PrintInColor("DIED", ConsoleColor.Red);
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
                    double damage = SkillMenu(enemy);
                    return "You hit the " + enemy.Name + " for " + damage + " Damage";
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

        public static double Attack(double Damage, Fighter target) //one fighter attacks another
        {
            double damageDealt = Damage;

            if (target.Armor < damageDealt)
            {
                damageDealt -= target.Armor;
                target.HitPoints -= damageDealt;
            }
            else
            {
                damageDealt = 0;
            }

            return damageDealt;
        }

        public static double SkillMenu(Fighter target)
        {
            List<Skill> skillList = new List<Skill>();
            foreach (Weapon weapon in Player1.EquippedWeapons)
            {
                if (weapon != null)
                {
                    if (weapon.skillSet != null)
                    {
                        foreach (Skill skill in weapon.skillSet)
                        {
                            if (!(skillList.Contains(skill)))
                            {
                                skillList.Add(skill);
                            }
                        }
                    }
                }


            } //create a list of skills
            int input = Menu.ActionMenu(skillList, "Choose A skill");
            if (input == 0)
            {
                return 0;
            }
            Skill chosenSkill = skillList[input - 1];
            double damage = chosenSkill.Damage * Player1.AttackPoints;
            double finaldamage = Attack(damage, target);
            return finaldamage;
        } //choosing a skill to attack

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

        public static void RefreshScreen(Monster Enemy) //used for battle screen refresh
        {
            Console.Clear();
            ShowStats(Enemy);
            OnlyShowFightMenu();
            ShowUI();
        }
        #endregion

        #region Generators

        public static void GenerateShop()
        {
            CurrentShop.Items.Clear();
            CurrentShop.TileName = Map.CurrentTile.Name;

            for (int i = 0; i < 6; i++)
            {
                Random rnd = new Random();
                int x = rnd.Next(1);
                if (x == 0)
                {
                    CurrentShop.Items.Add(GenerateItem(Weapons, true));
                }
                if (x == 1)
                {
                    CurrentShop.Items.Add(GenerateItem(Consumables, true));
                }
                if (x == 2)
                {
                    CurrentShop.Items.Add(GenerateItem(Equipment, true));
                }
            }
        } //Generate new Shop

        public static Monster GenerateMonster() //generate a random monster
        {
            Monster X = RandomMonsterSpawn(Monsters);
            Monster enemy = Cloner.CloneJson(X);
            Item weapon = GenerateItem(Weapons);
            Item item = GenerateItem(Consumables);
            Item equipment = GenerateItem(Equipment);
            int gold = 20;

            enemy.Gold = gold;
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

        public static Item GenerateItem<T>(List<T> items, bool NoEmpty = false)
        {
            List<Item> convertedItems = items.Cast<Item>().ToList();
            Item origin = RandomItemDrop(convertedItems, NoEmpty);
            if (origin == null)
            {
                return origin;
            }

            if (origin is Weapon)
            {
                Weapon convertedOrigin = origin as Weapon;
                Weapon item = Cloner.CloneJson(convertedOrigin);
                Rarity randomRarity = RandomRarityDrop();
                item.Rarity = randomRarity;
                item.SellPrice = origin.SellPrice;
                item.BuyPrice = origin.BuyPrice;
                item.UpdateStats();
                return item;
            }
            if (origin is Consumable)
            {
                Consumable convertedOrigin = origin as Consumable;
                Consumable item = Cloner.CloneJson(convertedOrigin);
                Rarity randomRarity = RandomRarityDrop();
                item.Rarity = randomRarity;
                item.SellPrice = origin.SellPrice;
                item.BuyPrice = origin.BuyPrice;
                item.UpdateStats();
                return item;
            }
            if (origin is Equipment)
            {
                Equipment convertedOrigin = origin as Equipment;
                Equipment item = Cloner.CloneJson(convertedOrigin);
                Rarity randomRarity = RandomRarityDrop();
                item.Rarity = randomRarity;
                item.SellPrice = origin.SellPrice;
                item.BuyPrice = origin.BuyPrice;
                item.UpdateStats();
                return item;
            }
            else
            {
                Item item = Cloner.CloneJson(origin);
                Rarity randomRarity = RandomRarityDrop();
                item.Rarity = randomRarity;
                item.SellPrice = origin.SellPrice;
                item.BuyPrice = origin.BuyPrice;
                item.UpdateStats();
                return item;
            }
        }
        public static Item RandomItemDrop(List<Item> items, bool NoEmpty = false)
        {
            int roll;
            if (NoEmpty == true)
            {
                roll = new Random().Next(0, 1000);
            }
            else
            {
                roll = new Random().Next(0, 1800);
            }
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

        public static Map GenerateMap()
        {
            Map map = new Map();
            GenerateLayers(map);
            map.InitMap();
            return map;
        }
        public static void GenerateLayers(Map map)
        {
            for (int i = 0; i < 10; i++)
            {
                Layer layer = new Layer();

                Array values = Enum.GetValues(typeof(LayerType));
                Random random = new Random();
                LayerType randomType = (LayerType)values.GetValue(random.Next(values.Length));
                layer.Type = randomType;
                layer.Num = i;
                GenerateTiles(layer);

                map.Layers[i] = layer;
            }
        }
        public static void GenerateTiles(Layer layer)
        {
            for (int i = 0; i < 8; i++)
            {
                Tile tile = new Tile();
                List<TileType> tileTypePool = new List<TileType>();

                if (i == 0) //first tile types
                {
                    tileTypePool.Add(TileType.Battle);
                }
                else if (i == 7) //Last tile typs (Boss) 
                {
                    tileTypePool.Add(TileType.Boss);
                }
                else
                {
                    tileTypePool.Add(TileType.Battle);
                    tileTypePool.Add(TileType.Shop);
                }

                Random random = new Random();
                int randomInt = random.Next(tileTypePool.Count);
                TileType randomType = tileTypePool[randomInt];
                tile.Type = randomType;
                tile.Num = i;

                int count = i + 1;
                if (layer.Type == LayerType.Desert)
                {
                    tile.Name = "Desert " + count;
                }
                if (layer.Type == LayerType.Forest)
                {
                    tile.Name = "Forest " + count;
                }
                if (i == 7) //if tile is last in layer
                {
                    tile.Type = TileType.Boss;

                    if (layer.Type == LayerType.Desert)
                    {
                        tile.Name = "Desert Boss";
                    }
                    if (layer.Type == LayerType.Forest)
                    {
                        tile.Name = "Forest Boss";
                    }
                }

                layer.Tiles[i] = tile;
            }
        } //generte tiles for the layer of the map

        #endregion

        #region UI

        #region Shop

        public static void ViewShop()
        {
            Menu shopMenu = new Menu("Action Menu", new List<Option> {
            new Option("Buy", (Action)BuyFromShop),
            new Option("Sell", (Action)SellToShop)
            });

            shopMenu.ChooseAction()();

        } //show shop UI
        public static void BuyFromShop()//Buying UI
        {
            bool doneShopping = false;
            while (doneShopping == false)
            {
                int input = Menu.ActionMenu(CurrentShop.Items, "Please choose an item to BUY.",true,showShopItem:true);
                if (input == 0)
                {
                    doneShopping = true;
                    continue;
                }
                Item chosenItem = CurrentShop.Items[input - 1];
                bool result = Player1.BuyItem(chosenItem);
                if (result == false)
                {
                    Console.WriteLine("Not enough Minerals.");
                    Thread.Sleep(800);
                }
                else
                {
                    CurrentShop.Items.Remove(chosenItem);
                }
            }
        } 
        public static void SellToShop() // Selling UI
        {
            bool doneShopping = false;
            while (doneShopping == false)
            {
                Item chosenItem = new Item();
                Menu sellMenu = new Menu("Choose Type", new List<Option>
                {
                    new Option("Weapons",1),
                    new Option("Armor",2),
                    new Option("Consumables",3)
                });
                int result = sellMenu.ChooseNum();
                if (result == 1)
                {
                    chosenItem = Menu.ItemMenu(Player1.WeaponInventory, "Please choose a Weapon to SELL.", true);
                }
                if (result == 2)
                {
                    List<Item> armor = new List<Item>();
                    foreach (Item item in Player1.ItemInventory)
                    {
                        if (item is Equipment)
                        {
                            armor.Add(item);
                        }
                    }
                    chosenItem = Menu.ItemMenu(armor, "Please choose an armor to SELL.", true);
                }
                if (result == 3)
                {
                    List<Item> items = new List<Item>();
                    foreach (Item item in Player1.ItemInventory)
                    {
                        if (item is Consumable)
                        {
                            items.Add(item);
                        }
                    }
                    chosenItem = Menu.ItemMenu(items, "Please choose an item to SELL.", true);
                }

                if (chosenItem == null)
                {
                    doneShopping = true;
                    continue;
                }
                Player1.SellItem(chosenItem);
            }
        }

        #endregion

        public static void ShowPlayerInventory()
        {

        } // handles all inventory UI

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

                Console.Write("Name: " + body.Name + "\nLevel: ");
                PrintInColor(Player1.Level.ToString(), ConsoleColor.Green);

                Console.Write(" EXP: ");
                string exp = Player1.Exp + " \\ " + Player1.MaxExp;
                PrintInColor(exp, ConsoleColor.Green);

                Console.Write("\nHp: ");
                string hp = player.HitPoints + " \\ " + Player1.MaxHitPoints;
                PrintInColor(hp, ConsoleColor.Red);

                Console.Write("\nArmor: ");
                PrintInColor(Player1.Armor.ToString(), ConsoleColor.DarkGray);

                Console.Write(" Gold: ");
                PrintInColor(Player1.Gold.ToString(), ConsoleColor.Yellow);

                Console.Write("\nMain Hand: ");
                Menu.DescribeItem(Player1.EquippedWeapons[0],false);
                if (Player1.EquippedWeapons[1] != null)
                {
                    Console.Write("Off Hand: ");
                    Menu.DescribeItem(Player1.EquippedWeapons[1],false);
                }
                for (int i = 0; i < Player1.BodySlots.Length; i++)
                {
                    if (Player1.BodySlots[i].Name != null)
                    {
                        Menu.DescribeItem(Player1.BodySlots[i],false);
                    }
                    
                }
                Console.WriteLine("\n\n");

            }
            else if (body is Fighter)
            {
                Fighter fighter = (Fighter)body;
                Console.WriteLine("Name:" + body.Name + " HP:" + fighter.HitPoints + "\n");
            }
            else
                Console.WriteLine("Name: " + body.Name + "Power: " + body.Power + "\n");

        } //shows a body stats

        public static void ShowUI()
        {
            ShowStats(Player1);
            ShowMap();
        } // Shows the player stats

        public static void ShowMap()
        {
            if (Map != null)
            {
                if (Map.CurrentLayer.Type == LayerType.Desert)
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                }
                if (Map.CurrentLayer.Type == LayerType.Forest)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                }
                Console.WriteLine("-- "+ Map.LayerName +" --");
                Console.ResetColor();

                foreach (Tile tile in Map.CurrentLayer.Tiles)
                {
                    if (tile == Map.CurrentTile)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.Write("-[ " + tile.Name + " - " + tile.Type +  " ]-");
                    Console.ResetColor();
                }
                Console.WriteLine('\n');
            }
        } // show the map layout

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

        static public void PrintInColor(string text, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.Write(text);
            Console.ResetColor();
        } //print text in foreground color
        static public void PrintInBackColor(string text, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.BackgroundColor = consoleColor;
            Console.Write(text);
            Console.ResetColor();
        } // print text in background color

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