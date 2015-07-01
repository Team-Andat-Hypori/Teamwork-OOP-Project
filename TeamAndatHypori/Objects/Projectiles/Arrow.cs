namespace TeamAndatHypori.Objects.Projectiles
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using TeamAndatHypori.Enums;

    public class Arrow : Projectile
    {
        #region Constants
        private const Name DefaultName = Name.Arrow;
        private const int DefaultWidth = 80;
        private const int DefaultHeight = 30;
        #endregion

        public Arrow(int x, int y, Direction direction, int damage)
            : base(x, y, direction, damage)
        {
            this.Name = DefaultName;
            this.Width = DefaultWidth;
            this.Height = DefaultHeight;
        }
    }
}
