using Microsoft.Xna.Framework;
using QwertysRandomContent.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Cactus
{
    public class Cactum : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cactum");
            Tooltip.SetDefault("Rapidly shoots high velocity cactus needles");
        }
        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 24;
            item.ranged = true;
            item.useAmmo =  mod.ItemType("CactusNeedle");
            item.shoot = mod.ProjectileType("CactusNeedleP");
            item.shootSpeed = 2f;
            item.useTime = 6;
            item.useAnimation = 6;
            item.damage = 1;
            
            item.useStyle = 5;
            item.knockBack = 0f;
            item.value = Item.sellPrice(silver: 3, copper: 60) ;
            item.rare = 1;
            item.UseSound = SoundID.Item11;
            
            item.noMelee = true;
            item.autoReuse = true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, 0);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Cactus, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
    
    public class CactusNeedle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cactus Needle");
            Tooltip.SetDefault("Sticks to enemies dealing stacking DOT");
        }
        public override void SetDefaults()
        {
            item.ammo = mod.ItemType("CactusNeedle");
            item.shoot = mod.ProjectileType("CactusNeedleP");
            item.damage = 1;
            item.ranged = true;
            item.maxStack = 999;
            item.consumable = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Cactus, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
    }
    public class CactusNeedleP : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.ranged = true;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.width = 2;
            projectile.height = projectile.width;
            projectile.extraUpdates = 14;
            projectile.friendly = true;
            projectile.GetGlobalProjectile<ImplaingProjectile>().CanImpale = true;
            projectile.GetGlobalProjectile<ImplaingProjectile>().damagePerImpaler = 1;
        }
        
        int npcIndex = -1;
        public override void AI()
        {
            if(projectile.ai[0] == 1)
            {
                projectile.extraUpdates = 0;
                projectile.ignoreWater = true; // Make sure the projectile ignores water
                projectile.tileCollide = false; // Make sure the projectile doesn't collide with tiles anymore
                int aiFactor = 15; // Change this factor to change the 'lifetime' of this sticking javelin
                bool killProj = false; // if true, kill projectile at the end
                bool hitEffect = false; // if true, perform a hit effect
                projectile.localAI[0] += 1f;
                // Every 30 ticks, the javelin will perform a hit effect
                hitEffect = projectile.localAI[0] % 30f == 0f;
                int projTargetIndex = (int)projectile.ai[1];
                if (projectile.localAI[0] >= (float)(60 * aiFactor)// If it's time for this javelin to die, kill it
                    || (projTargetIndex < 0 || projTargetIndex >= 200)) // If the index is past its limits, kill it
                {
                    killProj = true;
                }
                else if (Main.npc[projTargetIndex].active && !Main.npc[projTargetIndex].dontTakeDamage) // If the target is active and can take damage
                {
                    // Set the projectile's position relative to the target's center
                    projectile.Center = Main.npc[projTargetIndex].Center - projectile.velocity * 2f;
                    projectile.gfxOffY = Main.npc[projTargetIndex].gfxOffY;
                    if (hitEffect) // Perform a hit effect here
                    {
                        Main.npc[projTargetIndex].HitEffect(0, 1.0);
                    }
                }
                else // Otherwise, kill the projectile
                {
                    killProj = true;
                }

                if (killProj) // Kill the projectile
                {
                    projectile.Kill();
                }
            }
            else
            {
                projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2;
            }
            

        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit,
            ref int hitDirection)
        {

            projectile.ai[0] = 1;
            projectile.ai[1] = target.whoAmI; // Set the target whoAmI
            projectile.velocity =
                (target.Center - projectile.Center) *
                0.75f; // Change velocity based on delta center of targets (difference between entity centers)
            projectile.netUpdate = true; // netUpdate this javelin
            target.AddBuff(mod.BuffType("Impaled"), 900); // Adds the Impaled debuff
            
            projectile.damage = 0; // Makes sure the sticking javelins do not deal damage anymore
            projectile.friendly = false;
            // The following code handles the javelin sticking to the enemy hit.
            int maxStickingJavelins = 10; // This is the max. amount of javelins being able to attach
            Point[] stickingJavelins = new Point[maxStickingJavelins]; // The point array holding for sticking javelins
            int javelinIndex = 0; // The javelin index
            for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
            {
                Projectile currentProjectile = Main.projectile[i];
                if (i != projectile.whoAmI // Make sure the looped projectile is not the current javelin
                    && currentProjectile.active // Make sure the projectile is active
                    && currentProjectile.owner == Main.myPlayer // Make sure the projectile's owner is the client's player
                    && currentProjectile.type == projectile.type // Make sure the projectile is of the same type as this javelin
                    && currentProjectile.ai[0] == 1f // Make sure ai0 state is set to 1f (set earlier in ModifyHitNPC)
                    && currentProjectile.ai[1] == (float)target.whoAmI
                ) // Make sure ai1 is set to the target whoAmI (set earlier in ModifyHitNPC)
                {
                    stickingJavelins[javelinIndex++] =
                        new Point(i, currentProjectile.timeLeft); // Add the current projectile's index and timeleft to the point array
                    if (javelinIndex >= stickingJavelins.Length
                    ) // If the javelin's index is bigger than or equal to the point array's length, break
                    {
                        break;
                    }
                }
            }
            // Here we loop the other javelins if new javelin needs to take an older javelin's place.
            if (javelinIndex >= stickingJavelins.Length)
            {
                int oldJavelinIndex = 0;
                // Loop our point array
                for (int i = 1; i < stickingJavelins.Length; i++)
                {
                    // Remove the already existing javelin if it's timeLeft value (which is the Y value in our point array) is smaller than the new javelin's timeLeft
                    if (stickingJavelins[i].Y < stickingJavelins[oldJavelinIndex].Y)
                    {
                        oldJavelinIndex = i; // Remember the index of the removed javelin
                    }
                }
                // Remember that the X value in our point array was equal to the index of that javelin, so it's used here to kill it.
                Main.projectile[stickingJavelins[oldJavelinIndex].X].Kill();
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;


        }
    }
} 
