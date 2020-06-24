using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.DevItems.Kerdo
{
    [AutoloadEquip(EquipType.Head)]
    public class PugMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pug Mask");
            Tooltip.SetDefault("Kerdo Loves Pugs \nDev Item");
        }

        public override void SetDefaults()
        {
            item.value = 0;
            item.rare = 10;

            item.vanity = true;
            item.width = 20;
            item.height = 20;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = false;
        }
    }

    public class AnimatedPug : ModPlayer
    {
        public static readonly PlayerLayer Pug = new PlayerLayer("QwertysRandomContent", "PugHead", PlayerLayer.Head, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>();
            if (drawPlayer.head == mod.GetEquipSlot("PugMask", EquipType.Head))
            {
                //Main.NewText("Pug!");
                //Main.NewText(drawPlayer.bodyFrame);
                int f = 3;
                if (drawPlayer.velocity.X == 0)
                {
                    f = 0;
                }
                if (drawPlayer.velocity.Y > 0)
                {
                    f = 1;
                }
                else if (drawPlayer.velocity.Y < 0)
                {
                    f = 0;
                }
                int fHeight = 56;

                Texture2D texture = mod.GetTexture("Items/DevItems/Kerdo/PugMask_AnimatedHead");
                Color color12 = drawInfo.upperArmorColor;
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
                    if (drawPlayer.velocity.Y == 0)
                    {
                        f = 0;
                    }
                }
                Rectangle frame = new Rectangle(0, f * fHeight, 40, fHeight);
                Vector2 origin = new Vector2((float)frame.Width * 0.5f, (float)frame.Height * 0.5f);
                //pos.Y -= drawPlayer.mount.PlayerOffset;
                DrawData data = new DrawData(texture, pos, frame, color12, 0f, origin, 1f, drawInfo.spriteEffects, 0);
                data.shader = drawInfo.headArmorShader;
                Main.playerDrawData.Add(data);
            }
        });

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int headLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Head"));
            if (headLayer != -1)
            {
                Pug.visible = true;
                layers.Insert(headLayer + 1, Pug);
            }
        }
    }
}