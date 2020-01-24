using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.BladeBoss
{
    public class BladeMinion : ModNPC
    {
        public override void SetStaticDefaults()
        {

            DisplayName.SetDefault("Imperious the III");
            Main.npcFrameCount[npc.type] = 1;

        }

        public override void SetDefaults()
        {

            npc.width = 336;
            npc.height = 336;
            npc.damage = 50;
            npc.defense = 18;
            //npc.boss = true;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.value = 60f;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            //aiType = 10;
            animationType = -1;
            npc.noGravity = true;
            npc.noTileCollide = true;

            npc.lifeMax = 600;
            //bossBag = mod.ItemType("AncientMachineBag");
            //npc.buffImmune[20] = true;

            npc.ai[3] = 1;

        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {

            npc.lifeMax = 800;
            npc.damage = 80;

        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            return 0f;

        }
        float bladeWidth = 26;
        float HiltLength = 62;
        float HiltWidth = 34;
        Vector2 BladeStart;
        Vector2 BladeTip;
        float BladeLength = 132;
        int timer;
        bool endIntro = false;
        bool runOnce = true;
        bool swing;
        Vector2 hitPoint;
        Vector2 endPoint;
        bool toEndPoint;
        float acceleration = .1f;
        float maxSpeed = 16;
        public override void AI()
        {
            if (runOnce)
            {
                npc.rotation = npc.ai[0];
                runOnce = false;
            }
            npc.TargetClosest(false);
            Player player = Main.player[npc.target];
            BladeStart = npc.Center + QwertyMethods.PolarVector(HiltLength / 2, npc.rotation + (float)Math.PI / 2);
            BladeTip = npc.Center + QwertyMethods.PolarVector((HiltLength / 2) + BladeLength, npc.rotation + (float)Math.PI / 2);
            timer++;
            //npc.rotation += (float)Math.PI / 60;

            if (swing)
            {
                if (toEndPoint)
                {
                    npc.velocity = (endPoint - npc.Center) * acceleration;
                    if ((npc.Center - endPoint).Length() < 100)
                    {
                        swing = false;
                        toEndPoint = false;
                    }
                }
                else
                {
                    npc.velocity = (hitPoint - npc.Center) * acceleration;
                    if ((npc.Center - hitPoint).Length() < 100)
                    {
                        toEndPoint = true;
                    }

                }
                if (npc.velocity.Length() > maxSpeed)
                {
                    npc.velocity = npc.velocity.SafeNormalize(-Vector2.UnitY) * maxSpeed;
                }
                if (player.Center.X > npc.Center.X)
                {
                    npc.rotation = QwertyMethods.SlowRotation(npc.rotation, npc.velocity.ToRotation() + (float)Math.PI, 4);
                }
                else
                {
                    npc.rotation = QwertyMethods.SlowRotation(npc.rotation, npc.velocity.ToRotation(), 4);
                }

            }
            else if (endIntro)
            {
                //npc.rotation += (float)Math.PI / 60;
                if (npc.Center.Y + 400 < player.Center.Y)
                {
                    npc.velocity = Vector2.Zero;
                    hitPoint = player.Center;
                    if (player.Center.X > npc.Center.X)
                    {
                        hitPoint.X -= BladeLength / 2;
                    }
                    else
                    {
                        hitPoint.X += BladeLength / 2;
                    }
                    endPoint = hitPoint;
                    endPoint.Y += 400;
                    swing = true;
                }
                else
                {
                    npc.rotation = QwertyMethods.SlowRotation(npc.rotation, (float)Math.PI, 4);
                    npc.velocity = new Vector2(0, -8);
                }
            }
            else
            {
                npc.rotation = npc.ai[0];
                npc.velocity = QwertyMethods.PolarVector(20, npc.ai[0] + (float)Math.PI / 2);
            }

            float goTo = ((npc.Center - player.Center).ToRotation() + (float)Math.PI / 2);
            goTo = QwertyMethods.PolarVector(1, goTo).ToRotation();
            npc.rotation = QwertyMethods.PolarVector(1, npc.rotation).ToRotation();

            if (Math.Abs(npc.rotation - goTo) > (float)Math.PI / 2)
            {

                endIntro = true;
            }

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/BladeBoss/BladeMinion");
            spriteBatch.Draw(texture, new Vector2(npc.Center.X - Main.screenPosition.X, npc.Center.Y - Main.screenPosition.Y),
                        npc.frame, drawColor, npc.rotation,
                        new Vector2(HiltWidth * 0.5f, HiltLength * 0.5f), 1f, SpriteEffects.None, 0f);
            return false;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            float col = 0;
            return Collision.CheckAABBvLineCollision(target.Hitbox.TopLeft(), target.Hitbox.Size(), BladeStart, BladeTip, bladeWidth, ref col);
        }
        public override bool? CanBeHitByProjectile(Projectile target)
        {
            float col = 0;
            return Collision.CheckAABBvLineCollision(target.Hitbox.TopLeft(), target.Hitbox.Size(), BladeStart, BladeTip, bladeWidth, ref col);
        }
        public override bool? CanBeHitByItem(Player player, Item target)
        {
            float col = 0;
            return Collision.CheckAABBvLineCollision(target.Hitbox.TopLeft(), target.Hitbox.Size(), BladeStart, BladeTip, bladeWidth, ref col);
        }
        public override bool? CanHitNPC(NPC target)
        {
            float col = 0;
            return Collision.CheckAABBvLineCollision(target.Hitbox.TopLeft(), target.Hitbox.Size(), BladeStart, BladeTip, bladeWidth, ref col);
        }
        Vector2 CollisionOffset;

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Main.netMode == 0)
            {
                npc.width = 2;
                npc.height = 2;
                CollisionOffset = projectile.Center - npc.position;
                npc.position += CollisionOffset;
            }
        }
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            if (Main.netMode == 0)
            {
                npc.width = 336;
                npc.height = 336;
                npc.position -= CollisionOffset;
            }

        }
        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (Main.netMode == 0)
            {
                npc.width = 2;
                npc.height = 2;
                CollisionOffset = item.Center - npc.position;
                npc.position += CollisionOffset;
            }
        }
        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            if (Main.netMode == 0)
            {
                npc.width = 698;
                npc.height = 698;
                npc.position -= CollisionOffset;
            }

        }


    }

}
