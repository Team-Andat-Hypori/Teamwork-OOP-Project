namespace TeamAndatHypori.Objects
{
    using Microsoft.Xna.Framework;

    using TeamAndatHypori.Enums;

    public class Explosion : GameObject
    {
        #region Constants
        private const Name DefaultName = Name.Explosion;
        private const int DefaultWidth = 150;
        private const int DefaultHeight = 150;
        #endregion

        public Explosion(int x, int y, int damage)
        {
            this.Name = DefaultName;
            this.Width = DefaultWidth;
            this.Height = DefaultHeight;
            this.Position = new Vector2(x, y);
            this.Damage = damage;
            this.Bounds = new BoundingBox(new Vector3(x, y, 0), new Vector3(x + this.Width, y + this.Width, 0));
            this.AnimationFrame = 0;
            this.AnimationDelay = 10;
        }

        public int AnimationFrame { get; set; }

        public int AnimationDelay { get; set; }

        public int Damage { get; set; }
    }
}
