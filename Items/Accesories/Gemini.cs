using Microsoft.Xna.Framework;
using QwertysRandomContent.Items.B4Items;
using QwertysRandomContent.Items.Fortress.CaeliteWeapons;
using System;
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

        public override void UpdateEquip(Player player)
        {
            
            player.GetModPlayer<GeminiEffect>(mod).effect = true;
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
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        bool runOnce = true;
        public override void AI(Projectile projectile)
        {
            
            if(Main.player[projectile.owner].GetModPlayer<GeminiEffect>(mod).effect && runOnce && projectile.arrow && projectile.type != mod.ProjectileType("HydraArrowP2") && projectile.type != mod.ProjectileType("AqueousP") && !(projectile.type == ProjectileID.DD2BetsyArrow && projectile.ai[1] == -1))
            {
                if(projectile.type == mod.ProjectileType("CobaltArrowP"))
                {
                    Projectile child = Main.projectile[Projectile.NewProjectile(projectile.Center, -projectile.velocity, projectile.type, (int)(projectile.damage * .3f), projectile.knockBack, projectile.owner, 0, 1)];
                    child.GetGlobalProjectile<GeminiProjectileEffect>(mod).runOnce = false;
                    child.GetGlobalProjectile<arrowHoming>(mod).B4HomingArrow = projectile.GetGlobalProjectile<arrowHoming>(mod).B4HomingArrow;
                    child.GetGlobalProjectile<ArrowWarping>(mod).warpedArrow = projectile.GetGlobalProjectile<ArrowWarping>(mod).warpedArrow;
                    child.GetGlobalProjectile<arrowgigantism>(mod).GiganticArrow = projectile.GetGlobalProjectile<arrowgigantism>(mod).GiganticArrow;
                }
                else
                {
                    Projectile child = Main.projectile[Projectile.NewProjectile(projectile.Center, -projectile.velocity, projectile.type, (int)(projectile.damage*.3f), projectile.knockBack, projectile.owner)];
                    child.GetGlobalProjectile<GeminiProjectileEffect>(mod).runOnce = false;
                    child.GetGlobalProjectile<arrowHoming>(mod).B4HomingArrow = projectile.GetGlobalProjectile<arrowHoming>(mod).B4HomingArrow;
                    child.GetGlobalProjectile<ArrowWarping>(mod).warpedArrow = projectile.GetGlobalProjectile<ArrowWarping>(mod).warpedArrow;
                    child.GetGlobalProjectile<arrowgigantism>(mod).GiganticArrow = projectile.GetGlobalProjectile<arrowgigantism>(mod).GiganticArrow;
                }
                runOnce = false;
            }
        }
    }
}

