using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.ShapeShifter
{
    public class ThornMass : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shape Shift: Thorn Mass");
            Tooltip.SetDefault("Turn into a mass of thorns that massively damages what runs into it.");
        }
        public override void SetDefaults()
        {
            item.damage = 30;
            item.crit = 0;
            item.knockBack = .3f;
            item.GetGlobalItem<ShapeShifterItem>().morph = true;
            item.GetGlobalItem<ShapeShifterItem>().morphDef = -1;
            item.GetGlobalItem<ShapeShifterItem>().morphType = ShapeShifterItem.QuickShiftType;
            item.GetGlobalItem<ShapeShifterItem>().morphCooldown = 20;
            item.noMelee = true;

            item.useTime = 60;
            item.useAnimation = 60;
            item.useStyle = 5;

            item.value = Item.sellPrice(silver: 54);
            item.rare = 1;

            item.noUseGraphic = true;
            item.width = 76;
            item.height = 76;

            //item.autoReuse = true;
            item.shoot = mod.ProjectileType("ThornMassShift");
            item.shootSpeed = 0f;
            item.channel = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            speedX = player.velocity.X;
            speedY = player.velocity.Y;
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.JungleSpores, 6);
            recipe.AddIngredient(ItemID.Stinger, 9);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
    public class ThornMassShift : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Thorn Mass");
            Main.projFrames[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.width = 76;
            projectile.height = 76;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 600;
            projectile.extraUpdates = 1;
        }

        private float dustYoffset;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            player.Center = projectile.Center;
            player.immune = true;
            player.immuneTime = 120;
            player.statDefense = 0;
            player.itemAnimation = 2;
            player.itemTime = 2;
            player.GetModPlayer<ShapeShifterPlayer>().noDraw = true;

            projectile.rotation += projectile.velocity.X * .01f;
            
            projectile.velocity.Y += .4f;
            if (projectile.velocity.Y > 10)
            {
                projectile.velocity.Y = 10;
            }
        }

        public override void Kill(int timeLeft)
        {
            List<Projectile> thorns = QwertyMethods.ProjectileSpread(projectile.Center, 16, 1f, mod.ProjectileType("Thorn"), projectile.damage, projectile.knockBack, projectile.owner);
            foreach(Projectile thorn in thorns)
            {
                thorn.velocity *= Main.rand.NextFloat(4f, 8f);
            }
        }
    }
    public class Thorn : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Thorn");
        }
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.HornetStinger);
            aiType = ProjectileID.HornetStinger;
            projectile.minion = false;
            projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
            projectile.timeLeft = 60;
        }
    }
}
