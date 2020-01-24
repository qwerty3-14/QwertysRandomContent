using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertysRandomContent.Items.Fortress
{
    public class DivineBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Divine Blade");
            Tooltip.SetDefault("Deflects projectiles" + "\nCan only deflect projectiles weaker than the sword");

        }
        public override void SetDefaults()
        {
            item.damage = 33;
            item.melee = true;

            item.useTime = 41;
            item.useAnimation = 41;
            item.useStyle = 1;
            item.knockBack = 3;
            item.value = 25000;
            item.rare = 3;
            item.UseSound = SoundID.Item1;

            item.width = 58;
            item.height = 58;
            //item.crit = 5;
            item.autoReuse = true;
            //item.scale = 5;



        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CaeliteBar"), 12);
            recipe.AddIngredient(mod.ItemType("CaeliteCore"), 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public bool HittingBlade(Projectile target)
        {
            Player player = Main.player[item.owner];
            float swordLength = item.Size.Length() * item.scale;
            float r = player.direction == 1 ? player.itemRotation - (float)Math.PI / 4 : player.itemRotation + 5 * (float)Math.PI / 4;
            if (player.gravDir == -1)
            {
                r += MathHelper.PiOver2 * player.direction;
            }
            return Collision.CheckAABBvLineCollision(target.position, target.Size, player.MountedCenter, player.MountedCenter + new Vector2((float)Math.Cos(r), (float)Math.Sin(r)) * swordLength);
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Lighting.AddLight(hitbox.Center.ToVector2(), 1f, 1f, 1f);
            int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, mod.DustType("CaeliteDust"));
            Main.dust[dust].frame.Y = 0;
            Main.dust[dust].scale = .5f;
            for (int p = 0; p < 1000; p++)
            {
                clearCheck = Main.projectile[p];
                if ((clearCheck.hostile || (clearCheck.friendly && !clearCheck.minion && (Main.player[clearCheck.owner].team != Main.player[item.owner].team) || (Main.player[item.owner].team == 0 && clearCheck.owner != item.owner))) && Collision.CheckAABBvAABBCollision(hitbox.TopLeft(), hitbox.Size(), clearCheck.position, clearCheck.Size) && HittingBlade(clearCheck) && clearCheck.damage * 2 * (Main.expertMode ? 2 : 1) < item.damage * player.meleeDamage && clearCheck.velocity.Length() > .1f)
                {
                    clearCheck.Kill();
                    if (clearCheck.velocity.Length() > 0f)
                    {
                        Projectile.NewProjectile(clearCheck.Center, -clearCheck.velocity, mod.ProjectileType("FriendlyDeflected"), clearCheck.damage * 2, clearCheck.knockBack, item.owner);
                    }
                }
            }

            /*
            float swordLength = item.Size.Length() * item.scale;
            float r = player.direction == 1 ? player.itemRotation - (float)Math.PI / 4 : player.itemRotation + 5*(float)Math.PI / 4;
            if(player.gravDir == -1)
            {
                r += MathHelper.PiOver2 * player.direction;
            }
            Dust d = Dust.NewDustPerfect(player.MountedCenter + new Vector2((float)Math.Cos(r), (float)Math.Sin(r)) * swordLength, DustID.Fire);
            d.noGravity = true;
            d.velocity = Vector2.Zero;
            */
        }
        Projectile clearCheck;
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            target.AddBuff(mod.BuffType("PowerDown"), 420);


        }
    }

    public class FriendlyDeflected : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Deflected");
            Main.projFrames[projectile.type] = 2;

        }
        public override void SetDefaults()
        {
            projectile.aiStyle = -1;
            //aiType = ProjectileID.Bullet;
            projectile.width = 22;
            projectile.height = 22;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.light = .6f;
            projectile.tileCollide = false;


        }
        public override void AI()
        {
            if (Main.rand.Next(2) == 0)
            {
                Dust dust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("CaeliteDust"))];
                dust2.scale = .5f;
            }
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.frameCounter++;
            if (projectile.frameCounter % 10 == 0)
            {
                projectile.frame++;
                if (projectile.frame > 1)
                {
                    projectile.frame = 0;
                }

            }
        }




    }


}

