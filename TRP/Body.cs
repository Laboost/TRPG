using System;
using System.Collections.Generic;
using System.Text;

namespace TRP
{
    class Body
    {
        protected string name;
        protected int power;

        public string Name { get { return name; } }
        public int Power { get { return power; } }
    }

    class Fighter : Body
    {
        protected string[] inventory;
        public int hitPoints;              //fix!
        protected int attackPoints;

        public int AttackPoints { get { return attackPoints; } set { attackPoints = AttackPoints; } }
        public int HitPoints { get { return hitPoints; } set { hitPoints = HitPoints; } }
        public string[] Inventory { get { return inventory; } }
    }

    class Player : Fighter
    {
        private Weapon equippedWeapon;
        public Weapon EquippedWeapon { get { return equippedWeapon; } set { equippedWeapon = EquippedWeapon;} }

        public Player(string name, int hitPoints , int power , Weapon equippedWeapon)
        {
            this.equippedWeapon = equippedWeapon;
            this.name = name;
            this.hitPoints = hitPoints;
            this.power = power;
        }

        public void UpdateAP()
        {
            this.attackPoints = equippedWeapon.Power + this.power;
        }
    }

    class Monster : Fighter
    {
        #region Private Vars

        private string[] loot;

        #endregion
    
        public Monster(string name, int hitPoints, int attackPoints)
        {
            this.name = name;
            this.hitPoints = hitPoints;
            this.attackPoints = attackPoints;
        }
    }
}
