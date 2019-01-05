using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace QwertysRandomContent.NPCs.CloakedDarkBoss
{
    public class CloakedDarkBoss : ModNPC
    {
        public override void SetStaticDefaults()
        {

            DisplayName.SetDefault("Cloaked Dark Boss");
            Main.npcFrameCount[npc.type] = 1;

        }

        public override void SetDefaults()
        {

            npc.width = 74;
            npc.height = 74;
            npc.damage = 50;
            npc.defense = 18;
            npc.boss = true;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.value = 60f;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            //aiType = 10;
            animationType = -1;
            npc.noGravity = true;
            npc.noTileCollide = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/BuiltToDestroy");
            npc.lifeMax = 7500;
            //bossBag = mod.ItemType("AncientMachineBag");
            //npc.buffImmune[20] = true;

            

        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {

            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
            npc.damage = (int)(npc.damage * .6f);

        }
       

        public override void AI()
        {
            //npc.rotation += (float)Math.PI / 60;
            npc.TargetClosest(false);
            Player player = Main.player[npc.target];
            npc.ai[0] = 1;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            npc.TargetClosest(false);
            Player player = Main.player[npc.target];

        }
        /*
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return true;
        }
        */

    }
    public class cloak : ModPlayer
    {
        public static readonly PlayerLayer Cloak = new PlayerLayer("QwertysRandomContent", "Cloak", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            int drawCounter =0;
            int Tsize = 20;
            for(int i =0; i<200; i++)
            {
                if(Main.npc[i].active && Main.npc[i].type == mod.NPCType("CloakedDarkBoss") && Main.npc[i].ai[0] == 1)
                {
                    for (int p = -Tsize; p < Main.screenWidth; p++)
                    {
                        for (int q = -Tsize; q < Main.screenHeight; q++)
                        {
                            drawCounter++;
                            int WorldX = (int)Main.screenPosition.X + p;
                            int WorldY = (int)Main.screenPosition.Y + q;
                            if (WorldX % Tsize == 0 && WorldY % Tsize == 0)
                            {
                                bool draw = false;
                                for (int w=0; w < 255; w++)
                                {
                                    if ((Main.player[w].Center - new Vector2(WorldX + Tsize/2, WorldY + Tsize/2)).Length() < 100)
                                    {
                                        break;
                                    }
                                    else if(w ==254)
                                    {
                                        DrawData value = new DrawData(mod.GetTexture("NPCs/CloakedDarkBoss/CloakPattern"), new Vector2(p, q), new Rectangle(0, 0, Tsize, Tsize), Color.White);

                                        Main.playerDrawData.Add(value);
                                    }
                                    
                                }
                                
                            }
                        }
                    }
                    //Main.NewText(drawCounter);
                    drawCounter =0;
                    break;
                }
            }
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>(mod);
           
        });
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int frontLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("MiscEffectsFront"));
            if (frontLayer != -1)
            {
                Cloak.visible = true;
                layers.Insert(frontLayer + 1, Cloak);
            }

        }
    }
}
