using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Items.Armor.Bionic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.ShapeShifter
{
    class SpazShift : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shape Shift: The Spazer's Eye");
            Tooltip.SetDefault("Breifly turns you into Spazmatazim breathing eye fire and charging");
        }

        

        public override void SetDefaults()
        {
            item.damage = 400;
            item.crit = 0;
            item.knockBack = 6f;
            item.GetGlobalItem<ShapeShifterItem>().morph = true;
            item.GetGlobalItem<ShapeShifterItem>().morphDef = -1;
            item.GetGlobalItem<ShapeShifterItem>().morphType = ShapeShifterItem.QuickShiftType;
            item.GetGlobalItem<ShapeShifterItem>().morphCooldown = 30;
            item.noMelee = true;

            item.useTime = 60;
            item.useAnimation = 60;
            item.useStyle = 5;

            item.value = Item.sellPrice(gold: 7);
            item.rare = 5;

            item.noUseGraphic = true;
            item.width = 18;
            item.height = 32;

            //item.autoReuse = true;
            item.shoot = mod.ProjectileType("SpazMorph");
            item.shootSpeed = 13f;
            item.channel = true;
        }

        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < 1000; ++i)
            {
                if ((Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot) || player.HasBuff(mod.BuffType("MorphCooldown")))
                {
                    return false;
                }
            }
            //Main.PlaySound(SoundID.Roar, player.position, 0);

            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofSight, 20);
            recipe.AddIngredient(ItemID.HallowedBar, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
    public class SpazMorph : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spazmatasim?");
            Main.projFrames[projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.width = 110;
            projectile.height = 110;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
            //projectile.tileCollide = false;
            projectile.timeLeft = 300;
            projectile.usesLocalNPCImmunity = true;
            projectile.extraUpdates = 1;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return projectile.timeLeft < 60;
        }
        int timer;
        Projectile reti = null;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            
            
            if (projectile.timeLeft < 60)
            {
                
                projectile.velocity = QwertyMethods.PolarVector( 15f, projectile.rotation - (float)Math.PI / 2);
            }
            else
            {
                if (projectile.timeLeft == 60)
                {
                    Main.PlaySound(SoundID.Roar, player.Center, 0);
                }
                projectile.velocity = Vector2.Zero;
                float r = (QwertysRandomContent.GetLocalCursor(projectile.owner) - projectile.Center).ToRotation();
                projectile.rotation =  r + (float)Math.PI / 2;
                timer++;
                if(timer % 8 ==0)
                {
                    Projectile.NewProjectile(projectile.Center, QwertyMethods.PolarVector(6f, r), mod.ProjectileType("EyefireFriendly"), projectile.damage / 4, projectile.knockBack / 2, projectile.owner);
                }
                if(timer % 22 == 0)
                {
                    Main.PlaySound(SoundID.Item34, projectile.Center);
                }
                if (projectile.timeLeft == 299 && player.GetModPlayer<BionicEffects>().eyeEquiped)
                {
                   reti = Main.projectile[ Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("RetiMorph"), projectile.damage, 0f, projectile.owner, r)];
                }
            }
            

            projectile.frameCounter++;
            if (projectile.frameCounter % 7 == 0)
            {
                projectile.frame++;
                if (projectile.frame >= 3)
                {
                    projectile.frame = 0;
                }
            }
            
            player.Center = projectile.Center;
            player.immune = true;
            player.immuneTime = 2;
            player.statDefense = 0;
            player.GetModPlayer<ShapeShifterPlayer>().noDraw = true;
            player.itemTime = 2;
            player.itemAnimation = 2;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Main.player[projectile.owner].Center.X < target.Center.X)
            {
                hitDirection = 1;
            }
            else
            {
                hitDirection = -1;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D link = Main.chain12Texture;
            if (reti != null && reti.active)
            {
                float r = (reti.Center - projectile.Center).ToRotation();

                for (int d = 0; d < (reti.Center - projectile.Center).Length(); d += link.Height)
                {
                    spriteBatch.Draw(link, projectile.Center + QwertyMethods.PolarVector(d, r) - Main.screenPosition, null, lightColor, r + (float)Math.PI / 2, link.Size() * (Vector2.UnitX * .5f), 1f, 0, 0);
                }
            }

            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle(0, Main.projectileTexture[projectile.type].Height / 3 * projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 3), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            
            
            
            
            return true;
        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            for (int k = 0; k < 200; k++)
            {
                projectile.localNPCImmunity[k] = 0;
            }
            if (projectile.velocity.X != velocityChange.X)
            {
                projectile.velocity.X = -velocityChange.X;
            }
            if (projectile.velocity.Y != velocityChange.Y)
            {
                projectile.velocity.Y = -velocityChange.Y;
            }
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }
       
    }
    public class EyefireFriendly : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye Fire?");
        }
        public override void SetDefaults()
        {
            projectile.width = 6;
            projectile.height = 6;
            projectile.aiStyle = 23;
            aiType = 101;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.alpha = 255;
            projectile.penetrate = -1;
            projectile.extraUpdates = 3;
            projectile.usesLocalNPCImmunity = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = 10;
            target.immune[projectile.owner] = 0;
        }
    }
    public class RetiMorph : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Retinizer?");
            Main.projFrames[projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.width = 110;
            projectile.height = 110;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 240;
            projectile.usesLocalNPCImmunity = true;
            projectile.extraUpdates = 1;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }
        int timer = 0;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            float r = (QwertysRandomContent.GetLocalCursor(projectile.owner) - projectile.Center).ToRotation();
            projectile.rotation = r + (float)Math.PI / 2;
            timer++;
            if(timer % 16 == 0)
            {
                Projectile.NewProjectile(projectile.Center + QwertyMethods.PolarVector(90, r), QwertyMethods.PolarVector(9f, r), mod.ProjectileType("RetniLaser"), projectile.damage / 4, 0, projectile.owner);
            }
            Vector2 flyTo = player.Center + QwertyMethods.PolarVector(300, (projectile.timeLeft / 240f) * (float)Math.PI + projectile.ai[0] + (float)Math.PI / 2);
            projectile.velocity = projectile.Center.MoveToward(flyTo, 12f);


        }
        
    }
    public class RetniLaser : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.aiStyle = 1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.light = 0.75f;
            projectile.alpha = 255;
            projectile.extraUpdates = 2;
            projectile.scale = 1.2f;
            projectile.timeLeft = 600;
            projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
            aiType = 84;
        }
        public override void AI()
        {
            
        }
    }

}
