using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.Graphics.Capture;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Effects;
using QwertysRandomContent.Items.Accesories;
using QwertysRandomContent.Items.Armor.Rhuthinium;
using QwertysRandomContent.Items.Armor.Vargule;
using Terraria.DataStructures;
using QwertysRandomContent;
using System.Linq;
using QwertysRandomContent.NPCs;
using Terraria.Localization;





using Terraria.DataStructures;




namespace QwertysRandomContent
{
	public class QwertyGloabalNPC : GlobalNPC
	{
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public int age= 0;
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
		{
            
            var modPlayer = player.GetModPlayer<QwertyPlayer>();
			if(QwertyWorld.DinoEvent)
			{
				
				if(NPC.AnyNPCs(mod.NPCType("TheGreatTyrannosaurus")) && !NPC.downedMoonlord)
				{
					spawnRate = 0;
					maxSpawns = 0;
				}
				else
				{
                    
                    if(NPC.downedMoonlord)
                    {
                        spawnRate = 30;
                        maxSpawns = 30;
                    }
                    else
                    {
                        spawnRate = 10;
                        maxSpawns = 10;
                    }
					
				}
			}
            if (modPlayer.TheAbstract)
            {
                spawnRate = 100;
                maxSpawns = 100;
            }

        }
     
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (npc.HasBuff(mod.BuffType("LuneCurse")) && crit)
            {
                //Main.NewText("Boost!");
                damage = (int)(damage * 1.5f);

            }
            if(projectile.melee && projectile.minion)
            {
                crit = false;
            }
        }
        
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (npc.HasBuff(mod.BuffType("LuneCurse")) && crit)
            {
                damage = (int)(damage * 1.5f);
            }
        }
        public override bool PreAI(NPC npc)
        {
            age++;
            //Main.NewText(npc.lifeRegenExpectedLossPerSecond);
            if (npc.HasBuff(mod.BuffType("TitanicGrasp")) || npc.HasBuff(mod.BuffType("Stunned")))
            {
                if(npc.HasBuff(mod.BuffType("Stunned")))
                {
                    npc.velocity = Vector2.Zero;
                }
                return false;
            }
            return base.PreAI(npc);
        }
        float stunCounter = 0;
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {
            
            if(npc.HasBuff(mod.BuffType("Stunned")))
            {
                //float area = npc.width * npc.height;
                float widthForScale = npc.width;
                if(widthForScale < 30)
                {
                    widthForScale = 30;
                }
                if(widthForScale > 300)
                {
                    widthForScale = 300;
                }
                float scale = widthForScale / 100f;
                float stunnedHorizontalMovement = (npc.width/2) * 1.5f;
                float heightofStunned = (npc.height / 2) * 1.2f;
                stunCounter += (float)Math.PI / 60;
                Texture2D texture = mod.GetTexture("Items/DinoItems/Stun");
                //Main.NewText((float)Math.Sin(stunCounter));
                if ((float)Math.Cos(stunCounter) > 0)
                {
                    Vector2 CenterOfStunned = new Vector2(npc.Center.X + (float)Math.Sin(stunCounter) * stunnedHorizontalMovement, npc.Center.Y - heightofStunned);
                    
                    spriteBatch.Draw(texture, new Vector2(CenterOfStunned.X - Main.screenPosition.X, CenterOfStunned.Y - Main.screenPosition.Y),
                            new Rectangle(0, 0, texture.Width, texture.Height), drawColor, stunCounter,
                            new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), scale, SpriteEffects.None, 0f);


                    CenterOfStunned = new Vector2(npc.Center.X - (float)Math.Sin(stunCounter) * stunnedHorizontalMovement, npc.Center.Y - heightofStunned);
                    spriteBatch.Draw(texture, new Vector2(CenterOfStunned.X - Main.screenPosition.X, CenterOfStunned.Y - Main.screenPosition.Y),
                            new Rectangle(0, 0, texture.Width, texture.Height), drawColor, stunCounter,
                            new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), scale, SpriteEffects.None, 0f);
                }
                else
                {
                    Vector2 CenterOfStunned = new Vector2(npc.Center.X - (float)Math.Sin(stunCounter) * stunnedHorizontalMovement, npc.Center.Y - heightofStunned);
                    
                    spriteBatch.Draw(texture, new Vector2(CenterOfStunned.X - Main.screenPosition.X, CenterOfStunned.Y - Main.screenPosition.Y),
                            new Rectangle(0, 0, texture.Width, texture.Height), drawColor, stunCounter,
                            new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), scale, SpriteEffects.None, 0f);


                    CenterOfStunned = new Vector2(npc.Center.X + (float)Math.Sin(stunCounter) * stunnedHorizontalMovement, npc.Center.Y - heightofStunned);
                    spriteBatch.Draw(texture, new Vector2(CenterOfStunned.X - Main.screenPosition.X, CenterOfStunned.Y - Main.screenPosition.Y),
                            new Rectangle(0, 0, texture.Width, texture.Height), drawColor, stunCounter,
                            new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), scale, SpriteEffects.None, 0f);
                }
            }
        }

    }
	
	
}
