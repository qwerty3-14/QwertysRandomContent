using Microsoft.Xna.Framework;
using QwertysRandomContent.Config;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.BladeBossItems
{
    public class SwordStormStaff : ModItem
    {
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicImperious ? base.Texture + "_Old" : base.Texture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swordpocalypse");
            Tooltip.SetDefault("Unleashes a barrage of swords!");
            Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
        }

        public override void SetDefaults()
        {
            item.damage = 30;
            item.mana = 10;
            item.width = 46;
            item.height = 46;
            item.useTime = 4;
            item.useAnimation = 20;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 1f;
            item.value = 500000;
            item.rare = 7;
            item.UseSound = SoundID.Item43;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("SwordDrop");
            item.magic = true;
            item.shootSpeed = 12;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-26, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for(int i = 0; i < 1; i++)
            {
                float trueSpeed = new Vector2(speedX, speedY).Length();
                float rot = new Vector2(speedX, speedY).ToRotation();
                Vector2 Rposition = position + QwertyMethods.PolarVector(-1200, rot + Main.rand.NextFloat(-(float)Math.PI / 32, (float)Math.PI / 32));
                Vector2 goHere = Main.MouseWorld + QwertyMethods.PolarVector(Main.rand.NextFloat(-40, 40), rot + (float)Math.PI / 2);
                Vector2 diff = goHere - Rposition;
                float dist = diff.Length();
               
                Projectile.NewProjectile(Rposition, diff.SafeNormalize(-Vector2.UnitY) * trueSpeed, type, damage, knockBack, player.whoAmI, dist);
            }
            
            return false;
        }
    }

    public class SwordDrop : ModProjectile
    {
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicImperious ? base.Texture + "_Old" : base.Texture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sworddrop");
        }

        public override void SetDefaults()
        {
            //projectile.aiStyle = 1;
            //aiType = ProjectileID.Bullet;
            projectile.width = 22;
            projectile.height = 22;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.magic = true;

            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 30;
            projectile.tileCollide = false;
            projectile.extraUpdates = 2;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown; //set local immunity
            target.immune[projectile.owner] = 0; //disable normal immune mechanic
        }
        public override void AI()
        {
            projectile.ai[0] -= projectile.velocity.Length();
            projectile.tileCollide = projectile.ai[0] <= 0;
            projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2;
        }
    }
}