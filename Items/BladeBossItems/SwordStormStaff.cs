using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertysRandomContent.Items.BladeBossItems      
{
    public class SwordStormStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swordpocalypse");
            Tooltip.SetDefault("Covers the screen in swords!");
            Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun

        }

        public override void SetDefaults()
        {

            item.damage = 16;
            item.mana = 4;
            item.width = 46;
            item.height = 46;
            item.useTime = 9;
            item.useAnimation = 9;
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
            float trueSpeed = new Vector2(speedX, speedY).Length();
            Vector2 Rposition = new Vector2(position.X, position.Y - 500);
            for (int i = 0; i < 15; i++)
            {


                int shift = Main.rand.Next(-600, 600);
                int Yshift = Main.rand.Next(50);
                float sX = 0;
                float sY = trueSpeed / 3;
                Projectile.NewProjectile(new Vector2(Rposition.X + shift, Rposition.Y - Yshift), new Vector2(sX, sY).RotatedByRandom(Math.PI / 16), type, damage, knockBack, player.whoAmI);
            }
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 65f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            for (int i = 0; i < 2; i++)
            {
                Projectile.NewProjectile(position, new Vector2(speedX, speedY).RotatedByRandom(Math.PI / 16), type, damage, knockBack, player.whoAmI);
            }

            return false;
        }
    }
    public class SwordDrop : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sworddrop");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            //aiType = ProjectileID.Bullet;
            projectile.width = 22;
            projectile.height = 22;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.magic = true;

            projectile.usesLocalNPCImmunity = true; 
            projectile.localNPCHitCooldown = 30;
            projectile.tileCollide = true;


        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown; //set local immunity
            target.immune[projectile.owner] = 0; //disable normal immune mechanic
        }







    }




}