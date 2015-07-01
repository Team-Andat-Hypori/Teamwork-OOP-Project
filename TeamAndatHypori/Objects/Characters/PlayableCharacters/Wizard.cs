using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamAndatHypori.Objects.Characters.PlayableCharacters
{
    class Wizard : Player
    {
        public override void RespondToAttack(int damage)
        {
            this.Health -= damage - this.Defense;
        }
    }
}
