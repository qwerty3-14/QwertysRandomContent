using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
    public class EnchantedWhetstone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enchanted Whetstone");
            Tooltip.SetDefault("Melee attacks inflict a bit of extra magic damage");
        }

        public override void SetDefaults()
        {
            item.value = 10000;
            item.rare = 1;
            item.width = 24;
            item.height = 24;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<WhetStoneEffect>().effect += .2f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FallenStar, 3);
            recipe.AddIngredient(ItemID.StoneBlock, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class MagicBonusOnProj : GlobalProjectile
    {
        public int magicBoost = 0;
        public bool whetStoned = false;
        public override bool InstancePerEntity => true;

        public override void AI(Projectile projectile)
        {
            if (Main.player[projectile.owner].GetModPlayer<WhetStoneEffect>().effect > 0f && (!whetStoned && projectile.melee))
            {
                magicBoost += (int)(Main.player[projectile.owner].GetModPlayer<WhetStoneEffect>().effect * Main.player[projectile.owner].HeldItem.damage);
                whetStoned = true;
            }
        }
    }

    public class WhetStoneEffect : ModPlayer
    {
        public float effect = 0f;

        public override void ResetEffects()
        {
            effect = 0f;
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (effect != 0f && proj.melee)
            {
                QwertyMethods.PokeNPC(player, target, proj.GetGlobalProjectile<MagicBonusOnProj>().magicBoost * player.magicDamage, magic: true);
            }
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (effect != 0f && item.melee)
            {
                QwertyMethods.PokeNPC(player, target, damage * effect, magic: true);
            }
        }
    }

    public class WhetstoneTooltips : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            if (player.GetModPlayer<WhetStoneEffect>().effect > 0f && item.melee)
            {
                int TLIndex = tooltips.FindIndex(TooltipLine => TooltipLine.Name.Equals("CritChance"));
                TooltipLine line = new TooltipLine(mod, "MagicBoost", (int)(item.damage * player.GetModPlayer<WhetStoneEffect>().effect * player.magicDamage) + " magic damage");
                {
                    line.overrideColor = Color.SkyBlue;
                }
                if (TLIndex != -1)
                {
                    tooltips.Insert(TLIndex + 1, line);
                }

                line = new TooltipLine(mod, "MagicBoostCrit", player.magicCrit + "% critical strike chance");
                {
                    line.overrideColor = Color.SkyBlue;
                }
                if (TLIndex != -1)
                {
                    tooltips.Insert(TLIndex + 2, line);
                }
            }
        }
    }
}