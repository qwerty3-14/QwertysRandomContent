using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.NPCs.Fortress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Etims
{
    public class EtimsSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Massacre");
        }
        public override void SetDefaults()
        {
            item.useStyle = 101; //custom use style
            item.autoReuse = true;
            item.melee = true;
            item.noMelee = true;
            item.damage = 39;
            item.knockBack = 7f;
            item.width = 36;
            item.height = 34;
            item.noUseGraphic = true;
            item.useTime = 18;
            item.useAnimation = 18;
            item.rare = 3;
            item.value = 120000;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("EtimsMaterial"), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override bool CanUseItem(Player player)
        {

            return player.itemAnimation == 0;
        }
       

    }
    public class AltSword : ModPlayer
    {
        int[] localNPCImmunity = new int[Main.npc.Length];
        public override void PostItemCheck()
        {

            if (!player.inventory[player.selectedItem].IsAir)
            {
                Item item = player.inventory[player.selectedItem];

                if (item.useStyle == 101)
                {
                    float shift = 0f;
                    if (player.itemAnimation < player.itemAnimationMax * .25f)
                    {
                        shift = (float)Math.PI / -4 * ((player.itemAnimation) / (player.itemAnimationMax * .25f));
                    }
                    else if (player.itemAnimation < player.itemAnimationMax * .75f)
                    {
                        shift = (float)Math.PI / -2 * (1 - (player.itemAnimation - (player.itemAnimationMax * .25f)) / (player.itemAnimationMax * .5f)) + (float)Math.PI / 4;
                    }
                    else
                    {
                        shift = (float)Math.PI / 4 * (1 - (player.itemAnimation - (player.itemAnimationMax * .75f)) / (player.itemAnimationMax * .25f));
                    }
                    player.itemRotation = (float)Math.PI / -4 + player.direction * ((float)Math.PI / 2 + shift);
                    //Main.NewText(MathHelper.ToDegrees(player.itemRotation));
                    if (player.itemAnimation < player.itemAnimationMax * .15f)
                    {
                        player.bodyFrame.Y = player.bodyFrame.Height * 3;

                    }
                    else if (player.itemAnimation < player.itemAnimationMax * .35f)
                    {
                        player.bodyFrame.Y = player.bodyFrame.Height * 2;
                    }
                    else if (player.itemAnimation < player.itemAnimationMax * .65f)
                    {
                        player.bodyFrame.Y = player.bodyFrame.Height * 3;
                    }
                    else if (player.itemAnimation < player.itemAnimationMax * .85f)
                    {
                        player.bodyFrame.Y = player.bodyFrame.Height * 4;
                    }
                    else
                    {
                        player.bodyFrame.Y = player.bodyFrame.Height * 3;
                    }
                    Vector2 vector24 = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
                    if (player.direction != 1)
                    {
                        vector24.X = (float)player.bodyFrame.Width - vector24.X;
                    }
                    if (player.gravDir != 1f)
                    {
                        vector24.Y = (float)player.bodyFrame.Height - vector24.Y;
                    }
                    vector24 -= new Vector2((float)(player.bodyFrame.Width - player.width), (float)(player.bodyFrame.Height - 42)) / 2f;
                    player.itemLocation = player.position + vector24;
                   
                    float swordLength = new Vector2(Main.itemTexture[item.type].Width, Main.itemTexture[item.type].Height).Length();
                    swordLength *= item.scale;
                    for (int n = 0; n < Main.npc.Length; n++)
                    {
                        localNPCImmunity[n]--;
                        if (!Main.npc[n].dontTakeDamage && (!Main.npc[n].friendly || (Main.npc[n].type == 22 && player.killGuide) || (Main.npc[n].type == 54 && player.killClothier)) && player.itemAnimation > 0 && localNPCImmunity[n] <= 0 && Collision.CheckAABBvLineCollision(Main.npc[n].position, Main.npc[n].Size, player.itemLocation, player.itemLocation + QwertyMethods.PolarVector(swordLength, player.itemRotation - (float)Math.PI / 4)))
                        {
                            localNPCImmunity[n] = item.useAnimation/3;
                            int damageBeforeVariance = item.damage;
                            if (item.melee)
                            {
                                damageBeforeVariance = (int)((float)item.damage * player.meleeDamage);
                            }
                            if (item.ranged)
                            {
                                damageBeforeVariance = (int)((float)item.damage * player.rangedDamage);
                            }
                            if (item.magic)
                            {
                                damageBeforeVariance = (int)((float)item.damage * player.magicDamage);
                            }
                            if (item.summon)
                            {
                                damageBeforeVariance = (int)((float)item.damage * player.minionDamage);
                            }
                            if (item.thrown)
                            {
                                damageBeforeVariance = (int)((float)item.damage * player.thrownDamage);
                            }
                            float num314 = item.knockBack;
                            float num315 = 1f;
                            if (player.kbGlove)
                            {
                                num315 += 1f;
                            }
                            if (player.kbBuff)
                            {
                                num315 += 0.5f;
                            }
                            num314 *= num315;
                            bool crit = false;
                            if (item.melee && Main.rand.Next(1, 101) <= player.meleeCrit)
                            {
                                crit = true;
                            }
                            if (item.ranged && Main.rand.Next(1, 101) <= player.rangedCrit)
                            {
                                crit = true;
                            }
                            if (item.magic && Main.rand.Next(1, 101) <= player.magicCrit)
                            {
                                crit = true;
                            }
                            if (item.thrown && Main.rand.Next(1, 101) <= player.thrownCrit)
                            {
                                crit = true;
                            }
                            int num324 = Item.NPCtoBanner(Main.npc[n].BannerID());
                            if (num324 > 0 && player.NPCBannerBuff[num324])
                            {
                                if (Main.expertMode)
                                {
                                    damageBeforeVariance = (int)((float)damageBeforeVariance * ItemID.Sets.BannerStrength[Item.BannerToItem(num324)].ExpertDamageDealt);
                                }
                                else
                                {
                                    damageBeforeVariance = (int)((float)damageBeforeVariance * ItemID.Sets.BannerStrength[Item.BannerToItem(num324)].NormalDamageDealt);
                                }
                            }
                            if (player.parryDamageBuff && item.melee)
                            {
                                damageBeforeVariance *= 5;
                                player.parryDamageBuff = false;
                                player.ClearBuff(198);
                            }
                            //////////////////////
                            if(Main.npc[n].GetGlobalNPC<FortressNPCGeneral>().fortressNPC)
                            {
                                for (int i = 0; i < damageBeforeVariance / 3; i++)
                                {
                                    Dust d = Dust.NewDustPerfect(Main.npc[n].Center, mod.DustType("BloodforceDust"));
                                    d.velocity *= 5f;
                                }
                                damageBeforeVariance *= 2;
                            }
                            int damageAfterVariation = Main.DamageVar((float)damageBeforeVariance);
                            player.StatusNPC(item.type, n);
                            player.OnHit(Main.npc[n].Center.X, Main.npc[n].Center.Y, Main.npc[n]);
                            if (player.armorPenetration > 0)
                            {
                                damageAfterVariation += Main.npc[n].checkArmorPenetration(player.armorPenetration);
                            }
                            int num326 = (int)Main.npc[n].StrikeNPC(damageAfterVariation, num314, player.direction, crit, false, false);
                            bool flag28 = !Main.npc[n].immortal;
                            if (player.beetleOffense && flag28)
                            {
                                player.beetleCounter += (float)num326;
                                player.beetleCountdown = 0;
                            }
                            
                            if (player.meleeEnchant == 7)
                            {
                                Projectile.NewProjectile(Main.npc[n].Center.X, Main.npc[n].Center.Y, Main.npc[n].velocity.X, Main.npc[n].velocity.Y, 289, 0, 0f, player.whoAmI, 0f, 0f);
                            }
                            if (player.inventory[player.selectedItem].type == 3106)
                            {
                                player.stealth = 1f;
                                if (Main.netMode == 1)
                                {
                                    NetMessage.SendData(84, -1, -1, null, player.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                                }
                            }
                            
                            if (Main.npc[n].value > 0f && player.coins && Main.rand.Next(5) == 0)
                            {
                                int type11 = 71;
                                if (Main.rand.Next(10) == 0)
                                {
                                    type11 = 72;
                                }
                                if (Main.rand.Next(100) == 0)
                                {
                                    type11 = 73;
                                }
                                int num331 = Item.NewItem((int)Main.npc[n].position.X, (int)Main.npc[n].position.Y, Main.npc[n].width, Main.npc[n].height, type11, 1, false, 0, false, false);
                                Main.item[num331].stack = Main.rand.Next(1, 11);
                                Main.item[num331].velocity.Y = (float)Main.rand.Next(-20, 1) * 0.2f;
                                Main.item[num331].velocity.X = (float)Main.rand.Next(10, 31) * 0.2f * (float)player.direction;
                                if (Main.netMode == 1)
                                {
                                    NetMessage.SendData(21, -1, -1, null, num331, 0f, 0f, 0f, 0, 0, 0);
                                }
                            }
                            int num332 = Item.NPCtoBanner(Main.npc[n].BannerID());
                            if (num332 >= 0)
                            {
                                player.lastCreatureHit = num332;
                            }
                            if (Main.netMode != 0)
                            {
                                if (crit)
                                {
                                    NetMessage.SendData(28, -1, -1, null, n, (float)damageAfterVariation, num314, (float)player.direction, 1, 0, 0);
                                }
                                else
                                {
                                    NetMessage.SendData(28, -1, -1, null, n, (float)damageAfterVariation, num314, (float)player.direction, 0, 0, 0);
                                }
                            }
                            if (player.accDreamCatcher)
                            {
                                player.addDPS(damageAfterVariation);
                            }
                        }
                    }

                }


            }
        }
        public static readonly PlayerLayer AltSwordVisual = new PlayerLayer("QwertysRandomContent", "AltSwordVisual", PlayerLayer.HeldItem, delegate (PlayerDrawInfo drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            if (!drawPlayer.HeldItem.IsAir && drawPlayer.HeldItem.type == mod.ItemType("EtimsSword") && drawPlayer.itemAnimation>0)
            {
                Item item = drawPlayer.HeldItem;
                Texture2D texture = Main.itemTexture[item.type];
                Vector2 origin = new Vector2(0, texture.Height);
                DrawData value = new DrawData(texture, drawPlayer.itemLocation - Main.screenPosition, texture.Frame(), color12, drawPlayer.itemRotation, origin, item.scale, SpriteEffects.None, 0);
                Main.playerDrawData.Add(value);
            }
            
        });
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int itemLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("HeldItem"));
            if (itemLayer != -1)
            {
                AltSwordVisual.visible = true;
                layers.Insert(itemLayer + 1, AltSwordVisual);
            }

        }
    }
}
