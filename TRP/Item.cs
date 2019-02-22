using System;
using System.Collections.Generic;
using System.Text;

namespace TRP
{

    enum Rarity {Common,Rare,Legendary,Divine}

    class Item : Body
    {
        private Rarity rarity;
        public Rarity Rarity { get; set; }

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
