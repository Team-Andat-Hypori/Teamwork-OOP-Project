namespace TeamAndatHypori.Objects.Characters
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using TeamAndatHypori.Enums;

    public abstract class Character : GameObject
    {
        public virtual int AttackDamage { get; protected set; }

        public virtual int Defense { get; protected set; }

        public virtual int Speed { get; protected set; }

        public int Health { get; set; }

        public int AttackRadius { get; set; }

        public BoundingBox AttackBounds { get; set; }

        public bool IsAlive { get; set; }

        public Direction Direction { get; set; }

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

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Image, this.Position, Color.White);
        }

        protected abstract void Move();
    }
}
