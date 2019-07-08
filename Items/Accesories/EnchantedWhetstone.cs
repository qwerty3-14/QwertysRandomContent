using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public override void AI(Projectile projectile)
        {
            if(Main.player[projectile.owner].GetModPlayer<WhetStoneEffect>().effect > 0f && (!whetStoned && projectile.melee ))
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
        void Hit(NPC target,  int damage)
        {
            Projectile p = Main.projectile[Projectile.NewProjectile(target.Center, Vector2.Zero, mod.ProjectileType("WhetstoneSpell"), (int)(damage * player.magicDamage), 0f, player.whoAmI)];
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n] != target)
                {
                    p.localNPCImmunity[n] = -1;
                }
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (effect != 0f && proj.melee)
            {
                Hit(target,  proj.GetGlobalProjectile<MagicBonusOnProj>().magicBoost);
            }
        }
        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if(effect != 0f && item.melee)
            {
                Hit(target,  (int)(damage * effect));
            }
        }
        

    }
    public class WhetstoneSpell : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 2;
            projectile.height = 2;
            projectile.magic = true;
            projectile.friendly = true;
            projectile.timeLeft = 2;
            projectile.usesLocalNPCImmunity = true;
            for(int n = 0; n< Main.npc.Length; n++)
           
            projectile.tileCollide = false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }

    }
    public class WhetstoneTooltips : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Player player = Main.player[item.owner];
            if (player.GetModPlayer<WhetStoneEffect>().effect > 0f && item.melee)
            {
                int TLIndex = tooltips.FindIndex(TooltipLine => TooltipLine.Name.Equals("CritChance"));
                TooltipLine line = new TooltipLine(mod, "MagicBoost", (int)(item.damage * player.GetModPlayer<WhetStoneEffect>().effect * player.magicDamage) + " magic damage");
                {
                    line.overrideColor = Color.Blue;
                }
                if (TLIndex != -1)
                {
                    tooltips.Insert(TLIndex + 1, line);
                }
                
                line = new TooltipLine(mod, "MagicBoostCrit", player.magicCrit + "% critical strike chance");
                {
                    line.overrideColor = Color.Blue;
                }
                if (TLIndex != -1)
                {
                    tooltips.Insert(TLIndex + 2, line);
                }
            }
        }
    }
}
