namespace TeamAndatHypori.Objects
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class GameObject
    {
        public Texture2D Image { get; set; }

        public virtual BoundingBox Bounds { get; set; }

        public Vector2 Position { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public virtual void Initialize(Texture2D texture, Vector2 position)
        {
            this.Image = texture;
            this.Position = position;
        }

        public abstract void Update();

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
