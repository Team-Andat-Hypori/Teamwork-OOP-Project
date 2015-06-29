namespace TeamAndatHypori.Objects.Items
{
    using TeamAndatHypori.Interfaces.Items;

    public abstract class Item : GameObject,IItem
    {
        public int SpeedPointsBuff { get; set; }

        public int DefensePointsBuff { get; set; }

        public int AttackPointsBuff { get; set; }

        public int HealthPointsBuff { get; set; }
    }
}
