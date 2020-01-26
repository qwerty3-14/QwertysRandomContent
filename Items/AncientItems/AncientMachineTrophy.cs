using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.AncientItems
{
    public class AncientMachineTrophy : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Machine Trophy");
            if (ModContent.GetInstance<SpriteSettings>().ClassicAncient)
            {
                Main.itemTexture[item.type] = mod.GetTexture("Items/AncientItems/Old/AncientMachineTrophy_Old");
            }
        }
        public override void SetDefaults()
        {

            item.width = 32;
            item.height = 32;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.value = 50000;
            item.createTile = mod.TileType("AncientMachineTrophy");
            item.placeStyle = 0;
            if (!Main.dedServ)
            {
                item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.GetInstance<SpriteSettings>().ClassicAncient ? mod.GetTexture("Items/AncientItems/Old/AncientMachineTrophy_Old_Glow") : mod.GetTexture("Items/AncientItems/AncientMachineTrophy_Glow");
            }
        }

        
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.GetInstance<SpriteSettings>().ClassicAncient ? mod.GetTexture("Items/AncientItems/Old/AncientMachineTrophy_Old_Glow") : mod.GetTexture("Items/AncientItems/AncientMachineTrophy_Glow");
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
    }
}
