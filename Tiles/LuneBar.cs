using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Tiles
{
	public class LuneBar: ModTile
{
    public override void SetDefaults()
    {
            Main.tileShine[Type] = 1100;
            Main.tileSolid[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);



            dustType = mod.DustType("LuneDust");
            soundType = 21;
        soundStyle = 2;
        minPick = 1; 
        AddMapEntry(new Color(102, 143, 204));
        drop = mod.ItemType("LuneBar");
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    {
        r = 0.5f;
        g = 0.5f;
        b = 0.5f;
    }

  
}}