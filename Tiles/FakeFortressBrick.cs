using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader;
using Terraria.ObjectData;
namespace QwertysRandomContent.Tiles
{
    public class FakeFortressBrick : ModTile
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            if (Config.alternateFortressLook)
            {
                texture = "QwertysRandomContent/Tiles/FortressBrick_Alternate";
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
            //HitWire(i, j);
            if (Main.netMode != 1)
            {
                NPC youngTile = Main.npc[NPC.NewNPC(i * 16 + 8, j * 16, mod.NPCType("YoungTile"), ai3: 1)];
                youngTile.velocity = QwertyMethods.PolarVector(2, (float)Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI));
            }
        }
        public override void HitWire(int i, int j)
        {
            WorldGen.KillTile(i, j);
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