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

        private bool IsUpperWall(Entity e)
        {
            if (e.HasComponent<Render>())
            {
                Render entityRender = e.GetComponent<Render>();
                return entityRender.Texture == Globals.Textures["wall1"] || entityRender.Texture == Globals.Textures["wall2"] ||
                    entityRender.Texture == Globals.Textures["wall_left"] || entityRender.Texture == Globals.Textures["wall_right"];
            }
            return false;
        }

        private void BuildWalls()
        {
            Random randIntGen = new Random();
            for(int i=1;i<DungeonHeight-1;i++) //y
            {
                for(int j=1;j<DungeonWidth-1;j++) //x
                {
                    if(DungeonTileGrid[j,i] != Tile.Air && DungeonTileGrid[j,i] == Tile.Floor)
                    {
                        //loop through all adjacent locations
                        for(int k=-1;k<2;k++) //y
                        {
                            for(int l=-1;l<2;l++) //x
                            {
                                if(DungeonTileGrid[j+l,i+k] == Tile.Air)
                                {
                                    bool IsTopWall = true;
                                    Entity tempEnt = null;
                                    
                                    for(int ia=0;ia<Math.Min(2,i+k);ia++)
                                    {
                                        if(DungeonTileGrid[j + l, (i + k) - ia] != Tile.Air && DungeonTileGrid[j+l,(i+k)-ia] == Tile.Floor)
                                        {
                                            IsTopWall = false;
                                            break;
                                        }
                                    }
                                    if(DungeonTileGrid[j+l,(i+k)+1] == Tile.Air)
                                    {
                                        IsTopWall = false;
                                    }
                                    if (IsTopWall)
                                    {

                                        if(DungeonTileGrid[(j + l) + 1, i + k] != Tile.Air && DungeonTileGrid[(j + l) + 1, i + k] == Tile.Floor) //get tile to the right
                                        {
                                            tempEnt = GetEmptyTile(Globals.Textures["wall_right"]);
                                        }
                                        else if(DungeonTileGrid[(j + l) - 1, i + k] != Tile.Air && DungeonTileGrid[(j + l) - 1, i + k] == Tile.Floor)
                                        {
                                            tempEnt = GetEmptyTile(Globals.Textures["wall_left"]);
                                        }
                                        else
                                        {
                                            if ((j + l) % 2 == 0)
                                            {
                                                tempEnt = GetEmptyTile(Globals.Textures["wall1"]);
                                            }
                                            else
                                            {
                                                tempEnt = GetEmptyTile(Globals.Textures["wall2"]);
                                            }
                                        }
                                    }

                                    //set position and add to dungeon grid
                                    if (tempEnt != null)
                                    {
                                        PositionVector posEnt = tempEnt.GetComponent<PositionVector>();
                                        posEnt.X = (j + l) * Globals.TILE_WIDTH;
                                        posEnt.Y = (i + k) * Globals.TILE_HEIGHT;
                                        tempEnt.Tag = BniTypes.Tag.Wall;
                                        //DungeonGrid[j+l, i+k] = tempEnt;
                                        AddTile(j + l, i + k, tempEnt);
                                        Walls.Add(tempEnt);
                                    }
                                    else
                                    {
                                        DungeonTileGrid[j + l, i + k] = Tile.Wall;
                                    }
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
                    if(DungeonTileGrid[j,i] == Tile.Wall)
                    {
                        Entity tempWallEnt = null;
                        if(DungeonEntityGrid[j+1,i] != null && IsUpperWall(DungeonEntityGrid[j+1,i]) && DungeonEntityGrid[j, i] == null)
                        {
                            if (DungeonTileGrid[j+1, i - 1] == Tile.Floor)
                            {
                                tempWallEnt = GetEmptyTile(Globals.Textures["outer_left"]);
                                PositionVector posEnt = tempWallEnt.GetComponent<PositionVector>();
                                posEnt.X = j * Globals.TILE_WIDTH;
                                posEnt.Y = i * Globals.TILE_HEIGHT;
                            }
                            else
                            {
                                tempWallEnt = GetEmptyTile(Globals.Textures["outer_upleftcorner"]);
                                PositionVector posEnt = tempWallEnt.GetComponent<PositionVector>();
                                posEnt.X = j * Globals.TILE_WIDTH;
                                posEnt.Y = i * Globals.TILE_HEIGHT;
                            }
                        }
                        if (DungeonEntityGrid[j - 1, i] != null && IsUpperWall(DungeonEntityGrid[j - 1, i]) && DungeonEntityGrid[j, i] == null)
                        {
                            if (DungeonTileGrid[j-1, i - 1] == Tile.Floor)
                            {
                                tempWallEnt = GetEmptyTile(Globals.Textures["outer_right"]);
                                PositionVector posEnt = tempWallEnt.GetComponent<PositionVector>();
                                posEnt.X = j * Globals.TILE_WIDTH;
                                posEnt.Y = i * Globals.TILE_HEIGHT;
                            }
                            else
                            {
                                tempWallEnt = GetEmptyTile(Globals.Textures["outer_uprightcorner"]);
                                PositionVector posEnt = tempWallEnt.GetComponent<PositionVector>();
                                posEnt.X = j * Globals.TILE_WIDTH;
                                posEnt.Y = i * Globals.TILE_HEIGHT;
                            }
                        }
                        if (tempWallEnt != null)
                        {
                            Walls.Add(tempWallEnt);
                        }
                    }
                    else if(DungeonTileGrid[j,i] == Tile.Floor)
                    {
                        //check for walls on the left of the tiles
                        bool IsOuterLeft = true;
                        for(int k=0;k<j;k++)
                        {
                            if(DungeonTileGrid[j-k,i] == Tile.Floor)
                            {
                                IsOuterLeft = false;
                            }
                        }
                        if(IsOuterLeft)
                        {
                            Entity tempWallEnt = GetEmptyTile(Globals.Textures["outer_right"]);
                            PositionVector posEnt = tempWallEnt.GetComponent<PositionVector>();
                            posEnt.X = (j-1) * Globals.TILE_WIDTH;
                            posEnt.Y = i * Globals.TILE_HEIGHT;
                            Walls.Add(tempWallEnt);
                        }
                        else if(DungeonEntityGrid[j-1,i] == null)
                        {
                            Entity tempWallEnt = GetEmptyTile(Globals.Textures["inner_right"]);
                            PositionVector posEnt = tempWallEnt.GetComponent<PositionVector>();
                            posEnt.X = (j-1) * Globals.TILE_WIDTH;
                            posEnt.Y = i * Globals.TILE_HEIGHT;
                            Walls.Add(tempWallEnt);
                        }

                        //check for walls on the right of the tiles
                        bool IsOuterRight = true;
                        for (int k = 1; k < DungeonWidth-j; k++)
                        {
                            if (DungeonTileGrid[j + k, i] == Tile.Floor)
                            {
                                IsOuterRight = false;
                                break;
                            }
                        }
                        if (IsOuterRight && DungeonEntityGrid[j + 1, i] != null && !IsUpperWall(DungeonEntityGrid[j + 1, i]))
                        {
                            Entity tempWallEnt = GetEmptyTile(Globals.Textures["outer_right"]);
                            PositionVector posEnt = tempWallEnt.GetComponent<PositionVector>();
                            posEnt.X = (j + 1) * Globals.TILE_WIDTH;
                            posEnt.Y = i * Globals.TILE_HEIGHT;
                            Walls.Add(tempWallEnt);
                        }
                        else if (DungeonEntityGrid[j + 1, i] == null)
                        {
                            Entity tempWallEnt = GetEmptyTile(Globals.Textures["inner_left"]);
                            PositionVector posEnt = tempWallEnt.GetComponent<PositionVector>();
                            posEnt.X = (j + 1) * Globals.TILE_WIDTH;
                            posEnt.Y = i * Globals.TILE_HEIGHT;
                            Walls.Add(tempWallEnt);
                        }

                        //check the bottom

                        bool IsOuterBottom = true;
                        for (int k = 1; k < DungeonHeight-i; k++)
                        {
                            if (DungeonTileGrid[j, i+k] == Tile.Floor)
                            {
                                IsOuterBottom = false;
                                break;
                            }
                        }

                        if(IsOuterBottom)
                        {
                            Entity tempWallEnt = GetEmptyTile(Globals.Textures["inner_up"]); //temp inner until outer texture is fixed
                            PositionVector posEnt = tempWallEnt.GetComponent<PositionVector>();
                            posEnt.X = j * Globals.TILE_WIDTH;
                            posEnt.Y = (i+1) * Globals.TILE_HEIGHT;
                            Walls.Add(tempWallEnt);
                        }
                        else if(DungeonEntityGrid[j,i+1] == null)
                        {
                            Entity tempWallEnt = GetEmptyTile(Globals.Textures["inner_up"]);
                            PositionVector posEnt = tempWallEnt.GetComponent<PositionVector>();
                            posEnt.X = j * Globals.TILE_WIDTH;
                            posEnt.Y = (i + 1) * Globals.TILE_HEIGHT;
                            Walls.Add(tempWallEnt);
                        }
                        if(DungeonTileGrid[j-1,i] == Tile.Wall && DungeonEntityGrid[j-1,i+1] == null)
                        {
                            Entity tempWallEnt = GetEmptyTile(Globals.Textures["outer_bottomleftcorner"]);
                            PositionVector posEnt = tempWallEnt.GetComponent<PositionVector>();
                            posEnt.X = (j-1) * Globals.TILE_WIDTH;
                            posEnt.Y = (i + 1) * Globals.TILE_HEIGHT;
                            Walls.Add(tempWallEnt);
                        }
                        //do bottom right last

                        //check the top
                        if(DungeonEntityGrid[j,i-1] == null)
                        {
                            Entity tempWallEnt = GetEmptyTile(Globals.Textures["inner_down"]);
                            PositionVector posEnt = tempWallEnt.GetComponent<PositionVector>();
                            posEnt.X = j * Globals.TILE_WIDTH;
                            posEnt.Y = (i - 1) * Globals.TILE_HEIGHT;
                            Walls.Add(tempWallEnt);
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
