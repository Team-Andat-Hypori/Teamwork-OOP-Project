namespace TeamAndatHypori.Objects.Characters.NPCs.Enemies
{
    using TeamAndatHypori.Objects.Characters.PlayableCharacters;

    public abstract class Enemy : Character
    {
        public int ExperienceReward { get; protected set; }

        public virtual void Attack(Player player)
        {
            if (player.IsAlive)
            {
                //add case for warrior
                player.RespondToAttack(this.AttackDamage);
            }
        }

        public override void RespondToAttack(int damage)
        {
            base.RespondToAttack(damage);

        }
    }

    
}
