namespace TeamAndatHypori.Objects.Characters.NPCs.Enemies
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using TeamAndatHypori.Enums;

    public class OrcLord : Enemy
    {
        #region Constants
        private const Name DefaultName = Name.OrcLeader;
        private const int DefaultAttackPoints = 50;
        private const int DefaultDefensePoints = 20;
        private const int DefaultSpeedPoints = 3;
        private const int DefaultHealthPoints = 250;
        private const int DefaultAttackRadius = 60;
        private const int DefaultLevel = 2;
        private const int DefaultExperienceReward = 300;
        private const int DefaultWidth = 80;
        private const int DefaultHeight = 120;
        #endregion

        public OrcLord(int x,int y)
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
