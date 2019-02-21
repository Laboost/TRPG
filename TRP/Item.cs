using System;
using System.Collections.Generic;
using System.Text;

namespace TRP
{
    class Item : Body
    {
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

    public class ItemEventArgs : EventArgs
    {
        public int MyProperty { get; set; }
    }
}
