using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons
{
    public class AddDungeonLoot : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "AddDungeonLoot"; }
        }


        public override string Description
        {
            get { return "Adds dungeon loot"; }
        }
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            DungeonLoot.AddDungeonLoot();
        }

    }
    class DungeonLoot : ModWorld
    {
        public override void PostWorldGen()
        {
            AddDungeonLoot();
        }
        public static void AddDungeonLoot()
        {

            List<int> validChests = new List<int>();
            for (int c = 0; c < Main.chest.Length; c++)
            {
                if (Main.chest[c] != null)
                {
                    if (Main.chest[c].item[0].type == ItemID.MagicMissile || Main.chest[c].item[0].type == ItemID.Muramasa || Main.chest[c].item[0].type == ItemID.CobaltShield || Main.chest[c].item[0].type == ItemID.AquaScepter || Main.chest[c].item[0].type == ItemID.Handgun || Main.chest[c].item[0].type == ItemID.BlueMoon || Main.chest[c].item[0].type == ItemID.ShadowKey || Main.chest[c].item[0].type == ItemID.Valor || Main.chest[c].item[0].type == ItemID.BoneWelder)
                    {
                        validChests.Add(c);
                    }
                }
            }

            for (int a = 0; a < 2; a++)
            {
                int picked = Main.rand.Next(validChests.Count);
                for (int i = 0; i < Main.chest[validChests[picked]].item.Length; i++)
                {
                    if (Main.chest[validChests[picked]].item[i].IsAir)
                    {
                        Main.chest[validChests[picked]].item[i].SetDefaults(QwertysRandomContent.Instance.ItemType("Aqueous"), false);
                        break;
                    }

                }

                validChests.RemoveAt(picked);

            }


            for (int w = 0; w < 5; w++)
            {

                if (validChests.Count > 0)
                {
                    int picked = Main.rand.Next(validChests.Count);
                    for (int i = 0; i < Main.chest[validChests[picked]].item.Length; i++)
                    {
                        if (Main.chest[validChests[picked]].item[i].IsAir)
                        {
                            String name = "AmuletOfPatience";
                            switch (w)
                            {
                                case 0:
                                    name = "AmuletOfPatience";
                                    break;
                                case 1:
                                    name = "BurstMiner";
                                    break;
                                case 2:
                                    name = "Hydrospear";
                                    break;
                                case 3:
                                    name = "LaunchingHook";
                                    break;
                                case 4:
                                    name = "Riptide";
                                    break;
                            }
                            Main.chest[validChests[picked]].item[i].SetDefaults(QwertysRandomContent.Instance.ItemType(name), false);
                            break;
                        }
                    }
                    validChests.RemoveAt(picked);
                }
            }



            while (validChests.Count > 0)
            {

                int picked = Main.rand.Next(validChests.Count);
                for (int i = 0; i < Main.chest[validChests[picked]].item.Length; i++)
                {
                    if (Main.chest[validChests[picked]].item[i].IsAir)
                    {
                        String name = "AmuletOfPatience";
                        switch (Main.rand.Next(5))
                        {
                            case 0:
                                name = "AmuletOfPatience";
                                break;
                            case 1:
                                name = "BurstMiner";
                                break;
                            case 2:
                                name = "Hydrospear";
                                break;
                            case 3:
                                name = "LaunchingHook";
                                break;
                            case 4:
                                name = "Riptide";
                                break;
                        }
                        Main.chest[validChests[picked]].item[i].SetDefaults(QwertysRandomContent.Instance.ItemType(name), false);
                        break;
                    }
                }
                validChests.RemoveAt(picked);


            }


        }


    }
}
