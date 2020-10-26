using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscSpells       ///We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class HydraMissileStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Missile Rod");
            Tooltip.SetDefault("Fires a Hydra head that explodes and splits into more hydra heads which explodes and splits into more hydra heads!");
            Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
        }

        public override void SetDefaults()
        {
            item.damage = 40;
            item.mana = 10;
            item.width = 100;
            item.height = 100;
            item.useTime = 28;
            item.useAnimation = 28;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 1f;
            item.value = 250000;
            item.rare = 5;
            item.UseSound = SoundID.Item43;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("HydraMissileBig");
            item.magic = true;
            item.shootSpeed = 8;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-26, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 131f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            return true;
        }
    }

    public class HydraMissileBig : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 36;
            projectile.height = 36;
            projectile.friendly = true;
            projectile.aiStyle = 0;
            projectile.magic = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 300;
            //projectile.usesLocalNPCImmunity = true;
        }

        private NPC target;
        private float speed = 15;
        private bool runOnce = true;
        private float direction;

        public override void AI()
        {
            if (runOnce)
            {
                direction = projectile.velocity.ToRotation();
                projectile.rotation = direction + ((float)Math.PI / 2);
                runOnce = false;
            }
            Player player = Main.player[projectile.owner];
            if (QwertyMethods.ClosestNPC(ref target, 10000, projectile.Center))
            {
                direction = QwertyMethods.SlowRotation(direction, (target.Center - projectile.Center).ToRotation(), 3f);
            }
            projectile.velocity = new Vector2((float)Math.Cos(direction) * speed, (float)Math.Sin(direction) * speed);
            Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("HydraBeamGlow"));
            projectile.rotation = direction + ((float)Math.PI / 2);
        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            if (projectile.velocity.X != velocityChange.X)
            {
                projectile.velocity.X = -velocityChange.X;
            }
            if (projectile.velocity.Y != velocityChange.Y)
            {
                projectile.velocity.Y = -velocityChange.Y;
            }
            direction = projectile.velocity.ToRotation();
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //projectile.localNPCImmunity[target.whoAmI] = -1;
            //target.immune[projectile.owner] = 0;
        }

        public override void Kill(int timeLeft)
        {
            projectile.alpha = 255;
            projectile.width = 100;
            projectile.height = 100;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            for (int d = 0; d < 400; d++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("HydraBeamGlow"));
            }
            //Main.PlaySound(SoundID.Item62, projectile.position);
            for (int g = 0; g < 2; g++)
            {
                float launchDirection = Main.rand.NextFloat() * (float)Math.PI * 2;
                Projectile.NewProjectile(projectile.Center, new Vector2((float)Math.Cos(launchDirection) * speed, (float)Math.Sin(launchDirection) * speed), mod.ProjectileType("HydraMissileMedium"), (int)(projectile.damage * .8f), projectile.knockBack * .8f, projectile.owner);
            }
        }
    }

    public class HydraMissileMedium : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.aiStyle = 0;
            projectile.magic = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 300;
            //projectile.usesLocalNPCImmunity = true;
        }

        private NPC target;
        private NPC possibleTarget;
        private bool foundTarget;
        private float maxDistance = 10000f;
        private float distance;
        private int timer;
        private float speed = 15;
        private bool runOnce = true;
        private float direction;

        public override void AI()
        {
            if (runOnce)
            {
                direction = projectile.velocity.ToRotation();
                projectile.rotation = direction + ((float)Math.PI / 2);
                runOnce = false;
            }
            Player player = Main.player[projectile.owner];
            if (QwertyMethods.ClosestNPC(ref target, 10000, projectile.Center))
            {
                direction = QwertyMethods.SlowRotation(direction, (target.Center - projectile.Center).ToRotation(), 3f);
            }
            projectile.velocity = new Vector2((float)Math.Cos(direction) * speed, (float)Math.Sin(direction) * speed);
            Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("HydraBeamGlow"));
            projectile.rotation = direction + ((float)Math.PI / 2);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //projectile.localNPCImmunity[target.whoAmI] = -1;
            //target.immune[projectile.owner] = 0;
        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            if (projectile.velocity.X != velocityChange.X)
            {
                projectile.velocity.X = -velocityChange.X;
            }
            if (projectile.velocity.Y != velocityChange.Y)
            {
                projectile.velocity.Y = -velocityChange.Y;
            }
            direction = projectile.velocity.ToRotation();
            return false;
        }

        public override void Kill(int timeLeft)
        {
            projectile.alpha = 255;
            projectile.width = 50;
            projectile.height = 50;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            for (int d = 0; d < 200; d++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("HydraBeamGlow"));
            }
            //Main.PlaySound(SoundID.Item62, projectile.position);
            for (int g = 0; g < 2; g++)
            {
                float launchDirection = Main.rand.NextFloat() * (float)Math.PI * 2;
                Projectile.NewProjectile(projectile.Center, new Vector2((float)Math.Cos(launchDirection) * speed, (float)Math.Sin(launchDirection) * speed), mod.ProjectileType("HydraMissileSmall"), (int)(projectile.damage * .8f), projectile.knockBack * .8f, projectile.owner);
            }
        }
    }

    public class HydraMissileSmall : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.aiStyle = 0;
            projectile.magic = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 300;
            //projectile.usesLocalNPCImmunity = true;
        }

        private NPC target;
        private NPC possibleTarget;
        private bool foundTarget;
        private float maxDistance = 10000f;
        private float distance;
        private int timer;
        private float speed = 15;
        private bool runOnce = true;
        private float direction;

        public override void AI()
        {
            if (runOnce)
            {
                direction = projectile.velocity.ToRotation();
                projectile.rotation = direction + ((float)Math.PI / 2);
                runOnce = false;
            }
            Player player = Main.player[projectile.owner];
            if (QwertyMethods.ClosestNPC(ref target, 10000, projectile.Center))
            {
                direction = QwertyMethods.SlowRotation(direction, (target.Center - projectile.Center).ToRotation(), 3f);
            }
            projectile.velocity = new Vector2((float)Math.Cos(direction) * speed, (float)Math.Sin(direction) * speed);
            Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("HydraBeamGlow"));
            projectile.rotation = direction + ((float)Math.PI / 2);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            if (projectile.velocity.X != velocityChange.X)
            {
                projectile.velocity.X = -velocityChange.X;
            }
            if (projectile.velocity.Y != velocityChange.Y)
            {
                projectile.velocity.Y = -velocityChange.Y;
            }
            direction = projectile.velocity.ToRotation();
            return false;
        }

        public override void Kill(int timeLeft)
        {
            projectile.alpha = 255;
            projectile.width = 20;
            projectile.height = 20;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            for (int d = 0; d < 100; d++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("HydraBeamGlow"));
            }
        }
    }
}