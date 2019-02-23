using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TRP
{

    enum Rarity {Common,Rare,Legendary,Divine}
    public enum WieldAttribute { MainHand, OffHand, TwoHanded }

    class Item : Body
    {
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

        public Item()
        {

        }

        public Item(string name, int power)
        {
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
        public WieldAttribute WieldAttribute { get { return wieldAttribute; }
        }

        public Weapon()
        {

        }
        public Weapon(string name, int power,WieldAttribute wieldAttribute) : base(name, power)
        {
            this.wieldAttribute = wieldAttribute;
        }
        public Weapon(string name, int power, Rarity rarity , WieldAttribute wieldAttribute) : base(name, power, rarity)
        {
            this.wieldAttribute = wieldAttribute;
        }
    }

}
