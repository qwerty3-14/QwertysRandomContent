using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Robes
{
    public class HighHeelLegDraw : ModPlayer
    {
        public static readonly PlayerLayer HeelLegs = new PlayerLayer("QwertysRandomContent", "HeelLegs", PlayerLayer.Skin, delegate (PlayerDrawInfo drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            if ((drawPlayer.legs == mod.GetEquipSlot("DuelistPants_FemaleLegs", EquipType.Legs) && !drawPlayer.Male) || (drawPlayer.legs == mod.GetEquipSlot("ConduitRobes_Female", EquipType.Legs) && !drawPlayer.Male) || (drawPlayer.shoe == mod.GetEquipSlot("HighHeels", EquipType.Shoes)) || (drawPlayer.legs == mod.GetEquipSlot("TwistedDarkLegs_FemaleLegs", EquipType.Legs)))
            {
                Texture2D texture = mod.GetTexture("Items/Armor/Robes/HeelLegs");
                Vector2 Position = drawInfo.position;
                Position.Y += 14;
                Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                DrawData value = new DrawData(texture, pos, new Microsoft.Xna.Framework.Rectangle?(drawPlayer.legFrame), drawInfo.legColor, drawPlayer.legRotation, drawInfo.legOrigin, 1f, drawInfo.spriteEffects, 0);

                Main.playerDrawData.Add(value);
            }
        });

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int legLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Skin"));
            if (legLayer != -1)
            {
                HeelLegs.visible = true;
                layers.Insert(legLayer + 1, HeelLegs);
            }
        }
    }
}