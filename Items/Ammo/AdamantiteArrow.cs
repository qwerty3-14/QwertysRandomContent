using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Ammo
{
    public class AdamantiteArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Adamantite Arrow");
            Tooltip.SetDefault("Gives your enemies a nasty punch");
        }

        public override void SetDefaults()
        {
            item.damage = 18;
            item.ranged = true;
            item.knockBack = 30;
            item.value = 5;
            item.rare = 3;
            item.width = 14;
            item.height = 32;

            item.shootSpeed = 40f;

            item.consumable = true;
            item.shoot = mod.ProjectileType("AdamantiteArrowP");
            item.ammo = 40;
            item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AdamantiteBar, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
    }

    public class AdamantiteArrowP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Adamantite Arrow");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.ranged = true;
            projectile.arrow = true;
            projectile.tileCollide = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            knockback = 0;
            if (crit)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/PUNCH").WithVolume(.8f).WithPitchVariance(.5f));
                if (!target.boss && !target.immortal)
                {
                    target.velocity.X = projectile.velocity.X * 1f;
                    target.velocity.Y = projectile.velocity.Y * .5f;
                }
            }
            else
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/PUNCH").WithVolume(.4f).WithPitchVariance(.5f));
                if (!target.boss && !target.immortal)
                {
                    target.velocity.X = projectile.velocity.X * .5f;
                    target.velocity.Y = projectile.velocity.Y * .25f;
                }
            }
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            if (crit)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/PUNCH").WithVolume(.8f).WithPitchVariance(.5f));

                target.velocity.X = projectile.velocity.X * 1f;
                target.velocity.Y = projectile.velocity.Y * .5f;
                if (Main.netMode == 1)
                {
                    QwertysRandomContent.UpdatePlayerVelocity(target.whoAmI, target.velocity);
                }
            }
            else
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/PUNCH").WithVolume(.4f).WithPitchVariance(.5f));

                target.velocity.X = projectile.velocity.X * .5f;
                target.velocity.Y = projectile.velocity.Y * .25f;
                if (Main.netMode == 1)
                {
                    QwertysRandomContent.UpdatePlayerVelocity(target.whoAmI, target.velocity);
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
        }
    }
}