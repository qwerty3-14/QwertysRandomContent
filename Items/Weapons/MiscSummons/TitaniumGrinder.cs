using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.World.Generation;

namespace QwertysRandomContent.Items.Weapons.MiscSummons
{
    public class TitaniumGrinderStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Titanium Grinder staff");
            Tooltip.SetDefault("A very terryfiying and effective sentry" + "\nIgnores defense");

        }

        public override void SetDefaults()
        {

            item.damage = 6;
            item.mana = 20;
            item.width = 32;
            item.height = 32;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 0f;
            item.value = 161000;
            item.rare = 4;
            item.UseSound = SoundID.Item44;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("TitaniumGrinder");
            item.summon = true;
            item.sentry = true;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TitaniumBar, 13);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld;   //this make so the projectile will spawn at the mouse cursor position







            return true;
        }


        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim();
            }
            return base.UseItem(player);
        }
    }

    public class TitaniumGrinder : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Titanium Grinder");
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; 
            Main.projFrames[projectile.type] = 1; 
        }

        public override void SetDefaults()
        {

            projectile.sentry = true;
            projectile.width = 60;
            projectile.height = 60;  
            projectile.hostile = false;   
            projectile.friendly = false;  
            projectile.ignoreWater = true;   
            projectile.timeLeft = Projectile.SentryLifeTime;
            projectile.knockBack = 10f;
            projectile.penetrate = -1; 
            projectile.tileCollide = true; 
            projectile.sentry = true;
            projectile.minion = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
        }

        NPC target;
        NPC possibleTarget;
        bool foundTarget;
        float maxDistance = 500f;
        float distance;
        int timer;
        float pullAcceleration = .1f;
        float maxPullSpeed = 10f;
        bool runOnce = true;
        bool rotateSaws;
        bool drawChain;
        int grindTimer;
        public override void AI()
        {
           
            Player player = Main.player[projectile.owner];
            player.UpdateMaxTurrets();
            timer++;
            if (timer > 240)
            {
                for (int k = 0; k < 200; k++)
                {
                    possibleTarget = Main.npc[k];
                    distance = (possibleTarget.Center - projectile.Center).Length();
                    if (distance < maxDistance && possibleTarget.active && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && !possibleTarget.boss && !possibleTarget.immortal  && Collision.CanHit(projectile.Center, 0, 0, possibleTarget.Center, 0, 0) && (possibleTarget.lifeMax<2000 || (possibleTarget.lifeMax<4000 && Main.expertMode)))
                    {
                        target = possibleTarget;
                        foundTarget = true;
                        maxDistance = (target.Center - projectile.Center).Length();
                    }

                }
            }
            if(foundTarget)
            {
                target = Main.npc[target.whoAmI];
                target.AddBuff(mod.BuffType("TitanicGrasp"), 3);
                
                if((projectile.Center-target.Center).Length()<  11  && target.active)
                {

                    rotateSaws = true;
                    drawChain = false;
                    timer = 0;
                    target.velocity = Vector2.Zero;
                    target.Center = projectile.Center;
                    //target.noGravity = true;
                    grindTimer++;
                    if(grindTimer %3 ==0)
                    {
                        Projectile.NewProjectile(target.Center, Vector2.Zero, mod.ProjectileType("TitaniumGrind"), projectile.damage, 0, projectile.owner, target.whoAmI);
                    }

                    
                }
                else
                {
                    drawChain = true;
                    rotateSaws = false;
                    target.velocity = QwertyMethods.PolarVector(maxPullSpeed, (projectile.Center - target.Center).ToRotation());
                    foundTarget = false;
                }
               
            }
            else
            {
                rotateSaws = false;
                drawChain = false;
            }
            
            maxDistance = 500f;
            /*
            projectile.frameCounter++;
            if(projectile.frameCounter>10)
            {
                projectile.frame++;
                if(projectile.frame >=4)
                {
                    projectile.frame = 0;
                }
                projectile.frameCounter = 0;
            }
            */

        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            // Add this projectile to the list of projectiles that will be drawn BEFORE tiles and NPC are drawn. This makes the projectile appear to be BEHIND the tiles and NPC.
            drawCacheProjsBehindNPCsAndTiles.Add(index);
        }
        float sawRotation = 0;
        float sawRotation2 = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            /*
            spriteBatch.Draw(mod.GetTexture("Items/Weapons/MiscSummons/TitaniumGrinder"), new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y),
                       new Rectangle(0, 0, projectile.width, projectile.height), lightColor, projectile.rotation,
                       new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
                       */
           
            return true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if(drawChain)
            {
                Vector2 playerCenter = target.Center;
                Vector2 center = projectile.Center;
                Vector2 distToProj = playerCenter - projectile.Center;
                float projRotation = distToProj.ToRotation() - 1.57f;
                float distance = distToProj.Length();
                while (distance > 30f && !float.IsNaN(distance))
                {
                    distToProj.Normalize();                 //get unit vector
                    distToProj *= 8f;                      //speed = 12
                    center += distToProj;                   //update draw position
                    distToProj = playerCenter - center;    //update distance
                    distance = distToProj.Length();
                    Color drawColor = lightColor;

                    //Draw chain
                    spriteBatch.Draw(mod.GetTexture("Items/Weapons/MiscSummons/TitaniumChain"), new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, 14, 8), drawColor, projRotation,
                        new Vector2(14 * 0.5f, 8 * 0.5f), 1f, SpriteEffects.None, 0f);
                }
            }
            if (rotateSaws)
            {
                sawRotation += (float)Math.PI / 10;
                sawRotation2 -= (float)Math.PI / 10;
            }
            Texture2D texture = mod.GetTexture("Items/Weapons/MiscSummons/TitaniumSaw");
            spriteBatch.Draw(texture, new Vector2(projectile.TopLeft.X - Main.screenPosition.X, projectile.TopLeft.Y + 2f - Main.screenPosition.Y),
            new Rectangle(0, 0, 48, 48), lightColor, sawRotation,
            new Vector2(48 * 0.5f, 48 * 0.5f), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, new Vector2(projectile.TopRight.X - Main.screenPosition.X, projectile.TopRight.Y + 2f - Main.screenPosition.Y),
            new Rectangle(0, 0, 48, 48), lightColor, sawRotation2,
            new Vector2(48 * 0.5f, 48 * 0.5f), 1f, SpriteEffects.FlipHorizontally, 0f);
            spriteBatch.Draw(texture, new Vector2(projectile.BottomLeft.X - Main.screenPosition.X, projectile.BottomLeft.Y + 2f - Main.screenPosition.Y),
            new Rectangle(0, 0, 48, 48), lightColor, sawRotation,
            new Vector2(48 * 0.5f, 48 * 0.5f), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, new Vector2(projectile.BottomRight.X - Main.screenPosition.X, projectile.BottomRight.Y + 2f - Main.screenPosition.Y),
            new Rectangle(0, 0, 48, 48), lightColor, sawRotation2,
            new Vector2(48 * 0.5f, 48 * 0.5f), 1f, SpriteEffects.FlipHorizontally, 0f);
        }

    }
   
    public class TitaniumGrind : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Titanium Grind");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.minion = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 2;
            projectile.usesLocalNPCImmunity = true;
            projectile.tileCollide = false;

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
        public override void AI()
        {
            
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            int finalDefense = target.defense - player.armorPenetration;
            target.ichor = false;
            target.betsysCurse = false;
            if (finalDefense < 0)
            {
                finalDefense = 0;
            }
            damage += finalDefense / 2;
        }



    }



}