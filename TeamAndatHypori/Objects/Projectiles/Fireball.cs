namespace TeamAndatHypori.Objects.Projectiles
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using TeamAndatHypori.Enums;

    public class Fireball : Projectile
    {
        #region Constants
        private const Name DefaultName = Name.Fireball;
        private const int DefaultWidth = 90;
        private const int DefaultHeight = 50;
        #endregion

        public Fireball(int x, int y, Direction direction, int damage)
            : base(x, y, direction, damage)
        {
            this.Name = DefaultName;
            this.Width = DefaultWidth;
            this.Height = DefaultHeight;
        }
    }
}
