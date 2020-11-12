using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.AbstractClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.DukeFishron
{
    public class SSHurricaneItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shape Shifte: S.S. Hurricane");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.value = Item.sellPrice(gold: 5);
            item.rare = 8;
            item.width = 40;
            item.height = 48;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.value = 120000;
            item.rare = 3;
            item.UseSound = SoundID.Item79;
            item.noMelee = true;
            item.damage = 666;
            item.crit = 0;
            item.knockBack = 2f;
            item.GetGlobalItem<ShapeShifterItem>().morph = true;
            item.GetGlobalItem<ShapeShifterItem>().morphDef = 100;
            item.GetGlobalItem<ShapeShifterItem>().morphType = ShapeShifterItem.StableShiftType;
            item.shoot = mod.ProjectileType("SSHurricane");
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(mod.BuffType("SSHurricaneB"), 2);
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }

        public override bool UseItem(Player player)
        {
            player.GetModPlayer<ShapeShifterPlayer>().justStableMorphed();
            return base.UseItem(player);
        }

    }
    public class SSHurricaneB : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("S.S. Huricane");
            Description.SetDefault("Expect nothing less");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.GetModPlayer<ShapeShifterPlayer>().delayThing <= 0)
            {
                player.buffTime[buffIndex] = 2;
            }
        }
    }
    public class SSHurricane : StableMorph
    {
        private int shotCooldown = 0;
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 6;
        }
        public override void SetSafeDefaults()
        {
            projectile.width = 184;
            projectile.height = 132;
            buffName = "SSHurricaneB";
            itemName = "SSHurricaneItem";
        }

        public override void Effects(Player player)
        {
            player.noKnockback = true;
        }
        public override void Movement(Player player)
        {
            Vector2 shootFrom = projectile.position + new Vector2(projectile.spriteDirection == 1 ? 110 : 184 - 110, 38);
            Vector2 LocalCursor = QwertysRandomContent.GetLocalCursor(player.whoAmI);
            float pointAt = (LocalCursor - shootFrom).ToRotation();
            if (LocalCursor.Y > projectile.Top.Y)
            {
                if (LocalCursor.X > projectile.Top.X)
                {
                    pointAt = 0;
                }
                else
                {
                    pointAt = (float)Math.PI;
                }
            }
            player.GetModPlayer<ShapeShifterPlayer>().drawTankCannon = true;
            player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation = QwertyMethods.SlowRotation(player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation, pointAt, 3);
            if (player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation > 0)
            {
                if (player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation > (float)Math.PI / 2)
                {
                    player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation = (float)Math.PI;
                }
                else
                {
                    player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation = 0;
                }
            }
            if (shotCooldown > 0)
            {
                shotCooldown--;
            }
            if (player.whoAmI == Main.myPlayer && Main.mouseLeft && !player.HasBuff(mod.BuffType("MorphSickness")) && shotCooldown == 0)
            {
                shotCooldown = 15;
                Projectile.NewProjectile(shootFrom + QwertyMethods.PolarVector(50, player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation), QwertyMethods.PolarVector(16, player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation), mod.ProjectileType("BattleshipShot"), (int)projectile.damage, projectile.knockBack, player.whoAmI);
            }
            projectile.velocity.Y = 0f;
            projectile.frameCounter++;
            projectile.frame = (projectile.frameCounter / 10) % 6;
        }

        public override bool Running()
        {
            speed = 8;
            acceleration = .2f;
            jumpHeight = 0;
            jumpSpeed = 0f;
            gravity = 0;
            terminalVelocity = 0;
            return true;
        }

        public override void PostDrawMorphExtras(SpriteBatch spriteBatch, Color lightColor)
        {
            Player drawPlayer = Main.player[projectile.owner];

            //Main.NewText("Tank!!");
            Texture2D texture = mod.GetTexture("Items/Weapons/DukeFishron/HurricaneGun");
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16, Color.White), 0f);
            spriteBatch.Draw(texture,
                projectile.position + new Vector2(projectile.spriteDirection == 1 ? 110 : 184-110, 38) - Main.screenPosition,
                null,
                color12,
                drawPlayer.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation,
                drawPlayer.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation > -(float)Math.PI / 2 ? new Vector2(20, 25) : new Vector2(20, 34-25),
                1f,
                drawPlayer.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation > -(float)Math.PI/2 ? SpriteEffects.None : SpriteEffects.FlipVertically,
                0);
        }
    }
    public class BattleshipShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("S.S. Hurricane");
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;

            projectile.width = 32;
            projectile.height = 32;
            projectile.extraUpdates = 2;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 600;
            projectile.usesLocalNPCImmunity = true;
        }

        public bool runOnce = true;

        public override void AI()
        {
            //Main.NewText(projectile.damage);
            if (runOnce)
            {
                Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 19);
                // Fire Dust spawn
                for (int i = 0; i < 20; i++)
                {
                    int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, 217, 0f, 0f, 100, default(Color), 3f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 2f;
                    dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, 217, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 1f;
                    Main.dust[dustIndex].noGravity = true;
                }
                runOnce = false;
            }
            projectile.frameCounter++;
            projectile.frame = projectile.frameCounter % 3;
            for(int i = 0; i < 5; i++)
            {
                int dustIndex2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 217, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex2].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
            Projectile e = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("HurricaneBlast"), projectile.damage, projectile.knockBack, projectile.owner)];
            e.localNPCImmunity[target.whoAmI] = -1;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile e = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("HurricaneBlast"), projectile.damage, projectile.knockBack, projectile.owner)];
            return true;
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];

            Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 19);
            // Fire Dust spawn
            for (int i = 0; i < 30; i++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dustIndex = Dust.NewDustPerfect(projectile.Center, 217, QwertyMethods.PolarVector(Main.rand.NextFloat() * 8f, theta));
                dustIndex.noGravity = true;
                theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                dustIndex = Dust.NewDustPerfect(projectile.Center, 217, QwertyMethods.PolarVector(Main.rand.NextFloat() * 8f, theta), Scale: 2f);
                dustIndex.noGravity = true;
            }
        }
    }

    public class HurricaneBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("S.S. Hurricane");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 140;
            projectile.height = 140;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 2;
            projectile.usesLocalNPCImmunity = true;
            projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
    }
}

