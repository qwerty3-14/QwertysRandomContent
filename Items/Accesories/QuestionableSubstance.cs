using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
    public class QuestionableSubstance : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Questionable Substance");
            Tooltip.SetDefault("14% increased non summon damage " + "\nYou have trouble aiming straight" + "\nShould you really be taking this?");
        }

        public override void SetDefaults()
        {
            item.value = 200000;
            item.rare = 2;

            item.width = 16;
            item.height = 22;

            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.allDamage += .14f;
            player.minionDamage -= .14f;
            player.GetModPlayer<BadAim>().intoxicated = true;
            player.GetModPlayer<BadAim>().intoxicatedRate /= 2;
        }
    }

    public class BadAim : ModPlayer
    {
        public bool intoxicated;
        public int intoxicatedRate = 128;

        public override void ResetEffects()
        {
            intoxicated = false;
            intoxicatedRate = 32;
        }

        private Vector2 oldMousePos;

        public override bool PreItemCheck()
        {
            if (intoxicated)
            {
                oldMousePos = new Vector2(Main.mouseX, Main.mouseY);
                float direction = (oldMousePos - (player.Center - Main.screenPosition)).ToRotation() + Main.rand.NextFloat(-(float)Math.PI / intoxicatedRate, (float)Math.PI / intoxicatedRate);
                float dist = (oldMousePos - (player.Center - Main.screenPosition)).Length();
                Vector2 newMousePos = (player.Center - Main.screenPosition) + QwertyMethods.PolarVector(dist, direction);
                Main.mouseX = (int)newMousePos.X;
                Main.mouseY = (int)newMousePos.Y;
            }
            return base.PreItemCheck();
        }

        public override void PostItemCheck()
        {
            if (intoxicated)
            {
                Main.mouseX = (int)oldMousePos.X;
                Main.mouseY = (int)oldMousePos.Y;
            }
            base.PostItemCheck();
        }
    }
}