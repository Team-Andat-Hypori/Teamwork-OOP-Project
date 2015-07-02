using TeamAndatHypori.Struct;

namespace TeamAndatHypori.Configuration
{
    using Microsoft.Xna.Framework.Input;

    public static class Config
    {
         // GUI settings
        public const int InventoryIsFullMessageTimeout = 300;

        // Screen size
        public const int ScreenWidth = 1091;
        public const int ScreenHeight = 680;

        // Players base constatnts
        public const int InventorySize = 5;
        public const int OffsetX = 100;
        public const int OffsetY = 80;

        // Controls
        public static readonly Keys[] UseItemKeys = { Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5 };
        public static readonly Keys[] DiscardItemKeys = { Keys.Q, Keys.W, Keys.E, Keys.R, Keys.T };

        // Keys sequence "Hand", "Armor", "Head", "Boots", "Arms"
        public static readonly Keys[] UnequipItemKeys = { Keys.A, Keys.S, Keys.D, Keys.F, Keys.G };
    }
}
