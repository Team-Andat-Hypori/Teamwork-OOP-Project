namespace TeamAndatHypori.Objects.Characters.NPCs.Enemies
{
    using TeamAndatHypori.Objects.Characters.PlayableCharacters;

    public abstract class Enemy : Character
    {
        public int ExperienceReward { get; protected set; }

        public void Attack(Player player)
        {
            if (player.IsAlive)
            {
                //add case for warrior
                player.Health -= this.AttackDamage;
            }
        }
    }
}
