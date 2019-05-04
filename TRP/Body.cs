using System;
using System.Collections.Generic;
using System.Text;

namespace TRP
{
    class Body
    {
        protected string name;
        protected double power;
        protected double hitPoints;
        protected double armor;

        public double Armor { get { return armor; } set { armor = value; } }
        public double HitPoints { get { return hitPoints; } set { hitPoints = value; } }
        public string Name { get { return name; } set { name = value; } }
        public double Power { get { return power; } set { power = value; } }

    }

    class Fighter : Body
    {
        protected List<Item> itemInventory = new List<Item>();
        
        protected double attackPoints;
        protected double maxHitPoints;
        public double MaxHitPoints { get { return maxHitPoints; } set { maxHitPoints = value; } }
        public double AttackPoints { get { return attackPoints; } set { attackPoints = value; } }
        
        public List<Item> ItemInventory
        {
            get { return itemInventory; }
            set { itemInventory = value; }
        }
    }

    class Player : Fighter
    {
        private int level;
        public int Level { get { return level; } }

        private double exp;
        public double Exp { get { return exp; } }
        public double MaxExp { get; set; }

        #region Equipment Fields
        static private Equipment emptyEquipment = new Equipment();
        public Equipment[] BodySlots = new Equipment[] {
            emptyEquipment , emptyEquipment , emptyEquipment , emptyEquipment , emptyEquipment , emptyEquipment };
        private Equipment head { get { return BodySlots[0]; } set { BodySlots[0] = value; } }
        private Equipment chest { get { return BodySlots[1]; } set { BodySlots[1] = value; } }
        private Equipment hands { get { return BodySlots[2]; } set { BodySlots[2] = value; } }
        private Equipment wrists { get { return BodySlots[3]; } set { BodySlots[3] = value; } }
        private Equipment legs { get { return BodySlots[4]; } set { BodySlots[4] = value; } }
        private Equipment feet { get { return BodySlots[5]; } set { BodySlots[5] = value; } }
        #endregion

        #region Weapon Fields


        private List<Item> weaponInventory = new List<Item>();
        public List<Item> WeaponInventory { get { return weaponInventory; } set { weaponInventory = value; } }

        static private Weapon twoHanded = new Weapon("Two Handed", 0, WieldAttribute.OneHanded,0,null);

        private Weapon[] weaponSlots = new Weapon[] { twoHanded, twoHanded };
        private Weapon mainHand { get { return weaponSlots[0]; } set { weaponSlots[0] = value; } }
        private Weapon offHand { get { return weaponSlots[1]; } set { weaponSlots[1] = value; } }

        public Weapon[] EquippedWeapons { get { return weaponSlots; } }

        #endregion

        #region Methods

        public Player(string name, double hitPoints, Weapon weapon) 
        {
            mainHand = weapon;
            offHand = null;
            this.name = name;
            this.hitPoints = hitPoints;
            power = 10;
            level = 1;
            exp = 0; 
            MaxExp = 40;
            maxHitPoints = 100; 
        }

        public void UpdateStats() //updates the player AttackPoints
        {
            #region Armor

            double sumOfArmor = 0;

            for (int i = 0; i < BodySlots.Length; i++)
            {
                sumOfArmor += BodySlots[i].Armor;
            }
            armor = sumOfArmor;

            #endregion

            #region Attack Points

            #region Level

            double levelPower = 2;
            for (int i = 1; i < level; i++)
            {
                this.power += levelPower;
                levelPower = levelPower * 1.5;
            }

            #endregion

            #region weapons

            if (offHand != null)
            {
                attackPoints = mainHand.Power + offHand.Power + power;
            }
            else attackPoints = mainHand.Power + power;

            #endregion

            #region Equipment

            for (int i = 0; i < BodySlots.Length; i++)
            {
                power += BodySlots[i].Power;
            }

            #endregion

            #endregion
        }

        #region Weapon Methods

        public void EquipWeapon(Weapon weapon , int inventorySlot) //equip given weapon
        {
            RemoveFromWeaponInventory(inventorySlot);
            bool weaponSwaped = false;
            while (weaponSwaped == false)
            {
                if (weapon.WieldAttribute == WieldAttribute.MainHand) //equipping Main hand Weapon
                {
                    if (mainHand == null)
                    {
                        mainHand = weapon;
                        weaponSwaped = true;
                        break;
                    }
                    if (mainHand != null && mainHand.WieldAttribute != WieldAttribute.TwoHanded)
                    {
                        UnEquipWeapon(mainHand);
                        mainHand = weapon;
                        weaponSwaped = true;
                        break;
                    }
                    if (mainHand != null && mainHand.WieldAttribute == WieldAttribute.TwoHanded)
                    {
                        UnEquipWeapon(mainHand);
                        mainHand = weapon;
                        weaponSwaped = true;
                        break;
                    }
                }
                if (weapon.WieldAttribute == WieldAttribute.OneHanded) //equipping One Handed Weapon
                {
                    if (mainHand == null && offHand == null)
                    {
                        mainHand = weapon;
                        weaponSwaped = true;
                        break;
                    }
                    if (mainHand != null && offHand == null)
                    {
                        offHand = weapon;
                        weaponSwaped = true;
                        break;
                    }
                    if (mainHand != null && offHand != null && mainHand.WieldAttribute != WieldAttribute.TwoHanded)
                    {
                        UnEquipWeapon(offHand);
                        offHand = weapon;
                        weaponSwaped = true;
                        break;
                    }
                    if (mainHand.WieldAttribute == WieldAttribute.TwoHanded)
                    {
                        UnEquipWeapon(mainHand);
                        mainHand = weapon;
                        weaponSwaped = true;
                        break;
                    }

                }
                if (weapon.WieldAttribute == WieldAttribute.TwoHanded) //equipping two Handed Weapon
                {
                    if (mainHand == null && offHand == null)
                    {
                        mainHand = weapon;
                        offHand = twoHanded;
                        weaponSwaped = true;
                        break;
                    }
                    if (mainHand != null && offHand == null )
                    {
                        UnEquipWeapon(mainHand);
                        mainHand = weapon;
                        offHand = twoHanded;
                        weaponSwaped = true;
                        break;
                    }
                    if (mainHand != null && offHand != null && mainHand.WieldAttribute != WieldAttribute.TwoHanded  )
                    {
                        UnEquipWeapon(mainHand);
                        UnEquipWeapon(offHand);
                        mainHand = weapon;
                        offHand = twoHanded;
                        weaponSwaped = true;
                        break;
                    }
                    if (mainHand.WieldAttribute == WieldAttribute.TwoHanded)
                    {
                        mainHand = weapon;
                        weaponSwaped = true;
                        break;
                    }
                }
            }

            UpdateStats();
        }
        public void UnEquipWeapon(Weapon hand) //unequip current weapon
        {
            AddToWeaponInventory(hand);

            if (hand.WieldAttribute == WieldAttribute.TwoHanded)
            {
                mainHand = null;
                offHand = null;
            }
            else if (hand.WieldAttribute == WieldAttribute.MainHand)
            {
                mainHand = null;
            }       
        }
        #endregion

        #region Equipment methods

        public void AddToWeaponInventory(Item item) //adds item to player's inventory
        {
            WeaponInventory.Add(item);
        }
        public void RemoveFromWeaponInventory(int slot) //removes item from player's inventory
        {
            WeaponInventory.RemoveAt(slot);
        }

        public void AddToItemInventory(Item item)
        {
            ItemInventory.Add(item);
        }
        public void RemoveFromItemInventory(int slot)
        {
            ItemInventory.RemoveAt(slot);
        }

        public void Equip(Equipment equipment , int inventorySlot)
        {
            bool finishedEquipping = false;
            while (finishedEquipping == false)
            {
                if (equipment.EquipmentSlot == EquipmentSlot.Head)
                {
                    UnEquip(head);
                    head = equipment;
                    finishedEquipping = true;
                }
                if (equipment.EquipmentSlot == EquipmentSlot.Chest)
                {
                    UnEquip(chest);
                    chest = equipment;
                    finishedEquipping = true;
                }
                if (equipment.EquipmentSlot == EquipmentSlot.Hands)
                {
                    UnEquip(hands);
                    hands = equipment;
                    finishedEquipping = true;
                }
                if (equipment.EquipmentSlot == EquipmentSlot.Wrists)
                {
                    UnEquip(wrists);
                    wrists = equipment;
                    finishedEquipping = true;
                }
                if (equipment.EquipmentSlot == EquipmentSlot.Legs)
                {
                    UnEquip(legs);
                    legs = equipment;
                    finishedEquipping = true;
                }
                if (equipment.EquipmentSlot == EquipmentSlot.Feet)
                {
                    UnEquip(feet);
                    feet = equipment;
                    finishedEquipping = true;
                }
            }
            UpdateStats();

        }
        public void UnEquip(Equipment bodySlot)
        {
            if (bodySlot.Name != null)
            {
               itemInventory.Add(bodySlot);
            }
            bodySlot = emptyEquipment;
        }

        public void Use(Item item,int slot)
        {
            item.Use(this);
            RemoveFromItemInventory(slot);
        }
        #endregion

        #region Level Methods

        public void AddExp(double exp) //add exp to the player
        {
            this.exp += exp;
            checkLevel();
            UpdateStats();
        }
        private void checkLevel()//call level up when player is above expCap
        {
            if (exp >= MaxExp)
            {
                levelUp();
            }
        } 

        private void levelUp()//Level up the player
        {
            level++;
            exp = 0;
            updateMaxExp();
            updateMaxHp();
            hitPoints = maxHitPoints;
        } 
        private void updateMaxExp()//update the player's MaxExp
        {
            double neededExp = 40;
            for (int i = 1; i <= level; i++)
            {
                MaxExp += neededExp;
                neededExp = neededExp * 1.5;
            }
        } 
        private void updateMaxHp()// update the player's maxHP
        {
            double hpPerLevel = 20;
            for (int i = 1; i < level; i++)
            {
                MaxHitPoints += hpPerLevel;
                hpPerLevel = hpPerLevel * 1.1;
            }
        }

        #endregion


        #endregion
    }

    class Monster : Fighter
    {
        private double exp;
        public double Exp { get { return exp; } set { exp = value; } }
        private double dropChance;
        public double DropChance { get { return dropChance; } set { dropChance = value; } }

        public Monster()
        {

        }
        public Monster(string name, double hitPoints, double attackPoints, double dropChance , double exp)
        {
            this.name = name;
            this.hitPoints = hitPoints;
            this.attackPoints = attackPoints;
            this.dropChance = dropChance;
            this.exp = exp;
        }
    }
}


