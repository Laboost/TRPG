using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TRP
{

    public enum Rarity { Common, Rare, Legendary, Divine }
    public enum WieldAttribute { MainHand, OneHanded, TwoHanded }
    public enum EquipmentSlot {Head,Chest,Hands,Wrists,Legs,Feet,Weapon }
    public enum ConsumableType { HealthPotion}

    class Item : Body
    {
        protected string description;
        public string Description { get { return description; } set { description = value; } }

        private int dropChance;
        public int DropChance { get { return dropChance; } set { dropChance = value; } }

        private Rarity rarity;
        public Rarity Rarity
        {
            get { return rarity; }
            set
            {
                if (rarity != value)
                {
                    rarity = value;
                }
            }
        }

        public Item()
        {

        }

        public Item(string name, double power, int dropChance)
        {
            this.dropChance = dropChance;
            this.name = name;
            this.power = power;
            UpdateStats();
        }

        public Item(string name, double power, Rarity rarity)
        {
            this.name = name;
            this.power = power;
            this.rarity = rarity;
            UpdateStats();
        }

        public virtual void Use(Player player)
        {

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

    class Weapon : Equipment
    {

        private WieldAttribute wieldAttribute;
        public WieldAttribute WieldAttribute { get { return wieldAttribute; } set { wieldAttribute = value; } }

        public Weapon()
        {

        }

        public Weapon(string name, double power, WieldAttribute wa, int dropChance) : base(name, power,0,EquipmentSlot.Weapon,dropChance)
        {
            wieldAttribute = wa;
        }
        public Weapon(string name, double power, Rarity rarity, WieldAttribute wa, int dropChance) : base(name, power, rarity)
        {
            wieldAttribute = wa;
        }
    }

    class Equipment: Item
    {
        private EquipmentSlot equipmentSlot;
        public EquipmentSlot EquipmentSlot { get { return equipmentSlot; } set { equipmentSlot = value; } }

        public Equipment()
        {

        }

        public Equipment(string name, double power,double armor, EquipmentSlot equipmentSlot,int dropChance) : base(name,power,dropChance)
        {
            this.equipmentSlot = equipmentSlot;
            this.armor = armor;
        }

        public Equipment(string name , double power, Rarity rarity)
        {

        }
    }

    class Consumable : Item
    {
        private ConsumableType consumableType;
        public ConsumableType ConsumableType { get { return consumableType; } set { consumableType = value; } }

        public Consumable()
        {

        }
        public Consumable(string name, double power, int dropChance, ConsumableType consumableType, string description) : base(name, power, dropChance)
        {
            this.consumableType = consumableType;
            this.description = description;
        }

        public Consumable(string name, double power, Rarity rarity) : base(name, power, rarity)
        {

        }

        public virtual void Consume(Player player)
        {
            if (consumableType == ConsumableType.HealthPotion)
            {
                if (player.MaxHitPoints - player.HitPoints > power)
                {
                   player.HitPoints += power;
                }
                else
                {
                    player.HitPoints = player.MaxHitPoints;
                }
            }
        }

        public override void Use(Player player)
        {
            Consume(player);
        }
    }

}
