using QwertysRandomContent.AbstractClasses;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Jungle
{
    public class FrogGrenade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frog Grenade");
            Tooltip.SetDefault("Ribbit!");
        }
        public override void SetDefaults()
        {
            item.useStyle = 5;
            item.shootSpeed = 6f;
            item.shoot = mod.ProjectileType("FrogGrenadeP");
            item.width = 20;
            item.height = 20;
            item.maxStack = 999;
            item.consumable = true;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 38;
            item.useTime = 38;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.value = 75;
            item.damage = 85;
            item.knockBack = 8f;
            item.thrown = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.JungleSpores, 2);
            recipe.AddIngredient(ItemID.Frog);
            recipe.AddIngredient(ItemID.Grenade, 50);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }
    }
    public class FrogGrenadeP : Grenade
    {
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.thrown = true;
            projectile.timeLeft = 180;
            sticky = false;
            bouncyness = .6f;
            explosionSize = 2;
        }
        public override bool ExplosionArea(int explosionSize)
        {
            return false;
        }
        public override bool ExplosionEffect(int explosionSize)
        {
            Main.PlaySound(SoundID.Frog, projectile.position);
            int frogCount = Main.rand.Next(explosionSize) + 2;
            for (int i = 0; i < frogCount; i++)
            {

                NPC frog = Main.npc[NPC.NewNPC((int)projectile.Center.X, (int)projectile.Center.Y, NPCID.Frog)];
                frog.SpawnedFromStatue = true;
                frog.velocity.Y = -4f - 3f * Main.rand.NextFloat();
                frog.velocity.X = Main.rand.NextFloat(-6, 6);

            }
            return false;
        }
    }
}
