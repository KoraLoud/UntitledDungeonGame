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
        public int MaxRooms;
        public int RoomMinSize;
        public int RoomMaxSize;

        public int DungeonWidth;
        public int DungeonHeight;

        public Dungeon(int maxRooms, int roomMinSize, int roomMaxSize, int dungeonWidth, int dungeonHeight)
        {
            Rooms = new List<DungeonRoom>();
            MaxRooms = maxRooms;
            RoomMinSize = roomMinSize;
            RoomMaxSize = roomMaxSize;
            DungeonWidth = dungeonWidth;
            DungeonHeight = dungeonHeight;
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

                int roomY = randInt.Next(0, DungeonHeight - roomHeight);
                int roomX = randInt.Next(0, DungeonWidth - roomWidth);

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

        }

        public List<Entity> GetEntities()
        {
            List<Entity> entities = new List<Entity>();
            for(int i=0;i<Rooms.Count;i++)
            {
                DungeonRoom cRoom = Rooms.ElementAt(i); //get the room
                Entity[,] roomEntityArray = cRoom.GetEntities(); //get all the entities in the room
                for(int j=0;j<cRoom.RoomWidth;j++) //loop through the 2D array
                {
                    for(int k=0;k<cRoom.RoomHeight;k++)
                    {
                        entities.Add(roomEntityArray[j, k]); //add it to the list
                        Camera.Position = roomEntityArray[j, k].GetComponent<PositionVector>().Position;
                    }
                }
            }
            return entities;
        }

        public class DungeonRoom
        {
            /// <summary>
            /// The tiles in the room. This is things like the floor/walls. does not include entities on ground or enemies.
            /// </summary>
            public Tile[,] RoomTiles;
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

            public Entity[,] GetEntities()
            {
                Entity[,] entities = new Entity[RoomWidth, RoomHeight];
                for (int i = 0; i < RoomWidth; i++)
                {
                    for (int j = 0; j < RoomHeight; j++)
                    {

                        Entity Tile = new Entity();
                        Render entRender = new Render(Globals.Textures[RoomTiles[i,j]]);
                        PositionVector entPositionVector = new PositionVector();
                        entPositionVector.X = (RoomX * Globals.TILE_WIDTH) + (i * Globals.TILE_WIDTH); //convert to global position
                        entPositionVector.Y = (RoomY * Globals.TILE_HEIGHT) + (j * Globals.TILE_HEIGHT); //convert to global position
                        Tile.AddComponent(entPositionVector);
                        Tile.AddComponent(entRender);
                        entities[i,j] = Tile;
                    }
                }
                return entities;
            }

        }
    }
}
