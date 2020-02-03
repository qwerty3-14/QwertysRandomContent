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
            Tooltip.SetDefault("20% thrown damage and velocity" + "\nYou have trouble throwing straight" + "\nShould you really be taking this?");

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
            player.thrownDamage += .2f;
            player.thrownVelocity += .2f;
            player.GetModPlayer<BadAim>().intoxicated = true;
        }



    }
    public class BadAim : ModPlayer
    {
        public bool intoxicated;
        public override void ResetEffects()
        {
            intoxicated = false;

        }
    }
    public class BadThrowingAim : GlobalItem
    {

        public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.GetModPlayer<BadAim>().intoxicated && item.thrown)
            {
                float trueSpeed = new Vector2(speedX, speedY).Length();
                float direction = new Vector2(speedX, speedY).ToRotation() + Main.rand.NextFloat(-(float)Math.PI / 8, (float)Math.PI / 8);
                speedX = QwertyMethods.PolarVector(trueSpeed, direction).X;
                speedY = QwertyMethods.PolarVector(trueSpeed, direction).Y;
            }
            return true;
        }
    }

}

