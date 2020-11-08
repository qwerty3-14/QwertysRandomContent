using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.DukeFishron
{
    public class Whirlpool : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Whirlpool");
            Tooltip.SetDefault("Uses darts as ammo\n50% chance not to consume ammo");
        }
        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.Next(2) == 0;
        }

        public override void SetDefaults()
        {
            item.damage = 75;
            item.ranged = true;
            item.knockBack = 7f;
            item.value = Item.sellPrice(gold: 5);
            item.rare = 8;
            item.width = 48;
            item.height = 30;
            item.useStyle = 5;
            item.shootSpeed = 15f;
            item.useTime = 15;
            item.useAnimation = 15;
            item.shoot = 10;
            item.useAmmo = AmmoID.Dart;
            item.noUseGraphic = false;
            item.noMelee = true;
            item.UseSound = SoundID.Item95;
            item.autoReuse = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float r = (new Vector2(speedX, speedY)).ToRotation();
            position += QwertyMethods.PolarVector(-12f, r) + QwertyMethods.PolarVector(-12f * player.direction, r + (float)Math.PI/2);
            Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
            int amt = Main.rand.Next(2) + 2;
            for(int i = 0; i < amt; i++)
            {
                Dust.NewDustPerfect(position + QwertyMethods.PolarVector(30f, r), 217, QwertyMethods.PolarVector(Main.rand.NextFloat() * 3f + 1f, r + Main.rand.NextFloat(-(float)Math.PI / 16, (float)Math.PI / 16)), 100);
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-16, -8);
        }
    }
}
