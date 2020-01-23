using Bunni.Resources.Components;
using Bunni.Resources.Modules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//rewrite upper wall corner code
//fill grid w tiles and go back and set textures

namespace UntitledDungeonGame.Resources.Game
{
    public class Dungeon
    {
        public List<DungeonRoom> Rooms;
        public Tile[,] DungeonEntityGrid;
        public DTypes.TileType[,] DungeonTileGrid;
        public List<Entity> Walls;

        public int MaxRooms;
        public int RoomMinSize;
        public int RoomMaxSize;

        public int DungeonWidth;
        public int DungeonHeight;
        /*public Vector2 DungeonSpawn;
        public Vector2 DungeonEnd;*/

        public Dungeon(int maxRooms, int roomMinSize, int roomMaxSize, int dungeonWidth, int dungeonHeight)
        {
            Rooms = new List<DungeonRoom>();
            Walls = new List<Entity>();
            MaxRooms = maxRooms;
            RoomMinSize = roomMinSize;
            RoomMaxSize = roomMaxSize;
            DungeonWidth = dungeonWidth;
            DungeonHeight = dungeonHeight;
            DungeonTileGrid = new DTypes.TileType[dungeonWidth, dungeonHeight];

            for (int i = 0; i < DungeonTileGrid.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < DungeonTileGrid.GetUpperBound(1) + 1; j++)
                {
                    DungeonTileGrid[i, j] = DTypes.TileType.Air;
                }
            }

            DungeonEntityGrid = new Tile[dungeonWidth, dungeonHeight];
            BuildDungeon();

        }

        public void AddTile(int x, int y, Tile e)
        {
            DungeonTileGrid[x, y] = e.TileType;
            DungeonEntityGrid[x, y] = e;
        }


        private void BuildDungeon()
        {
            List<Rectangle> RoomRectangles = new List<Rectangle>();
            Random randInt = new Random();
            for (int i = 0; i < MaxRooms; i++)
            {
                //in tile space
                int roomWidth = randInt.Next(RoomMinSize, RoomMaxSize);
                int roomHeight = randInt.Next(RoomMinSize, RoomMaxSize);

                int roomY = randInt.Next(2, DungeonHeight - roomHeight - 2);
                int roomX = randInt.Next(2, DungeonWidth - roomWidth - 2);

                Rectangle roomRect = new Rectangle(roomX, roomY, roomWidth, roomHeight);
                bool place = true;
                for (int j = 0; j < RoomRectangles.Count; j++)
                {
                    if (roomRect.Intersects(RoomRectangles.ElementAt(j)))
                    {
                        place = false;
                    }
                }
                if (place)
                {
                    RoomRectangles.Add(roomRect);
                    DungeonRoom Room = new DungeonRoom(roomX, roomY, roomWidth, roomHeight);
                    Rooms.Add(Room);
                }
            }
            RoomsToDungeonGrid(); //write rooms into grid
            BuildHallways();
            BuildWalls();
            GenerateFeatures();
        }

        private void GenerateFeatures()
        {
            //create spawn
            Random randRoom = new Random();
            int StartRoomNumber = randRoom.Next(Rooms.Count);
            DungeonRoom SpawnRoom = Rooms.ElementAt(StartRoomNumber);
            int x = randRoom.Next(1, SpawnRoom.RoomWidth);
            int y = randRoom.Next(1, SpawnRoom.RoomHeight);
            //DungeonSpawn = new Vector2(SpawnRoom.RoomX * x, SpawnRoom.RoomY * y);
            //SpawnRoom.RoomTiles[x, y] = Tile.Entrance;
            DungeonTileGrid[SpawnRoom.RoomX + x, SpawnRoom.RoomY + y] = DTypes.TileType.Entrance;
            DungeonEntityGrid[SpawnRoom.RoomX + x, SpawnRoom.RoomY + y].TileType = DTypes.TileType.Entrance;

            //create exit
            int EndRoomNumber = randRoom.Next(Rooms.Count);
            if(EndRoomNumber==StartRoomNumber)
            {
                EndRoomNumber++;
                EndRoomNumber %= Rooms.Count;
            }
            DungeonRoom EndRoom = Rooms.ElementAt(EndRoomNumber);
            int ex = randRoom.Next(1, EndRoom.RoomWidth);
            int ey = randRoom.Next(1, EndRoom.RoomHeight);
            //DungeonEnd = new Vector2(EndRoom.RoomX * ex, EndRoom.RoomY * ey);
            DungeonTileGrid[EndRoom.RoomX + ex, EndRoom.RoomY + ey] = DTypes.TileType.Exit;
            DungeonEntityGrid[EndRoom.RoomX + ex, EndRoom.RoomY + ey].TileType = DTypes.TileType.Exit;

            //create items
            //create enemies
        }

        private void RoomsToDungeonGrid()
        {
            for(int i=0;i<Rooms.Count;i++)
            {
                DungeonRoom cRoom = Rooms.ElementAt(i);
                for(int j=0;j<cRoom.RoomWidth;j++)
                {
                    for(int k=0;k<cRoom.RoomHeight;k++)
                    {
                        AddTile(cRoom.RoomX + j, cRoom.RoomY + k, cRoom.RoomEntities[j, k]);
                    }
                }
            }
        }

        private void PlaceHorizontal(int x1, int x2, int y)
        {
            Random randTex = new Random();
            if(x1>0 && x1<DungeonWidth && x2>0 && x2<DungeonWidth)
            {
                if (x1 < x2) //build to the right
                {
                    for (int i = 0; i < x2 - x1; i++)
                    {
                        int texture = randTex.Next(1, Globals.FLOORS_COUNT + 1);
                        Tile tempEnt = new Tile(Globals.Textures["floor" + texture], new Vector2((x1 + i) * Globals.TILE_WIDTH, y * Globals.TILE_HEIGHT));
                        tempEnt.TileType = DTypes.TileType.Floor;
                        tempEnt.TileVersion = DTypes.TileVersions.Floor;
                        AddTile(x1 + i, y, tempEnt);
                    }
                }else if(x1 > x2) //build to the left
                {
                    for(int i=x1;i>x2;i--)
                    {
                        int texture = randTex.Next(1, Globals.FLOORS_COUNT + 1);
                        Tile tempEnt = new Tile(Globals.Textures["floor" + texture], new Vector2(i * Globals.TILE_WIDTH, y * Globals.TILE_HEIGHT));
                        tempEnt.TileType = DTypes.TileType.Floor;
                        tempEnt.TileVersion = DTypes.TileVersions.Floor;
                        AddTile(i, y, tempEnt);
                    }
                }
            }
        }

        public void PlaceVertical(int y1, int y2, int x)
        {
            Random randTex = new Random();
            if (y1 > 0 && y1 < DungeonHeight && y2 > 0 && y2 < DungeonHeight)
            {
                if (y1 < y2) //build up
                {
                    for (int i = 0; i < y2 - y1; i++)
                    {
                        int texture = randTex.Next(1, Globals.FLOORS_COUNT + 1);
                        Tile tempEnt = new Tile(Globals.Textures["floor" + texture], new Vector2(x * Globals.TILE_WIDTH, (y1 + i) * Globals.TILE_HEIGHT));
                        tempEnt.TileType = DTypes.TileType.Floor;
                        tempEnt.TileVersion = DTypes.TileVersions.Floor;
                        AddTile(x, y1 + i, tempEnt);
                    }
                }
                else if (y1 > y2) //build down
                {
                    for (int i = y1; i > y2; i--)
                    {
                        int texture = randTex.Next(1, Globals.FLOORS_COUNT + 1);
                        Tile tempEnt = new Tile(Globals.Textures["floor" + texture], new Vector2(x * Globals.TILE_WIDTH, i*Globals.TILE_HEIGHT));
                        tempEnt.TileType = DTypes.TileType.Floor;
                        tempEnt.TileVersion = DTypes.TileVersions.Floor;
                        AddTile(x, i, tempEnt);
                    }
                }
            }
        }



        private void BuildHallways()
        {
            Random randInt = new Random();
            for(int i=0;i<Rooms.Count-1;i++)
            {
                DungeonRoom cRoom = Rooms.ElementAt(i);
                DungeonRoom Room2 = Rooms.ElementAt(i + 1);
                if (randInt.Next(0,1)==0)
                {
                    PlaceHorizontal(cRoom.RoomX + (cRoom.RoomWidth / 2), Room2.RoomX + (Room2.RoomWidth / 2), cRoom.RoomY + (cRoom.RoomHeight / 2));
                    PlaceVertical(cRoom.RoomY + (cRoom.RoomHeight / 2), Room2.RoomY + (Room2.RoomHeight / 2), Room2.RoomX + (Room2.RoomWidth / 2));
                }else
                {
                    PlaceVertical(cRoom.RoomY + (cRoom.RoomHeight / 2), Room2.RoomY + (Room2.RoomHeight / 2), cRoom.RoomX + (cRoom.RoomWidth / 2));
                    PlaceHorizontal(cRoom.RoomX + (cRoom.RoomWidth / 2), Room2.RoomX + (Room2.RoomWidth / 2), Room2.RoomY + (Room2.RoomHeight / 2));
                }
            }
            
        }

        private bool IsUpperWall(int x, int y)
        {
            if (DungeonTileGrid[x, y] != DTypes.TileType.Wall) return false;
            if (DungeonTileGrid[x, y + 1] != DTypes.TileType.Floor) return false;
            /*for (int k = 1; k < Math.Min(y, 2); k++)
            {
                if (DungeonTileGrid[x, y - k] == Tile.Floor)
                {
                    return false;
                }
            }*/
            return true;
        }

        private void BuildWalls()
        {
            Random randIntGen = new Random();
            for(int i=1;i<DungeonHeight-1;i++) //y
            {
                for(int j=1;j<DungeonWidth-1;j++) //x
                {
                    if(DungeonTileGrid[j,i] == DTypes.TileType.Floor)
                    {
                        //loop through all adjacent locations
                        for(int k=-1;k<2;k++) //y
                        {
                            for(int l=-1;l<2;l++) //x
                            {
                                if(DungeonTileGrid[j+l,i+k] == DTypes.TileType.Air)
                                {
                                    DungeonTileGrid[j + l, i + k] = DTypes.TileType.Wall;
                                }
                            }
                        }

                    }
                }
            }

            for (int i = 1; i < DungeonHeight - 1; i++) //y
            {
                for (int j = 1; j < DungeonWidth - 1; j++) //x
                {
                    if (DungeonTileGrid[j, i] == DTypes.TileType.Wall)
                    {
                        bool TopCornerPlaced = false;

                        if (!IsUpperWall(j, i) && IsUpperWall(j + 1, i) && DungeonTileGrid[j, i + 1] != DTypes.TileType.Air && DungeonTileGrid[j, i-1] != DTypes.TileType.Floor) //top left corners
                        {
                            if (DungeonTileGrid[j+1, i-1] != DTypes.TileType.Floor || DungeonTileGrid[j,i-1] != DTypes.TileType.Wall)
                            {
                                Tile tempEnt = new Tile(Globals.Textures["outer_topleftcorner"], new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                                tempEnt.TileType = DungeonTileGrid[j, i];
                                tempEnt.TileVersion = DTypes.TileVersions.Corner;
                                TopCornerPlaced = true;
                                Walls.Add(tempEnt);
                            }
                            
                        }

                        if (!IsUpperWall(j, i) && IsUpperWall(j - 1, i) && DungeonTileGrid[j, i + 1] != DTypes.TileType.Air && DungeonTileGrid[j,i-1] != DTypes.TileType.Floor) //top right corners
                        {
                            if (DungeonTileGrid[j - 1, i - 1] != DTypes.TileType.Floor || DungeonTileGrid[j, i - 1] != DTypes.TileType.Wall)
                            {
                                Tile tempEnt = new Tile(Globals.Textures["outer_toprightcorner"], new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                                tempEnt.TileType = DungeonTileGrid[j, i];
                                tempEnt.TileVersion = DTypes.TileVersions.Corner;
                                TopCornerPlaced = true;
                                Walls.Add(tempEnt);
                            }
                        }

                        if (IsUpperWall(j, i)) //top walls
                        {
                            if (DungeonTileGrid[j - 1, i] == DTypes.TileType.Wall && DungeonTileGrid[j + 1, i] == DTypes.TileType.Wall) //walls on both sides
                            {
                                Tile tempEnt = new Tile(Globals.Textures["topwall" + (j % 2)+"small"], new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                                tempEnt.TileType = DungeonTileGrid[j, i];
                                tempEnt.TileVersion = DTypes.TileVersions.TopWall;
                                Walls.Add(tempEnt);
                                continue;
                            }
                            else if (DungeonTileGrid[j - 1, i] == DTypes.TileType.Wall && DungeonTileGrid[j + 1, i] != DTypes.TileType.Wall) //walls only to the left
                            {
                                Tile tempEnt = new Tile(Globals.Textures["topwall_rightsmall"],new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                                tempEnt.TileType = DungeonTileGrid[j, i];
                                tempEnt.TileVersion = DTypes.TileVersions.TopWall;
                                Walls.Add(tempEnt);
                                continue;

                            }
                            else if (DungeonTileGrid[j + 1, i] == DTypes.TileType.Wall && DungeonTileGrid[j - 1, i] != DTypes.TileType.Wall) //walls only to the right
                            {
                                Tile tempEnt = new Tile(Globals.Textures["topwall_leftsmall"], new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                                tempEnt.TileType = DungeonTileGrid[j, i];
                                tempEnt.TileVersion = DTypes.TileVersions.TopWall;
                                Walls.Add(tempEnt);
                                continue;
                            }
                            else if (DungeonTileGrid[j - 1, i] == DTypes.TileType.Floor && DungeonTileGrid[j + 1, i] == DTypes.TileType.Floor) // no walls
                            {
                                Tile tempEnt = new Tile(Globals.Textures["topwall_leftrightsmall"], new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                                tempEnt.TileType = DungeonTileGrid[j, i];
                                tempEnt.TileVersion = DTypes.TileVersions.TopWall;
                                Walls.Add(tempEnt);
                                continue;
                            }
                        }
                        //left walls
                        if (DungeonTileGrid[j + 1, i] == DTypes.TileType.Floor || IsUpperWall(j + 1, i) && !TopCornerPlaced)
                        {
                            if (DungeonTileGrid[j, i - 1] == DTypes.TileType.Wall && DungeonTileGrid[j, i + 1] == DTypes.TileType.Wall)
                            {
                                Tile tempEnt = new Tile(Globals.Textures["wall_left"], new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                                tempEnt.TileType = DungeonTileGrid[j, i];
                                tempEnt.TileVersion = DTypes.TileVersions.LeftWall;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j, i - 1] == DTypes.TileType.Floor && DungeonTileGrid[j, i + 1] == DTypes.TileType.Wall)
                            {
                                Tile tempEnt = new Tile(Globals.Textures["inner_lefttop"], new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                                tempEnt.TileType = DungeonTileGrid[j, i];
                                tempEnt.TileVersion = DTypes.TileVersions.LeftWall;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j, i - 1] == DTypes.TileType.Floor && DungeonTileGrid[j, i + 1] == DTypes.TileType.Floor)
                            {
                                Tile tempEnt = new Tile(Globals.Textures["inner_leftmid"], new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                                tempEnt.TileType = DungeonTileGrid[j, i];
                                tempEnt.TileVersion = DTypes.TileVersions.LeftWall;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j, i - 1] == DTypes.TileType.Wall && DungeonTileGrid[j, i + 1] == DTypes.TileType.Floor)
                            {
                                Tile tempEnt = new Tile(Globals.Textures["inner_leftbottom"], new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                                tempEnt.TileType = DungeonTileGrid[j, i];
                                tempEnt.TileVersion = DTypes.TileVersions.LeftWall;
                                Walls.Add(tempEnt);
                            }
                        }

                        //right walls
                        if (DungeonTileGrid[j - 1, i] == DTypes.TileType.Floor || IsUpperWall(j-1,i) && !TopCornerPlaced)
                        {
                            if (DungeonTileGrid[j, i - 1] == DTypes.TileType.Wall && DungeonTileGrid[j, i + 1] == DTypes.TileType.Wall)
                            {
                                Tile tempEnt = new Tile(Globals.Textures["wall_right"], new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                                tempEnt.TileType = DungeonTileGrid[j, i];
                                tempEnt.TileVersion = DTypes.TileVersions.RightWall;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j, i - 1] == DTypes.TileType.Floor && DungeonTileGrid[j, i + 1] == DTypes.TileType.Wall)
                            {
                                Tile tempEnt = new Tile(Globals.Textures["inner_righttop"], new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                                tempEnt.TileType = DungeonTileGrid[j, i];
                                tempEnt.TileVersion = DTypes.TileVersions.RightWall;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j, i - 1] == DTypes.TileType.Floor && DungeonTileGrid[j, i + 1] == DTypes.TileType.Floor)
                            {
                                Tile tempEnt = new Tile(Globals.Textures["inner_rightmid"], new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                                tempEnt.TileType = DungeonTileGrid[j, i];
                                tempEnt.TileVersion = DTypes.TileVersions.RightWall;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j, i - 1] == DTypes.TileType.Wall && DungeonTileGrid[j, i + 1] == DTypes.TileType.Floor)
                            {
                                Tile tempEnt = new Tile(Globals.Textures["inner_rightbottom"], new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                                tempEnt.TileType = DungeonTileGrid[j, i];
                                tempEnt.TileVersion = DTypes.TileVersions.RightWall;
                                Walls.Add(tempEnt);
                            }
                        }

                        //inner top walls
                        if (DungeonTileGrid[j, i - 1] == DTypes.TileType.Floor)
                        {
                            if (DungeonTileGrid[j - 1, i] == DTypes.TileType.Wall && DungeonTileGrid[j - 1, i + 1] != DTypes.TileType.Floor && DungeonTileGrid[j + 1, i] == DTypes.TileType.Wall && DungeonTileGrid[j + 1, i + 1] != DTypes.TileType.Floor)
                            {
                                Tile tempEnt = new Tile(Globals.Textures["inner_leftright"], new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                                tempEnt.TileType = DungeonTileGrid[j, i];
                                tempEnt.TileVersion = DTypes.TileVersions.InnerTopWall;
                                tempEnt.GetComponent<Render>().ZLayer = -1;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j - 1, i] == DTypes.TileType.Wall && DungeonTileGrid[j - 1, i + 1] != DTypes.TileType.Floor && DungeonTileGrid[j + 1, i] == DTypes.TileType.Floor || DungeonTileGrid[j - 1, i] == DTypes.TileType.Wall && DungeonTileGrid[j - 1, i + 1] != DTypes.TileType.Floor && DungeonTileGrid[j + 1, i] == DTypes.TileType.Wall && DungeonTileGrid[j+1, i+1] == DTypes.TileType.Floor)
                            {
                                Tile tempEnt = new Tile(Globals.Textures["inner_right"], new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                                tempEnt.TileType = DungeonTileGrid[j, i];
                                tempEnt.TileVersion = DTypes.TileVersions.InnerTopWall;
                                tempEnt.GetComponent<Render>().ZLayer = -1;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j - 1, i] == DTypes.TileType.Floor && DungeonTileGrid[j + 1, i] == DTypes.TileType.Wall && DungeonTileGrid[j+1, i+1] != DTypes.TileType.Floor || DungeonTileGrid[j - 1, i] == DTypes.TileType.Wall && DungeonTileGrid[j - 1, i+1] == DTypes.TileType.Floor && DungeonTileGrid[j + 1, i] == DTypes.TileType.Wall && DungeonTileGrid[j + 1, i + 1] != DTypes.TileType.Floor)
                            {
                                Tile tempEnt = new Tile(Globals.Textures["inner_left"], new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                                tempEnt.TileType = DungeonTileGrid[j, i];
                                tempEnt.TileVersion = DTypes.TileVersions.InnerTopWall;
                                tempEnt.GetComponent<Render>().ZLayer = -1;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j - 1, i] == DTypes.TileType.Floor && DungeonTileGrid[j + 1, i] == DTypes.TileType.Floor || DungeonTileGrid[j - 1, i+1] == DTypes.TileType.Floor && DungeonTileGrid[j + 1, i+1] == DTypes.TileType.Floor && DungeonTileGrid[j-1,i] == DTypes.TileType.Wall && DungeonTileGrid[j+1,i] == DTypes.TileType.Wall || DungeonTileGrid[j + 1, i] == DTypes.TileType.Floor && DungeonTileGrid[j - 1, i] == DTypes.TileType.Wall && DungeonTileGrid[j-1,i+1] == DTypes.TileType.Floor)
                            {
                                Tile tempEnt = new Tile(Globals.Textures["inner_up"], new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                                tempEnt.TileType = DungeonTileGrid[j, i];
                                tempEnt.TileVersion = DTypes.TileVersions.InnerTopWall;
                                tempEnt.GetComponent<Render>().ZLayer = -1;
                                Walls.Add(tempEnt);
                            }
                        }

                        //bottom walls
                        if (DungeonTileGrid[j, i + 1] == DTypes.TileType.Floor)
                        {
                            if (DungeonTileGrid[j - 1, i] == DTypes.TileType.Wall && DungeonTileGrid[j + 1, i] == DTypes.TileType.Wall)
                            {
                                Tile tempEnt = new Tile(Globals.Textures["bot"], new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                                tempEnt.TileType = DungeonTileGrid[j, i];
                                tempEnt.TileVersion = DTypes.TileVersions.BottomWall;
                                tempEnt.GetComponent<Render>().ZLayer = -1;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j - 1, i] == DTypes.TileType.Wall && DungeonTileGrid[j + 1, i] == DTypes.TileType.Floor)
                            {
                                Tile tempEnt = new Tile(Globals.Textures["bot_right"], new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                                tempEnt.TileType = DungeonTileGrid[j, i];
                                tempEnt.TileVersion = DTypes.TileVersions.BottomWall;
                                tempEnt.GetComponent<Render>().ZLayer = -1;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j - 1, i] == DTypes.TileType.Floor && DungeonTileGrid[j + 1, i] == DTypes.TileType.Wall)
                            {
                                Tile tempEnt = new Tile(Globals.Textures["bot_left"], new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                                tempEnt.TileType = DungeonTileGrid[j, i];
                                tempEnt.TileVersion = DTypes.TileVersions.BottomWall;
                                tempEnt.GetComponent<Render>().ZLayer = -1;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j - 1, i] == DTypes.TileType.Floor && DungeonTileGrid[j + 1, i] == DTypes.TileType.Floor)
                            {
                                Tile tempEnt = new Tile(Globals.Textures["bot_rightleft"], new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                                tempEnt.TileType = DungeonTileGrid[j, i];
                                tempEnt.TileVersion = DTypes.TileVersions.BottomWall;
                                tempEnt.GetComponent<Render>().ZLayer = -1;
                                Walls.Add(tempEnt);
                            }
                        }

                        //corners
                        if(DungeonTileGrid[j,i-1] == DTypes.TileType.Wall && DungeonTileGrid[j+1,i] == DTypes.TileType.Wall && DungeonTileGrid[j+1,i-1] == DTypes.TileType.Floor && !IsUpperWall(j+1,i))
                        {
                            Tile tempEnt = new Tile(Globals.Textures["outer_bottomleftcorner"], new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                            tempEnt.TileType = DungeonTileGrid[j, i];
                            tempEnt.TileVersion = DTypes.TileVersions.Corner;
                            tempEnt.GetComponent<Render>().ZLayer = -1;
                            Walls.Add(tempEnt);
                        }
                        if (DungeonTileGrid[j, i - 1] == DTypes.TileType.Wall && DungeonTileGrid[j - 1, i] == DTypes.TileType.Wall && DungeonTileGrid[j -1, i - 1] == DTypes.TileType.Floor && !IsUpperWall(j-1,i))
                        {
                            Tile tempEnt = new Tile(Globals.Textures["outer_bottomrightcorner"], new Vector2(j * Globals.TILE_WIDTH, i * Globals.TILE_HEIGHT));
                            tempEnt.TileType = DungeonTileGrid[j, i];
                            tempEnt.TileVersion = DTypes.TileVersions.Corner;
                            tempEnt.GetComponent<Render>().ZLayer = -1;
                            Walls.Add(tempEnt);
                        }
                    }
                }
            }
        }

        public class DungeonRoom
        {
            /// <summary>
            /// The tiles in the room. This is things like the floor/walls. does not include entities on ground or enemies.
            /// </summary>
            public DTypes.TileType[,] RoomTiles;
            public Tile[,] RoomEntities;
            public int RoomX;
            public int RoomY;
            public int RoomWidth;
            public int RoomHeight;

            public DungeonRoom(int roomX, int roomY, int roomWidth, int roomHeight)
            {
                RoomTiles = new DTypes.TileType[roomWidth, roomHeight];
                RoomX = roomX;
                RoomY = roomY;
                RoomWidth = roomWidth;
                RoomHeight = roomHeight;
                BuildRoom();
                BuildEntities();
            }

            private void BuildRoom()
            {
                for(int i=0;i<RoomWidth;i++)
                {
                    for(int j=0;j<RoomHeight;j++)
                    {
                        RoomTiles[i, j] = DTypes.TileType.Floor;
                    }
                }
            }

            public void BuildEntities()
            {
                Tile[,] entities = new Tile[RoomWidth, RoomHeight];
                Random randTex = new Random();
                for (int i = 0; i < RoomWidth; i++)
                {
                    for (int j = 0; j < RoomHeight; j++)
                    {
                        int texture = randTex.Next(1, Globals.FLOORS_COUNT + 1);
                        Tile TileEnt = new Tile(Globals.Textures["floor" + texture], new Vector2((RoomX * Globals.TILE_WIDTH) + (i * Globals.TILE_WIDTH), (RoomY * Globals.TILE_HEIGHT) + (j * Globals.TILE_HEIGHT))); //convert to global position
                        TileEnt.TileType = RoomTiles[i, j];
                        entities[i,j] = TileEnt;
                    }
                }
                RoomEntities = entities;
            }

        }
    }
}
