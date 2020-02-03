using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            item.mountType = mod.MountType("SpikedSlimeMorph");
            item.damage = dmg;
            item.crit = crt;
            item.knockBack = kb;
            item.GetGlobalItem<ShapeShifterItem>().morph = true;
            item.GetGlobalItem<ShapeShifterItem>().morphDef = def;
            item.GetGlobalItem<ShapeShifterItem>().morphType = ShapeShifterItem.StableShiftType;

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
            if (player.GetModPlayer<ShapeShifterPlayer>().EyeBlessing)
            {

                player.GetModPlayer<ShapeShifterPlayer>().EyeBlessing = false;
            }
            else
            {
                player.AddBuff(mod.BuffType("MorphSickness"), 180);
            }
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
            player.mount.SetMount(mod.MountType("SpikedSlimeMorph"), player);
            player.buffTime[buffIndex] = 10;
        }
    }
    public class SpikedSlimeMorph : ModMountData
    {
        public override void SetDefaults()
        {

            mountData.buff = mod.BuffType("SpikedSlimeMorphB");
            mountData.spawnDust = 15;

            mountData.heightBoost = -8;
            mountData.flightTimeMax = 0;
            mountData.fallDamage = 0.4f;

            mountData.acceleration = 0.13f;
            mountData.jumpHeight = 15;
            mountData.jumpSpeed = 6f;


            mountData.totalFrames = 2;
            mountData.constantJump = true;
            int[] array = new int[mountData.totalFrames];
            for (int l = 0; l < array.Length; l++)
            {
                array[l] = 0;
            }
            mountData.playerYOffsets = array;
            mountData.xOffset = 0;
            mountData.bodyFrame = 1;
            mountData.yOffset = 2;
            mountData.playerHeadOffset = 0;
            mountData.standingFrameCount = 2;
            mountData.standingFrameDelay = 10;
            mountData.standingFrameStart = 0;
            mountData.runningFrameCount = mountData.standingFrameCount;
            mountData.runningFrameDelay = mountData.standingFrameDelay;
            mountData.runningFrameStart = mountData.standingFrameStart;
            mountData.flyingFrameCount = mountData.standingFrameCount;
            mountData.flyingFrameDelay = mountData.standingFrameDelay;
            mountData.flyingFrameStart = mountData.standingFrameStart;
            mountData.inAirFrameCount = mountData.standingFrameCount;
            mountData.inAirFrameDelay = mountData.standingFrameDelay;
            mountData.inAirFrameStart = mountData.standingFrameStart;
            mountData.idleFrameCount = mountData.standingFrameCount;
            mountData.idleFrameDelay = mountData.standingFrameDelay;
            mountData.idleFrameStart = mountData.standingFrameStart;
            mountData.idleFrameLoop = true;
            mountData.swimFrameCount = mountData.inAirFrameCount;
            mountData.swimFrameDelay = mountData.inAirFrameDelay;
            mountData.swimFrameStart = mountData.inAirFrameStart;

            if (Main.netMode != 2)
            {

                mountData.textureWidth = mountData.backTexture.Width;
                mountData.textureHeight = mountData.backTexture.Height;
            }
        }

        public override void UpdateEffects(Player player)
        {

            player.GetModPlayer<ShapeShifterPlayer>().noDraw = true;
            Mount mount = player.mount;
            player.GetModPlayer<ShapeShifterPlayer>().morphed = true;

            player.noItems = true;
            player.statDefense = SpikedSlimeShift.def + player.GetModPlayer<ShapeShifterPlayer>().morphDef;
        }

        public override bool UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
        {

            if (mountedPlayer.GetModPlayer<SpikedSlimeControl>().count > 0)
            {
                mountedPlayer.GetModPlayer<SpikedSlimeControl>().count--;
            }
            if (mountedPlayer.wet)
            {
                mountedPlayer.velocity.Y = -7f;
            }
            if (state == 2 || state == 4)
            {

                mountData.runSpeed = 8f;
                mountData.dashSpeed = 8f;
            }
            else
            {
                mountData.runSpeed = 0f;
                mountData.dashSpeed = 0f;
                mountedPlayer.velocity.X *= .9f;
                if (mountedPlayer.GetModPlayer<SpikedSlimeControl>().count <= 0 && mountedPlayer.whoAmI == Main.myPlayer && Main.mouseLeft && !mountedPlayer.HasBuff(mod.BuffType("MorphSickness")))
                {
                    mountedPlayer.GetModPlayer<SpikedSlimeControl>().count = 12;
                    Projectile.NewProjectile(mountedPlayer.Center, QwertyMethods.PolarVector(10, (Main.MouseWorld - mountedPlayer.Center).ToRotation() + Main.rand.NextFloat(-1, 1) * (float)Math.PI / 16), mod.ProjectileType("PlayerSlimeSpike"), SpikedSlimeShift.dmg, SpikedSlimeShift.kb, mountedPlayer.whoAmI);
                }
            }

            return base.UpdateFrame(mountedPlayer, state, velocity);
        }

    }
    public class SpikedSlimeControl : ModPlayer
    {
        public int count = 0;
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
        int shader;
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