using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;
using Terraria.Localization;

namespace QwertysRandomContent.Items.Armor.Lune
{
    [AutoloadEquip(EquipType.Body)]
    public class LuneCrestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lune Crestplate");
            Tooltip.SetDefault("Ranged attacks pierce an extra enemy and use local immunity");

        }


        public override void SetDefaults()
        {

            item.value = 30000;
            item.rare = 1;


            item.width = 22;
            item.height = 12;
            item.defense = 6;



        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(mod.ItemType("LuneBar"), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void UpdateEquip(Player player)
        {
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].ranged && Main.projectile[i].owner == player.whoAmI && !Main.projectile[i].GetGlobalProjectile<LunePierce>().lunePierce)
                {
                    Main.projectile[i].GetGlobalProjectile<LunePierce>().lunePierce = true;
                }
            }
            


        }
        public override bool IsVanitySet(int head, int body, int legs)
        {
            return base.IsVanitySet(head, body, legs);
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
           
            return head.type == mod.ItemType("LuneHat") && legs.type == mod.ItemType("LuneLeggings");
            
        }

        public override void ArmorSetShadows(Player player)
        {
            //Main.NewText("active set effect");
            if (!Main.dayTime)
            {

                float radius = Main.rand.NextFloat(200);
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dust = Dust.NewDustPerfect(player.Center + QwertyMethods.PolarVector(radius, theta), mod.DustType("LuneDust"));
                dust.shader = GameShaders.Armor.GetSecondaryShader(player.ArmorSetDye(), player);
            }
            player.armorEffectDrawShadow = true;
        }

        public override void UpdateArmorSet(Player player)
        {

            player.setBonus = Language.GetTextValue("Mods.QwertysRandomContent.LuneCrestSet");
            player.GetModPlayer<crestSet>().setBonus = true;
            //Main.NewText(player.ArmorSetDye());




        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            String s = "Please go to conrols and bind the 'Yet another special ability key'";
            foreach (String key in QwertysRandomContent.YetAnotherSpecialAbility.GetAssignedKeys()) //get's the string of the hotkey's name
            {
                s = "Set Bonus: Shoot the moon!" + "\nPress the " + key +" key to summon a moon"+ "\nRanged attacks shot through the moon will be boosted";

            }
            foreach (TooltipLine line in tooltips) //runs through all tooltip lines
            {
                if (line.mod == "Terraria" && line.Name == "SetBonus") //this checks if it's the line we're interested in
                {
                    line.text = s;//change tooltip
                }

            }
        }
        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawArms = true;
            drawHands = true;

        }






    }
    public class LunePierce : GlobalProjectile
    {
        public bool lunePierce;
        public bool runOnce = true;
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public override void AI(Projectile projectile)
        {
            

            if (lunePierce && projectile.ranged)
            {
                projectile.usesLocalNPCImmunity = true;
                if (runOnce)
                {
                    if (projectile.penetrate > 0)
                    {
                        projectile.penetrate += 1;
                        projectile.localNPCHitCooldown = 30;
                    }

                    runOnce = false;
                }







            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            if (lunePierce && projectile.ranged)
            {
                projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
                target.immune[projectile.owner] = 0;


            }
        }



    }
    public class crestSet : ModPlayer
    {
        public bool setBonus=false;
        public override void ResetEffects()
        {
            setBonus = false;
        }
       
        public bool justSummonedMoon;
        public override void ProcessTriggers(TriggersSet triggersSet) //runs hotkey effects
        {
            justSummonedMoon = false;
            if (QwertysRandomContent.YetAnotherSpecialAbility.JustPressed) //hotkey is pressed
            {
                if (setBonus)
                {
                    if (player.HasBuff(mod.BuffType("MoonCooldown")))
                    {

                    }
                    else
                    {
                        Projectile.NewProjectile(Main.MouseWorld, new Vector2(0, 0), mod.ProjectileType("MoonTarget"), 0, 0, player.whoAmI, 0, 0);
                        player.AddBuff(mod.BuffType("MoonCooldown"), 3 * 60);
                        justSummonedMoon = true;
                    }
                }
            }
        }
        


    }
    public class MoonTarget : ModProjectile
    {
        
        public override void SetDefaults()
        {
            projectile.width = 100;
            projectile.height = 100;
            projectile.aiStyle = -1;
            projectile.timeLeft = 60 * 15;
            projectile.light = 1;
            projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.

        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {

            // Add this projectile to the list of projectiles that will be drawn BEFORE tiles and NPC are drawn. This makes the projectile appear to be BEHIND  NPC.
            drawCacheProjsBehindNPCsAndTiles.Add(index);
            drawCacheProjsBehindProjectiles.Add(index);


        }
        int timer;


        int shader;
        bool runOnce = true;
        public override void AI()
        {
            
            Player player = Main.player[projectile.owner];
            if (runOnce)
            {
                shader = player.ArmorSetDye();
                //Main.NewText(shader);
                runOnce = false;
            }
            //Main.NewText(projectile.timeLeft + ", " + player.ArmorSetDye());
            //projectile.rotation += (float)Math.PI / 30;
            timer++;
            if((timer>10 && player.GetModPlayer<crestSet>().justSummonedMoon) ||!player.GetModPlayer<crestSet>().setBonus)
            {
                projectile.Kill();
            }
            Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("LuneDust"))];
           
            dust.shader = GameShaders.Armor.GetSecondaryShader(player.ArmorSetDye(), player);
            for (int i=0; i <200; i++)
            {
                if (Main.projectile[i].ranged && Main.projectile[i].owner == projectile.owner && Collision.CheckAABBvAABBCollision(Main.projectile[i].position, new Vector2(Main.projectile[i].width, Main.projectile[i].height), projectile.position, new Vector2(projectile.width, projectile.height)))
                {
                    Main.projectile[i].GetGlobalProjectile<moonBoost>().boosted = true;
                }
            }
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            // As mentioned above, be sure not to forget this step.
            Player player = Main.player[projectile.owner];
            if (shader != 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];

            if (shader != 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                GameShaders.Armor.GetSecondaryShader(shader, player).Apply(null);
            }
            return true;
        }
    }
    public class moonBoost : GlobalProjectile
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public bool boosted;
        bool runOnce = true;
        public override void AI(Projectile projectile)
        {
            if (boosted)
            {
                if (runOnce)
                {
                    //Main.NewText("Boost!");
                    projectile.damage = (int)(projectile.damage * 1.2f);
                    //projectile.velocity *= 2;
                    runOnce = false;
                }
                Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("LuneDust"))];
                Player player = Main.player[projectile.owner];
                dust.shader = GameShaders.Armor.GetSecondaryShader(player.ArmorSetDye(), player);
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            if(boosted)
            {
                target.AddBuff(mod.BuffType("LuneCurse"), 60*3);

            }
        }

    }

}

