namespace TeamAndatHypori.Objects.Items.Equipment
{
    using TeamAndatHypori.Enums;
    using TeamAndatHypori.Interfaces.Items;

    public abstract class Equipment : Item, IEquipable
    {
        public EquipmentSlot Slot { get; set; }
    }
}
