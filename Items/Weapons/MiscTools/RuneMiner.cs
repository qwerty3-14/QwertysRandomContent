using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Items.Fortress.CaeliteWeapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscTools
{
    public class RuneMiner : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rune Miner");
            Tooltip.SetDefault("Mines a 5x5 area");
        }


        public override void SetDefaults()
        {
            item.damage = 80;
            item.melee = true;

            item.useTime = 12;
            item.useAnimation = 30;
            item.useStyle = 1;
            item.knockBack = 3;
            item.value = 500000;
            item.rare = 9;
            item.UseSound = SoundID.Item1;
            //item.prefix = 0;
            item.width = 16;
            item.height = 16;
            //item.crit = 5;
            item.autoReuse = true;
            item.pick = 200;
            item.tileBoost = 4;
            item.GetGlobalItem<AoePick>().miningRadius = 2;
            if (!Main.dedServ)
            {
                item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/Weapons/MiscTools/RuneMiner_Glow");
            }
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = mod.GetTexture("Items/Weapons/MiscTools/RuneMiner_Glow");
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
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(mod.ItemType("AncientMiner"));
            recipe.AddIngredient(mod.ItemType("CraftingRune"), 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
