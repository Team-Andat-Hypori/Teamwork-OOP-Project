using TeamAndatHypori.Objects.Items.Consumables;

namespace TeamAndatHypori.Objects.Characters.PlayableCharacters
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    using TeamAndatHypori.Configuration;
    using TeamAndatHypori.Enums;
    using TeamAndatHypori.Objects.Characters.NPCs.Enemies;
    using TeamAndatHypori.Objects.Items;
    using TeamAndatHypori.Objects.Items.Equipment;

    public abstract class Player : Character
    {
        private int inventoryIsFullTimeout;

        protected Player()
        {
            this.ActivePotions = new List<Potion>();
            this.PlayerEquipment = new Dictionary<EquipmentSlot, Equipment>();
            this.Inventory = new Item[Config.InventorySize];
            this.Direction = Direction.Right;
        }

        public Dictionary<EquipmentSlot, Equipment> PlayerEquipment { get; set; }

        public IList<Potion> ActivePotions { get; set; }

        public Item[] Inventory { get; set; }

        public int Experience { get; set; }

        public override int AttackDamage
        {
            get
            {
                int attackBonus = this.PlayerEquipment.Sum(item => item.Value.AttackPointsBuff);
                attackBonus += this.ActivePotions.Sum(potion => potion.AttackPointsBuff);
                return base.AttackDamage + attackBonus;
            }
        }

        public override int Defense
        {
            get
            {
                int defenseBonus = this.PlayerEquipment.Sum(item => item.Value.DefensePointsBuff);
                defenseBonus += this.ActivePotions.Sum(potion => potion.DefensePointsBuff);
                return base.Defense + defenseBonus;
            }
        }

        public override int Speed
        {
            get
            {
                int speedBonus = this.PlayerEquipment.Sum(item => item.Value.SpeedPointsBuff);
                speedBonus += this.ActivePotions.Sum(potion => potion.SpeedPointsBuff);
                return base.Speed + speedBonus;
            }
        }

        public int MaxHealth { get; protected set; }

        public int Level { get; protected set; }

        public bool InventoryIsFull
        {
            get
            {
                if (this.inventoryIsFullTimeout > 0)
                {
                    this.inventoryIsFullTimeout--;
                    return true;
                }

                return false;
            }
        }

        public override void Update()
        {
            this.Position = new Vector2(
                MathHelper.Clamp(this.Position.X, -Config.OffsetX, Config.ScreenWidth - Config.OffsetX - this.Width),
                MathHelper.Clamp(this.Position.Y, (float)(Config.ScreenHeight / 2.0), Config.ScreenHeight - Config.OffsetY - this.Height));
            this.Bounds = new BoundingBox(new Vector3(this.Position.X + Config.OffsetX, this.Position.Y + Config.OffsetY, 0), new Vector3(this.Position.X + Config.OffsetX + this.Width, this.Position.Y + Config.OffsetY + this.Height, 0));

            this.TimeOutPotions();
            this.UpdateAttackBounds();
        }

        public IList<Enemy> GetEnemiesInRange(IList<Enemy> enemies)
        {
            return enemies.Where(enemy => this.AttackBounds.Intersects(enemy.Bounds)).ToList();
        }

        public virtual void Attack(IList<Enemy> enemiesInRange)
        {
            foreach (var enemy in enemiesInRange)
            {
                enemy.RespondToAttack(this.AttackDamage);
            }
        }

        public void AddExperience(Enemy enemy)
        {
            this.Experience += enemy.ExperienceReward;
        }

        public void DiscardItem(int inventoryIndex)
        {
            this.Inventory[inventoryIndex] = null;
        }

        public void AddToInventory(Item item)
        {
            bool isAdded = false;
            for (int i = 0; i < this.Inventory.Length; i++)
            {
                if (this.Inventory.ElementAt(i) == null)
                {
                    isAdded = true;
                    this.Inventory[i] = item;
                    break;
                }
            }

            if (!isAdded)
            {
                this.inventoryIsFullTimeout = Config.InventoryIsFullMessageTimeout;
            }
        }

        public void UnequipItem(EquipmentSlot slot)
        {
            if (!this.IsInventoryFull())
            {
                if (this.PlayerEquipment.ContainsKey(slot))
                {
                    this.AddToInventory(this.PlayerEquipment[slot]);
                    this.PlayerEquipment.Remove(slot);
                }
            }
            else
            {
                this.inventoryIsFullTimeout = Config.InventoryIsFullMessageTimeout;
            }
        }

        public void UseItem(int inventoryIndex)
        {
            {
                if (this.Inventory.ElementAt(inventoryIndex) is Potion)
                {
                    var potion = this.Inventory.ElementAt(inventoryIndex) as Potion;
                    if (potion is HealingPotion && this.Health < this.MaxHealth)
                    {
                        var newHealthPoints = this.Health + potion.HealthPointsBuff;
                        this.Health = newHealthPoints > this.MaxHealth ? this.MaxHealth : newHealthPoints;

                        this.Inventory[inventoryIndex] = null;
                    }
                    else if (potion is HealingPotion == false)
                    {
                        this.ActivePotions.Add(potion);
                        this.Inventory[inventoryIndex] = null;
                    }
                }
                else if (this.Inventory.ElementAt(inventoryIndex) is Equipment)
                {
                    var equipment = this.Inventory[inventoryIndex] as Equipment;

                    if (!this.PlayerEquipment.ContainsKey(equipment.Slot))
                    {
                        this.PlayerEquipment[equipment.Slot] = equipment;
                        this.Inventory[inventoryIndex] = null;
                    }
                }

            }
        }


        private bool IsInventoryFull()
        {
            return this.Inventory.All(t => t != null);
        }

        private void TimeOutPotions()
        {
            for (int i = 0; i < this.ActivePotions.Count; i++)
            {
                if (this.ActivePotions[i].Duration == 0)
                {
                    this.ActivePotions.RemoveAt(i);
                    continue;
                }
                this.ActivePotions[i].Duration--;
            }
        }

        private void UpdateAttackBounds()
        {
            if (this.Direction == Direction.Right)
            {
                this.AttackBounds = new BoundingBox(new Vector3(this.Position.X + Config.OffsetX + this.Width, this.Position.Y, 0), new Vector3(this.Position.X + Config.OffsetX + this.Width + this.AttackRadius, this.Position.Y + Config.OffsetY + this.Height, 0));
            }
            else if (this.Direction == Direction.Left)
            {
                this.AttackBounds = new BoundingBox(new Vector3(this.Position.X + Config.OffsetX - this.AttackRadius, this.Position.Y + Config.OffsetY - this.AttackRadius, 0), new Vector3(this.Position.X + Config.OffsetX, this.Position.Y + Config.OffsetY + this.Height, 0));
            }
        }
    }
}
