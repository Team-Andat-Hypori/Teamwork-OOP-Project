using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamAndatHypori.Objects.Characters.PlayableCharacters
{
    class Warrior : Player
    {
        private static Random Rand;

        public Warrior()
        {
              Rand = new Random();
        }
        public override void RespondToAttack(int damage)
        {
            int blockRoll = Rand.Next(0, 100);
            if (blockRoll < 50)
            {
                this.Health -= damage - this.Defense;
            }
        }
    }
}
