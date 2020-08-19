using Microsoft.Xna.Framework;
using QwertysRandomContent.Items.Fortress.CaeliteWeapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.HydraItems
{
    public class Hydrill : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Mines a 3x3 area");
        }

        public override void SetDefaults()
        {
            item.damage = 72;
            item.melee = true;
            item.width = 20;
            item.height = 12;
            item.useTime = 6;
            item.useAnimation = 25;
            item.channel = true;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.pick = 195;
            item.tileBoost++;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 6;
            item.value = 250000;
            item.rare = 5;
            item.UseSound = SoundID.Item23;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("HydrillP");
            item.shootSpeed = 40f;
            item.tileBoost = -2;
            item.GetGlobalItem<AoePick>().miningRadius = 1;
        }
    }

    public class HydrillP : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 22;
            projectile.aiStyle = 20;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.hide = true;
            projectile.ownerHitCheck = true; //so you can't hit enemies through walls
            projectile.melee = true;
        }

        public override void AI()
        {
            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("HydraBeamGlow"), projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, default(Color), 1.9f);
            Main.dust[dust].noGravity = true;
        }
    }
}