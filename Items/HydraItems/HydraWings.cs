using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.HydraItems
{
    [AutoloadEquip(EquipType.Wings)]
    public class HydraWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fast wings with a low flight time" + "\nDouble tap to dash (5 dash power)");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 28;
            item.value = 250000;
            item.rare = 5;
            item.expert = true;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.wingTimeMax = 15;
            var modPlayer = player.GetModPlayer<QwertyPlayer>();
            if (modPlayer.customDashSpeed < 5f)
            {
                modPlayer.customDashSpeed = 5f;
            }
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.85f;
            ascentWhenRising = 0.15f;
            maxCanAscendMultiplier = 1f;
            maxAscentMultiplier = 3f; //max speed
            constantAscend = 1f; //acceleration
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 9f;
            acceleration *= 2.5f;
        }
    }

    public class HydraWingsGlowMask : ModPlayer
    {
        public static readonly PlayerLayer HydraWingsGlow = new PlayerLayer("QwertysRandomContent", "HydraWingsGlow", PlayerLayer.Wings, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>();
            if (drawPlayer.wings == mod.GetEquipSlot("HydraWings", EquipType.Wings))
            {
                //Main.NewText("Legs!");
                //Main.NewText(drawPlayer.bodyFrame);
                Texture2D texture = mod.GetTexture("Items/HydraItems/HydraWings_Wings_Glow");

                int num65 = 0;
                int num66 = 0;
                int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
                int drawY = (int)(drawPlayer.position.Y - Main.screenPosition.Y);
                Vector2 Position = drawInfo.position;
                Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 8);
                Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X + (float)(drawPlayer.width / 2) - (float)(9 * drawPlayer.direction)) + num66 * drawPlayer.direction), (float)((int)(Position.Y - Main.screenPosition.Y + (float)(drawPlayer.height / 2) + 2f * drawPlayer.gravDir + (float)num65 * drawPlayer.gravDir)));
                //pos.Y -= drawPlayer.mount.PlayerOffset;
                DrawData data = new DrawData(texture, pos, new Rectangle(0, texture.Height / 4 * drawPlayer.wingFrame, texture.Width, texture.Height / 4), Color.White, 0f, origin, 1f, drawInfo.spriteEffects, 0);
                data.shader = drawInfo.wingShader;
                Main.playerDrawData.Add(data);
            }
        });

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int wingLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Wings"));
            if (wingLayer != -1)
            {
                HydraWingsGlow.visible = true;
                layers.Insert(wingLayer + 1, HydraWingsGlow);
            }
        }
    }
}