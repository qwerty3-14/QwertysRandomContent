using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.TundraBoss
{
    public class AgentPenguin : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Agent Penguin");
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            npc.width = 22;
            npc.height = 38;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
            npc.aiStyle = 0;
            npc.lifeMax = 10;
            npc.defense = 4;
            npc.damage = 0;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.behindTiles = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }
        int preJump = 180;
        float maxRopeLength = 300;
        Vector2 start = Vector2.Zero;
        
        public override void AI()
        {
            preJump--;
            if (preJump > 0)
            {
                
                npc.velocity = Vector2.Zero;
                Dust.NewDust(npc.BottomLeft + Vector2.UnitY * npc.width, npc.width, npc.width, DustID.Ice);
                start = npc.Center;
            }
            else
            {
                npc.noGravity = false;
                if(preJump == -120 || preJump == -180 && Main.netMode != 1)
                {
                    npc.TargetClosest(true);
                    Vector2 pos = npc.Center + new Vector2(11 * npc.spriteDirection * -1, 0);
                    Projectile p = Main.projectile[Projectile.NewProjectile(pos, QwertyMethods.PolarVector(11, (Main.player[npc.target].Center - pos).ToRotation()), ProjectileID.SnowBallFriendly, 10, 0, 255)];
                    p.hostile = true;
                    p.friendly = false;
                }
                if(preJump < -240 && maxRopeLength > 0)
                {
                    maxRopeLength -= 2;
                }
                if(maxRopeLength <= 0)
                {
                    npc.active = false;
                }
                else
                {
                    Vector2 diff = npc.Center - start;
                    npc.position += (-1 * npc.velocity) * (diff.Length() / maxRopeLength);
                }
            }
            
        }
        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            frame = ((preJump < -120 && preJump > -150) || (preJump < -180 && preJump > -210)) ? 1 : 0;
            npc.frame.Y = frame * frameHeight;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if(preJump <= 0)
            {
                Vector2 diff =  start - npc.Center;
                for(int i =0; i < diff.Length(); i+=8)
                {
                    Texture2D rope = mod.GetTexture("NPCs/TundraBoss/Rope");
                    spriteBatch.Draw(rope, npc.Center + QwertyMethods.PolarVector(i, diff.ToRotation()) - Main.screenPosition, null, drawColor, diff.ToRotation() + (float)Math.PI / 2, new Vector2(3, 0), 1f, 0, 0);
                }
            }
            return preJump <= 0;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, 160);
                Gore.NewGore(new Vector2(npc.position.X, npc.position.Y), npc.velocity, 161);
            }
        }
    }
    
}
