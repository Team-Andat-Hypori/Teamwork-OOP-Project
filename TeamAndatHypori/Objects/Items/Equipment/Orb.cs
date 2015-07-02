using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using TeamAndatHypori.Enums;

namespace TeamAndatHypori.Objects.Items.Equipment
{
    public class Orb : Equipment
    {
        #region Constants
        private const Name DefaultName = Name.Armor;
        private const int DefaultAttackPointsBonus = 5;
        private const int DefaultDefencePointsBonus = 0;
        private const int DefaultSpeedPointsBonus = 0;
        private const int DefaultHealthPointsBonus = 0;
        #endregion

        public Orb()
            : base(DefaultSpeedPointsBonus, DefaultDefencePointsBonus, DefaultAttackPointsBonus, DefaultHealthPointsBonus)
        {
              this.Slot = EquipmentSlot.Hands;  
        }
    }
}
