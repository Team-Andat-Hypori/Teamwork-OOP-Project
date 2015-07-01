namespace TeamAndatHypori.Objects.Characters.NPCs.Enemies
{
    using TeamAndatHypori.Enums;
    using TeamAndatHypori.Objects.Characters.PlayableCharacters;

    public abstract class Enemy : Character
    {
        protected Enemy()
        {
            this.Direction = Direction.Left;
        }

        public int ExperienceReward { get; protected set; }

        public virtual void Attack(Player player)
        {
            if (player.IsAlive)
            {
                player.RespondToAttack(this.AttackDamage);
            }
        }
    }
}
