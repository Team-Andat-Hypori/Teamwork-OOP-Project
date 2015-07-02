namespace TeamAndatHypori.Objects.Items.Equipment
{
    using TeamAndatHypori.Enums;
    using TeamAndatHypori.Interfaces.Items;

    public abstract class Equipment : Item, IEquipable
    {
        public Equipment(int speedBonus,int defenseBonus, int damageBonus, int healthBonus)
        {
            this.SpeedPointsBuff = speedBonus;
            this.DefensePointsBuff = defenseBonus;
            this.AttackPointsBuff = damageBonus;
            this.HealthPointsBuff = healthBonus;
        }
        public EquipmentSlot Slot { get; set; }
    }
}
