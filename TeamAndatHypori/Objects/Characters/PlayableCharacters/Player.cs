namespace TeamAndatHypori.Objects.Characters.PlayableCharacters
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    using TeamAndatHypori.Config;
    using TeamAndatHypori.Enums;
    using TeamAndatHypori.Objects.Characters.NPCs.Enemies;
    using TeamAndatHypori.Objects.Items;
    using TeamAndatHypori.Objects.Items.Equipment;

    public abstract class Player : Character
    {
        private int inventoryIsFullTimeout;

        public Dictionary<EquipmentSlot, Equipment> PlayerEquipment { get; set; }

        public IList<Potion> ActivePotions { get; set; }

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

        public int Level { get; protected set; }

        public override void Update()
        {
            this.Move();
            this.Position = new Vector2(
                MathHelper.Clamp(this.Position.X, -Config.OffsetX, Config.ScreenWidth - Config.OffsetX - this.Width),
                MathHelper.Clamp(this.Position.Y, -Config.OffsetX, Config.ScreenHeight - Config.OffsetY - this.Height));
            this.Bounds = new BoundingBox(new Vector3(this.Position.X + Config.OffsetX, this.Position.Y + Config.OffsetY, 0), new Vector3(this.Position.X + Config.OffsetX + this.Width, this.Position.Y + Config.OffsetY + this.Height, 0));

            this.TimeOutPotions();
            this.UpdateDirection();
            this.UpdateAttackBounds();
        }

        public IList<Enemy> GetEnemiesInRange(IList<Enemy> enemies)
        {
            return enemies.Where(enemy => this.AttackBounds.Intersects(enemy.Bounds)).ToList();
        }

        public void Attack(IList<Enemy> enemiesInRange)
        {
            foreach (var enemy in enemiesInRange)
            {
                enemy.Health -= this.AttackDamage;
            }
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

        private void UpdateDirection()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                this.Direction = Direction.Right;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                this.Direction = Direction.Left;
            }

        }

        private void UpdateAttackBounds()
        {
            if (this.Direction == Direction.Right)
            {
                this.AttackBounds = new BoundingBox(new Vector3(this.Position.X + Config.OffsetX + this.Width, this.Position.Y, 0), new Vector3(this.Position.X + (Config.OffsetX * 2) + this.Width, this.Position.Y + Config.OffsetY + this.Height, 0));
            }
            else if (this.Direction == Direction.Left)
            {
                this.AttackBounds = new BoundingBox(new Vector3(this.Position.X, this.Position.Y, 0), new Vector3(this.Position.X + Config.OffsetX, this.Position.Y + Config.OffsetY + this.Height, 0));
            }
        }
    }
}
