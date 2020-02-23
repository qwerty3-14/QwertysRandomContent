using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace QwertysRandomContent.Items.Accesories
{
    public class H_Y_D_R_A : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("H.Y.D.R.A");
            Tooltip.SetDefault("35% increased thrown damage and velocity" + "\nYou have trouble throwing straight" + "\n [c/c20600:Highly addictive]");

        }
        public override void SetDefaults()
        {
            item.value = 200000;
            item.rare = 5;
            item.width = 16;
            item.height = 22;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BadAim>().intoxicated = true;
            player.GetModPlayer<Addiction>().hasHydra = true;
            player.thrownDamage += .35f ;
            player.thrownVelocity += .35f ;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("HydraScale"), 6);
            recipe.AddIngredient(mod.ItemType("QuestionableSubstance"));
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {

            foreach (TooltipLine line in tooltips) //runs through all tooltip lines
            {
                if (line.mod == "Terraria" && line.Name == "Tooltip0") //this checks if it's the line we're interested in
                {
                    line.text = 35 - (int)(((float)Main.player[item.owner].GetModPlayer<Addiction>().dependence / (float)Addiction.maxDependence)*25) + "% increased thrown damage and velocity";//change tooltip
                }

            }
        }
        public override bool CloneNewInstances => true;
    }
    public class Addiction : ModPlayer
    {
        public bool hasHydra = false;
        public int dependence = 0;
        public static int maxDependence = 24*60 * 60; //24 min
        public override void ResetEffects()
        {
            hasHydra = false;
        }
        public override void PostUpdateEquips()
        {
            player.thrownDamage -= ((float)dependence / Addiction.maxDependence) * .25f;
            player.thrownVelocity -= ((float)dependence / Addiction.maxDependence) * .25f;
            if (hasHydra)
            {
                player.buffImmune[mod.BuffType("Withdraw")]=true;
                if(dependence < maxDependence)
                {
                    dependence++;
                }
            }
            else
            {
                
                if(dependence > 0)
                {
                    dependence--;
                    if(dependence>300)
                    {
                        player.AddBuff(mod.BuffType("Withdraw"), dependence);
                    }
                    player.lifeRegen -= 2;
                    player.allDamage -= .1f;
                    player.statDefense -= 10;

                }
            }
        }
        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"addiction", dependence }
            };
        }
        public override void Load(TagCompound tag)
        {
            dependence = tag.GetInt("addiction");
        }
        float quesyCounter = 0f;
        float radius = 0f;
        public override void ModifyScreenPosition()
        {
            
            if(!hasHydra && dependence > 0)
            {
                quesyCounter += (float)Math.PI / 120;
                radius =300f*(float)Math.Sin(quesyCounter / 6f) * (float)dependence / Addiction.maxDependence ;
                Main.screenPosition += QwertyMethods.PolarVector(radius, quesyCounter);
            }
        }
    }
}

