using Bunni.Resources.Components;
using Bunni.Resources.Modules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntitledDungeonGame.Resources.Game
{
    public class Dungeon
    {
        public List<DungeonRoom> Rooms;
        public Entity[,] DungeonGrid;
        public List<Entity> Walls;

        public int MaxRooms;
        public int RoomMinSize;
        public int RoomMaxSize;

        public int DungeonWidth;
        public int DungeonHeight;

        public Dungeon(int maxRooms, int roomMinSize, int roomMaxSize, int dungeonWidth, int dungeonHeight)
        {
            Rooms = new List<DungeonRoom>();
            Walls = new List<Entity>();
            MaxRooms = maxRooms;
            RoomMinSize = roomMinSize;
            RoomMaxSize = roomMaxSize;
            DungeonWidth = dungeonWidth;
            DungeonHeight = dungeonHeight;
            DungeonGrid = new Entity[dungeonWidth, dungeonHeight];
            BuildDungeon();

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
                        DungeonGrid[cRoom.RoomX + j, cRoom.RoomY + k] = cRoom.RoomEntities[j,k];
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
                        Entity tempEnt = GetEmptyTile(Globals.Textures["floor" + texture]);
                        PositionVector posEnt = tempEnt.GetComponent<PositionVector>();
                        posEnt.X = (x1 + i) * Globals.TILE_WIDTH;
                        posEnt.Y = y * Globals.TILE_HEIGHT;
                        tempEnt.Tag = BniTypes.Tag.Floor;
                        DungeonGrid[x1 + i, y] = tempEnt;
                    }
                }else if(x1 > x2) //build to the left
                {
                    for(int i=x1;i>x2;i--)
                    {
                        int texture = randTex.Next(1, Globals.FLOORS_COUNT + 1);
                        Entity tempEnt = GetEmptyTile(Globals.Textures["floor" + texture]);
                        PositionVector posEnt = tempEnt.GetComponent<PositionVector>();
                        posEnt.X = i * Globals.TILE_WIDTH;
                        posEnt.Y = y * Globals.TILE_HEIGHT;
                        tempEnt.Tag = BniTypes.Tag.Floor;
                        DungeonGrid[i, y] = tempEnt;
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
                        Entity tempEnt = GetEmptyTile(Globals.Textures["floor" + texture]);
                        PositionVector posEnt = tempEnt.GetComponent<PositionVector>();
                        posEnt.X = x * Globals.TILE_WIDTH;
                        posEnt.Y = (y1+i) * Globals.TILE_HEIGHT;
                        tempEnt.Tag = BniTypes.Tag.Floor;
                        DungeonGrid[x, y1+i] = tempEnt;
                    }
                }
                else if (y1 > y2) //build down
                {
                    for (int i = y1; i > y2; i--)
                    {
                        int texture = randTex.Next(1, Globals.FLOORS_COUNT + 1);
                        Entity tempEnt = GetEmptyTile(Globals.Textures["floor" + texture]);
                        PositionVector posEnt = tempEnt.GetComponent<PositionVector>();
                        posEnt.X = x * Globals.TILE_WIDTH;
                        posEnt.Y = i * Globals.TILE_HEIGHT;
                        tempEnt.Tag = BniTypes.Tag.Floor;
                        DungeonGrid[x,i] = tempEnt;
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

        private void BuildWalls()
        {
            Random randIntGen = new Random();
            for(int i=1;i<DungeonHeight-1;i++) //y
            {
                for(int j=1;j<DungeonWidth-1;j++) //x
                {
                    if(DungeonGrid[j,i] != null && DungeonGrid[j,i].Tag == BniTypes.Tag.Floor)
                    {
                        //loop through all adjacent locations
                        for(int k=-1;k<2;k++) //y
                        {
                            for(int l=-1;l<2;l++) //x
                            {
                                if(DungeonGrid[j+l,i+k] == null)
                                {
                                    bool IsTopWall = true;
                                    Entity tempEnt = GetEmptyTile(Globals.Textures["floor1"]);
                                    Render tempEntRender = tempEnt.GetComponent<Render>();
                                    
                                    for(int ia=0;ia<i+k;ia++)
                                    {
                                        if(DungeonGrid[j+l,(i+k)-ia] != null)
                                        {
                                            IsTopWall = false;
                                        }
                                    }
                                    if(IsTopWall)
                                    {

                                        if(DungeonGrid[(j + l) + 1, i + k] != null && DungeonGrid[(j + l) + 1, i + k].Tag == BniTypes.Tag.Floor) //get tile to the right
                                        {
                                            tempEntRender.Texture = Globals.Textures["wall_right"];
                                        }
                                        else if(DungeonGrid[(j + l) - 1, i + k] != null)
                                        {
                                            tempEntRender.Texture = Globals.Textures["wall_left"];
                                        }
                                        else
                                        {
                                            if ((j + l) % 2 == 0)
                                            {
                                                tempEntRender.Texture = Globals.Textures["wall1"];
                                            }
                                            else
                                            {
                                                tempEntRender.Texture = Globals.Textures["wall2"];
                                            }
                                        }
                                    }

                                    //set position and add to dungeon grid
                                    PositionVector posEnt = tempEnt.GetComponent<PositionVector>();
                                    posEnt.X = (j+l) * Globals.TILE_WIDTH;
                                    posEnt.Y = (i+k) * Globals.TILE_HEIGHT;
                                    tempEnt.Tag = BniTypes.Tag.Wall;
                                    DungeonGrid[j+l, i+k] = tempEnt;
                                }
                            }
                        }

                    }
                }
            }
        }

        private Entity GetEmptyTile(Texture2D tex)
        {
            Entity tempEnt = new Entity();
            PositionVector posEnt = new PositionVector();
            Render rendEnt = new Render(tex);
            tempEnt.AddComponent(posEnt);
            tempEnt.AddComponent(rendEnt);
            return tempEnt;
        }

        public class DungeonRoom
        {
            /// <summary>
            /// The tiles in the room. This is things like the floor/walls. does not include entities on ground or enemies.
            /// </summary>
            public Tile[,] RoomTiles;
            public Entity[,] RoomEntities;
            public int RoomX;
            public int RoomY;
            public int RoomWidth;
            public int RoomHeight;

            public DungeonRoom(int roomX, int roomY, int roomWidth, int roomHeight)
            {
                RoomTiles = new Tile[roomWidth, roomHeight];
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
                        RoomTiles[i, j] = Tile.Floor;
                    }
                }
            }

            public void BuildEntities()
            {
                Entity[,] entities = new Entity[RoomWidth, RoomHeight];
                Random randTex = new Random();
                for (int i = 0; i < RoomWidth; i++)
                {
                    for (int j = 0; j < RoomHeight; j++)
                    {

                        Entity TileEnt = new Entity();
                        Render entRender = new Render(Globals.Textures["floor1"]);
                        switch(RoomTiles[i,j])
                        {
                            case Tile.Floor:
                                {
                                    int texture = randTex.Next(1, Globals.FLOORS_COUNT+1);
                                    entRender.Texture = Globals.Textures["floor" + texture];
                                }
                                break;
                        }
                        PositionVector entPositionVector = new PositionVector();
                        entPositionVector.X = (RoomX * Globals.TILE_WIDTH) + (i * Globals.TILE_WIDTH); //convert to global position
                        entPositionVector.Y = (RoomY * Globals.TILE_HEIGHT) + (j * Globals.TILE_HEIGHT); //convert to global position
                        TileEnt.AddComponent(entPositionVector);
                        TileEnt.AddComponent(entRender);
                        switch(RoomTiles[i,j])
                        {
                            case Tile.Wall:
                                TileEnt.Tag = BniTypes.Tag.Wall;
                                break;
                            case Tile.Floor:
                                TileEnt.Tag = BniTypes.Tag.Floor;
                                break;
                            default:
                                TileEnt.Tag = BniTypes.Tag.Air;
                                break;
                        }
                        entities[i,j] = TileEnt;
                    }
                }
                RoomEntities = entities;
            }

        }
    }
}
