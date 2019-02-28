using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TRP
{

    enum Rarity {Common,Rare,Legendary,Divine}
    public enum WieldAttribute { MainHand, OneHanded, TwoHanded }

    class Item : Body
    {
        private double dropChance;
        public double DropChance { get { return dropChance; } set { dropChance = value; } }

        private Rarity rarity;
        public Rarity Rarity {
            get { return rarity; }
            set {
                if (rarity != value)
                {
                    rarity = value;
                }
           }
        }

        public Item(string name, int power, double dropChance)
        {
            this.dropChance = dropChance;
            this.name = name;
            this.power = power;        
        }

        public Item(string name, int power, Rarity rarity)
        {
            this.name = name;
            this.power = power;
            this.rarity = rarity;
        }

        public void UpdateStats()
        {
            if (rarity == Rarity.Rare)
            {
                power = power * 1.20;
            }
            if (rarity == Rarity.Legendary)
            {
                power = power * 2;
            }
            if (rarity == Rarity.Divine)
            {
                power = power * 2.5;
            }
        } //Updates the item stats by his Attributes
    }

    class Weapon : Item
    {

        private WieldAttribute wieldAttribute;
        public WieldAttribute WieldAttribute { get { return wieldAttribute; } set { wieldAttribute = value; } }
                
        public Weapon(string name, int power,WieldAttribute wa,int dropChance) : base(name, power,dropChance)
        {
           wieldAttribute = wa;
        }
        public Weapon(string name, int power, Rarity rarity , WieldAttribute wa) : base(name, power, rarity)
        {
           wieldAttribute = wa;
        }
    }
}
