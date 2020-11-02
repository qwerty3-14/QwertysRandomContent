using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent
{
    public class NPCItemSales : GlobalNPC
    {
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.SkeletonMerchant)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("ConjureBone"));
                nextSlot++;
                if (NPC.downedBoss1)
                {
                    shop.item[nextSlot].SetDefaults(mod.ItemType("DuelistHeadband"));
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(mod.ItemType("DuelistShirt"));
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(mod.ItemType("DuelistPants"));
                    nextSlot++;
                }
                if (Main.moonPhase < 4)
                {
                    shop.item[nextSlot].SetDefaults(mod.ItemType("ArcaneArmorBreaker"));
                    nextSlot++;
                }
                else
                {
                    shop.item[nextSlot].SetDefaults(mod.ItemType("AerodynamicFins"));
                    nextSlot++;
                }
            }
            if (type == NPCID.Demolitionist)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("PrimedGrenadeCore"));
                nextSlot++;
            }
            if (type == NPCID.Cyborg)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("TeleportationArrow"));
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("TeleportationArrowGrab"));
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("TeleportationArrowSwap"));
                nextSlot++;
            }
            if (type == NPCID.WitchDoctor)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("MinionFang"));
                nextSlot++;
            }
            if (type == NPCID.Dryad)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("DecorativePlant"));
                nextSlot++;
            }
            if (type == NPCID.Mechanic)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("Fan"));
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("VFan"));
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("Illuminator"));
                nextSlot++;
            }
            if (type == NPCID.ArmsDealer)
            {
                if (QwertyWorld.downedAncient)
                {
                    shop.item[nextSlot].SetDefaults(mod.ItemType("TankShift"));
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(mod.ItemType("MiniTankStaff"));
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(mod.ItemType("TankCommanderHelmet"));
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(mod.ItemType("TankCommanderJacket"));
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(mod.ItemType("TankCommanderPants"));
                    nextSlot++;
                }
                if(Main.hardMode)
                {
                    shop.item[nextSlot].SetDefaults(mod.ItemType("ChaosRifle"));
                    nextSlot++;
                }
                if(QwertyWorld.downedRuneGhost)
                {
                    shop.item[nextSlot].SetDefaults(mod.ItemType("ChargingShotgun"));
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(mod.ItemType("RingOfGuns"));
                    nextSlot++;
                }
            }
            if (type == NPCID.Merchant)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("Flechettes"));
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("Flashlight"));
                nextSlot++;
            }
            if (type == NPCID.Wizard)
            {
                shop.item[nextSlot].SetDefaults(ItemID.MagicMirror);
                nextSlot++;
            }
            if (type == NPCID.Steampunker)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("Steambath"));
                nextSlot++;
            }
            if (type == NPCID.DyeTrader)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("CustomDye"));
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("CustomDye2"));
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("CustomDye3"));
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("CustomDye4"));
                nextSlot++;
            }
            if (type == NPCID.Clothier)
            {
               
                shop.item[nextSlot].SetDefaults(mod.ItemType("Miniskirt"));
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("Shorts"));
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("LeatherBelt"));
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("LeatherShoesMale"));
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("LeatherShoesFemale"));
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("LeatherBootsMale"));
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("LeatherBootsFemale"));
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("HighHeels"));
                nextSlot++;
            }
        }

        public override void SetupTravelShop(int[] shop, ref int nextSlot)
        {
            if (Main.rand.Next(0, 5) == 0)
            {
                shop[nextSlot] = mod.ItemType("Metronome");
                nextSlot++;
            }
            int selectAccesory = Main.rand.Next(5);
            switch (selectAccesory)
            {
                case 0:
                    shop[nextSlot] = mod.ItemType("SwordEmbiggenner");
                    nextSlot++;
                    break;

                case 1:
                    shop[nextSlot] = mod.ItemType("Gemini");
                    nextSlot++;
                    break;

                case 2:
                    shop[nextSlot] = mod.ItemType("QuestionableSubstance");
                    nextSlot++;
                    break;

                case 4:
                    shop[nextSlot] = mod.ItemType("BookOfMinionTactics");
                    nextSlot++;
                    break;
                case 5:
                    shop[nextSlot] = mod.ItemType("MorphGem");
                    nextSlot++;
                    break;
            }
        }
    }
}