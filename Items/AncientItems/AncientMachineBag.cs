using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.AncientItems
{
    public class AncientMachineBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }
        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.width = 48;
            item.height = 32;
            item.rare = 9;
            item.expert = true;
            bossBagNPC = mod.NPCType("AncientMachine");
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = mod.GetTexture("Items/AncientItems/AncientMachineBag_Glow");
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            
            string[] loot = QwertysRandomContent.AMLoot.Draw(3);
            
            foreach(string item in loot)
            {
                player.QuickSpawnItem(mod.ItemType(item));
            }
            
            if (Main.rand.Next(100) < 18)
            {
                player.QuickSpawnItem(mod.ItemType("AncientMiner"));
            }
            player.QuickSpawnItem(73, 8);
            player.QuickSpawnItem(mod.ItemType("AncientGemstone"));
        }
    }
}
