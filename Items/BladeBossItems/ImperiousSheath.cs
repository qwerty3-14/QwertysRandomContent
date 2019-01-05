using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.BladeBossItems
{


    public class ImperiousSheath : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Imperious's Sheath");
           
            Tooltip.SetDefault("After dealing 10,000 damage you can summon Imperious to fight for you breifly with the " + "'" + "Special Ability" + "' key" + "\nCan be changed in controls");

        }
        public override void PostUpdate()
        {
           
        }
        public override void SetDefaults()
        {

            item.value = 200000;
            item.rare = 7;
            item.expert = true;

            item.width = 34;
            item.height = 32;
            item.damage = 500;
            item.summon = true;
            item.knockBack = 8f;
            item.accessory = true;



        }
        
        public override void UpdateEquip(Player player)
        {
            
            player.GetModPlayer<ImperiousEffect>(mod).effect = true;
        }
        //this changes the tooltip based on what the hotkey is configured to
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (String key in QwertysRandomContent.YetAnotherSpecialAbility.GetAssignedKeys()) //get's the string of the hotkey's name
            {
                
                foreach (TooltipLine line in tooltips) //runs through all tooltip lines
                {
                    if (line.mod == "Terraria" && line.Name == "Tooltip0") //this checks if it's the line we're interested in
                    {
                        line.text = "After dealing 10,000 damage you can summon Imperious to fight for you breifly with the " + "'" + key + "' key"; //change tooltip
                    }
                    
                }
            }
        }
        


    }
    public class ImperiousEffect : ModPlayer
    {
        public bool effect = false; //does the player get this effect
        public int damageTally; //used to count as damage is dealt
        public int damageTallyMax = 10000; 
        public override void ResetEffects() //used to reset if the player unequips the accesory
        {
            effect = false;
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit) //runs when an npc is hit by the player's projectile
        {
            if (proj.owner == player.whoAmI && effect && !target.immortal && proj.type != mod.ProjectileType("Imperious")) //check if vallid npc and effect is active
            {
                damageTally += damage; //count up
            }
        }
        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit) //runs when an npc is hit by an item (sword blade)
        {
            if (effect && !target.immortal)  //check if vallid npc  and effect is active
            {
                damageTally += damage; //count up
            }
        }
        public override void PreUpdate() //runs every frame
        {
            if (damageTally > damageTallyMax)
            {
                damageTally = damageTallyMax; //limits the tally
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet) //runs hotkey effects
        {
            if (QwertysRandomContent.YetAnotherSpecialAbility.JustPressed) //hotkey is pressed
            {
                
                if(effect && damageTally >= damageTallyMax) 
                {
                    if(NPC.AnyNPCs(mod.NPCType("BladeBoss"))) //don't want to summon Imperious if he's already active!
                    {
                        Main.NewText("You're already fighting Imperious!");
                    }
                    else
                    {
                        Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("Imperious"), (int)(500f * player.minionDamage), 8f * player.minionKB, player.whoAmI); //summons Imperious to fight!
                        damageTally = 0; //resets the tally
                    }
                }
            }
        }
        //the rest of the ModPlayer class focusses on drawing the progress bar below the player
        public static readonly PlayerLayer SheathBar = new PlayerLayer("QwertysRandomContent", "SheathBar", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            
            if (drawPlayer.GetModPlayer<ImperiousEffect>().effect)
            {
                
                Texture2D texture = mod.GetTexture("Items/BladeBossItems/SheathProgress");
                


                int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
                int drawY = (int)(drawPlayer.position.Y - Main.screenPosition.Y);
                Vector2 Position = drawInfo.position;
                Vector2 origin = texture.Size() / 2;
                Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                pos.Y += 50;
                
                DrawData data = new DrawData(texture, pos, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0f, origin, 1f, SpriteEffects.None, 0);
                
                Main.playerDrawData.Add(data);

            }
        });
        public static readonly PlayerLayer SheathProgress = new PlayerLayer("QwertysRandomContent", "SheathProgress", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");

            if (drawPlayer.GetModPlayer<ImperiousEffect>().effect)
            {

                Texture2D texture = mod.GetTexture("Items/BladeBossItems/SheathBlip");


                Color color = Color.White;

                int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
                int drawY = (int)(drawPlayer.position.Y - Main.screenPosition.Y);
                Vector2 Position = drawInfo.position;
                Vector2 origin = new Vector2((texture.Width - 2) / 2, texture.Height / 4);
                Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                pos.Y += 50f;


                float amountComplete = (float)(drawPlayer.GetModPlayer<ImperiousEffect>().damageTally) / (float)(drawPlayer.GetModPlayer<ImperiousEffect>().damageTallyMax);
                int frame = 0;
                if (amountComplete >= 1f)
                {
                    frame = 10;
                }

                DrawData data = new DrawData(texture, pos, new Rectangle(0, frame, (int)((texture.Width - 2) * amountComplete), texture.Height / 2), color, 0f, origin, 1f, SpriteEffects.None, 0);
                
                Main.playerDrawData.Add(data);
                


            }
        });
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int f = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Body"));
            if (f != -1)
            {
                SheathProgress.visible = true;
                layers.Insert(f + 2, SheathProgress);
                SheathBar.visible = true;
                layers.Insert(f + 1, SheathBar);
                
            }
            

        }
    }
    public class Imperious : ModProjectile
    {
        int rotateDirection=1;
        public override void SetDefaults()
        {
            projectile.width = 84;
            projectile.height = 94;
            projectile.friendly = true;
            projectile.aiStyle = -1;
            projectile.tileCollide = false;
            projectile.timeLeft = 180;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 20;
            projectile.rotation = (float)Math.PI;
            Player player = Main.player[projectile.owner];
            rotateDirection = player.direction;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown; //local immunity
            target.immune[projectile.owner] = 0;
        }
        float bladeWidth = 74;
        float HiltLength = 94;
        float HiltWidth = 84;
        Vector2 BladeStart;
        Vector2 BladeTip;
        float BladeLength = 300;

        public override void AI()
        {
            BladeStart = projectile.Center + QwertyMethods.PolarVector(HiltLength / 2, projectile.rotation + (float)Math.PI / 2);
            BladeTip = projectile.Center + QwertyMethods.PolarVector((HiltLength / 2) + BladeLength, projectile.rotation + (float)Math.PI / 2);
            Player player = Main.player[projectile.owner];
            projectile.Center = player.Center;
            projectile.rotation += (float)Math.PI / 15* rotateDirection;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) //custom collision
        {
            float col = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), BladeStart, BladeTip, bladeWidth, ref col);
        }
    }


}

