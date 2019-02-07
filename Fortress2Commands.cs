using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace QwertysRandomContent
{
    public class GenerateTinyRoom : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "generateTinyRoom"; }
        }
        public override string Usage
        {
            get { return "/generateTinyRoom type"; }
        }

        public override string Description
        {
            get { return "Generates a tiny room where you click, giving it a number will pick a specific type of room"; }
        }
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if(input != "/generateTinyRoom")
            {
                caller.Player.GetModPlayer<RoomPlacement>(mod).roomType = int.Parse(args[0]);
            }
            caller.Player.GetModPlayer<RoomPlacement>(mod).placeTinyRoom = true;
            // create a writer and open the file
            TextWriter tw = new StreamWriter("Hi.txt");

            // write a line of text to the file
            tw.WriteLine("Hello World");

            // close the stream
            tw.Close();
        }
    }
    public class ExportTinyRoom : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "exportTinyRoom"; }
        }
        public override string Usage
        {
            get { return "/exportTinyRoom name"; }
        }

        public override string Description
        {
            get { return "Exports the room so it's easy to add into the mod's code!"; }
        }
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (input == "/exportTinyRoom")
            {
                Main.NewText("This command requires you give a name");
            }
            else
            {
                caller.Player.GetModPlayer<RoomPlacement>(mod).exportTinyRoom = true;
                caller.Player.GetModPlayer<RoomPlacement>(mod).roomName = args[0];
                
            }
            
            
        }
    }
    public class GenerateSmallRoom : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "generateSmallRoom"; }
        }
        public override string Usage
        {
            get { return "/generateSmallRoom type"; }
        }

        public override string Description
        {
            get { return "Generates a small room where you click, giving it a number will pick a specific type of room"; }
        }
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (input != "/generateSmallRoom")
            {
                caller.Player.GetModPlayer<RoomPlacement>(mod).roomType = int.Parse(args[0]);
            }
            caller.Player.GetModPlayer<RoomPlacement>(mod).placeSmallRoom = true;
            
        }
    }
    public class ExportSmallRoom : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "exportSmallRoom"; }
        }
        public override string Usage
        {
            get { return "/exportSmallRoom name"; }
        }

        public override string Description
        {
            get { return "Exports the room so it's easy to add into the mod's code!"; }
        }
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (input == "/exportSmallRoom")
            {
                Main.NewText("This command requires you give a name");
            }
            else
            {
                caller.Player.GetModPlayer<RoomPlacement>(mod).exportSmallRoom = true;
                caller.Player.GetModPlayer<RoomPlacement>(mod).roomName = args[0];

            }


        }
    }
    public class GenerateMediumRoom : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "generateMediumRoom"; }
        }
        public override string Usage
        {
            get { return "/generateMediumRoom type"; }
        }

        public override string Description
        {
            get { return "Generates a medium room where you click, giving it a number will pick a specific type of room"; }
        }
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (input != "/generateMediumRoom")
            {
                caller.Player.GetModPlayer<RoomPlacement>(mod).roomType = int.Parse(args[0]);
            }
            caller.Player.GetModPlayer<RoomPlacement>(mod).placeMediumRoom = true;

        }
    }
    public class ExportMediumRoom : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "exportMediumRoom"; }
        }
        public override string Usage
        {
            get { return "/exportMediumRoom name"; }
        }

        public override string Description
        {
            get { return "Exports the room so it's easy to add into the mod's code!"; }
        }
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (input == "/exportMediumRoom")
            {
                Main.NewText("This command requires you give a name");
            }
            else
            {
                caller.Player.GetModPlayer<RoomPlacement>(mod).exportMediumRoom = true;
                caller.Player.GetModPlayer<RoomPlacement>(mod).roomName = args[0];

            }


        }
    }
    public class GenerateLargeRoom : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "generateLargeRoom"; }
        }
        public override string Usage
        {
            get { return "/generateLargeRoom type"; }
        }

        public override string Description
        {
            get { return "Generates a large room where you click, giving it a number will pick a specific type of room"; }
        }
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (input != "/generateLargeRoom")
            {
                caller.Player.GetModPlayer<RoomPlacement>(mod).roomType = int.Parse(args[0]);
            }
            caller.Player.GetModPlayer<RoomPlacement>(mod).placeLargeRoom = true;

        }
    }
    public class ExportLargeRoom : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "exportLargeRoom"; }
        }
        public override string Usage
        {
            get { return "/exportLargeRoom name"; }
        }

        public override string Description
        {
            get { return "Exports the room so it's easy to add into the mod's code!"; }
        }
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (input == "/exportLargeRoom")
            {
                Main.NewText("This command requires you give a name");
            }
            else
            {
                caller.Player.GetModPlayer<RoomPlacement>(mod).exportLargeRoom = true;
                caller.Player.GetModPlayer<RoomPlacement>(mod).roomName = args[0];

            }


        }
    }
    public class GenerateAltarRoom : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "generateAltarRoom"; }
        }
        public override string Usage
        {
            get { return "/generateAltarRoom type"; }
        }

        public override string Description
        {
            get { return "Generates an altar room where you click, giving it a number will pick a specific type of room"; }
        }
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (input != "/generateAltarRoom")
            {
                caller.Player.GetModPlayer<RoomPlacement>(mod).roomType = int.Parse(args[0]);
            }
            caller.Player.GetModPlayer<RoomPlacement>(mod).placeAltarRoom = true;

        }
    }
    public class ExportAltarRoom : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "exportAltarRoom"; }
        }
        public override string Usage
        {
            get { return "/exportAltarRoom name"; }
        }

        public override string Description
        {
            get { return "Exports the room so it's easy to add into the mod's code!"; }
        }
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (input == "/exportAltarRoom")
            {
                Main.NewText("This command requires you give a name");
            }
            else
            {
                caller.Player.GetModPlayer<RoomPlacement>(mod).exportAltarRoom = true;
                caller.Player.GetModPlayer<RoomPlacement>(mod).roomName = args[0];

            }


        }
    }
    public class PlaceAltar : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "placeAltar"; }
        }
        

        public override string Description
        {
            get { return "Allows you to place a fortress Altar"; }
        }
        public override void Action(CommandCaller caller, string input, string[] args)
        {
           
            caller.Player.GetModPlayer<RoomPlacement>(mod).placeAltar = true;

        }
    }
    public class RoomPlacement : ModPlayer
    {
        public Vector2 UIDimensions = Vector2.Zero;
        public bool placeSmallRoom = false;
        public bool exportSmallRoom = false;
        public bool placeTinyRoom = false;
        public bool exportTinyRoom = false;
        public int roomType = -1;
        public string roomName = "";
        public bool placeMediumRoom = false;
        public bool exportMediumRoom = false;
        public bool placeLargeRoom = false;
        public bool exportLargeRoom = false;
        public bool placeAltarRoom = false;
        public bool  exportAltarRoom = false;
        public bool placeAltar = false;
        void PlaceUpdate()
        {
            placeSmallRoom = false;
            placeTinyRoom = false;
            UIDimensions = Vector2.Zero;
            roomType = -1;
            exportTinyRoom = false;
            exportSmallRoom = false;
            roomName = "";
            placeMediumRoom = false;
            exportMediumRoom = false;
            placeLargeRoom = false;
            exportLargeRoom = false;
            placeAltarRoom = false;
            exportAltarRoom = false;
            placeAltar = false;
        }
        public static readonly PlayerLayer PlacementUI = new PlayerLayer("QwertysRandomContent", "PlacementUI", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            Vector2 dimensions = drawPlayer.GetModPlayer<RoomPlacement>(mod).UIDimensions;
            float r = 0.24f;
            float g = 0.8f;
            float b = 0.9f;
            float a = 1f;
            float scale2 = 0.8f;
            Color color = new Microsoft.Xna.Framework.Color(r, g, b, a) * scale2;
            Texture2D texture = Main.extraTexture[2];
            Rectangle frame = new Rectangle(0, 0, 16, 16);
            Vector2 position = Main.MouseWorld / 16f;
            position.X = (int)position.X;
            position.Y = (int)position.Y;
            for (int i = 0; i < dimensions.X; i++)
            {
                for (int j = 0; j < dimensions.Y; j++)
                {
                    DrawData data = new DrawData(texture,
                    ((position + new Vector2(i, j)) * 16) - Main.screenPosition,
                    frame,
                    color,
                    0f,
                    Vector2.Zero,
                    1f,
                    0,
                    0);
                    Main.playerDrawData.Add(data);
                }
            }
        });
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int f = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("MiscEffectsFront"));
            if (f != -1)
            {
                PlacementUI.visible = true;
                layers.Insert(f + 1, PlacementUI);

            }


        }
        public override void PreUpdate()
        {
            /*
            Vector2 positionB = Main.MouseWorld / 16f;
            positionB.X = (int)positionB.X;
            positionB.Y = (int)positionB.Y;
            UIDimensions = new Vector2(1, 1);
            Main.NewText(Main.tile[(int)positionB.X, (int)positionB.Y].nactive());
            */
            if(placeAltar)
            {
                UIDimensions = new Vector2(3, 2);

                if (Main.mouseLeft)
                {
                    Vector2 position = Main.MouseWorld / 16f;
                    position.X = (int)position.X;
                    position.Y = (int)position.Y;
                    WorldGen.PlaceTile((int)position.X +1, (int)position.Y+1, mod.TileType("FortressAltar"));

                    PlaceUpdate();
                }

            }
            if (placeTinyRoom)
            {
                UIDimensions = new Vector2(20, 10);

                if (Main.mouseLeft)
                {
                    Vector2 position = Main.MouseWorld / 16f;
                    position.X = (int)position.X;
                    position.Y = (int)position.Y;
                    Fortress2Blueprints.BuildRoom((int)position.X, (int)position.Y, Fortress2Blueprints.TinyRoomTileTypes, Fortress2Blueprints.TinyRooms, roomType);

                    PlaceUpdate();
                }


            }
            if(exportTinyRoom)
            {
                UIDimensions = new Vector2(20, 10);
                if (Main.mouseLeft)
                {
                    Vector2 position = Main.MouseWorld / 16f;
                    ExportRoom(position, Fortress2Blueprints.TinyRooms);
                }

                
            }
            if (placeSmallRoom)
            {
                UIDimensions = new Vector2(40, 20);

                if (Main.mouseLeft)
                {
                    Vector2 position = Main.MouseWorld / 16f;
                    position.X = (int)position.X;
                    position.Y = (int)position.Y;
                    Fortress2Blueprints.BuildRoom((int)position.X, (int)position.Y, Fortress2Blueprints.SmallRoomTileTypes, Fortress2Blueprints.SmallRooms, roomType);

                    PlaceUpdate();
                }


            }
            if (exportSmallRoom)
            {
                UIDimensions = new Vector2(40, 20);
                if (Main.mouseLeft)
                {
                    Vector2 position = Main.MouseWorld / 16f;
                    ExportRoom(position, Fortress2Blueprints.SmallRooms);
                }


            }
            if (placeMediumRoom)
            {
                UIDimensions = new Vector2(60, 30);

                if (Main.mouseLeft)
                {
                    Vector2 position = Main.MouseWorld / 16f;
                    position.X = (int)position.X;
                    position.Y = (int)position.Y;
                    Fortress2Blueprints.BuildRoom((int)position.X, (int)position.Y, Fortress2Blueprints.MediumRoomTileTypes, Fortress2Blueprints.MediumRooms, roomType);

                    PlaceUpdate();
                }


            }
            if (exportMediumRoom)
            {
                UIDimensions = new Vector2(60, 30);
                if (Main.mouseLeft)
                {
                    Vector2 position = Main.MouseWorld / 16f;
                    ExportRoom(position, Fortress2Blueprints.MediumRooms);
                }


            }
            if (placeLargeRoom)
            {
                UIDimensions = new Vector2(80, 40);

                if (Main.mouseLeft)
                {
                    Vector2 position = Main.MouseWorld / 16f;
                    position.X = (int)position.X;
                    position.Y = (int)position.Y;
                    Fortress2Blueprints.BuildRoom((int)position.X, (int)position.Y, Fortress2Blueprints.LargeRoomTileTypes, Fortress2Blueprints.LargeRooms, roomType);

                    PlaceUpdate();
                }


            }
            if (exportLargeRoom)
            {
                UIDimensions = new Vector2(80, 40);
                if (Main.mouseLeft)
                {
                    Vector2 position = Main.MouseWorld / 16f;
                    ExportRoom(position, Fortress2Blueprints.LargeRooms);
                }


            }
            if (placeAltarRoom)
            {
                UIDimensions = new Vector2(100, 50);

                if (Main.mouseLeft)
                {
                    Vector2 position = Main.MouseWorld / 16f;
                    position.X = (int)position.X;
                    position.Y = (int)position.Y;
                    Fortress2Blueprints.BuildRoom((int)position.X, (int)position.Y, Fortress2Blueprints.AltarRoomTileTypes, Fortress2Blueprints.AltarRooms, roomType);

                    PlaceUpdate();
                }


            }
            if (exportAltarRoom)
            {
                UIDimensions = new Vector2(100, 50);
                if (Main.mouseLeft)
                {
                    Vector2 position = Main.MouseWorld / 16f;
                    ExportRoom(position, Fortress2Blueprints.AltarRooms);
                }


            }

        }
        public void ExportRoom(Vector2 position, int[,,,] Rooms)
        {
            
            position.X = (int)position.X;
            position.Y = (int)position.Y;
            //BuildTinyRoom((int)position.X, (int)position.Y, roomType);


            String tileList = "Tile type list: new int[] {-1";
            String wallList = "Wall type list: new int[] {-1";
            List<int> foundTiles = new List<int>() { -1 };
            List<int> foundWalls = new List<int>() { -1 };
            for (int y = 0; y < Rooms.GetLength(2); y++)
            {
                for (int x = 0; x < Rooms.GetLength(3); x++)
                {
                    int i = (int)position.X + x;
                    int j = (int)position.Y + y;
                    if (Main.tile[i, j].active())
                    {
                        if (!foundTiles.Contains(Main.tile[i, j].type))
                        {

                            foundTiles.Add(Main.tile[i, j].type);
                           
                            if (Main.tile[i, j].type > 470) //checks if it's a modded tile
                            {
                               
                                if(Main.tile[i, j].type == mod.TileType("FortressBrick"))
                                {
                                    tileList += ", mod.TileType(\"FortressBrick\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("DnasBrick"))
                                {
                                    tileList += ", mod.TileType(\"DnasBrick\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("ReverseSand"))
                                {
                                    tileList += ", mod.TileType(\"ReverseSand\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("CaeliteBar"))
                                {
                                    tileList += ", mod.TileType(\"CaeliteBar\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("ChiselledFortressBrick"))
                                {
                                    tileList += ", mod.TileType(\"ChiselledFortressBrick\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("DnasLantern"))
                                {
                                    tileList += ", mod.TileType(\"DnasLantern\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("DnasTransmutator"))
                                {
                                    tileList += ", mod.TileType(\"DnasTransmutator\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FakeFortressBrick"))
                                {
                                    tileList += ", mod.TileType(\"FakeFortressBrick\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressAltar"))
                                {
                                    tileList += ", mod.TileType(\"FortressAltar\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressCarving1"))
                                {
                                    tileList += ", mod.TileType(\"FortressCarving1\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressCarving2"))
                                {
                                    tileList += ", mod.TileType(\"FortressCarving2\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressCarving3"))
                                {
                                    tileList += ", mod.TileType(\"FortressCarving3\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressPillar"))
                                {
                                    tileList += ", mod.TileType(\"FortressPillar\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressPlatform"))
                                {
                                    tileList += ", mod.TileType(\"FortressPlatform\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressTrap"))
                                {
                                    tileList += ", mod.TileType(\"FortressTrap\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressTrap"))
                                {
                                    tileList += ", mod.TileType(\"FortressTrap\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("LaunchPad"))
                                {
                                    tileList += ", mod.TileType(\"LaunchPad\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("ReverseSand"))
                                {
                                    tileList += ", mod.TileType(\"ReverseSand\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressBathtub"))
                                {
                                    tileList += ", mod.TileType(\"FortressBathtub\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressBed"))
                                {
                                    tileList += ", mod.TileType(\"FortressBed\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressBench"))
                                {
                                    tileList += ", mod.TileType(\"FortressBench\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressBookcase"))
                                {
                                    tileList += ", mod.TileType(\"FortressBookcase\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressCandelabra"))
                                {
                                    tileList += ", mod.TileType(\"FortressCandelabra\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressCandle"))
                                {
                                    tileList += ", mod.TileType(\"FortressCandle\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressChair"))
                                {
                                    tileList += ", mod.TileType(\"FortressChair\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressChandelier"))
                                {
                                    tileList += ", mod.TileType(\"FortressChandelier\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressClock"))
                                {
                                    tileList += ", mod.TileType(\"FortressClock\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressCouch"))
                                {
                                    tileList += ", mod.TileType(\"FortressCouch\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressDresser"))
                                {
                                    tileList += ", mod.TileType(\"FortressDresser\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressLamp"))
                                {
                                    tileList += ", mod.TileType(\"FortressLamp\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressLantern"))
                                {
                                    tileList += ", mod.TileType(\"FortressLantern\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressPiano"))
                                {
                                    tileList += ", mod.TileType(\"FortressPiano\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressSink"))
                                {
                                    tileList += ", mod.TileType(\"FortressSink\")";
                                }
                                else if (Main.tile[i, j].type == mod.TileType("FortressTable"))
                                {
                                    tileList += ", mod.TileType(\"FortressTable\")";
                                }
                                else
                                {
                                    tileList += ", " + Main.tile[i, j].type;
                                    tileList += "m";
                                }
                                
                            }
                            else
                            {
                                tileList += ", " + Main.tile[i, j].type;
                            }
                        }
                    }
                    if (Framing.GetTileSafely(i, j).active())
                    {
                        if (!foundWalls.Contains(Framing.GetTileSafely(i, j).wall))
                        {
                            foundWalls.Add(Framing.GetTileSafely(i, j).wall);
                            wallList += ", " + Framing.GetTileSafely(i, j).wall;
                            if (Framing.GetTileSafely(i, j).wall > 230) // checks if it's a modded wall
                            {
                                wallList += "m";
                            }
                        }
                    }
                }
            }
            tileList += "}";
            wallList += "}";
            // create a writer and open the file
            TextWriter tw = new StreamWriter(roomName + ".txt");

            // write a line of text to the file
            tw.WriteLine(tileList);
            tw.WriteLine(wallList);
            tw.WriteLine("Tile Structure: ");
            String tileStructure = "{ { {";
            for (int y = 0; y < Rooms.GetLength(2); y++)
            {
                if (y != 0)
                {
                    tileStructure += "},{";
                }

                for (int x = 0; x < Rooms.GetLength(3); x++)
                {
                    int i = (int)position.X + x;
                    int j = (int)position.Y + y;
                    if (x != 0)
                    {
                        tileStructure += ", ";
                    }

                    if (Main.tile[i, j].active())
                    {

                        if (Main.tileFrameImportant[Main.tile[i, j].type])
                        {
                            int width = TileObjectData.GetTileData(Main.tile[i, j].type, Main.tile[i, j].frameX).Width;
                            int height = TileObjectData.GetTileData(Main.tile[i, j].type, Main.tile[i, j].frameX).Height;
                            if ((Main.tile[i, j].frameX / 18) % width == TileObjectData.GetTileData(Main.tile[i, j].type, Main.tile[i, j].frameX).Origin.X && (Main.tile[i, j].frameY / 18) % height == TileObjectData.GetTileData(Main.tile[i, j].type, Main.tile[i, j].frameX).Origin.Y)
                            {
                                tileStructure += foundTiles.IndexOf(Main.tile[i, j].type);
                            }
                            else
                            {
                                tileStructure += "0";
                            }
                        }
                        else
                        {
                            tileStructure += foundTiles.IndexOf(Main.tile[i, j].type);
                        }

                    }
                    else
                    {
                        tileStructure += "0";
                    }
                }
            }
            tileStructure += "} }, { {";
            for (int y = 0; y < Rooms.GetLength(2); y++)
            {
                if (y != 0)
                {
                    tileStructure += "},{";
                }

                for (int x = 0; x < Rooms.GetLength(3); x++)
                {
                    int i = (int)position.X + x;
                    int j = (int)position.Y + y;
                    if (x != 0)
                    {
                        tileStructure += ", ";
                    }

                    if (foundWalls.IndexOf(Framing.GetTileSafely(i, j).wall) != -1)
                    {

                        tileStructure += foundWalls.IndexOf(Framing.GetTileSafely(i, j).wall);
                    }
                    else
                    {
                        tileStructure += "0";
                    }
                }
            }
            tileStructure += "} }, { {";
            for (int y = 0; y < Rooms.GetLength(2); y++)
            {
                if (y != 0)
                {
                    tileStructure += "},{";
                }

                for (int x = 0; x < Rooms.GetLength(3); x++)
                {
                    int i = (int)position.X + x;
                    int j = (int)position.Y + y;
                    if (x != 0)
                    {
                        tileStructure += ", ";
                    }
                    int slope = Main.tile[i, j].slope();
                    if (Main.tile[i, j].halfBrick())
                    {
                        slope += 100;
                    }
                    tileStructure += slope;
                }
            }
            tileStructure += "} }, { {";
            for (int y = 0; y < Rooms.GetLength(2); y++)
            {
                if (y != 0)
                {
                    tileStructure += "},{";
                }

                for (int x = 0; x < Rooms.GetLength(3); x++)
                {
                    int i = (int)position.X + x;
                    int j = (int)position.Y + y;
                    if (x != 0)
                    {
                        tileStructure += ", ";
                    }
                    int wires = 0;
                    if (Main.tile[i, j].wire())
                    {
                        wires += 1;
                    }
                    if (Main.tile[i, j].wire2())
                    {
                        wires += 10;
                    }
                    if (Main.tile[i, j].wire3())
                    {
                        wires += 100;
                    }
                    if (Main.tile[i, j].wire4())
                    {
                        wires += 1000;
                    }
                    
                    if(Main.tile[i, j].actuator())
                    {
                        wires += 10000;
                    }
                    
                    tileStructure += wires;
                }
            }
            tileStructure += "} }, { {";
            for (int y = 0; y < Rooms.GetLength(2); y++)
            {
                if (y != 0)
                {
                    tileStructure += "},{";
                }

                for (int x = 0; x < Rooms.GetLength(3); x++)
                {
                    int i = (int)position.X + x;
                    int j = (int)position.Y + y;
                    if (x != 0)
                    {
                        tileStructure += ", ";
                    }

                    tileStructure += Main.tile[i, j].liquid;
                }
            }
            tileStructure += "} }, { {";
            for (int y = 0; y < Rooms.GetLength(2); y++)
            {
                if (y != 0)
                {
                    tileStructure += "},{";
                }

                for (int x = 0; x < Rooms.GetLength(3); x++)
                {
                    int i = (int)position.X + x;
                    int j = (int)position.Y + y;
                    if (x != 0)
                    {
                        tileStructure += ", ";
                    }

                    tileStructure += Main.tile[i, j].frameX;

                }
            }
            tileStructure += "} }, { {";
            for (int y = 0; y < Rooms.GetLength(2); y++)
            {
                if (y != 0)
                {
                    tileStructure += "},{";
                }

                for (int x = 0; x < Rooms.GetLength(3); x++)
                {
                    int i = (int)position.X + x;
                    int j = (int)position.Y + y;
                    if (x != 0)
                    {
                        tileStructure += ", ";
                    }

                    tileStructure += Main.tile[i, j].frameY;

                }
            }
            tileStructure += "} } }";
            tw.WriteLine(tileStructure);
            // close the stream
            tw.Close();
            PlaceUpdate();
        }


    }
}
