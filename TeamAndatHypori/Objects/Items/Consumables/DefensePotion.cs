using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace TeamAndatHypori.Objects.Items.Consumables
{
    public class DefensePotion : Potion
    {
        public override void Update()
        {
            this.Duration = 2000;
            this.DefensePointsBuff = 10;
        }


    }
}
