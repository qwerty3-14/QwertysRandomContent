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
    class BuildFortress : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "buildFortress"; }
        }
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            Fortress2Builder.BuildFortress();
        }
    }
    class Fortress2Builder : ModWorld
    {

        public static void BuildFortress()
        {
            int lowerLimit;
            int upperLimit = 60;
            int CenterX;
            int maxDistanceFromCenter;
            int mediumRoomCount = Main.rand.Next(8, 11);
            int smallRoomsPerMed = 2;
            int tinyRoomsPerMed = 3;

            if (Main.dungeonX < Main.maxTilesX * .5f)
            {
                CenterX = (int)((double)Main.maxTilesX * 0.8);
            }
            else
            {
                CenterX = (int)((double)Main.maxTilesX * 0.2);
            }
            if (Main.maxTilesX > 8000)
            {
                lowerLimit = 320;
                maxDistanceFromCenter = 750;
                mediumRoomCount *= 6;

            }
            else if (Main.maxTilesX > 6000)
            {
                lowerLimit = 230;
                maxDistanceFromCenter = 550;
                mediumRoomCount *= 3;
            }
            else
            {
                lowerLimit = 130;
                maxDistanceFromCenter = 320;
            }
            int height = lowerLimit - upperLimit;
            Vector2 topLeft = new Vector2(CenterX - maxDistanceFromCenter, upperLimit);
            QwertyMethods.BreakTiles(CenterX - maxDistanceFromCenter, 0, maxDistanceFromCenter * 2, (int)(lowerLimit));
            bool[,] region = new bool[maxDistanceFromCenter * 2, height];
            /*
            for(int i = 0; i < region.GetLength(0); i++)
            {
                for(int j = 0; i < region.GetLength(1); j++)
                {
                    region[i, j] = false;
                }
            }*/
            Vector2 offset = new Vector2(maxDistanceFromCenter - Fortress2Blueprints.AltarRooms.GetLength(3) / 2, height / 2 - Fortress2Blueprints.AltarRooms.GetLength(2) / 2);
            Fortress2Blueprints.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Fortress2Blueprints.AltarRoomTileTypes, Fortress2Blueprints.AltarRooms);
            OccupyRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.AltarRooms.GetLength(3), Fortress2Blueprints.AltarRooms.GetLength(2), ref region);
            int l = 4;
            if (Main.maxTilesX < 6000)
            {
               
                //for (int l/2 = 1; l/2 < l; l/2++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && l/2 % 2 == 0) || (b % 2 == 0 && l/2 % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Fortress2Blueprints.LargeRooms.GetLength(3) / 2, l/2 * height / l - Fortress2Blueprints.LargeRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.LargeRooms.GetLength(3), Fortress2Blueprints.LargeRooms.GetLength(2), region))
                            {
                                Fortress2Blueprints.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Fortress2Blueprints.LargeRoomTileTypes, Fortress2Blueprints.LargeRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.LargeRooms.GetLength(3), Fortress2Blueprints.LargeRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }
                
                l = 8;
               // for (int l/2 = 1; l/2 < l; l/2++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && l/2 % 2 == 0) || (b % 2 == 0 && l/2 % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Fortress2Blueprints.MediumRooms.GetLength(3) / 2, l/2 * height / l - Fortress2Blueprints.MediumRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.MediumRooms.GetLength(3), Fortress2Blueprints.MediumRooms.GetLength(2), region))
                            {
                                Fortress2Blueprints.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Fortress2Blueprints.MediumRoomTileTypes, Fortress2Blueprints.MediumRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.MediumRooms.GetLength(3), Fortress2Blueprints.MediumRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }
                l = 16;

                //for (int l/2 = 1; l/2 < l; l/2++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && l/2 % 2 == 0) || (b % 2 == 0 && l/2 % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Fortress2Blueprints.SmallRooms.GetLength(3) / 2, l/2 * height / l - Fortress2Blueprints.SmallRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.SmallRooms.GetLength(3), Fortress2Blueprints.SmallRooms.GetLength(2), region))
                            {
                                Fortress2Blueprints.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Fortress2Blueprints.SmallRoomTileTypes, Fortress2Blueprints.SmallRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.SmallRooms.GetLength(3), Fortress2Blueprints.SmallRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }

                l = 32;
                //for (int l/2 = 1; l/2 < l; l/2++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && l/2 % 2 == 0) || (b % 2 == 0 && l/2 % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Fortress2Blueprints.TinyRooms.GetLength(3) / 2, l/2 * height / l - Fortress2Blueprints.TinyRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.TinyRooms.GetLength(3), Fortress2Blueprints.TinyRooms.GetLength(2), region))
                            {
                                Fortress2Blueprints.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Fortress2Blueprints.TinyRoomTileTypes, Fortress2Blueprints.TinyRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.TinyRooms.GetLength(3), Fortress2Blueprints.TinyRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }
            }
            else if(Main.maxTilesX < 8000)
            {
                for (int q = 1; q < l; q++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Fortress2Blueprints.LargeRooms.GetLength(3) / 2, q * height / l - Fortress2Blueprints.LargeRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.LargeRooms.GetLength(3), Fortress2Blueprints.LargeRooms.GetLength(2), region))
                            {
                                Fortress2Blueprints.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Fortress2Blueprints.LargeRoomTileTypes, Fortress2Blueprints.LargeRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.LargeRooms.GetLength(3), Fortress2Blueprints.LargeRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }

                l = 6;
                for (int q = 1; q < l; q++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Fortress2Blueprints.MediumRooms.GetLength(3) / 2, q * height / l - Fortress2Blueprints.MediumRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.MediumRooms.GetLength(3), Fortress2Blueprints.MediumRooms.GetLength(2), region))
                            {
                                Fortress2Blueprints.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Fortress2Blueprints.MediumRoomTileTypes, Fortress2Blueprints.MediumRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.MediumRooms.GetLength(3), Fortress2Blueprints.MediumRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }
                l = 12;

                for (int q = 1; q < l; q++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Fortress2Blueprints.SmallRooms.GetLength(3) / 2, q * height / l - Fortress2Blueprints.SmallRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.SmallRooms.GetLength(3), Fortress2Blueprints.SmallRooms.GetLength(2), region))
                            {
                                Fortress2Blueprints.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Fortress2Blueprints.SmallRoomTileTypes, Fortress2Blueprints.SmallRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.SmallRooms.GetLength(3), Fortress2Blueprints.SmallRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }

                l = 18;
                for (int q = 1; q < l; q++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Fortress2Blueprints.TinyRooms.GetLength(3) / 2, q * height / l - Fortress2Blueprints.TinyRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.TinyRooms.GetLength(3), Fortress2Blueprints.TinyRooms.GetLength(2), region))
                            {
                                Fortress2Blueprints.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Fortress2Blueprints.TinyRoomTileTypes, Fortress2Blueprints.TinyRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.TinyRooms.GetLength(3), Fortress2Blueprints.TinyRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }
            }
            else
            {
                
                for (int q = 1; q < l; q++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Fortress2Blueprints.LargeRooms.GetLength(3) / 2, q * height / l - Fortress2Blueprints.LargeRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.LargeRooms.GetLength(3), Fortress2Blueprints.LargeRooms.GetLength(2), region))
                            {
                                Fortress2Blueprints.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Fortress2Blueprints.LargeRoomTileTypes, Fortress2Blueprints.LargeRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.LargeRooms.GetLength(3), Fortress2Blueprints.LargeRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }

                l = 8;
                for (int q = 1; q < l; q++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Fortress2Blueprints.MediumRooms.GetLength(3) / 2, q * height / l - Fortress2Blueprints.MediumRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.MediumRooms.GetLength(3), Fortress2Blueprints.MediumRooms.GetLength(2), region))
                            {
                                Fortress2Blueprints.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Fortress2Blueprints.MediumRoomTileTypes, Fortress2Blueprints.MediumRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.MediumRooms.GetLength(3), Fortress2Blueprints.MediumRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }
                l = 16;

                for (int q = 1; q < l; q++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Fortress2Blueprints.SmallRooms.GetLength(3) / 2, q * height / l - Fortress2Blueprints.SmallRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.SmallRooms.GetLength(3), Fortress2Blueprints.SmallRooms.GetLength(2), region))
                            {
                                Fortress2Blueprints.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Fortress2Blueprints.SmallRoomTileTypes, Fortress2Blueprints.SmallRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.SmallRooms.GetLength(3), Fortress2Blueprints.SmallRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }

                l = 32;
                for (int q = 1; q < l; q++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Fortress2Blueprints.TinyRooms.GetLength(3) / 2, q * height / l - Fortress2Blueprints.TinyRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.TinyRooms.GetLength(3), Fortress2Blueprints.TinyRooms.GetLength(2), region))
                            {
                                Fortress2Blueprints.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Fortress2Blueprints.TinyRoomTileTypes, Fortress2Blueprints.TinyRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.TinyRooms.GetLength(3), Fortress2Blueprints.TinyRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }
            }
           
            /*
            int l = 4;
            for(int q =1; q< l; q++)
            {
                for(int b =1; b < l; b++)
                {
                    if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0))
                    {
                        offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Fortress2Blueprints.LargeRooms.GetLength(3) / 2, q * height / l - Fortress2Blueprints.LargeRooms.GetLength(2) / 2);
                        if (CheckRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.LargeRooms.GetLength(3), Fortress2Blueprints.LargeRooms.GetLength(2), region))
                        {
                            Fortress2Blueprints.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Fortress2Blueprints.LargeRoomTileTypes, Fortress2Blueprints.LargeRooms);
                            OccupyRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.LargeRooms.GetLength(3), Fortress2Blueprints.LargeRooms.GetLength(2), ref region);
                        }
                    }
                }
            }
            
            l = 8;
            for (int q = 1; q < l; q++)
            {
                for (int b = 1; b < l; b++)
                {
                    if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0))
                    {
                        offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Fortress2Blueprints.MediumRooms.GetLength(3) / 2, q * height / l - Fortress2Blueprints.MediumRooms.GetLength(2) / 2);
                        if (CheckRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.MediumRooms.GetLength(3), Fortress2Blueprints.MediumRooms.GetLength(2), region))
                        {
                            Fortress2Blueprints.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Fortress2Blueprints.MediumRoomTileTypes, Fortress2Blueprints.MediumRooms);
                            OccupyRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.MediumRooms.GetLength(3), Fortress2Blueprints.MediumRooms.GetLength(2), ref region);
                        }
                    }
                }
            }
            l = 16;
            
            for (int q = 1; q < l; q++)
            {
                for (int b = 1; b < l; b++)
                {
                    if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0))
                    {
                        offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Fortress2Blueprints.SmallRooms.GetLength(3) / 2, q * height / l - Fortress2Blueprints.SmallRooms.GetLength(2) / 2);
                        if (CheckRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.SmallRooms.GetLength(3), Fortress2Blueprints.SmallRooms.GetLength(2), region))
                        {
                            Fortress2Blueprints.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Fortress2Blueprints.SmallRoomTileTypes, Fortress2Blueprints.SmallRooms);
                            OccupyRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.SmallRooms.GetLength(3), Fortress2Blueprints.SmallRooms.GetLength(2), ref region);
                        }
                    }
                }
            }
            l = 32;
            for (int q = 1; q < l; q++)
            {
                for (int b = 1; b < l; b++)
                {
                    if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0)) 
                    {
                        offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Fortress2Blueprints.TinyRooms.GetLength(3) / 2, q * height / l - Fortress2Blueprints.TinyRooms.GetLength(2) / 2);
                        if (CheckRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.TinyRooms.GetLength(3), Fortress2Blueprints.TinyRooms.GetLength(2), region))
                        {
                            Fortress2Blueprints.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Fortress2Blueprints.TinyRoomTileTypes, Fortress2Blueprints.TinyRooms);
                            OccupyRegion((int)offset.X, (int)offset.Y, Fortress2Blueprints.TinyRooms.GetLength(3), Fortress2Blueprints.TinyRooms.GetLength(2), ref region);
                        }
                    }
                }
            }
            */
            /*
             for (int i = 0; i < mediumRoomCount; i++)
             {
                 AttemptRoomPlace(topLeft, Fortress2Blueprints.MediumRoomTileTypes, Fortress2Blueprints.MediumRooms, ref region);
                 for (int k = 0; k < smallRoomsPerMed; k++)
                 {
                     AttemptRoomPlace(topLeft, Fortress2Blueprints.SmallRoomTileTypes, Fortress2Blueprints.SmallRooms, ref region);
                 }
                 for (int k = 0; k < tinyRoomsPerMed; k++)
                 {
                     AttemptRoomPlace(topLeft, Fortress2Blueprints.TinyRoomTileTypes, Fortress2Blueprints.TinyRooms, ref region);
                 }
             }*/




        }
        
        public static void OccupyRegion(int x, int y, int width, int height, ref bool[,] region)
        {
            for(int i = 0; i <width; i++)
            {
                for(int j = 0; j< height; j++)
                {
                    region[i + x, j + y] = true;
                }
            }

        }
        public static bool CheckRegion(int x, int y, int width, int height, bool[,] region)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if(region[i + x, j + y])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public static void AttemptRoomPlace( Vector2 TopLeft, List<int[]>[] RoomTileTypes, int[,,,] Rooms, ref bool[,] region, int type = -1)
        {
            for (int i = 0; i < 100; i++)
            {
                int x = Main.rand.Next(region.GetLength(0) - Rooms.GetLength(3));
                int y = Main.rand.Next(region.GetLength(1) - Rooms.GetLength(2));
                if (CheckRegion(x, y, Rooms.GetLength(3), Rooms.GetLength(2), region))
                {
                    Fortress2Blueprints.BuildRoom((int)(TopLeft.X + x), (int)(TopLeft.Y + y), RoomTileTypes, Rooms);
                    OccupyRegion(x, y, Rooms.GetLength(3), Rooms.GetLength(2), ref region);
                    return;
                }
            }

        }
    }
}
