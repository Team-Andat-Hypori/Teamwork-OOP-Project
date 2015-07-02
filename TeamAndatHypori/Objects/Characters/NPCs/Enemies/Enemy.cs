using Microsoft.Xna.Framework;
using TeamAndatHypori.Configuration;
using TeamAndatHypori.Interfaces.Characters;

namespace TeamAndatHypori.Objects.Characters.NPCs.Enemies
{
    using TeamAndatHypori.Enums;
    using TeamAndatHypori.Objects.Characters.PlayableCharacters;

    public abstract class Enemy : Character,IEnemy
    {
        protected Enemy()
        {
            this.Direction = Direction.Left;
        }

        public int ExperienceReward { get; protected set; }

        public virtual void Attack(Player player)
        {
            if (player.IsAlive)
            {
                player.RespondToAttack(this.AttackDamage);
            }
        }

        public override void Update()
        {
            this.Position = new Vector2(
                MathHelper.Clamp(this.Position.X, 0, Config.ScreenWidth - Config.OffsetX - this.Width),
                MathHelper.Clamp(this.Position.Y, 200f, Config.ScreenHeight - Config.OffsetY - this.Height - 135));
            this.Bounds = new BoundingBox(new Vector3(this.Position.X , this.Position.Y , 0), new Vector3(this.Position.X + this.Width, this.Position.Y + this.Height, 0));

            this.UpdateAttackBounds();
        }

         private void UpdateAttackBounds()
        {
            if (this.Direction == Direction.Right)
            {
                this.AttackBounds = new BoundingBox(new Vector3(this.Position.X + this.Width, this.Position.Y, 0), new Vector3(this.Position.X +  this.Width + this.AttackRadius, this.Position.Y + this.Height, 0));
            }
            else if (this.Direction == Direction.Left)
            {
                this.AttackBounds = new BoundingBox(new Vector3(this.Position.X + Config.OffsetX - this.AttackRadius, this.Position.Y + Config.OffsetY - this.AttackRadius, 0), new Vector3(this.Position.X + Config.OffsetX, this.Position.Y + Config.OffsetY + this.Height, 0));
            }
        }
    }
}
