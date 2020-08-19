using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.AbstractClasses;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.ShapeShifter
{
    public class SpikedSlimeShift : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shape Shift: Spiked Slime");
            Tooltip.SetDefault("Shoot spikes or jump around!");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.AnimatesAsSoul[item.type] = true;
            ItemID.Sets.ItemIconPulse[item.type] = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }

        public const int dmg = 26;
        public const int crt = 0;
        public const float kb = 1f;
        public const int def = 13;

        public override void SetDefaults()
        {
            item.width = 42;
            item.height = 42;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.value = 150000;
            item.rare = 1;
            item.UseSound = SoundID.Item79;
            item.noMelee = true;
            //item.mountType = mod.MountType("SpikedSlimeMorph");
            item.damage = dmg;
            item.crit = crt;
            item.knockBack = kb;
            item.GetGlobalItem<ShapeShifterItem>().morph = true;
            item.GetGlobalItem<ShapeShifterItem>().morphDef = def;
            item.GetGlobalItem<ShapeShifterItem>().morphType = ShapeShifterItem.StableShiftType;
            item.shoot = mod.ProjectileType("SpikedSlimeMorph");
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(mod.BuffType("SpikedSlimeMorphB"), 2);
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override bool GrabStyle(Player player)
        {
            Vector2 vectorItemToPlayer = player.Center - item.Center;
            Vector2 movement = vectorItemToPlayer.SafeNormalize(default(Vector2)) * 0.1f;
            item.velocity = item.velocity + movement;
            item.velocity = Collision.TileCollision(item.position, item.velocity, item.width, item.height);
            return true;
        }

        public override bool UseItem(Player player)
        {
            player.GetModPlayer<ShapeShifterPlayer>().justStableMorphed();
            return base.UseItem(player);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
        }
    }

    public class SpikedSlimeMorphB : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Spiked Slime Morph");
            Description.SetDefault("");
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

    public class SpikedSlimeMorph : StableMorph
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetSafeDefaults()
        {
            projectile.width = 40;
            projectile.height = 34;
            buffName = "SpikedSlimeMorphB";
            itemName = "SpikedSlimeShift";
        }

        public override void Effects(Player player)
        {
            if (projectile.velocity.Y == 0)
            {
                if (player.controlJump)
                {
                    projectile.velocity.Y -= 8f;
                }
                else
                {
                    if (count <= 0 && player.whoAmI == Main.myPlayer && Main.mouseLeft && !player.HasBuff(mod.BuffType("MorphSickness")))
                    {
                        count = 12;
                        Projectile.NewProjectile(player.Center, QwertyMethods.PolarVector(10, (Main.MouseWorld - player.Center).ToRotation() + Main.rand.NextFloat(-1, 1) * (float)Math.PI / 16), mod.ProjectileType("PlayerSlimeSpike"), (int)projectile.damage, projectile.knockBack, player.whoAmI);
                    }
                    projectile.velocity.X = 0;
                }
            }
            else
            {
            }
            base.Effects(player);
        }

        private int count = 12;

        public override void Movement(Player player)
        {
            count--;
            projectile.frameCounter++;
            projectile.frame = (projectile.frameCounter % 20 < 10 ? 0 : 1);
            if (projectile.wet)
            {
                projectile.velocity.Y = -7f;
            }
        }

        public override bool Running()
        {
            jumpSpeed = 6f;
            jumpHeight = 15;
            if (projectile.velocity.Y == 0)
            {
                acceleration = 0;
                speed = 0;
            }
            else
            {
                acceleration = 1f;
                speed = 8f;
            }
            return true;
        }
    }

    public class PlayerSlimeSpike : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
            projectile.alpha = 255;
            projectile.width = 6;
            projectile.height = 6;
            projectile.aiStyle = 1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }

        private int shader;

        public override void AI()
        {
            if (projectile.ai[1] == 0f)
            {
                projectile.ai[1] = 1f;
                shader = Main.player[projectile.owner].miscDyes[3].dye;
                Main.PlaySound(SoundID.Item17, projectile.position);
            }
            if (projectile.alpha == 0 && Main.rand.Next(3) == 0)
            {
                int num69 = Dust.NewDust(projectile.position - projectile.velocity * 3f, projectile.width, projectile.height, 4, 0f, 0f, 50, new Color(78, 136, 255, 150), 1.2f);
                Main.dust[num69].velocity *= 0.3f;
                Main.dust[num69].velocity += projectile.velocity * 0.3f;
                Main.dust[num69].noGravity = true;
                Main.dust[num69].shader = GameShaders.Armor.GetSecondaryShader(shader, Main.player[projectile.owner]);
            }
            projectile.alpha -= 50;
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }

            if (projectile.ai[0] >= 5f)
            {
                projectile.ai[0] = 5f;
                projectile.velocity.Y = projectile.velocity.Y + 0.15f;
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            // As mentioned above, be sure not to forget this step.
            Player player = Main.player[projectile.owner];
            if (shader != 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];

            if (shader != 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                GameShaders.Armor.GetSecondaryShader(shader, player).Apply(null);
            }
            return true;
        }
    }

    public class KingSlimeBagDrop : GlobalItem
    {
        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            if (context == "bossBag" && arg == ItemID.KingSlimeBossBag && Main.rand.Next(2) == 0)
            {
                player.QuickSpawnItem(mod.ItemType("SpikedSlimeShift"));
            }
        }
    }

    public class KingSlimeDrop : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if (npc.type == NPCID.KingSlime && Main.rand.Next(3) == 0 && !Main.expertMode)
            {
                Item.NewItem(npc.Hitbox, mod.ItemType("SpikedSlimeShift"));
            }
        }
    }
}