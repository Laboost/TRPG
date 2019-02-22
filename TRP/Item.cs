using System;
using System.Collections.Generic;
using System.Text;

namespace TRP
{

    enum Rarity {Common,Rare,Legendary,Divine}

    class Item : Body
    {
        public Rarity Rarity { get; set; }
        public Item(string name, int power)
        {
            this.name = name;
            this.power = power;        
        }
    }

    class Weapon : Item
    {
        public Weapon(string name, int power) : base(name, power)
        {

        }
    }
}
