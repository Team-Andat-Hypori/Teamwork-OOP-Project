using TeamAndatHypori.Interfaces.Projectiles;

namespace TeamAndatHypori.Objects.Projectiles
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using TeamAndatHypori.Enums;

    public abstract class Projectile : GameObject,IProjectile
    {
        
        protected Projectile(int x, int y, Direction direction,int damage)
        {
            this.Damage = damage;
            this.Position = new Vector2(x,y);
            this.Direction = direction;
            this.Bounds = new BoundingBox(new Vector3(x, y, 0), new Vector3(x + this.Width, y + this.Height, 0));
        }

        public int Speed { get; protected set; }

        public Direction Direction { get; private set; }

        public int Damage { get; private set; }
    }
}
