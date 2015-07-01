using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TeamAndatHypori.Objects;

namespace TeamAndatHypori.Objects
{
    public class Map : GameObject
    {
        public Map(Texture2D image)
        {
            this.Image = image;
        }
    }
}
