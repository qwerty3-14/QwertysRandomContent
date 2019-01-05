using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
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

            item.damage = 58;
            item.mana = 4;
            item.width = 100;
            item.height = 100;
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 1f;
            item.value = 500000;
            item.rare = 9;
            item.UseSound = SoundID.Item43;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("HydraMissileBig");
            item.magic = true;
            item.shootSpeed = 8;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);


            recipe.AddIngredient(mod.ItemType("CraftingRune"), 20);
            recipe.AddIngredient(mod.ItemType("HydraScale"), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
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
            //projectile.usesLocalNPCImmunity = true;
        }
        NPC target;
        NPC possibleTarget;
        bool foundTarget;
        float maxDistance = 10000f;
        float distance;
        int timer;
        float speed = 15;
        bool runOnce = true;
        float direction;
        public override void AI()
        {
            if(runOnce)
            {
                direction = projectile.velocity.ToRotation();
                projectile.rotation = direction + ((float)Math.PI/2);
                runOnce = false;
            }
            Player player = Main.player[projectile.owner];
            for (int k = 0; k < 200; k++)
            {
                possibleTarget = Main.npc[k];
                distance = (possibleTarget.Center - projectile.Center).Length();
                if (distance < maxDistance && possibleTarget.active && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && Collision.CanHit(projectile.Center, 0, 0, possibleTarget.Center, 0, 0))
                {
                    target = Main.npc[k];
                    foundTarget = true;


                    maxDistance = (target.Center - projectile.Center).Length();
                }

            }
            if(foundTarget)
            {
                direction = QwertyMethods.SlowRotation(direction, (target.Center - projectile.Center).ToRotation(), 3f);

            }
            projectile.velocity = new Vector2((float)Math.Cos(direction) *speed, (float)Math.Sin(direction) *speed);
            foundTarget = false;
            maxDistance = 10000f;
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
            for(int g=0; g<3; g++)
            {
                float launchDirection = Main.rand.NextFloat() * (float)Math.PI * 2; 
                Projectile.NewProjectile(projectile.Center, new Vector2((float)Math.Cos(launchDirection)*speed, (float)Math.Sin(launchDirection) * speed), mod.ProjectileType("HydraMissileMedium"), (int)(projectile.damage * .8f), projectile.knockBack * .8f, projectile.owner);
            }
        }
    }
    public class HydraMissileMedium : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = false;
            projectile.aiStyle = 0;
            projectile.magic = true;
            projectile.penetrate = 1;
            //projectile.usesLocalNPCImmunity = true;
        }
        NPC target;
        NPC possibleTarget;
        bool foundTarget;
        float maxDistance = 10000f;
        float distance;
        int timer;
        float speed = 15;
        bool runOnce = true;
        float direction;
        public override void AI()
        {
            if (runOnce)
            {
                direction = projectile.velocity.ToRotation();
                projectile.rotation = direction + ((float)Math.PI / 2);
                runOnce = false;
            }
            timer++;
            Player player = Main.player[projectile.owner];
            if (timer > 20)
            {
                projectile.friendly = true;
                for (int k = 0; k < 200; k++)
                {
                    possibleTarget = Main.npc[k];
                    distance = (possibleTarget.Center - projectile.Center).Length();
                    if (distance < maxDistance && possibleTarget.active && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && Collision.CanHit(projectile.Center, 0, 0, possibleTarget.Center, 0, 0))
                    {
                        target = Main.npc[k];
                        foundTarget = true;


                        maxDistance = (target.Center - projectile.Center).Length();
                    }

                }
                if (foundTarget)
                {
                    direction = QwertyMethods.SlowRotation(direction, (target.Center - projectile.Center).ToRotation(), 3f);

                }
                projectile.velocity = new Vector2((float)Math.Cos(direction) * speed, (float)Math.Sin(direction) * speed);
                foundTarget = false;
                maxDistance = 10000f;
                
            }
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
            for (int g = 0; g < 3; g++)
            {
                float launchDirection = Main.rand.NextFloat() * (float)Math.PI * 2;
                Projectile.NewProjectile(projectile.Center, new Vector2((float)Math.Cos(launchDirection)* speed, (float)Math.Sin(launchDirection) * speed) , mod.ProjectileType("HydraMissileSmall"), (int)(projectile.damage * .8f), projectile.knockBack * .8f, projectile.owner);
            }
        }
    }
    public class HydraMissileSmall : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = false;
            projectile.aiStyle = 0;
            projectile.magic = true;
            projectile.penetrate = 1;
            //projectile.usesLocalNPCImmunity = true;
        }
        NPC target;
        NPC possibleTarget;
        bool foundTarget;
        float maxDistance = 10000f;
        float distance;
        int timer;
        float speed = 15;
        bool runOnce = true;
        float direction;
        public override void AI()
        {
            if (runOnce)
            {
                direction = projectile.velocity.ToRotation();
                projectile.rotation = direction + ((float)Math.PI / 2);
                runOnce = false;
            }
            timer++;
            Player player = Main.player[projectile.owner];
            if (timer > 20)
            {
                projectile.friendly = true;
                for (int k = 0; k < 200; k++)
                {
                    possibleTarget = Main.npc[k];
                    distance = (possibleTarget.Center - projectile.Center).Length();
                    if (distance < maxDistance && possibleTarget.active && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && Collision.CanHit(projectile.Center, 0, 0, possibleTarget.Center, 0, 0))
                    {
                        target = Main.npc[k];
                        foundTarget = true;


                        maxDistance = (target.Center - projectile.Center).Length();
                    }

                }
                if (foundTarget)
                {
                    direction = QwertyMethods.SlowRotation(direction, (target.Center - projectile.Center).ToRotation(), 3f);

                }
                projectile.velocity = new Vector2((float)Math.Cos(direction) * speed, (float)Math.Sin(direction) * speed);
                foundTarget = false;
                maxDistance = 10000f;

            }
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