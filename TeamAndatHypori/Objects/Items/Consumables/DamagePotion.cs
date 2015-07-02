using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace TeamAndatHypori.Objects.Items.Consumables
{
    public class DamagePotion : Potion
    {
        public DamagePotion()
        {
            this.Duration = 3000;
            this.AttackPointsBuff = 30;
        }
    }
}
