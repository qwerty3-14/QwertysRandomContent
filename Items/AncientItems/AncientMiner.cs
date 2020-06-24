using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using QwertysRandomContent.Items.Fortress.CaeliteWeapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.AncientItems
{
    public class AncientMiner : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Miner");
            Tooltip.SetDefault("Right click to mine a 3x3 area");
        }

        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicAncient ? base.Texture + "_Old" : base.Texture;

        public override void SetDefaults()
        {
            item.damage = 29;
            item.melee = true;

            item.useTime = 14;
            item.useAnimation = 14;
            item.useStyle = 1;
            item.knockBack = 3;
            item.value = 25000;
            item.rare = 3;
            item.UseSound = SoundID.Item1;
            //item.prefix = 0;
            item.width = 16;
            item.height = 16;
            //item.crit = 5;
            item.autoReuse = true;
            item.pick = 100;
            item.tileBoost = 1;
            if (!Main.dedServ)
            {
                item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.GetInstance<SpriteSettings>().ClassicAncient ? mod.GetTexture("Items/AncientItems/AncientMiner_Glow_Old") : mod.GetTexture("Items/AncientItems/AncientMiner_Glow");
            }
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.GetInstance<SpriteSettings>().ClassicAncient ? mod.GetTexture("Items/AncientItems/AncientMiner_Glow_Old") : mod.GetTexture("Items/AncientItems/AncientMiner_Glow");
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

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.useTime = 24;
                item.useAnimation = 24;
                item.GetGlobalItem<AoePick>().miningRadius = 1;
            }
            else
            {
                item.useTime = 14;
                item.useAnimation = 14;
                item.GetGlobalItem<AoePick>().miningRadius = 0;
            }
            return base.CanUseItem(player);
        }
    }
}