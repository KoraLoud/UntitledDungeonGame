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
        public Entity[,] DungeonEntityGrid;
        public Tile[,] DungeonTileGrid;
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
            DungeonTileGrid = new Tile[dungeonWidth, dungeonHeight];

            for (int i = 0; i < DungeonTileGrid.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < DungeonTileGrid.GetUpperBound(1) + 1; j++)
                {
                    DungeonTileGrid[i, j] = Tile.Air;
                }
            }

            DungeonEntityGrid = new Entity[dungeonWidth, dungeonHeight];
            BuildDungeon();

        }

        public void AddTile(int x, int y, Entity e)
        {
            DungeonTileGrid[x, y] = GetEntityTag(e);
            DungeonEntityGrid[x, y] = e;
        }

        public Tile GetEntityTag(Entity e)
        {
            switch(e.Tag)
            {
                case BniTypes.Tag.Wall:
                    return Tile.Wall;
                case BniTypes.Tag.Floor:
                    return Tile.Floor;
                default:
                    return Tile.Air;
            }
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
                        Entity tempEnt = GetEmptyTile(Globals.Textures["floor" + texture]);
                        PositionVector posEnt = tempEnt.GetComponent<PositionVector>();
                        posEnt.X = (x1 + i) * Globals.TILE_WIDTH;
                        posEnt.Y = y * Globals.TILE_HEIGHT;
                        tempEnt.Tag = BniTypes.Tag.Floor;
                        AddTile(x1 + i, y, tempEnt);
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
                        Entity tempEnt = GetEmptyTile(Globals.Textures["floor" + texture]);
                        PositionVector posEnt = tempEnt.GetComponent<PositionVector>();
                        posEnt.X = x * Globals.TILE_WIDTH;
                        posEnt.Y = (y1+i) * Globals.TILE_HEIGHT;
                        tempEnt.Tag = BniTypes.Tag.Floor;
                        AddTile(x, y1 + i, tempEnt);
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
            if (DungeonTileGrid[x, y] != Tile.Wall) return false;
            if (DungeonTileGrid[x, y + 1] != Tile.Floor) return false;
            /*for (int k = 1; k < Math.Min(y, 2); k++)
            {
                if (DungeonTileGrid[x, y - k] == Tile.Floor)
                {
                    return false;
                }
            }*/
            return true;
        }

        private bool IsUpper(int x, int y)
        {
            if (DungeonTileGrid[x, y] != Tile.Wall) return false;
            /*for (int k = 1; k < Math.Min(y,2); k++)
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
                    if(DungeonTileGrid[j,i] == Tile.Floor)
                    {
                        //loop through all adjacent locations
                        for(int k=-1;k<2;k++) //y
                        {
                            for(int l=-1;l<2;l++) //x
                            {
                                if(DungeonTileGrid[j+l,i+k] == Tile.Air)
                                {
                                    DungeonTileGrid[j + l, i + k] = Tile.Wall;
                                    /*Entity tempEnt = GetEmptyTile(Globals.Textures["floor1"]);
                                    PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                                    tempPos.X = (j+l) * Globals.TILE_WIDTH;
                                    tempPos.Y = (i+k) * Globals.TILE_HEIGHT;
                                    tempEnt.GetComponent<Render>().Color = Color.Purple;
                                    Walls.Add(tempEnt);*/
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
                    if (DungeonTileGrid[j, i] == Tile.Wall)
                    {
                        bool TopWall = IsUpper(j, i);

                        if (TopWall)
                        {
                            if (!IsUpperWall(j, i) && IsUpperWall(j + 1, i) && DungeonTileGrid[j + 1, i] == Tile.Wall && DungeonTileGrid[j, i + 1] != Tile.Air) //top left corners
                            {
                                Entity tempEnt = GetEmptyTile(Globals.Textures["outer_topleftcorner"]);
                                PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                                tempPos.X = j * Globals.TILE_WIDTH;
                                tempPos.Y = i * Globals.TILE_HEIGHT;
                                tempEnt.Tag = BniTypes.Tag.Wall;
                                Walls.Add(tempEnt);
                            }

                            if (!IsUpperWall(j, i) && IsUpperWall(j - 1, i) && DungeonTileGrid[j, i + 1] != Tile.Air) //top right corners
                            {
                                Entity tempEnt = GetEmptyTile(Globals.Textures["outer_toprightcorner"]);
                                PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                                tempPos.X = j * Globals.TILE_WIDTH;
                                tempPos.Y = i * Globals.TILE_HEIGHT;
                                tempEnt.Tag = BniTypes.Tag.Wall;
                                Walls.Add(tempEnt);
                            }

                            if (IsUpperWall(j, i)) //top walls
                            {
                                if (DungeonTileGrid[j - 1, i] == Tile.Wall && DungeonTileGrid[j + 1, i] == Tile.Wall) //walls on both sides
                                {
                                    Entity tempEnt = GetEmptyTile(Globals.Textures["topwall" + (j % 2)]);
                                    PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                                    tempPos.X = j * Globals.TILE_WIDTH;
                                    tempPos.Y = i * Globals.TILE_HEIGHT;
                                    tempEnt.Tag = BniTypes.Tag.Wall;
                                    Walls.Add(tempEnt);
                                    continue;
                                }
                                else if (DungeonTileGrid[j - 1, i] == Tile.Wall && DungeonTileGrid[j + 1, i] != Tile.Wall) //walls only to the left
                                {
                                    Entity tempEnt = GetEmptyTile(Globals.Textures["topwall_right"]);
                                    PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                                    tempPos.X = j * Globals.TILE_WIDTH;
                                    tempPos.Y = i * Globals.TILE_HEIGHT;
                                    tempEnt.Tag = BniTypes.Tag.Wall;
                                    Walls.Add(tempEnt);
                                    continue;

                                }
                                else if (DungeonTileGrid[j + 1, i] == Tile.Wall && DungeonTileGrid[j - 1, i] != Tile.Wall) //walls only to the right
                                {
                                    Entity tempEnt = GetEmptyTile(Globals.Textures["topwall_left"]);
                                    PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                                    tempPos.X = j * Globals.TILE_WIDTH;
                                    tempPos.Y = i * Globals.TILE_HEIGHT;
                                    tempEnt.Tag = BniTypes.Tag.Wall;
                                    Walls.Add(tempEnt);
                                    continue;
                                }
                                else if (DungeonTileGrid[j - 1, i] == Tile.Floor && DungeonTileGrid[j + 1, i] == Tile.Floor) // no walls
                                {
                                    Entity tempEnt = GetEmptyTile(Globals.Textures["topwall_leftright"]);
                                    PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                                    tempPos.X = j * Globals.TILE_WIDTH;
                                    tempPos.Y = i * Globals.TILE_HEIGHT;
                                    tempEnt.Tag = BniTypes.Tag.Wall;
                                    Walls.Add(tempEnt);
                                    continue;
                                }
                            }
                        }
                        //left walls
                        if (DungeonTileGrid[j + 1, i] == Tile.Floor)
                        {
                            if (DungeonTileGrid[j, i - 1] == Tile.Wall && DungeonTileGrid[j, i + 1] == Tile.Wall)
                            {
                                Entity tempEnt = GetEmptyTile(Globals.Textures["wall_left"]);
                                PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                                tempPos.X = j * Globals.TILE_WIDTH;
                                tempPos.Y = i * Globals.TILE_HEIGHT;
                                tempEnt.Tag = BniTypes.Tag.Wall;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j, i - 1] == Tile.Floor && DungeonTileGrid[j, i + 1] == Tile.Wall)
                            {
                                Entity tempEnt = GetEmptyTile(Globals.Textures["inner_lefttop"]);
                                PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                                tempPos.X = j * Globals.TILE_WIDTH;
                                tempPos.Y = i * Globals.TILE_HEIGHT;
                                tempEnt.Tag = BniTypes.Tag.Wall;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j, i - 1] == Tile.Floor && DungeonTileGrid[j, i + 1] == Tile.Floor)
                            {
                                Entity tempEnt = GetEmptyTile(Globals.Textures["inner_leftmid"]);
                                PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                                tempPos.X = j * Globals.TILE_WIDTH;
                                tempPos.Y = i * Globals.TILE_HEIGHT;
                                tempEnt.Tag = BniTypes.Tag.Wall;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j, i - 1] == Tile.Wall && DungeonTileGrid[j, i + 1] == Tile.Floor)
                            {
                                Entity tempEnt = GetEmptyTile(Globals.Textures["inner_leftbottom"]);
                                PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                                tempPos.X = j * Globals.TILE_WIDTH;
                                tempPos.Y = i * Globals.TILE_HEIGHT;
                                tempEnt.Tag = BniTypes.Tag.Wall;
                                Walls.Add(tempEnt);
                            }
                        }

                        //right walls
                        if (DungeonTileGrid[j - 1, i] == Tile.Floor)
                        {
                            if (DungeonTileGrid[j, i - 1] == Tile.Wall && DungeonTileGrid[j, i + 1] == Tile.Wall)
                            {
                                Entity tempEnt = GetEmptyTile(Globals.Textures["wall_right"]);
                                PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                                tempPos.X = j * Globals.TILE_WIDTH;
                                tempPos.Y = i * Globals.TILE_HEIGHT;
                                tempEnt.Tag = BniTypes.Tag.Wall;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j, i - 1] == Tile.Floor && DungeonTileGrid[j, i + 1] == Tile.Wall)
                            {
                                Entity tempEnt = GetEmptyTile(Globals.Textures["inner_righttop"]);
                                PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                                tempPos.X = j * Globals.TILE_WIDTH;
                                tempPos.Y = i * Globals.TILE_HEIGHT;
                                tempEnt.Tag = BniTypes.Tag.Wall;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j, i - 1] == Tile.Floor && DungeonTileGrid[j, i + 1] == Tile.Floor)
                            {
                                Entity tempEnt = GetEmptyTile(Globals.Textures["inner_rightmid"]);
                                PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                                tempPos.X = j * Globals.TILE_WIDTH;
                                tempPos.Y = i * Globals.TILE_HEIGHT;
                                tempEnt.Tag = BniTypes.Tag.Wall;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j, i - 1] == Tile.Wall && DungeonTileGrid[j, i + 1] == Tile.Floor)
                            {
                                Entity tempEnt = GetEmptyTile(Globals.Textures["inner_rightbottom"]);
                                PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                                tempPos.X = j * Globals.TILE_WIDTH;
                                tempPos.Y = i * Globals.TILE_HEIGHT;
                                tempEnt.Tag = BniTypes.Tag.Wall;
                                Walls.Add(tempEnt);
                            }
                        }

                        //top walls
                        if (DungeonTileGrid[j, i - 1] == Tile.Floor)
                        {
                            if (DungeonTileGrid[j - 1, i] == Tile.Wall && DungeonTileGrid[j + 1, i] == Tile.Wall)
                            {
                                Entity tempEnt = GetEmptyTile(Globals.Textures["inner_leftright"]);
                                PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                                tempPos.X = j * Globals.TILE_WIDTH;
                                tempPos.Y = i * Globals.TILE_HEIGHT;
                                tempEnt.Tag = BniTypes.Tag.Wall;
                                tempEnt.GetComponent<Render>().ZLayer = -1;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j - 1, i] == Tile.Wall && DungeonTileGrid[j + 1, i] == Tile.Floor)
                            {
                                Entity tempEnt = GetEmptyTile(Globals.Textures["inner_right"]);
                                PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                                tempPos.X = j * Globals.TILE_WIDTH;
                                tempPos.Y = i * Globals.TILE_HEIGHT;
                                tempEnt.Tag = BniTypes.Tag.Wall;
                                tempEnt.GetComponent<Render>().ZLayer = -1;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j - 1, i] == Tile.Floor && DungeonTileGrid[j + 1, i] == Tile.Wall)
                            {
                                Entity tempEnt = GetEmptyTile(Globals.Textures["inner_left"]);
                                PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                                tempPos.X = j * Globals.TILE_WIDTH;
                                tempPos.Y = i * Globals.TILE_HEIGHT;
                                tempEnt.Tag = BniTypes.Tag.Wall;
                                tempEnt.GetComponent<Render>().ZLayer = -1;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j - 1, i] == Tile.Floor && DungeonTileGrid[j + 1, i] == Tile.Floor)
                            {
                                Entity tempEnt = GetEmptyTile(Globals.Textures["inner_up"]);
                                PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                                tempPos.X = j * Globals.TILE_WIDTH;
                                tempPos.Y = i * Globals.TILE_HEIGHT;
                                tempEnt.Tag = BniTypes.Tag.Wall;
                                tempEnt.GetComponent<Render>().ZLayer = -1;
                                Walls.Add(tempEnt);
                            }
                        }

                        //bottom walls
                        if (DungeonTileGrid[j, i + 1] == Tile.Floor)
                        {
                            if (DungeonTileGrid[j - 1, i] == Tile.Wall && DungeonTileGrid[j + 1, i] == Tile.Wall)
                            {
                                Entity tempEnt = GetEmptyTile(Globals.Textures["bot"]);
                                PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                                tempPos.X = j * Globals.TILE_WIDTH;
                                tempPos.Y = i * Globals.TILE_HEIGHT;
                                tempEnt.Tag = BniTypes.Tag.Wall;
                                tempEnt.GetComponent<Render>().ZLayer = -1;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j - 1, i] == Tile.Wall && DungeonTileGrid[j + 1, i] == Tile.Floor)
                            {
                                Entity tempEnt = GetEmptyTile(Globals.Textures["bot_right"]);
                                PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                                tempPos.X = j * Globals.TILE_WIDTH;
                                tempPos.Y = i * Globals.TILE_HEIGHT;
                                tempEnt.Tag = BniTypes.Tag.Wall;
                                tempEnt.GetComponent<Render>().ZLayer = -1;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j - 1, i] == Tile.Floor && DungeonTileGrid[j + 1, i] == Tile.Wall)
                            {
                                Entity tempEnt = GetEmptyTile(Globals.Textures["bot_left"]);
                                PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                                tempPos.X = j * Globals.TILE_WIDTH;
                                tempPos.Y = i * Globals.TILE_HEIGHT;
                                tempEnt.Tag = BniTypes.Tag.Wall;
                                tempEnt.GetComponent<Render>().ZLayer = -1;
                                Walls.Add(tempEnt);
                            }
                            else if (DungeonTileGrid[j - 1, i] == Tile.Floor && DungeonTileGrid[j + 1, i] == Tile.Floor)
                            {
                                Entity tempEnt = GetEmptyTile(Globals.Textures["bot_rightleft"]);
                                PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                                tempPos.X = j * Globals.TILE_WIDTH;
                                tempPos.Y = i * Globals.TILE_HEIGHT;
                                tempEnt.Tag = BniTypes.Tag.Wall;
                                tempEnt.GetComponent<Render>().ZLayer = -1;
                                Walls.Add(tempEnt);
                            }
                        }

                        //corners
                        if(DungeonTileGrid[j,i-1] == Tile.Wall && DungeonTileGrid[j+1,i] == Tile.Wall && DungeonTileGrid[j+1,i-1] == Tile.Floor)
                        {
                            Entity tempEnt = GetEmptyTile(Globals.Textures["outer_bottomleftcorner"]);
                            PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                            tempPos.X = j * Globals.TILE_WIDTH;
                            tempPos.Y = i * Globals.TILE_HEIGHT;
                            tempEnt.Tag = BniTypes.Tag.Wall;
                            tempEnt.GetComponent<Render>().ZLayer = -1;
                            Walls.Add(tempEnt);
                        }
                        if (DungeonTileGrid[j, i - 1] == Tile.Wall && DungeonTileGrid[j - 1, i] == Tile.Wall && DungeonTileGrid[j -1, i - 1] == Tile.Floor)
                        {
                            Entity tempEnt = GetEmptyTile(Globals.Textures["outer_bottomrightcorner"]);
                            PositionVector tempPos = tempEnt.GetComponent<PositionVector>();
                            tempPos.X = j * Globals.TILE_WIDTH;
                            tempPos.Y = i * Globals.TILE_HEIGHT;
                            tempEnt.Tag = BniTypes.Tag.Wall;
                            tempEnt.GetComponent<Render>().ZLayer = -1;
                            Walls.Add(tempEnt);
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
                        TileEnt.Tag = BniTypes.Tag.Floor;
                        entities[i,j] = TileEnt;
                    }
                }
                RoomEntities = entities;
            }

        }
    }
}
