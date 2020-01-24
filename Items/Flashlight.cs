using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items
{
    public class Flashlight : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flashlight");
            Tooltip.SetDefault("Lights up a large area when used");

        }
        public override void SetDefaults()
        {


            item.useTime = 2;
            item.useAnimation = 2;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = 10000;
            item.rare = 1;


            item.width = 34;
            item.height = 12;

            item.shoot = 1;
            item.shootSpeed = 1;
            item.noMelee = true;
            item.autoReuse = true;


        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float spread = (float)Math.PI / 8;
            int spreadPrecision = 30;
            Vector2 direction = new Vector2(speedX, speedY);
            direction = direction.RotatedBy(spread / 2f);
            for (int b = 0; b < spreadPrecision; b++)
            {
                direction = direction.RotatedBy(-spread / spreadPrecision);
                for (int l = 0; l < 1000; l++)
                {
                    if (Collision.CanHit(position, 0, 0, position + (l * direction), 0, 0))
                    {
                        Lighting.AddLight(position + (l * direction), .4f, .4f, .4f);
                    }

                }

            }
            return false;
        }
        public override void AutoLightSelect(ref bool dryTorch, ref bool wetTorch, ref bool glowstick)
        {
            glowstick = true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }







    }


}

