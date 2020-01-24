using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Thrown
{
    public class AdamantiteThrowingHand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Adamantite Throwing Hand");
            Tooltip.SetDefault("Body slams enemies dealing massive damage! (doesn't work on bosses)");

        }
        public override void SetDefaults()
        {
            item.damage = 50;
            item.thrown = true;
            item.knockBack = 5;
            item.value = 80;
            item.rare = 3;
            item.width = 14;
            item.height = 26;
            item.useStyle = 1;
            item.shootSpeed = 12f;
            item.useTime = 22;
            item.useAnimation = 22;
            item.consumable = true;
            item.shoot = mod.ProjectileType("AdamantiteThrowingHandP");
            item.noUseGraphic = true;
            item.noMelee = true;
            item.maxStack = 999;
            item.autoReuse = true;


        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AdamantiteBar, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 333);
            recipe.AddRecipe();
        }
    }
    public class AdamantiteThrowingHandP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Adamantite Hand");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 2;
            aiType = ProjectileID.ThrowingKnife;
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.thrown = true;


            projectile.tileCollide = true;


        }
        public NPC grabbed = new NPC();
        public bool hasGrabbed;
        public bool runOnce = true;
        public bool falling;
        public bool hasHit;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (hasGrabbed)
            {
                if (runOnce)
                {
                    projectile.velocity = new Vector2(0, -8);
                    runOnce = false;
                }
                else if (projectile.velocity.Y > -2 && !hasHit)
                {
                    falling = true;
                    grabbed.rotation = (float)Math.PI;
                    projectile.velocity = new Vector2(0, 20);
                }
                grabbed.position = projectile.Center - new Vector2((grabbed.width / 2), (grabbed.height / 2));
                grabbed.AddBuff(mod.BuffType("Grabbed"), 2);
            }

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.boss && !hasGrabbed && !target.immortal && !target.HasBuff(mod.BuffType("Grabbed")))
            {
                projectile.aiStyle = 1;
                projectile.friendly = false;
                grabbed = target;
                hasGrabbed = true;

            }
            if (hasHit)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/PUNCH").WithVolume(.8f).WithPitchVariance(.5f), (int)target.position.X, (int)target.position.Y);
            }

        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Player player = Main.player[projectile.owner];
            if (hasGrabbed)
            {

                grabbed.rotation = 0;
                if (falling)
                {
                    projectile.damage *= 6;
                    hasHit = true;
                }
                projectile.penetrate = 1;
                projectile.friendly = true;
                if (projectile.timeLeft > 10)
                {
                    projectile.timeLeft = 3;
                }
                return false;


            }
            else
            {
                return true;
            }
        }
        /*
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            if (hasGrabbed)
            {
                
                grabbed.rotation = 0;
                projectile.friendly = true;
                projectile.penetrate = 1;
                /*
                grabbed.life -= (int)(300 * player.thrownDamage);
                if(grabbed.life <0)
                {
                    
                }
                
            }
        }
         */





    }

}

