using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TeamAndatHypori.Configuration;
using TeamAndatHypori.CoreLogic;
using TeamAndatHypori.Enums;

namespace TeamAndatHypori.GUI
{
    public class Gui : Game
    {
        private const float BarMaxWidth = 175F;
        private const float HealthAlertLevel = 0.25F;

        private readonly Engine engine;
        private readonly Vector2[] inventoryPositions =
        {
            new Vector2(10, 580),
            new Vector2(70, 580),
            new Vector2(130, 580),
            new Vector2(190, 580),
            new Vector2(250, 580)
        };

        private readonly Dictionary<EquipmentSlot, Vector2> equipmentSlotPositions = new Dictionary<EquipmentSlot, Vector2>()
        {
            { EquipmentSlot.Head, new Vector2(10, 520) },
            { EquipmentSlot.Hands, new Vector2(70, 520) },
            { EquipmentSlot.Arms, new Vector2(130, 520) },
            { EquipmentSlot.Body, new Vector2(190, 520) },
            { EquipmentSlot.Feet, new Vector2(250, 520) }
        };

        private float barCurrentWidth;
        private Color barColor = Color.White;

        public Gui(Engine engine)
        {
            this.engine = engine;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Display inventory
            for (int i = 0; i < this.engine.Player.Inventory.Length; i++)
            {
                if (this.engine.Player.Inventory[i] != null)
                {
                    spriteBatch.Draw(this.engine.Player.Inventory[i].Image, this.inventoryPositions[i], Color.White);
                }
            }

            // Display current equipment
            foreach (var slot in this.engine.Player.PlayerEquipment)
            {
                spriteBatch.Draw(slot.Value.Image,this.equipmentSlotPositions[slot.Key],Color.White);
            }

            // Labels
            spriteBatch.DrawString(this.engine.Font, this.engine.Player.Level.ToString(), new Vector2(380, 580), Color.Black);
            spriteBatch.DrawString(this.engine.Font, this.engine.Player.AttackDamage.ToString(), new Vector2(380, 600), Color.Black);
            spriteBatch.DrawString(this.engine.Font, this.engine.Player.Defense.ToString(), new Vector2(380, 620), Color.Black);
            spriteBatch.DrawString(this.engine.Font, this.engine.Player.Speed.ToString(), new Vector2(380, 640), Color.Black);
            spriteBatch.DrawString(this.engine.Font, this.engine.Player.Experience.ToString(), new Vector2(380, 660), Color.Black);

            spriteBatch.DrawString(this.engine.Font, "Use: 1", new Vector2(10, 610), Color.LightBlue);
            spriteBatch.DrawString(this.engine.Font, "Use: 2", new Vector2(70, 610), Color.LightBlue);
            spriteBatch.DrawString(this.engine.Font, "Use: 3", new Vector2(130, 610), Color.LightBlue);
            spriteBatch.DrawString(this.engine.Font, "Use: 4", new Vector2(190, 610), Color.LightBlue);
            spriteBatch.DrawString(this.engine.Font, "Use: 5", new Vector2(250, 610), Color.LightBlue);

            spriteBatch.DrawString(this.engine.Font, "Drop:Q", new Vector2(10, 550), Color.LightBlue);
            spriteBatch.DrawString(this.engine.Font, "Drop:W", new Vector2(70, 550), Color.LightBlue);
            spriteBatch.DrawString(this.engine.Font, "Drop:E", new Vector2(130, 550), Color.LightBlue);
            spriteBatch.DrawString(this.engine.Font, "Drop:R", new Vector2(190, 550), Color.LightBlue);
            spriteBatch.DrawString(this.engine.Font, "Drop:T", new Vector2(250, 550), Color.LightBlue);

            // Healthbar
            //this.barCurrentWidth = (BarMaxWidth / this.engine.Player.MaxHealth) * this.engine.Player.Health;
            //this.barColor = this.barCurrentWidth < HealthAlertLevel * this.engine.Player.MaxHealth ? Color.Red : Color.White;

            //spriteBatch.Draw(this.engine.HealthBar, new Rectangle(600, 550, (int)this.barCurrentWidth, this.engine.HealthBar.Height), this.barColor);

            //spriteBatch.DrawString(this.engine.Font, string.Format("{0}/{1}", this.engine.Player.Health, this.engine.Player.MaxHealth), new Vector2(640, 480), Color.White);

            // Inventory full message
            if (this.engine.Player.InventoryIsFull)
            {
                spriteBatch.DrawString(this.engine.Font, "The inventory is full. Use or drop something!", new Vector2(700, 560), Color.Red);
            }
        }
    }
}
