using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.AbstractClasses;
using QwertysRandomContent.Config;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Etims
{
    public class GodOfBlasphemy : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shape Shifte: God of Blasphemy");
            Tooltip.SetDefault("");
        }

        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicNoehtnap ? base.Texture + "_Old" : base.Texture;

        public override void SetDefaults()
        {
            item.width = 50;
            item.height = 50;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.value = 120000;
            item.rare = 3;
            item.UseSound = SoundID.Item79;
            item.noMelee = true;
            //item.mountType = mod.MountType("GodOfBlasphemyShift");
            item.damage = 48;
            item.crit = 0;
            item.knockBack = 1f;
            item.GetGlobalItem<ShapeShifterItem>().morph = true;
            item.GetGlobalItem<ShapeShifterItem>().morphDef = 13;
            item.GetGlobalItem<ShapeShifterItem>().morphType = ShapeShifterItem.StableShiftType;
            item.shoot = mod.ProjectileType("GodOfBlasphemyShift");
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(mod.BuffType("GodOfBlasphemyB"), 2);
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

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("EtimsMaterial"), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class GodOfBlasphemyB : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("God of Blasphemy");
            Description.SetDefault("You look familiar...");
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

    public class GodOfBlasphemyShift : StableMorph
    {
        public override void SetSafeDefaults()
        {
            projectile.width = 166;
            projectile.height = 128;
            projectile.timeLeft = 2;
            buffName = "GodOfBlasphemyB";
            itemName = "GodOfBlasphemy";
        }

        private float flySpeed = 6.2f;
        private int shotCooldown = 20;
        private float pupilDirection = 0f;
        private float greaterPupilRadius = 18;
        private float lesserPupilRadius = 6;
        public float scale = 1f;
        public Vector2 pupilPosition;

        public override void Effects(Player player)
        {
            player.nightVision = true;
            //player.GetModPlayer<ShapeShifterPlayer>().drawGodOfBlasphemy = true;
        }

        public override void Movement(Player player)
        {
            player.gravity = 0f;
            player.accRunSpeed = 0;

            Vector2 LocalCursor = QwertysRandomContent.GetLocalCursor(player.whoAmI);
            float pupilStareOutAmount = (LocalCursor - player.Center).Length() / 300f;
            if (pupilStareOutAmount > 1f)
            {
                pupilStareOutAmount = 1f;
            }
            scale = 1f + .05f * (float)Math.Sin(player.GetModPlayer<ShapeShifterPlayer>().pulseCounter);
            pupilDirection = (LocalCursor - player.Center).ToRotation();
            pupilPosition = new Vector2((float)Math.Cos(pupilDirection) * greaterPupilRadius * pupilStareOutAmount, (float)Math.Sin(pupilDirection) * lesserPupilRadius) * scale;

            projectile.velocity = Vector2.Zero;
            if (player.controlUp)
            {
                projectile.velocity.Y += -1;
            }
            if (player.controlDown)
            {
                projectile.velocity.Y += 1;
            }
            if (player.controlLeft)
            {
                projectile.velocity.X += -1;
            }
            if (player.controlRight)
            {
                projectile.velocity.X += 1;
            }
            if (shotCooldown > 0)
            {
                shotCooldown--;
            }
            if (player.whoAmI == Main.myPlayer && Main.mouseLeft && !player.HasBuff(mod.BuffType("MorphSickness")) && shotCooldown == 0)
            {
                shotCooldown = 20;
                Projectile p = Main.projectile[Projectile.NewProjectile(player.Center + pupilPosition, QwertyMethods.PolarVector(10, (LocalCursor - player.Center).ToRotation()), mod.ProjectileType("EtimsicRayFreindly"), player.GetWeaponDamage(player.HeldItem), player.GetWeaponKnockback(player.HeldItem, projectile.knockBack), player.whoAmI)];

                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/PewPew").WithVolume(3f).WithPitchVariance(.5f), player.Center);
            }
            if (projectile.velocity.Length() > 0)
            {
                projectile.velocity = projectile.velocity.SafeNormalize(-Vector2.UnitY);
                projectile.velocity *= flySpeed;
            }
            player.direction = Math.Sign(LocalCursor.X - player.Center.X);
        }

        public override bool DrawMorphExtras(SpriteBatch spriteBatch, Color lightColor)
        {
            Player drawPlayer = Main.player[projectile.owner];
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16, Color.White), 0f);
            Texture2D texture = mod.GetTexture("Items/Etims/GodOfBlasphemyShift" + (ModContent.GetInstance<SpriteSettings>().ClassicNoehtnap ? "_Old" : ""));
            spriteBatch.Draw(texture,
                drawPlayer.Center - Main.screenPosition,
                null,
                color12,
                0,
                texture.Size() * .5f,
                scale,
                0,
                0);
            /*
            value.shader = drawPlayer.miscDyes[3].dye;
            Main.playerDrawData.Add(value);
            */

            texture = mod.GetTexture("Items/Etims/Pupil" + (ModContent.GetInstance<SpriteSettings>().ClassicNoehtnap ? "_Old" : ""));
            spriteBatch.Draw(texture,
                drawPlayer.Center + pupilPosition - Main.screenPosition,
                null,
                color12,
                0,
                texture.Size() * .5f,
                scale,
                0,
                0);
            /*
            value.shader = drawPlayer.miscDyes[3].dye;
            Main.playerDrawData.Add(value);
            */
            return false;
        }
    }

    public class EtimsicRayFreindly : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Etimsic Ray");
        }

        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicNoehtnap ? base.Texture + "_Old" : base.Texture;

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.extraUpdates = 1;
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 600;
            projectile.tileCollide = true;
            projectile.light = 1f;
            projectile.penetrate = 2;
            projectile.usesLocalNPCImmunity = true;
            projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
            projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
            projectile.GetGlobalProjectile<Etims>().effect = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }

        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsOverWiresUI.Add(index);
        }
    }
}