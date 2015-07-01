namespace TeamAndatHypori.Objects.Characters.PlayableCharacters
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using TeamAndatHypori.Configuration;
    using TeamAndatHypori.Enums;
    using TeamAndatHypori.Objects.Characters.NPCs.Enemies;
    using TeamAndatHypori.Objects.Items;

    public class Warrior : Player
    {
        #region Constants
        private const Name DefaultName = Name.Warrior;
        private const int DefaultAttackPoints = 40;
        private const int DefaultDefencePoints = 10;
        private const int DefaultSpeedPoints = 4;
        private const int DefaultHealthPoints = 200;
        private const int DefaultAttackRadius = 60;
        private const int DefaultLevel = 1;
        private const int DefaultExperience = 0;
        private const int DefaultWidth = 64;
        private const int DefaultHeight = 96;
        #endregion
        private static Random rand;

        public Warrior(int x, int y)
        {
            rand = new Random();
            this.Name = DefaultName;
            this.AttackDamage = DefaultAttackPoints;
            this.Defense = DefaultDefencePoints;
            this.Speed = DefaultSpeedPoints;
            this.Health = DefaultHealthPoints;
            this.Level = DefaultLevel;
            this.Experience = DefaultExperience;
            this.Width = DefaultWidth;
            this.Height = DefaultHeight;
            this.Position = new Vector2(x, y);
            this.Bounds = new BoundingBox(new Vector3(x + Config.OffsetX, y + Config.OffsetY, 0), new Vector3(x + Config.OffsetX + this.Width, y + Config.OffsetY + this.Height, 0));
            this.AttackRadius = DefaultAttackRadius;
            this.AttackBounds = new BoundingBox(new Vector3(this.Position.X + Config.OffsetX + this.Width, this.Position.Y, 0), new Vector3(this.Position.X + Config.OffsetX + this.Width + this.AttackRadius, this.Position.Y + Config.OffsetY + this.Height, 0));
            this.MaxHealth = DefaultHealthPoints;
        }

        public override void RespondToAttack(int damage)
        {
            int blockRoll = rand.Next(0, 100);
            if (blockRoll < 50)
            {
                this.Health -= damage - this.Defense;
            }
        }

        public virtual void SpecialAttack(IList<Enemy> enemiesInRange)
        {
            foreach (var enemy in enemiesInRange)
            {
                enemy.RespondToAttack(this.AttackDamage * 2);
            }
        }
    }
}
