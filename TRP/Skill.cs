using System;
using System.Collections.Generic;
using System.Text;

namespace TRP
{
    class Skill
    {
        public string  Name { get; set; }
        public int Damage { get; set; }

        public Skill(string Name, int Damage)
        {
            this.Name = Name;
            this.Damage = Damage;
        }
    }
}
