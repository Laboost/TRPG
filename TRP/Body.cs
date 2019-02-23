﻿using System;
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
        private Weapon equippedWeapon = new Weapon("Sword", 4);
        public Weapon EquippedWeapon { get { return equippedWeapon; } set { equippedWeapon = value; } }

        public Player(string name, double hitPoints, double power, Weapon equippedWeapon)
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

        public void EquipWeapon(Weapon weapon , int slot) //equip given weapon
        {
            UnEquipWeapon();
            RemoveFromInventory(slot);
            equippedWeapon = weapon;
            UpdateAP();
        }

        public void UnEquipWeapon() //unequip current weapon
        {
            AddToInventory(equippedWeapon);
            equippedWeapon = null;
            
        }

        public void AddToInventory(Item item) //adds item to player's inventory
        {
            Inventory.Add(item);
        }
        public void RemoveFromInventory(int slot) //removes item from player's inventory
        {
            Inventory.RemoveAt(slot);
        }

    }

    class Monster : Fighter , ICloneable
    { 
        public Monster(string name, double hitPoints, double attackPoints)
        {
            this.name = name;
            this.hitPoints = hitPoints;
            this.attackPoints = attackPoints;
        }
        public Object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}


