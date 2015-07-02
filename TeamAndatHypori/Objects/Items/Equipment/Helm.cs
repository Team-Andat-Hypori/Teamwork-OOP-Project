using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using TeamAndatHypori.Enums;

namespace TeamAndatHypori.Objects.Items.Equipment
{
    public class Helm : Equipment
    {
        #region Constants
        private const Name DefaultName = Name.Armor;
        private const int DefaultAttackPointsBonus = 0;
        private const int DefaultDefencePointsBonus = 2;
        private const int DefaultSpeedPointsBonus = 0;
        private const int DefaultHealthPointsBonus = 5;
        #endregion

        public Helm()
            : base(DefaultSpeedPointsBonus, DefaultDefencePointsBonus, DefaultAttackPointsBonus, DefaultHealthPointsBonus)
        {
               this.Slot = EquipmentSlot.Head; 
        }
    }
}
