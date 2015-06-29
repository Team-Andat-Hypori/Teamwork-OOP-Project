namespace TeamAndatHypori.Objects.Items
{
    using TeamAndatHypori.Interfaces.Items;

    public abstract class Potion : Item,IConsumable
    {
        public int Duration { get; set; }
    }
}
