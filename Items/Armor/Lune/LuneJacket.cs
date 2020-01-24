using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Lune
{
    [AutoloadEquip(EquipType.Body)]
    public class LuneJacket : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lune Jacket");
            Tooltip.SetDefault("Grappling hooks have double speed!" + "\nKilling an enemy with a ranged attack grants 10 sec of hunter");

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
            /*
            for (int i = 0; i < 200; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI)
                {
                    Main.projectile[i].GetGlobalProjectile<grappleBoost>().DoubleGrapple = true;
                }
            }
            */
            player.GetModPlayer<QwertyPlayer>().grappleBoost = true;
            player.GetModPlayer<JacketBonuses>().hunt = true;

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
            /*
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.Transform);
            for (int num10 = 0; num10 < 3; num10++)
            {

                Main.instance.DrawPlayer(player, player.shadowPos[num10], player.shadowRotation[num10], player.shadowOrigin[num10], 0.5f + 0.2f * (float)num10);

            }
            Main.spriteBatch.End();
            */

        }


        public override void UpdateArmorSet(Player player)
        {

            player.setBonus = Language.GetTextValue("Mods.QwertysRandomContent.LuneJacketSet");
            //player.GetModPlayer<crestSet>().setBonus = true;
            player.GetModPlayer<JacketBonuses>().setBonus = true;





        }
        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawArms = true;
            drawHands = true;

        }






    }
    public class GrappleBoost : GlobalProjectile
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public bool DoubleGrapple;
        bool runOnce = true;

        public override void GrapplePullSpeed(Projectile projectile, Player player, ref float speed)
        {
            if (player.GetModPlayer<QwertyPlayer>().grappleBoost)
            {
                speed *= 2;
                Dust dust = Main.dust[Dust.NewDust(player.position, player.width, player.height, mod.DustType("LuneDust"))];
                dust.shader = GameShaders.Armor.GetSecondaryShader(player.ArmorSetDye(), player);
            }
            //Main.NewText(speed);
        }
        /*
        public override void GrappleRetreatSpeed(Projectile projectile, Player player, ref float speed)
        {
            
            if (player.GetModPlayer<QwertyPlayer>().grappleBoost)
            {
                speed *= 2;

                
            }
        }
        */
        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            if (player.GetModPlayer<QwertyPlayer>().grappleBoost && projectile.aiStyle == 7 && runOnce)
            {
                projectile.extraUpdates += 1;
                runOnce = false;
                //Main.NewText("DOUBLE!!");
            }


        }



    }
    public class JacketBonuses : ModPlayer
    {
        public bool setBonus;
        public bool hunt;
        public override void ResetEffects()
        {
            setBonus = false;
            hunt = false;
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (proj.ranged && target.life < 0 && hunt)
            {
                player.AddBuff(BuffID.Hunter, 60 * 10);
            }
            if (proj.ranged && (target.Center - player.Center).Length() > 400 && setBonus)
            {

                target.AddBuff(mod.BuffType("LuneCurse"), (int)(target.Center - player.Center).Length());
            }
        }

    }
    public class JacketDamageBonus : GlobalNPC
    {
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            if ((npc.Center - player.Center).Length() > 400 && player.GetModPlayer<JacketBonuses>().setBonus && projectile.ranged)
                damage = (int)(damage * 1.2f);
        }
    }

}

