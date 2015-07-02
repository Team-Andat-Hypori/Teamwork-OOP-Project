using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamAndatHypori.Objects.Characters.NPCs.Enemies
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using TeamAndatHypori.Configuration;
    using TeamAndatHypori.Enums;

    public class Orc : Enemy
    {
        #region Constants
        private const Name DefaultName = Name.Orc;
        private const int DefaultAttackPoints = 30;
        private const int DefaultDefensePoints = 15;
        private const int DefaultSpeedPoints = 3;
        private const int DefaultHealthPoints = 200;
        private const int DefaultAttackRadius = 40;
        private const int DefaultLevel = 2;
        private const int DefaultExperienceReward = 80;
        private const int DefaultWidth = 80;
        private const int DefaultHeight = 120;
        #endregion

        public Orc(int x, int y)
        {
            this.Name = DefaultName;
            this.AttackDamage = DefaultAttackPoints;
            this.Defense = DefaultDefensePoints;
            this.Speed = DefaultSpeedPoints;
            this.Health = DefaultHealthPoints;
            this.ExperienceReward = DefaultExperienceReward;
            this.Width = DefaultWidth;
            this.Height = DefaultHeight;
            this.Position = new Vector2(x, y);
            this.Bounds = new BoundingBox(new Vector3(x, y, 0), new Vector3(x + this.Width, y + this.Height, 0));
            this.AttackRadius = DefaultAttackRadius;
            this.AttackBounds = new BoundingBox(new Vector3(this.Position.X + this.Width, this.Position.Y, 0), new Vector3(this.Position.X + +this.AttackRadius + this.Width, this.Position.Y + this.Height, 0));
        }

    }
}
