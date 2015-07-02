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
        //private const float BarMaxWidth = 175F;
        //private const float HealthAlertLevel = 0.25F;

        private readonly Engine engine;
        private readonly Vector2[] inventoryPositions =
        {
            new Vector2(52, 617),
            new Vector2(108, 617),
            new Vector2(166, 617),
            new Vector2(224, 617),
            new Vector2(278, 617)
        };

        private readonly Dictionary<EquipmentSlot, Vector2> equipmentSlotPositions = new Dictionary<EquipmentSlot, Vector2>()
        {
            { EquipmentSlot.Hands, new Vector2(52, 561) },
            { EquipmentSlot.Body, new Vector2(108, 561) },
            { EquipmentSlot.Feet, new Vector2(166, 561) },
            { EquipmentSlot.Head, new Vector2(224, 561) },
            { EquipmentSlot.Arms, new Vector2(278, 561) }
        };

        //private float barCurrentWidth;
        //private Color barColor = Color.White;

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
            spriteBatch.DrawString(this.engine.Font, this.engine.Player.Health.ToString(), new Vector2(500, 560), Color.Black);
            spriteBatch.DrawString(this.engine.Font, this.engine.Player.Level.ToString(), new Vector2(500, 575), Color.Black);
            spriteBatch.DrawString(this.engine.Font, this.engine.Player.AttackDamage.ToString(), new Vector2(500, 590), Color.Black);
            spriteBatch.DrawString(this.engine.Font, this.engine.Player.Defense.ToString(), new Vector2(500, 605), Color.Black);
            spriteBatch.DrawString(this.engine.Font, this.engine.Player.Speed.ToString(), new Vector2(500, 620), Color.Black);
            spriteBatch.DrawString(this.engine.Font, this.engine.Player.Experience.ToString(), new Vector2(500, 635), Color.Black);

            spriteBatch.DrawString(this.engine.Font, "Use:1", new Vector2(40, 600), Color.LightBlue);
            spriteBatch.DrawString(this.engine.Font, "Use:2", new Vector2(100, 600), Color.LightBlue);
            spriteBatch.DrawString(this.engine.Font, "Use:3", new Vector2(160, 600), Color.LightBlue);
            spriteBatch.DrawString(this.engine.Font, "Use:4", new Vector2(220, 600), Color.LightBlue);
            spriteBatch.DrawString(this.engine.Font, "Use:5", new Vector2(280, 600), Color.LightBlue);

            spriteBatch.DrawString(this.engine.Font, "Drop:Q", new Vector2(40, 545), Color.LightBlue);
            spriteBatch.DrawString(this.engine.Font, "Drop:W", new Vector2(100, 545), Color.LightBlue);
            spriteBatch.DrawString(this.engine.Font, "Drop:E", new Vector2(160, 545), Color.LightBlue);
            spriteBatch.DrawString(this.engine.Font, "Drop:R", new Vector2(220, 545), Color.LightBlue);
            spriteBatch.DrawString(this.engine.Font, "Drop:T", new Vector2(280, 545), Color.LightBlue);

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
