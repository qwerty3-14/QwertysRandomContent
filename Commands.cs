using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent
{
    internal class WhereAmI : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "WhereAmI"; }
        }

        public override string Description
        {
            get { return "gives the player's coordinates"; }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            Main.NewText("Entity coordinates: " + Main.player[Main.myPlayer].Center);
            Main.NewText("Tile coordinates: " + Main.player[Main.myPlayer].Center.ToTileCoordinates());
        }
    }
}