using Microsoft.Xna.Framework;
using QwertysRandomContent.Config;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Tiles
{
    public class FakeFortressBrick : ModTile
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
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            //Main.tileSpelunker[Type] = true;
            Main.tileBrick[Type] = true;
            Main.tileBlendAll[Type] = true;

            dustType = mod.DustType("FortressDust");
            soundType = 21;
            soundStyle = 2;
            minPick = 50;
            AddMapEntry(new Color(162, 184, 185));
            mineResist = 1;
            //drop = mod.ItemType("FortressBrick");
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (Main.netMode != 1 && !fail)
            {
                NPC youngTile = Main.npc[NPC.NewNPC(i * 16 + 8, j * 16, mod.NPCType("YoungTile"), ai3: 1)];
                youngTile.velocity = QwertyMethods.PolarVector(2, (float)Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI));
            }
        }

        public override void HitWire(int i, int j)
        {
            WorldGen.KillTile(i, j);
            NetMessage.SendData(MessageID.TileChange, -1, -1, null, 0, (float)i, (float)j);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.0f;
            g = 0.0f;
            b = 0.0f;
        }

        public override bool CanExplode(int i, int j)
        {
            return true;
        }
    }
}