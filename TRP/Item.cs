using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TRP
{

    enum Rarity {Common,Rare,Legendary,Divine}

    class Item : Body , ICloneable
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

        public object Clone()
        {
          return this.MemberwiseClone();
        }
    }

    class Weapon : Item
    {
        public Weapon(string name, int power) : base(name, power)
        {

        }
        public Weapon(string name, int power, Rarity rarity) : base(name, power, rarity)
        {

        }
    }

}
