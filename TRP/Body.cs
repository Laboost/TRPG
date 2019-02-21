using System;
using System.Collections.Generic;
using System.Text;

namespace TRP
{
    class Body
    {
        protected string name;
        protected int power;

        public string Name { get { return name; } set { name = value; } }
        public int Power { get { return power; } set { power = value; } }
    }

    class Fighter : Body
    {
        protected List<Item> inventory = new List<Item>();
        protected int hitPoints;
        protected int attackPoints;

        public int AttackPoints { get { return attackPoints; } set { attackPoints = value; } }
        public int HitPoints { get { return hitPoints; } set { hitPoints = value; } }
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

        public delegate void InventoryEventHandler(object source, ItemEventArgs args);

        public event InventoryEventHandler ItemAdded;
        public event InventoryEventHandler ItemRemoved;

        public Player(string name, int hitPoints, int power, Weapon equippedWeapon)
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
            OnItemAdded(item,inventory.Count - 1,this);
        }
        public void RemoveFromInventory(int slot) //removes item from player's inventory
        {
            Inventory.RemoveAt(slot);
        }

        protected virtual void OnItemAdded(Item addedItem , int itemSlot , Player player)
        {
            if (ItemAdded != null)
            {
                ItemAdded(this,new ItemEventArgs() {item = addedItem, slot = itemSlot, player = player } );
            }
        }
        protected virtual void onItemRemoved(Item addedItem , int itemSlot, Player player)
        {
            if (ItemRemoved!= null)
            {
                ItemRemoved(this, new ItemEventArgs() { item = addedItem, slot = itemSlot, player = player });
            }
        }
    }

    class Monster : Fighter
    { 
        public Monster(string name, int hitPoints, int attackPoints)
        {
            this.name = name;
            this.hitPoints = hitPoints;
            this.attackPoints = attackPoints;
        }
    }

    #region EventArgsRegion

    class ItemEventArgs : EventArgs
    {
        public Item item { get; set; }
        public int slot { get; set; }
        public Player player { get; set; }
    }
    #endregion
}

