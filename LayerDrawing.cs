using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace QwertysRandomContent
{
    
    public static class LayerDrawing
    {
        public static PlayerLayer DrawOnHead(string equip, string texturePath, string name = "HeadGlowmask", bool glowmask = true, int useShader = -1)
        {
            return new PlayerLayer("QwertysRandomContent", name, PlayerLayer.Head, delegate (PlayerDrawInfo drawInfo)
            {
                if (drawInfo.shadow != 0f)
                {
                    return;
                }
                Player drawPlayer = drawInfo.drawPlayer;
                Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
                if (glowmask)
                {
                    color12 = Color.White;
                }
                Mod mod = ModLoader.GetMod("QwertysRandomContent");
                if (drawPlayer.head == mod.GetEquipSlot(equip, EquipType.Head))
                {
                    Texture2D texture = mod.GetTexture(texturePath);
                    int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
                    int drawY = (int)(drawPlayer.position.Y - Main.screenPosition.Y);
                    Vector2 Position = drawInfo.position;
                    Vector2 origin = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.5f);
                    Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));

                    DrawData data = new DrawData(texture, pos, drawPlayer.bodyFrame, color12, 0f, origin, 1f, drawInfo.spriteEffects, 0);
                    if(useShader == -1)
                    {
                        data.shader = drawInfo.headArmorShader;
                    }
                    else
                    {
                        data.shader = drawPlayer.dye[useShader].dye;
                    }
                    
                    Main.playerDrawData.Add(data);
                }
            });
        }
        public static PlayerLayer DrawOnLegs(string equip, string texturePath, string femaleEquip = "n", string femaleTexturePath = "n",  string name = "LegGlowmask", bool glowmask = true, int useShader = -1)
        {
            return new PlayerLayer("QwertysRandomContent", name, PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
            {
                if (drawInfo.shadow != 0f)
                {
                    return;
                }
                Player drawPlayer = drawInfo.drawPlayer;
                Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
                if (glowmask)
                {
                    color12 = Color.White;
                }
                Mod mod = ModLoader.GetMod("QwertysRandomContent");
                if (drawPlayer.legs == mod.GetEquipSlot(equip, EquipType.Legs) || (femaleEquip != "n" && drawPlayer.legs == mod.GetEquipSlot(femaleEquip, EquipType.Legs)))
                {
                    Texture2D texture = mod.GetTexture(texturePath);
                    if (femaleTexturePath != "n" &&!drawPlayer.Male)
                    {
                        texture = mod.GetTexture(femaleTexturePath);
                    }
                    int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
                    int drawY = (int)(drawPlayer.position.Y - Main.screenPosition.Y);
                    Vector2 Position = drawInfo.position;
                    Vector2 origin = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.5f);
                    Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                    DrawData data = new DrawData(texture, pos, drawPlayer.legFrame, color12, 0f, origin, 1f, drawInfo.spriteEffects, 0);
                    if (useShader == -1)
                    {
                        data.shader = drawInfo.legArmorShader;
                    }
                    else
                    {
                        data.shader = drawPlayer.dye[useShader].dye;
                    }
                    Main.playerDrawData.Add(data);
                }
            });
        }
        public static PlayerLayer DrawOnBody(string equip, string texturePath, string femaleTexturePath = "n", string name = "BodyGlowmask", bool glowmask = true, int useShader = -1)
        {
            return new PlayerLayer("QwertysRandomContent", name, PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
            {
                if (drawInfo.shadow != 0f)
                {
                    return;
                }
                Player drawPlayer = drawInfo.drawPlayer;
                Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
                if (glowmask)
                {
                    color12 = Color.White;
                }
                Mod mod = ModLoader.GetMod("QwertysRandomContent");
                if (drawPlayer.body == mod.GetEquipSlot(equip, EquipType.Body))
                {
                    Texture2D texture = mod.GetTexture(texturePath);
                    if (femaleTexturePath != "n" && !drawPlayer.Male)
                    {
                        texture = mod.GetTexture(femaleTexturePath);
                    }


                    int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
                    int drawY = (int)(drawPlayer.position.Y - Main.screenPosition.Y);
                    Vector2 Position = drawInfo.position;
                    Vector2 origin = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.5f);
                    Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                    
                    DrawData data = new DrawData(texture, pos, drawPlayer.bodyFrame, color12, 0f, origin, 1f, drawInfo.spriteEffects, 0);
                    if (useShader == -1)
                    {
                        data.shader = drawInfo.bodyArmorShader;
                    }
                    else
                    {
                        data.shader = drawPlayer.dye[useShader].dye;
                    }
                    Main.playerDrawData.Add(data);
                }
            });
        }
        public static PlayerLayer DrawOnArms(string equip, string texturePath, string name = "ArmsGlowmask", bool glowmask = true, int useShader = -1)
        {
            return new PlayerLayer("QwertysRandomContent", name, PlayerLayer.Arms, delegate (PlayerDrawInfo drawInfo)
            {
                if (drawInfo.shadow != 0f)
                {
                    return;
                }
                Player drawPlayer = drawInfo.drawPlayer;
                Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
                if (glowmask)
                {
                    color12 = Color.White;
                }
                Mod mod = ModLoader.GetMod("QwertysRandomContent");
                if (drawPlayer.body == mod.GetEquipSlot(equip, EquipType.Body))
                {
                    //Main.NewText("Helmet!");
                    //Main.NewText(drawPlayer.bodyFrame);
                    Texture2D texture = mod.GetTexture(texturePath);



                    int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
                    int drawY = (int)(drawPlayer.position.Y - Main.screenPosition.Y);
                    Vector2 Position = drawInfo.position;
                    Vector2 origin = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.5f);
                    Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                    

                    DrawData data = new DrawData(texture, pos, drawPlayer.bodyFrame, color12, 0f, origin, 1f, drawInfo.spriteEffects, 0);
                    if (useShader == -1)
                    {
                        data.shader = drawInfo.bodyArmorShader;
                    }
                    else
                    {
                        data.shader = drawPlayer.dye[useShader].dye;
                    }
                    Main.playerDrawData.Add(data);

                }
            });
        }
        public static PlayerLayer DrawHeadSimple(string equip, string texturePath, string name = "Mask", bool glowmask = true, int useShader = -1)
        {
            return new PlayerLayer("QwertysRandomContent", name, PlayerLayer.Head, delegate (PlayerDrawInfo drawInfo)
            {
                if (drawInfo.shadow != 0f)
                {
                    return;
                }
                Player drawPlayer = drawInfo.drawPlayer;
                Mod mod = ModLoader.GetMod("QwertysRandomContent");
                if (drawPlayer.head == mod.GetEquipSlot(equip, EquipType.Head))
                {

                    int fHeight = 56;

                    Texture2D texture = mod.GetTexture(texturePath);

                    Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
                    if (glowmask)
                    {
                        color12 = Color.White;
                    }
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
                    DrawData data = new DrawData(texture, pos, frame, color12, 0f, origin, 1f, drawInfo.spriteEffects, 0);
                    if (useShader == -1)
                    {
                        data.shader = drawInfo.headArmorShader;
                    }
                    else
                    {
                        data.shader = drawPlayer.dye[useShader].dye;
                    }
                    Main.playerDrawData.Add(data);
                }
            });
        }
        public static PlayerLayer DrawOnFrontHand(string equip, string texturePath, string femaleTexturePath = "n", string name = "gloveF", bool glowmask = true, int useShader = -1, EquipType type = EquipType.Body)
        {
            return new PlayerLayer("QwertysRandomContent", name, PlayerLayer.HandOnAcc, delegate (PlayerDrawInfo drawInfo)
            {
                if (drawInfo.shadow != 0f)
                {
                    return;
                }
                Player drawPlayer = drawInfo.drawPlayer;
                Mod mod = ModLoader.GetMod("QwertysRandomContent");
                Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);


                if (drawPlayer.body == mod.GetEquipSlot(equip, type))
                {
                    Texture2D texture = mod.GetTexture(texturePath);
                    if (!drawPlayer.Male && femaleTexturePath != "n")
                    {
                        texture = mod.GetTexture(femaleTexturePath);
                    }
                    DrawData value = new DrawData(texture, new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2)), new Microsoft.Xna.Framework.Rectangle?(drawPlayer.bodyFrame), color12, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                    if (useShader == -1)
                    {
                        value.shader = drawInfo.bodyArmorShader;
                    }
                    else
                    {
                        value.shader = drawPlayer.dye[useShader].dye;
                    }
                    Main.playerDrawData.Add(value);

                }
            });
        }
        public static PlayerLayer DrawOnBackHand(string equip, string texturePath, string femaleTexturePath = "n", string name = "gloveB", bool glowmask = true, int useShader = -1, EquipType type = EquipType.Body)
        {
            return new PlayerLayer("QwertysRandomContent", name, PlayerLayer.HandOffAcc, delegate (PlayerDrawInfo drawInfo)
            {
                if (drawInfo.shadow != 0f)
                {
                    return;
                }
                Player drawPlayer = drawInfo.drawPlayer;
                Mod mod = ModLoader.GetMod("QwertysRandomContent");
                Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);


                if (drawPlayer.body == mod.GetEquipSlot(equip, type))
                {
                    Texture2D texture = mod.GetTexture(texturePath);
                    if (!drawPlayer.Male && femaleTexturePath != "n")
                    {
                        texture = mod.GetTexture(femaleTexturePath);
                    }
                    DrawData value = new DrawData(texture, new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2)), new Microsoft.Xna.Framework.Rectangle?(drawPlayer.bodyFrame), color12, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                    if (useShader == -1)
                    {
                        value.shader = drawInfo.bodyArmorShader;
                    }
                    else
                    {
                        value.shader = drawPlayer.dye[useShader].dye;
                    }
                    Main.playerDrawData.Add(value);

                }
            });
        }
        public static PlayerHeadLayer DrawHeadLayer(string equip, string texturePath, string name = "Mask", int useShader = 0)
        {
            return new PlayerHeadLayer("QwertysRandomContent", "Mask", delegate (PlayerHeadDrawInfo drawInfo)
            {

                Mod mod = ModLoader.GetMod("QwertysRandomContent");
                Player drawPlayer = drawInfo.drawPlayer;
                Vector2 vector = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.4f);
                bool drawIt = false;

                float mapScale;
                byte b = (byte)(255f * Main.mapOverlayAlpha);
                float X = 0;
                float Y = 0;
                float num = 0f;
                float num2 = 0f;
                float num3 = num;
                float num4 = num2;
                float num6 = 10f;
                float num7 = 10f;
                float num8 = (float)(Main.maxTilesX - Lighting.offScreenTiles - 1);
                float num9 = (float)(Main.maxTilesY - Lighting.offScreenTiles - 42);
                float num10 = 0f;
                float num11 = 0f;
                num8 = (float)(Main.maxTilesX - 10);
                num9 = (float)(Main.maxTilesY - 10);
                float num12 = 0f;
                float num13 = 0f;
                float num14 = num8 - 1f;
                float num15 = num9 - 1f;
                num = 200f;
                num2 = 300f;
                if (!Main.mapFullscreen && Main.mapStyle == 2)
                {
                    mapScale = Main.mapOverlayScale;
                    float num32 = (Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16f;
                    float num33 = (Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16f;
                    num32 *= mapScale;
                    num33 *= mapScale;
                    num = -num32 + (float)(Main.screenWidth / 2);
                    num2 = -num33 + (float)(Main.screenHeight / 2);
                    num += num6 * mapScale;
                    num2 += num7 * mapScale;


                    float num56 = (drawPlayer.position.X + (float)(drawPlayer.width / 2)) / 16f * mapScale;
                    float num57 = drawPlayer.position.Y / 16f * mapScale;
                    num56 += num;
                    num57 += num2;
                    num56 -= 6f;
                    num57 -= 2f;
                    num57 -= 2f - mapScale / 5f * 2f;
                    num56 -= 10f * mapScale;
                    num57 -= 10f * mapScale;
                    X = num56;
                    Y = num57;
                    drawIt = true;
                }
                if (!Main.mapFullscreen && Main.mapStyle == 1)
                {

                    num = (float)Main.miniMapX;
                    num2 = (float)Main.miniMapY;
                    num3 = num;
                    num4 = num2;
                    mapScale = Main.mapMinimapScale;
                    float num28 = (Main.screenPosition.X + (float)(PlayerInput.RealScreenWidth / 2)) / 16f;
                    float num29 = (Main.screenPosition.Y + (float)(PlayerInput.RealScreenHeight / 2)) / 16f;
                    num14 = (float)Main.miniMapWidth / mapScale;
                    num15 = (float)Main.miniMapHeight / mapScale;
                    num12 = (float)((int)num28) - num14 / 2f;
                    num13 = (float)((int)num29) - num15 / 2f;
                    float num78 = ((drawPlayer.position.X + (float)(drawPlayer.width / 2)) / 16f - num12) * mapScale;
                    float num79 = ((drawPlayer.position.Y + drawPlayer.gfxOffY + (float)(drawPlayer.height / 2)) / 16f - num13) * mapScale;
                    num78 += num3;
                    num79 += num4;
                    num78 -= 6f;
                    num79 -= 6f;
                    num79 -= 2f - mapScale / 5f * 2f;
                    num78 += num10;
                    num79 += num11;
                    if (Main.screenPosition.X != Main.leftWorld + 640f + 16f && Main.screenPosition.X + (float)Main.screenWidth != Main.rightWorld - 640f - 32f && Main.screenPosition.Y != Main.topWorld + 640f + 16f && Main.screenPosition.Y + (float)Main.screenHeight <= Main.bottomWorld - 640f - 32f && drawPlayer.whoAmI == Main.myPlayer && Main.zoomX == 0f && Main.zoomY == 0f)
                    {
                        num78 = num3 + (float)(Main.miniMapWidth / 2);
                        num79 = num4 + (float)(Main.miniMapHeight / 2);
                        num79 -= 3f;
                        num78 -= 4f;
                    }
                    if (!drawPlayer.dead && num78 > (float)(Main.miniMapX + 6) && num78 < (float)(Main.miniMapX + Main.miniMapWidth - 16) && num79 > (float)(Main.miniMapY + 6) && num79 < (float)(Main.miniMapY + Main.miniMapHeight - 14))
                    {
                        X = num78;
                        Y = num79;
                        drawIt = true;
                    }

                }
                if (Main.mapFullscreen)
                {
                    mapScale = Main.mapFullscreenScale;
                    float num20 = Main.mapFullscreenPos.X;
                    float num21 = Main.mapFullscreenPos.Y;
                    if (Main.resetMapFull)
                    {
                        num20 = (Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16f;
                        num21 = (Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16f;
                    }
                    num20 *= mapScale;
                    num21 *= mapScale;
                    num = -num20 + (float)(Main.screenWidth / 2);
                    num2 = -num21 + (float)(Main.screenHeight / 2);
                    num += num6 * mapScale;
                    num2 += num7 * mapScale;



                    float num137 = (((drawPlayer.position.X + drawPlayer.width / 2)) / 16f - num12) * mapScale;
                    float num138 = ((drawPlayer.position.Y + drawPlayer.gfxOffY + (float)(drawPlayer.height / 2)) / 16f - num13) * mapScale;
                    num137 += num;
                    num138 += num2;
                    num137 -= 6f;
                    num138 -= 2f;
                    num138 -= 2f - mapScale / 5f * 2f;
                    num137 -= 10f * mapScale;
                    num138 -= 10f * mapScale;


                    X = num137;
                    Y = num138;
                    drawIt = true;
                }
                Vector2 drawPos = Main.screenPosition;
                drawPos.X += X;
                drawPos.Y += Y;
                drawPos.X -= 6f;
                drawPos.Y -= 4f;
                float mountOff = (float)drawPlayer.mount.PlayerHeadOffset;
                drawPos.Y = drawPos.Y - mountOff;
                if (drawPlayer.head == mod.GetEquipSlot(equip, EquipType.Head) && drawIt)
                {


                    Texture2D texture = mod.GetTexture(texturePath);
                    DrawData data = new DrawData(texture,
                        new Vector2(drawPos.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2), drawPos.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f) + drawPlayer.headPosition + vector,
                        texture.Frame(), drawInfo.armorColor, 0f, drawInfo.drawOrigin, drawInfo.scale, drawInfo.spriteEffects, 0);
                    GameShaders.Armor.Apply(drawPlayer.dye[useShader].dye, drawPlayer, new DrawData?(data));
                    data.Draw(Main.spriteBatch);
                }


            });
        }
    }
}
