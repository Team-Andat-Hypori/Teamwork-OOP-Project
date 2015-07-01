namespace TeamAndatHypori.Interfaces.Items
{
    using TeamAndatHypori.Enums;

    public interface IEquipable
    {
        EquipmentSlot Slot { get; set; }
    }
}
