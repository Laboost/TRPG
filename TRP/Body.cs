using System;
using System.Collections.Generic;
using System.Text;

namespace TRP
{
    class Body
    {
        protected string name;
        protected int power;

        public string Name { get { return name; } set { name = value; } }
        public int Power { get { return power; } set { power = value; } }
    }

    class Fighter : Body
    {
        protected List<Item> inventory = new List<Item>();
        protected int hitPoints;
        protected int attackPoints;

        public int AttackPoints { get { return attackPoints; } set { attackPoints = value; } }
        public int HitPoints { get { return hitPoints; } set { hitPoints = value; } }
        public List<Item> Inventory
        {
            get { return inventory; }
            set { inventory = value; }
        }
    }
    class Player : Fighter
    {
        private Weapon equippedWeapon = new Weapon("Sword",4);
        public Weapon EquippedWeapon { get { return equippedWeapon; } set { equippedWeapon = value; } }
        

          public Player(string name, int hitPoints, int power, Weapon equippedWeapon)
        {
            this.equippedWeapon = equippedWeapon;
            this.name = name;
            this.hitPoints = hitPoints;
            this.power = power;           
        }

        public void UpdateAP() //updates the player AttackPoints
        {
            attackPoints = equippedWeapon.Power + power;
        }

        public void EquipWeapon(Weapon weapon)
        {
            UnEquipWeapon();
            equippedWeapon = weapon;
        }

        public void UnEquipWeapon()
        {
            inventory[inventory.Count - 1] = equippedWeapon;
            equippedWeapon = null;
        }
    }

    class Monster : Fighter
    { 
        public Monster(string name, int hitPoints, int attackPoints)
        {
            this.name = name;
            this.hitPoints = hitPoints;
            this.attackPoints = attackPoints;
        }
    }
}
    
