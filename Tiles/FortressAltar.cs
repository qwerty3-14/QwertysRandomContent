using Microsoft.Xna.Framework;
using QwertysRandomContent.Config;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace QwertysRandomContent.Tiles
{
    public class FortressAltar : ModTile
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            if (ModContent.GetInstance<SpriteSettings>().ClassicFortress)
            {
                texture += "_Classic";
            }
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 0, 0);
            TileObjectData.addTile(Type);
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            ModTranslation name = CreateMapEntryName();
            dustType = mod.DustType("CaeliteDust");
            soundType = 21;
            soundStyle = 2;
            minPick = 10000;
            AddMapEntry(new Color(162, 184, 185));
            name.SetDefault("Altar");
        }

        public override void RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            //QwertyMethods.ServerClientCheck();
            if (!NPC.AnyNPCs(mod.NPCType("FortressBoss")))
            {
                for (int b = 0; b < 58; b++) // this searches every invintory slot
                {
                    if (player.inventory[b].type == mod.ItemType("FortressBossSummon") && player.inventory[b].stack > 0) //this checks if the slot has the valid item
                    {
                        if (Main.netMode == 0)
                        {
                            QwertyWorld.FortressBossQuotes();
                            int npcID = NPC.NewNPC(i * 16 + 400, j * 16, mod.NPCType("FortressBoss"));
                        }
                        else
                        {
                            ModPacket packet = mod.GetPacket();
                            packet.Write((byte)ModMessageType.DivineCall);
                            packet.WriteVector2(new Vector2(i * 16 + 400, j * 16));
                            packet.Send();
                        }

                        player.inventory[b].stack--;
                        break;
                    }
                }
            }
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.showItemIcon = true;
            player.showItemIcon2 = mod.ItemType("FortressBossSummon");
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.5f;
            g = 0.5f;
            b = 0.5f;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}