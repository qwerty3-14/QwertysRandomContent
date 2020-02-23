using QwertysRandomContent.Items.B4Items;
using QwertysRandomContent.Items.Fortress.CaeliteWeapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{


    public class Gemini : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gemini's Quiver");
            Tooltip.SetDefault("When shooting a bow a second arrow will be shot in the opposite direction!");

        }

        public override void SetDefaults()
        {

            item.value = 200000;
            item.rare = 2;


            item.width = 34;
            item.height = 32;

            item.accessory = true;



        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {

            player.GetModPlayer<GeminiEffect>().effect = true;
        }



    }
    public class GeminiEffect : ModPlayer
    {
        public bool effect;
        public override void ResetEffects()
        {
            effect = false;

        }
    }

    public class GeminiProjectileEffect : GlobalProjectile
    {
       public override bool InstancePerEntity => true;
        bool runOnce = true;
        public override void AI(Projectile projectile)
        {

            if (Main.player[projectile.owner].GetModPlayer<GeminiEffect>().effect && runOnce && projectile.arrow && projectile.type != mod.ProjectileType("HydraArrowP2") && projectile.type != mod.ProjectileType("AqueousP") && projectile.type != mod.ProjectileType("BladedArrowP") && !(projectile.type == ProjectileID.DD2BetsyArrow && projectile.ai[1] == -1))
            {
                if (projectile.type == mod.ProjectileType("CobaltArrowP"))
                {
                    Projectile child = Main.projectile[Projectile.NewProjectile(projectile.Center, -projectile.velocity, projectile.type, (int)(projectile.damage * .3f), projectile.knockBack, projectile.owner, 0, 1)];
                    child.GetGlobalProjectile<GeminiProjectileEffect>().runOnce = false;
                    child.GetGlobalProjectile<arrowHoming>().B4HomingArrow = projectile.GetGlobalProjectile<arrowHoming>().B4HomingArrow;
                    child.GetGlobalProjectile<ArrowWarping>().warpedArrow = projectile.GetGlobalProjectile<ArrowWarping>().warpedArrow;
                    child.GetGlobalProjectile<arrowgigantism>().GiganticArrow = projectile.GetGlobalProjectile<arrowgigantism>().GiganticArrow;
                }
                else
                {
                    Projectile child = Main.projectile[Projectile.NewProjectile(projectile.Center, -projectile.velocity, projectile.type, (int)(projectile.damage * .3f), projectile.knockBack, projectile.owner)];
                    child.GetGlobalProjectile<GeminiProjectileEffect>().runOnce = false;
                    child.GetGlobalProjectile<arrowHoming>().B4HomingArrow = projectile.GetGlobalProjectile<arrowHoming>().B4HomingArrow;
                    child.GetGlobalProjectile<ArrowWarping>().warpedArrow = projectile.GetGlobalProjectile<ArrowWarping>().warpedArrow;
                    child.GetGlobalProjectile<arrowgigantism>().GiganticArrow = projectile.GetGlobalProjectile<arrowgigantism>().GiganticArrow;
                }
                runOnce = false;
            }
        }
    }
}

