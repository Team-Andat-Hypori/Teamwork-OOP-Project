namespace TeamAndatHypori.Objects.Characters
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using TeamAndatHypori.Configuration;
    using TeamAndatHypori.Enums;

    public abstract class Character : GameObject
    {

        protected Character()
        {
            this.AnimationFrame = 0;
            this.AnimationDelay = 10;
            this.IsAlive = true;
            this.State = State.Idle;
        }

        public virtual int AttackDamage { get; protected set; }

        public virtual int Defense { get; protected set; }

        public virtual int Speed { get; protected set; }

        public int Health { get; set; }

        public int AttackRadius { get; protected set; }

        public BoundingBox AttackBounds { get; protected set; }

        public bool IsAlive { get; protected set; }

        public Direction Direction { get; set; }

        public State State { get; set; }

        public int AnimationFrame { get; set; }

        public int AnimationDelay { get; set; }

        public bool Intersects(BoundingBox box)
        {
            return this.Bounds.Intersects(box);
        }

        public override void Update()
        {
            this.Bounds = new BoundingBox(new Vector3(this.Position.X, this.Position.Y, 0), new Vector3(this.Position.X + this.Width, this.Position.Y + this.Height, 0));

            if (this.Health <= 0)
            {
                this.IsAlive = false;
            }
        }

        public virtual void RespondToAttack(int damage)
        {
            this.Health -= damage - this.Defense;
        }
    }
}
