using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress.CaeliteWeapons
{
    public class CaelitePickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caelite Pickaxe");
            Tooltip.SetDefault("Mines a 3x3 area!!");

        }
        public override void SetDefaults()
        {
            item.damage = 7;
            item.melee = true;

            item.useTime = 28;
            item.useAnimation = 28;
            item.useStyle = 1;
            item.knockBack = 3;
            item.value = 25000;
            item.rare = 3;
            item.UseSound = SoundID.Item1;

            item.width = 32;
            item.height = 32;
            //item.crit = 5;
            item.autoReuse = true;
            item.pick = 95;
            item.tileBoost = 2;
            item.GetGlobalItem<AoePick>().miningRadius = 1;



        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CaeliteBar"), 16);
            recipe.AddIngredient(mod.ItemType("CaeliteCore"), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {

            Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, mod.DustType("CaeliteDust"));
            Lighting.AddLight(hitbox.Center.ToVector2(), new Vector3(.6f, .6f, .6f));
        }
    }
    public class AoePick : GlobalItem
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }
        public int miningRadius = 0;

    }
    public class SpecialPick : ModPlayer
    {

        public override void PostItemCheck()
        {
            if (!player.inventory[player.selectedItem].IsAir)
            {
                Item item = player.inventory[player.selectedItem];
                bool flag18 = player.position.X / 16f - (float)Player.tileRangeX - (float)item.tileBoost <= (float)Player.tileTargetX && (player.position.X + (float)player.width) / 16f + (float)Player.tileRangeX + (float)item.tileBoost - 1f >= (float)Player.tileTargetX && player.position.Y / 16f - (float)Player.tileRangeY - (float)item.tileBoost <= (float)Player.tileTargetY && (player.position.Y + (float)player.height) / 16f + (float)Player.tileRangeY + (float)item.tileBoost - 2f >= (float)Player.tileTargetY;
                if (player.noBuilding)
                {
                    flag18 = false;
                }
                if (flag18)
                {

                    if (item.GetGlobalItem<AoePick>().miningRadius > 0)
                    {

                        if ((item.pick > 0 && !Main.tileAxe[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type] && !Main.tileHammer[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type]) || (item.axe > 0 && Main.tileAxe[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type]) || (item.hammer > 0 && Main.tileHammer[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type]))
                        {

                        }
                        if (player.toolTime == 0 && player.itemAnimation > 0 && player.controlUseItem)
                        {
                            int tileId = player.hitTile.HitObject(Player.tileTargetX, Player.tileTargetY, 1);


                            if (item.pick > 0)
                            {
                                for (int i = -item.GetGlobalItem<AoePick>().miningRadius; i <= item.GetGlobalItem<AoePick>().miningRadius; i++)
                                {
                                    for (int j = -item.GetGlobalItem<AoePick>().miningRadius; j <= item.GetGlobalItem<AoePick>().miningRadius; j++)
                                    {
                                        if ((i != 0 || j != 0) && !Main.tileAxe[(int)Main.tile[Player.tileTargetX + i, Player.tileTargetY + j].type] && !Main.tileHammer[(int)Main.tile[Player.tileTargetX + i, Player.tileTargetY + j].type])
                                        {
                                            player.PickTile(Player.tileTargetX + i, Player.tileTargetY + j, item.pick);
                                        }
                                    }
                                }



                                player.itemTime = (int)((float)item.useTime * player.pickSpeed);
                            }




                            {
                                player.poundRelease = false;
                            }
                        }

                        if (player.releaseUseItem)
                        {
                            player.poundRelease = true;
                        }
                        int num263 = Player.tileTargetX;
                        int num264 = Player.tileTargetY;
                        bool flag24 = true;
                        if (Main.tile[num263, num264].wall > 0)
                        {
                            if (!Main.wallHouse[(int)Main.tile[num263, num264].wall])
                            {
                                for (int num265 = num263 - 1; num265 < num263 + 2; num265++)
                                {
                                    for (int num266 = num264 - 1; num266 < num264 + 2; num266++)
                                    {
                                        if (Main.tile[num265, num266].wall != Main.tile[num263, num264].wall)
                                        {
                                            flag24 = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                flag24 = false;
                            }
                        }
                        if (flag24 && !Main.tile[num263, num264].active())
                        {
                            int num267 = -1;
                            if ((double)(((float)Main.mouseX + Main.screenPosition.X) / 16f) < Math.Round((double)(((float)Main.mouseX + Main.screenPosition.X) / 16f)))
                            {
                                num267 = 0;
                            }
                            int num268 = -1;
                            if ((double)(((float)Main.mouseY + Main.screenPosition.Y) / 16f) < Math.Round((double)(((float)Main.mouseY + Main.screenPosition.Y) / 16f)))
                            {
                                num268 = 0;
                            }
                            for (int num269 = Player.tileTargetX + num267; num269 <= Player.tileTargetX + num267 + 1; num269++)
                            {
                                for (int num270 = Player.tileTargetY + num268; num270 <= Player.tileTargetY + num268 + 1; num270++)
                                {
                                    if (flag24)
                                    {
                                        num263 = num269;
                                        num264 = num270;
                                        if (Main.tile[num263, num264].wall > 0)
                                        {
                                            if (!Main.wallHouse[(int)Main.tile[num263, num264].wall])
                                            {
                                                for (int num271 = num263 - 1; num271 < num263 + 2; num271++)
                                                {
                                                    for (int num272 = num264 - 1; num272 < num264 + 2; num272++)
                                                    {
                                                        if (Main.tile[num271, num272].wall != Main.tile[num263, num264].wall)
                                                        {
                                                            flag24 = false;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                flag24 = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }

        }
    }
}

