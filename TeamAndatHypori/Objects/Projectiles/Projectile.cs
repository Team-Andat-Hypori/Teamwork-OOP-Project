namespace TeamAndatHypori.Objects.Projectiles
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using TeamAndatHypori.Enums;

    public abstract class Projectile : GameObject
    {
        
        protected Projectile(int x, int y, Direction direction,int damage)
        {
            this.Position = new Vector2(x,y);
            this.Direction = direction;
            this.Bounds = new BoundingBox(new Vector3(x, y, 0), new Vector3(x + this.Width, y + this.Height, 0));
        }

        public int Speed { get; set; }

        public Direction Direction { get; set; }

        public int Damage { get; set; }
    }
}
