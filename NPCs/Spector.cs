using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs
{
    class Spector : ModNPC
    {
        public override void SetStaticDefaults()
        {

            DisplayName.SetDefault("Sneaking Ghost");
            Main.npcFrameCount[npc.type] = 1;

        }
        public override void SetDefaults()
        {

            npc.width = 28;
            npc.height = 44;
            npc.damage = 120;
            npc.defense = 18;
            
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 60f;
            
            npc.aiStyle = -1;
            
            animationType = -1;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.alpha = 255;
            npc.lifeMax = 100;
            npc.rarity = 1000;
            banner = npc.type;
            bannerItem = mod.ItemType("SpectorBanner");

        }
        public float flyDirection;
        public float flySpeed = 3;
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            
            if (spawnInfo.player.ZoneDungeon && Main.hardMode && NPC.downedPlantBoss)
            {
                
                    return .1f;
                
            }
            else
            {
                return 0f;
            }

        }
        public override void AI()
        {
            Player player = Main.player[npc.target];
            npc.TargetClosest(true);
            if ((player.Center-npc.Center).Length() < 255)
            {
                npc.alpha = (int)(player.Center - npc.Center).Length();
                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, mod.ProjectileType("SpectorGlow"), 0, 0, Main.myPlayer);
            }
            else
            {
                npc.alpha = 255;
            }
            flyDirection =(player.Center - npc.Center).ToRotation();
            npc.velocity = new Vector2((float)Math.Cos(flyDirection) * flySpeed, (float)Math.Sin(flyDirection) * flySpeed);


            if(npc.alpha == 255)
            {
                npc.dontTakeDamage = true;
            }
            else
            {
                npc.dontTakeDamage = false;
            }
        }
        public override void FindFrame(int frameHeight)
        {
            npc.spriteDirection = npc.direction;
        }
        public override void NPCLoot()
        {
            if (Main.rand.Next(3) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SummoningRune"));
            }

        }
    }
    public class SpectorGlow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            
        }
        public override void SetDefaults()
        {
            projectile.light = 1f;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.alpha=255;
            projectile.timeLeft = 2;
        }
    }
    
    
}
