using System;
using System.Collections.Generic;
using System.Text;

namespace TRP
{
    class Body
    {
        protected string name;
        protected double power;

        public string Name { get { return name; } set { name = value; } }
        public double Power { get { return power; } set { power = value; } }

    }

    class Fighter : Body
    {
        protected List<Item> inventory = new List<Item>();
        protected double hitPoints;
        protected double attackPoints;

        public double AttackPoints { get { return attackPoints; } set { attackPoints = value; } }
        public double HitPoints { get { return hitPoints; } set { hitPoints = value; } }
        public List<Item> Inventory
        {
            get { return inventory; }
            set { inventory = value; }
        }
    }

    class Player : Fighter
    {
        static private Weapon twoHanded = new Weapon("Two Handed", 0, WieldAttribute.OneHanded,0);
        private Weapon[] hands = new Weapon[] { twoHanded, twoHanded };
        private Weapon mainHand { get { return hands[0]; } set { hands[0] = value; } }
        private Weapon offHand { get { return hands[1]; } set { hands[1] = value; } }

        public Weapon[] EquippedWeapons { get { return hands; } }

        #region Methods
        public Player(string name, double hitPoints, double power, Weapon weapon)
        {
            mainHand = weapon;
            offHand = null;
            this.name = name;
            this.hitPoints = hitPoints;
            this.power = power;
        }

        public void UpdateAP() //updates the player AttackPoints
        {
            if (offHand != null)
            {
                attackPoints = mainHand.Power + offHand.Power + power;
            }
            else attackPoints = mainHand.Power + power;
        }

        public void EquipWeapon(Weapon weapon , int inventorySlot) //equip given weapon
        {
            RemoveFromInventory(inventorySlot);
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
                    if (mainHand != null)
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
                    if (mainHand != null && offHand != null && offHand.WieldAttribute != WieldAttribute.TwoHanded)
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
                    if (mainHand != null && mainHand.WieldAttribute != WieldAttribute.TwoHanded && offHand !=null)
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

            UpdateAP();
        }
        public void UnEquipWeapon(Weapon hand) //unequip current weapon
        {
            AddToInventory(hand);
            if (hand.WieldAttribute == WieldAttribute.TwoHanded)
            {
                mainHand = null;
                offHand = null;
            }
            if (hand.WieldAttribute == WieldAttribute.OneHanded)
            {
                offHand = null;
            }
            else
            {
                mainHand = null;
            }           
        }

        public void AddToInventory(Item item) //adds item to player's inventory
        {
            Inventory.Add(item);
        }
        public void RemoveFromInventory(int slot) //removes item from player's inventory
        {
            Inventory.RemoveAt(slot);
        }
        #endregion
    }

    class Monster : Fighter
    { 
        public Monster()
        {

        }
        public Monster(string name, double hitPoints, double attackPoints)
        {
            this.name = name;
            this.hitPoints = hitPoints;
            this.attackPoints = attackPoints;
        }
    }
}


