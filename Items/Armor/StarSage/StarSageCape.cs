using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.StarSage
{
    [AutoloadEquip(EquipType.Back)]

    public class StarSageCape : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("StarSageCape");
            Tooltip.SetDefault("");

        }

        public override void SetDefaults()
        {

            item.value = 1000;
            item.rare = 1;


            item.width = 32;
            item.height = 32;
            item.accessory = true;
            item.vanity = true;



        }


    }
    public class CapeDraw : ModPlayer
    {
        public static readonly PlayerLayer StarCape = new PlayerLayer("QwertysRandomContent", "StarCape", PlayerLayer.BackAcc, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>();
            //Main.NewText(drawPlayer.wings);
            if (drawPlayer.back == mod.GetEquipSlot("StarSageCape", EquipType.Back) && drawPlayer.wings == 0)
            {
                Texture2D texture = mod.GetTexture("Items/Armor/StarSage/StarSageCape_Cape");
                if (!drawPlayer.Male)
                {
                    //texture = mod.GetTexture("Items/Armor/Robes/StarSageCape_Female");
                }
                DrawData value = new DrawData(texture, new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2)), new Microsoft.Xna.Framework.Rectangle?(drawPlayer.legFrame), color12, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                value.shader = drawInfo.backShader;
                Main.playerDrawData.Add(value);

            }
        });
        public static readonly PlayerLayer StarCapeFront = new PlayerLayer("QwertysRandomContent", "StarCapeFront", PlayerLayer.NeckAcc, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>();
            //Main.NewText(drawPlayer.wings);
            if (drawPlayer.back == mod.GetEquipSlot("StarSageCape", EquipType.Back))
            {
                Texture2D texture = mod.GetTexture("Items/Armor/StarSage/StarSageCape_Front");
                if (!drawPlayer.Male)
                {
                    //texture = mod.GetTexture("Items/Armor/Robes/StarSageCape_Female");
                }
                DrawData value = new DrawData(texture,
                    new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2)),
                    new Microsoft.Xna.Framework.Rectangle?(drawPlayer.bodyFrame), color12, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                value.shader = drawInfo.backShader;
                Main.playerDrawData.Add(value);

            }
        });
        public static readonly PlayerLayer StarCapeShoulder = new PlayerLayer("QwertysRandomContent", "StarCapeFront", PlayerLayer.FrontAcc, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>();
            //Main.NewText(drawPlayer.wings);
            if (drawPlayer.back == mod.GetEquipSlot("StarSageCape", EquipType.Back))
            {
                Texture2D texture = mod.GetTexture("Items/Armor/StarSage/StarSageCape_Shoulders");
                if (!drawPlayer.Male)
                {
                    //texture = mod.GetTexture("Items/Armor/Robes/StarSageCape_Female");
                }
                DrawData value = new DrawData(texture,
                    new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2)),
                    new Microsoft.Xna.Framework.Rectangle?(drawPlayer.bodyFrame), color12, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                value.shader = drawInfo.backShader;
                Main.playerDrawData.Add(value);

            }
        });
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int shoulderLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("FrontAcc"));
            if (shoulderLayer != -1)
            {
                StarCapeShoulder.visible = true;
                layers.Insert(shoulderLayer + 1, StarCapeShoulder);
            }
            int neckLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("NeckAcc"));
            if (neckLayer != -1)
            {
                StarCapeFront.visible = true;
                layers.Insert(neckLayer + 1, StarCapeFront);
            }
            int capeLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("BackAcc"));
            if (capeLayer != -1)
            {
                StarCape.visible = true;
                layers.Insert(capeLayer + 1, StarCape);
            }
        }
    }
}

