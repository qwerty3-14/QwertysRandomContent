using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Robes
{

    public class GypsyTweak : ModPlayer
    {


        public static readonly PlayerLayer GypsyBody = new PlayerLayer("QwertysRandomContent", "GypsyBody", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>();
            if (drawPlayer.body == 167)
            {

                if (drawPlayer.Male)
                {
                    Texture2D texture = mod.GetTexture("Items/Armor/Robes/GypsyBody");
                    DrawData value = new DrawData(texture, new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2)), new Microsoft.Xna.Framework.Rectangle?(drawPlayer.bodyFrame), color12, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                    value.shader = (int)drawPlayer.dye[2].dye;
                    Main.playerDrawData.Add(value);
                }
                Texture2D texture2 = mod.GetTexture("Items/Armor/Robes/GypsyJewels_Male");
                if (!drawPlayer.Male)
                {
                    texture2 = mod.GetTexture("Items/Armor/Robes/GypsyJewels_Female");
                }
                DrawData value2 = new DrawData(texture2, new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2)), new Microsoft.Xna.Framework.Rectangle?(drawPlayer.bodyFrame), color12, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                value2.shader = (int)drawPlayer.dye[3].dye;
                Main.playerDrawData.Add(value2);

            }
        });
        public static readonly PlayerLayer GypsyLegs = new PlayerLayer("QwertysRandomContent", "GypsyLegs", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");

            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>();
            if (drawPlayer.body == 167)
            {
                //Texture2D texture = mod.GetTexture("Items/Armor/Robes/Reagal_Legs_Gold");
                Texture2D texture = mod.GetTexture("Items/Armor/Robes/GypsyPants");
                if (!drawPlayer.Male)
                {
                    texture = mod.GetTexture("Items/Armor/Robes/GypsySkirt");
                }

                Vector2 Position = drawInfo.position;
                Position.Y += 14;
                Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                DrawData value = new DrawData(texture, pos, new Microsoft.Xna.Framework.Rectangle?(drawPlayer.legFrame), color12, drawPlayer.legRotation, drawInfo.legOrigin, 1f, drawInfo.spriteEffects, 0);
                value.shader = (int)drawPlayer.dye[2].dye;
                Main.playerDrawData.Add(value);

            }
        });
        public static readonly PlayerLayer GypsyArms = new PlayerLayer("QwertysRandomContent", "GypsyArms", PlayerLayer.Arms, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>();
            if (drawPlayer.body == 167)
            {
                Texture2D texture = mod.GetTexture("Items/Armor/Robes/GypsyArm");
                DrawData value = new DrawData(texture, new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2)), new Microsoft.Xna.Framework.Rectangle?(drawPlayer.bodyFrame), color12, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                value.shader = (int)drawPlayer.dye[3].dye;
                Main.playerDrawData.Add(value);

            }
        });


        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {


            int legLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Legs"));
            if (legLayer != -1)
            {
                GypsyLegs.visible = true;
                layers.Insert(legLayer + 1, GypsyLegs);
            }
            int armLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Arms"));
            if (armLayer != -1)
            {
                GypsyArms.visible = true;
                layers.Insert(armLayer + 1, GypsyArms);
            }
            int bodyLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Body"));
            if (bodyLayer != -1)
            {
                GypsyBody.visible = true;
                layers.Insert(bodyLayer + 1, GypsyBody);
            }
        }
    }

}
