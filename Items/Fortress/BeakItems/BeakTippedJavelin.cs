
using Microsoft.Xna.Framework;
using QwertysRandomContent.AbstractClasses;
using QwertysRandomContent.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
//copied from example javelin forom example mod
namespace QwertysRandomContent.Items.Fortress.BeakItems
{
    public class BeakTippedJavelin : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beak Tipped Javelin");
            Tooltip.SetDefault("If a crit fails to land the crit attempt will be rerolled for every javelin stuck in an enemy");

        }
        public override void SetDefaults()
        {
            // Alter any of these values as you see fit, but you should probably keep useStyle on 1, as well as the noUseGraphic and noMelee bools
            item.shootSpeed = 11f;
            item.damage = 110;
            item.knockBack = 5f;
            item.useStyle = 1;
            item.useAnimation = 38;
            item.useTime = 38;
            item.width = 68;
            item.height = 68;
            item.maxStack = 999;
            item.rare = 4;
            //item.crit = 5;
            item.value = 100;
            item.consumable = true;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.thrown = true;

            item.UseSound = SoundID.Item1;

            item.shoot = mod.ProjectileType("BeakTippedJavelinP");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CaeliteBar"), 2);
            recipe.AddIngredient(mod.ItemType("FortressHarpyBeak"), 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 222);
            recipe.AddRecipe();
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {

            float angle = (new Vector2(speedX, speedY)).ToRotation();
            float trueSpeed = (new Vector2(speedX, speedY)).Length();

            return true;
        }
    }
    public class BeakTippedJavelinP : Javelin
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beak Tipped Javelin");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.thrown = true;
            projectile.penetrate = 1;
            projectile.GetGlobalProjectile<ImplaingProjectile>().CanImpale = true;
            projectile.GetGlobalProjectile<ImplaingProjectile>().damagePerImpaler = 12;
            dropItem = mod.ItemType("BeakTippedJavelin");
            maxStickingJavelins = 3;
        }


    }
    public class JavelinCritReroll : GlobalNPC
    {
       public override bool InstancePerEntity => true;
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            //crit = true;

            if (npc.HasBuff(mod.BuffType("Impaled")) && (projectile.thrown || projectile.melee || projectile.ranged || projectile.magic))
            {
                if (!crit)
                {
                    int rerollAttempts = 0;

                    for (int p = 0; p < 1000; p++)
                    {
                        if (Main.projectile[p].active && Main.projectile[p].type == mod.ProjectileType("BeakTippedJavelinP") && Main.projectile[p].ai[0] == 1f && Main.projectile[p].ai[1] == (float)npc.whoAmI)
                        {
                            rerollAttempts++;
                        }
                    }
                    //Main.NewText(rerollAttempts);

                    for (int i = 0; i < rerollAttempts; i++)
                    {
                        if (projectile.melee && Main.rand.Next(1, 101) <= Main.player[projectile.owner].meleeCrit)
                        {
                            //Main.NewText("reroll");
                            crit = true;
                            i = rerollAttempts;
                            //return;
                        }
                        if (projectile.ranged && Main.rand.Next(1, 101) <= Main.player[projectile.owner].rangedCrit)
                        {
                            //Main.NewText("reroll");
                            crit = true;
                            i = rerollAttempts;
                            //return;
                        }
                        if (projectile.magic && Main.rand.Next(1, 101) <= Main.player[projectile.owner].magicCrit)
                        {
                            //Main.NewText("reroll");
                            crit = true;
                            i = rerollAttempts;
                            //return;
                        }
                        if (projectile.thrown && Main.rand.Next(1, 101) <= Main.player[projectile.owner].thrownCrit)
                        {
                            //Main.NewText("reroll");
                            crit = true;
                            i = rerollAttempts;
                            // return;
                        }
                    }

                    //rerollAttempts = 0;
                }
            }

        }
    }
}
