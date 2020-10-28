using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress.CaeliteWeapons
{
    public class SacredDaze : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sacred Daze");
            Tooltip.SetDefault("Stuns those who are not worthy!");
        }
        public override void SetDefaults()
        {
            item.damage = 7;
            item.crit = 30;
            item.magic = true;
            item.knockBack = 1;
            item.value = 50000;
            item.rare = 3;
            item.width = 26;
            item.height = 18;
            item.useStyle = 5;
            item.shootSpeed = 10f;
            item.useTime = 9;
            item.useAnimation = 9;
            item.mana = 4;
            item.shoot = mod.ProjectileType("SacredDazeP");
            item.noUseGraphic = false;
            item.noMelee = true;
            item.UseSound = SoundID.Item91;
            item.autoReuse = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CaeliteBar"), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
    public class SacredDazeP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sacred Daze");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.magic = true;
            projectile.timeLeft = 180;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 30;
            projectile.tileCollide = true;
            projectile.light = 1f;
            projectile.extraUpdates = 2;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 2; i++)
            {
                Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("CaeliteDust"))];
                dust.velocity *= 3f;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if(!target.boss)
            {
                target.AddBuff(mod.BuffType("Stunned"), 12);
            }
            if (Main.rand.Next(10) == 0)
            {
                target.AddBuff(mod.BuffType("PowerDown"), 120);
            }
        }
    }
}
