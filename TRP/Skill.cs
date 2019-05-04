using System;
using System.Collections.Generic;
using System.Text;

namespace TRP
{
    class Skill
    {
        public string  Name { get; set; }
        public double Damage { get; set; }

        public Skill(string Name, double Damage)
        {
            this.Name = Name;
            this.Damage = Damage;
        }
    }
}
