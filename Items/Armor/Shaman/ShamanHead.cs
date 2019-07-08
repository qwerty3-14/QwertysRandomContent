using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Shaman
{
	[AutoloadEquip(EquipType.Head)]
	public class ShamanHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shaman Skull");
			Tooltip.SetDefault("2% increased minion damage and melee critical strike chance \nImbues last much longer");
			
		}
		

		public override void SetDefaults()
		{

            item.value = Item.sellPrice(gold: 1);
            item.rare = 1;


            item.width = 22;
			item.height = 14;
			item.defense = 5;
            


        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.JungleSpores, 4);
            recipe.AddIngredient(ItemID.Bone, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void UpdateEquip(Player player)
		{
            player.minionDamage += .02f;
            player.meleeCrit += 2;
            player.GetModPlayer<ShamanHeadEffects>().ImbueBoost = true;
        }
		public override void DrawHair(ref bool  drawHair, ref bool  drawAltHair )
		{
            drawAltHair = true;
			
		}
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("ShamanBody") && legs.type == mod.ItemType("ShamanLegs");
        }
        public override void UpdateArmorSet(Player player)
        {
            String s = Language.GetTextValue("Mods.QwertysRandomContent.BindKey");
            foreach (String key in QwertysRandomContent.YetAnotherSpecialAbility.GetAssignedKeys()) //get's the string of the hotkey's name
            {
                s = Language.GetTextValue("Mods.QwertysRandomContent.ShamanSet1") + key + Language.GetTextValue("Mods.QwertysRandomContent.ShamanSet2");
            }
            player.setBonus = s;
            player.GetModPlayer<ShamanHeadEffects>().setBonus = true;
        }






    }
    public class ShamanHeadEffects : ModPlayer
    {

        public bool ImbueBoost = false;
        bool freezeImbueTime = false;
        public bool setBonus = false;
        public int hasteTime = 0;
        public override void ResetEffects()
        {
            ImbueBoost = false;
            setBonus = false;
            //hasteTime = 0;
        }
        public override void PreUpdate()
        {
            if(ImbueBoost)
            {
                if (freezeImbueTime)
                {
                    for (int i = 0; i < 22; i++)
                    {
                        if (player.buffTime[i] >= 1)
                        {
                            if(Main.meleeBuff[player.buffType[i]])
                            {
                                player.buffTime[i]++;
                                break;
                            }
                        }
                    }
                    freezeImbueTime = false;
                }
                else
                {
                    freezeImbueTime = true;
                }
            }
            
        }
        public override void UpdateEquips(ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff)
        {
            if (hasteTime > 0)
            {
                player.meleeSpeed += .4f;
                hasteTime--;
                for(int i =0; i<2; i++)
                {
                    Dust.NewDustPerfect(player.Center + QwertyMethods.PolarVector(player.Size.Length(), Main.rand.NextFloat(-1, 1) * (float)Math.PI), mod.DustType("SpiritDust"), new Vector2(0, -6));
                }
                
               
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet) //runs hotkey effects
        {
            if (QwertysRandomContent.YetAnotherSpecialAbility.JustPressed) //hotkey is pressed
            {

                if (setBonus && !player.HasBuff(mod.BuffType("SpiritCallCooldown")))
                {
                    hasteTime = 600;
                    player.AddBuff(mod.BuffType("SpiritCallCooldown"), 60 * 60);
                    for (int p = 0; p < 1000; p++)
                    {
                        Projectile proj = Main.projectile[p];
                        if ((proj.minion || proj.sentry) && proj.owner == player.whoAmI && proj.GetGlobalProjectile<HastedProjectiles>().haste ==0)
                        {
                            proj.extraUpdates++;
                            proj.GetGlobalProjectile<HastedProjectiles>().haste = 600 * (1+ proj.extraUpdates);
                        }
                    }
                }
            }
        }

        public static readonly PlayerLayer Mask = new PlayerLayer("QwertysRandomContent", "Mask", PlayerLayer.Head, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>(mod);
            if (drawPlayer.head == mod.GetEquipSlot("ShamanHead", EquipType.Head))
            {
                

                int fHeight = 56;

                Texture2D texture = mod.GetTexture("Items/Armor/Shaman/ShamanHead_HeadSimple");
                Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
                int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
                int drawY = (int)(drawPlayer.position.Y - Main.screenPosition.Y);
                Vector2 Position = drawInfo.position;



                Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                if (drawPlayer.bodyFrame.Y == 7 * fHeight || drawPlayer.bodyFrame.Y == 8 * fHeight || drawPlayer.bodyFrame.Y == 9 * fHeight || drawPlayer.bodyFrame.Y == 14 * fHeight || drawPlayer.bodyFrame.Y == 15 * fHeight || drawPlayer.bodyFrame.Y == 16 * fHeight)
                {
                    if (drawPlayer.gravDir == -1)
                    {
                        pos.Y += 2;
                    }
                    else
                    {
                        pos.Y -= 2;
                    }

                }
                Rectangle frame = new Rectangle(0, 0, 40, fHeight);
                Vector2 origin = new Vector2((float)frame.Width * 0.5f, (float)frame.Height * 0.5f);
                //pos.Y -= drawPlayer.mount.PlayerOffset;
                DrawData data = new DrawData(texture, pos, frame, color12, 0f, origin, 1f, drawInfo.spriteEffects, 0);
                data.shader = drawInfo.headArmorShader;
                Main.playerDrawData.Add(data);
            }
        });
        public static readonly PlayerLayer WarpaintHead = new PlayerLayer("QwertysRandomContent", "WarpaintHead", PlayerLayer.Face, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>(mod);
            if (drawPlayer.head == mod.GetEquipSlot("ShamanHead", EquipType.Head))
            {


                int fHeight = 56;

                Texture2D texture = mod.GetTexture("Items/Armor/Shaman/Warpaint_Head");
                Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
                int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
                int drawY = (int)(drawPlayer.position.Y - Main.screenPosition.Y);
                Vector2 Position = drawInfo.position;



                Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                if (drawPlayer.bodyFrame.Y == 7 * fHeight || drawPlayer.bodyFrame.Y == 8 * fHeight || drawPlayer.bodyFrame.Y == 9 * fHeight || drawPlayer.bodyFrame.Y == 14 * fHeight || drawPlayer.bodyFrame.Y == 15 * fHeight || drawPlayer.bodyFrame.Y == 16 * fHeight)
                {
                    if (drawPlayer.gravDir == -1)
                    {
                        pos.Y += 2;
                    }
                    else
                    {
                        pos.Y -= 2;
                    }

                }
                Rectangle frame = new Rectangle(0, 0, 40, fHeight);
                Vector2 origin = new Vector2((float)frame.Width * 0.5f, (float)frame.Height * 0.5f);
                //pos.Y -= drawPlayer.mount.PlayerOffset;
                DrawData data = new DrawData(texture, pos, frame, color12, 0f, origin, 1f, drawInfo.spriteEffects, 0);
                data.shader = (int)drawPlayer.dye[3].dye;
                Main.playerDrawData.Add(data);
            }
        });

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int headLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Head"));
            if (headLayer != -1)
            {
                Mask.visible = true;
                layers.Insert(headLayer + 1, Mask);
            }
            headLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Face"));
            if (headLayer != -1)
            {
                WarpaintHead.visible = true;
                layers.Insert(headLayer + 1, WarpaintHead);
            }


        }
    }
    public class HastedProjectiles : GlobalProjectile
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public int haste = 0;
        public override void AI(Projectile projectile)
        {
            if(haste>0)
            {
                haste--;
                if (haste == 1 && projectile.extraUpdates>0)
                {
                    projectile.extraUpdates--;
                    
                }
                Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(projectile.Size.Length(), Main.rand.NextFloat(-1, 1) * (float)Math.PI), mod.DustType("SpiritDust"), new Vector2(0, -6));
            }
            
        }
    }
    
}

