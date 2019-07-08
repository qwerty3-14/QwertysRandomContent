using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress.GaleArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class GaleSwiftplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gale Swiftplate");
			Tooltip.SetDefault("+12% chance to dodge an attack" + "\n+15% thrown critical strike chance and velocity" + "\n20% chance not to consume thrown items" + "\nAllows you to cling to walls" + "\nThrowing damage increased by 25% while clinging to walls");


        }
		

		public override void SetDefaults()
		{
			
			item.value = Item.sellPrice(0,0,75,0);
			item.rare = 4;
            item.defense = 2;
            //item.vanity = true;
            item.width = 20;
			item.height = 20;
			
			
			
			
		}
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {

            return head.type == mod.ItemType("GaleSwiftHelm") && legs.type == mod.ItemType("GaleSwiftRobes");

        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.QwertysRandomContent.GaleSet");
            player.GetModPlayer<GaleSetBonus>().setBonus = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<QwertyPlayer>().dodgeChance += 12;
            player.thrownCrit += 15;
            player.thrownVelocity += .15f;
            player.GetModPlayer<QwertyPlayer>().throwReduction *= .8f;
            player.spikedBoots = 2;
            if(player.sliding)
            {
                //Main.NewText("Sliding");
                player.thrownDamage += .25f;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CaeliteBar"), 10);
            recipe.AddIngredient(mod.ItemType("FortressHarpyBeak"), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }






    }
    public class GaleKnife : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gale Knife");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 18;
            projectile.height = 18;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 120;
            projectile.tileCollide = false;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
            projectile.thrown = true;

        }

        public int dustTimer;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
            target.immune[projectile.owner] = 0;
        }


    }
    public class GaleSetBonus : ModPlayer
    {
        public bool setBonus = false;
        public Vector3[,] orb = new Vector3[10,Main.player.Length];
        bool runOnce = true;
        public override void Initialize()
        {
            /*
            if (Main.netMode == 1)
            {
                Main.NewText("client: " + "Hello");
            }


            if (Main.netMode == 2) // Server
            {
                NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server: " + "Hello"), Color.White);
            }
            */
            


        }
        public override void ResetEffects()
        {
            setBonus = false;
        }
        public float counter = 0;
        public int timer;
        public override void UpdateEquips(ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff)
        {
            for (int b = 0; b < 10; b++)
            {
                if (orb[b, player.whoAmI].X !=0)
                {
                    player.GetModPlayer<QwertyPlayer>().dodgeChance++;
                }
            }
        }
        public int rightclickTimer;
        bool resetRTimer;
        int orbFrameCounter;
        public override void PreUpdate()
        {
            
                if (runOnce)
                {
                    
                    for (int b = 0; b < 10; b++)
                    {
                        orb[b, player.whoAmI] = new Vector3(0, 2 * (float)Math.PI * (b / (float)10), 0);
                    }
                    runOnce = false;
                }
                
                orbFrameCounter++;
                if (orbFrameCounter >= 40)
                {
                    orbFrameCounter = 0;
                }
                orb[orbFrameCounter % 10, player.whoAmI].Z = (orbFrameCounter - (orbFrameCounter % 10)) / 10;

                if (!setBonus)
                {
                    for (int b = 0; b < 10; b++)
                    {
                        orb[b, player.whoAmI].X = 0;
                    }
                }
                else
                {
                    if (Main.mouseRight && Main.mouseRightRelease)
                    {


                        if (rightclickTimer > 0)
                        {
                            for (int b = 0; b < 10; b++)
                            {
                                if (orb[b, player.whoAmI].X != 0)
                                {
                                    Vector2 Position = player.Center;
                                    Position.X += (float)Math.Sin(orb[b, player.whoAmI].Y) * 50;
                                    Position.Y += (float)Math.Sin(orb[b, player.whoAmI].Y) * 50 * (float)Math.Sin(counter);
                                    float speed = 10;
                                    Projectile.NewProjectile(Position, (Main.MouseWorld - Position).SafeNormalize(-Vector2.UnitY) * speed * player.thrownVelocity, mod.ProjectileType("GaleKnife"), (int)(100f * player.thrownDamage), 3f, player.whoAmI);
                                    orb[b, player.whoAmI].X = 0;
                                }
                            }
                            //Main.NewText("Double tap!");
                            rightclickTimer = 0;

                        }
                    }
                    if (rightclickTimer > 0)
                    {
                        rightclickTimer--;
                        //Main.NewText(rightclickTimer);
                    }
                    if (Main.mouseRight && resetRTimer)
                    {
                        //Main.NewText("Double tap!");

                        rightclickTimer = 15;

                        resetRTimer = false;
                    }
                    else if (!Main.mouseRight)
                    {
                        resetRTimer = true;
                    }
                }
                timer++;
                if (timer > 120)
                {
                    for (int b = 0; b < 10; b++)
                    {
                        if (setBonus && orb[b, player.whoAmI].X == 0)
                        {
                            orb[b, player.whoAmI].X = 1;
                            break;
                        }
                    }
                    timer = 0;
                }
                counter += (float)Math.PI / 120;
                for (int b = 0; b < 10; b++)
                {
                    /*
                    if (Main.netMode == 1)
                    {
                        Main.NewText("client: " + "Hello");
                    }


                    if (Main.netMode == 2) // Server
                    {
                        NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server: " + "Hello"), Color.White);
                    }
                    */
                    orb[b, player.whoAmI].Y += (float)Math.PI / 60 * player.direction;
                    //orb[b].Z = orb[b].Y ;
                    if (orb[b, player.whoAmI].X != 0)
                    {

                        if ((float)Math.Cos(orb[b, player.whoAmI].Y) < 0)
                        {
                            orb[b, player.whoAmI].X = 1;
                        }
                        else
                        {
                            orb[b, player.whoAmI].X = 2;
                        }
                    }
                }
                //Main.NewText(orb[0].Y);
            
            
        }
        public  PlayerLayer GaleOrb = new PlayerLayer("QwertysRandomContent", "GaleOrb", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            GaleSetBonus modPlayer = drawPlayer.GetModPlayer<GaleSetBonus>();
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            
            Texture2D texture = mod.GetTexture("Items/Fortress/GaleArmor/GaleOrb");


            for (int b = 0; b < 10; b++)
            {
                if (modPlayer.orb[b, drawPlayer.whoAmI].X == 2)
                {
                    int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
                    int drawY = (int)(drawPlayer.position.Y - Main.screenPosition.Y);
                    Vector2 Position = drawPlayer.Center;
                    Position.X += (float)Math.Sin(modPlayer.orb[b, drawPlayer.whoAmI].Y) * 50;
                    Position.Y += (float)Math.Sin(modPlayer.orb[b, drawPlayer.whoAmI].Y) * 50 * (float)Math.Sin(modPlayer.counter);
                    //Main.NewText(b + ", " + (float)Math.Sin(modPlayer.orb[b].Y) * 50);
                    Vector2 origin = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.5f);
                    Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                    pos.Y -= drawPlayer.mount.PlayerOffset;
                    DrawData data = new DrawData(texture, pos, new Rectangle(0, (int)modPlayer.orb[b, drawPlayer.whoAmI].Z * texture.Height / 4, texture.Width,  texture.Height/4), Color.White, 0, origin, 1f, drawPlayer.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                    data.shader = drawInfo.bodyArmorShader;
                    Main.playerDrawData.Add(data);
                }
            }

        });
        public PlayerLayer GaleOrbBack = new PlayerLayer("QwertysRandomContent", "GaleOrbBack", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            GaleSetBonus modPlayer = drawPlayer.GetModPlayer<GaleSetBonus>();
            Mod mod = ModLoader.GetMod("QwertysRandomContent");

            Texture2D texture = mod.GetTexture("Items/Fortress/GaleArmor/GaleOrb");


            for (int b = 0; b < 10; b++)
            {
                if (modPlayer.orb[b, drawPlayer.whoAmI].X == 1)
                {
                    int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
                    int drawY = (int)(drawPlayer.position.Y - Main.screenPosition.Y);
                    Vector2 Position = drawPlayer.Center;
                    Position.X += (float)Math.Sin(modPlayer.orb[b, drawPlayer.whoAmI].Y) * 50;
                    Position.Y += (float)Math.Sin(modPlayer.orb[b, drawPlayer.whoAmI].Y) * 50 * (float)Math.Sin(modPlayer.counter);
                    //Main.NewText(b + ", " + (float)Math.Sin(modPlayer.orb[b].Y) * 50);
                    Vector2 origin = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.5f);
                    Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                    pos.Y -= drawPlayer.mount.PlayerOffset;
                    DrawData data = new DrawData(texture, pos, new Rectangle(0, (int)modPlayer.orb[b, drawPlayer.whoAmI].Z * texture.Height / 4, texture.Width, texture.Height / 4), Color.White, 0, origin, 1f, drawPlayer.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                    data.shader = drawInfo.bodyArmorShader;
                    Main.playerDrawData.Add(data);
                }
            }

        });

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int bodyLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("MiscEffectsFront"));
            if (bodyLayer != -1 && setBonus)
            {
                GaleOrb.visible = true;
                layers.Insert(bodyLayer + 1, GaleOrb);
            }
            
            if (setBonus)
            {
                GaleOrbBack.visible = true;
                layers.Insert(0, GaleOrbBack);
            }


        }
    }
		
	
}

