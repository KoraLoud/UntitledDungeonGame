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
        public int Rooms { get; set; }
        public DungeonRoom[,] DungeonArray { get; set; }

        private Dictionary<Tile, Texture2D> Textures;

        public Dungeon(Dictionary<Tile, Texture2D> texs)
        {
            Textures = new Dictionary<Tile, Texture2D>();
            Textures = texs;
        }

        public void AddTexture(Tile tile, Texture2D tex)
        {
            Textures.Add(tile, tex);
        }

        public DungeonRoom CurrentRoom { get; set; }

        public Entity[,] BuildRoom(DungeonRoom room)
        {
            Entity[,] tempArray = new Entity[room.RoomArray.GetUpperBound(0)+1, room.RoomArray.GetUpperBound(1)+1];
            for(int i=0;i<room.RoomArray.GetUpperBound(0)+1;i++)
            {
                for(int j=0;j<room.RoomArray.GetUpperBound(1)+1;j++)
                {
                    PositionVector tempPos = new PositionVector();
                    Render tempRend = new Render(Textures[room.RoomArray[i,j]]);
                    tempPos.X = i * Textures[room.RoomArray[i, j]].Width;
                    tempPos.Y = j * Textures[room.RoomArray[i, j]].Height;
                    //Console.WriteLine(j * Textures[room.RoomArray[i, j]].Height);
                    Entity tempEntity = new Entity();
                    if(room.RoomArray[i,j] == Tile.Wall)
                    {
                        tempRend.Color = Color.Red;
                    }
                    tempEntity.AddComponent(tempPos);
                    tempEntity.AddComponent(tempRend);
                    tempArray[i, j] = tempEntity;
                }
            }
            return tempArray;
        }

        public void GenerateDungeon(int rooms)
        {
            Rooms = rooms;
            DungeonArray = new DungeonRoom[Rooms, Rooms];
            int lastMod = 0; //last direction that the room moved in
            int currentX = 0; //where the generation is currently at
            int currentY = 0;
            int roomsCreated = 0;
            Random randNum = new Random();
            while(roomsCreated<Rooms)
            {
                bool placed = false;
                do
                {
                    int newdir = randNum.Next(4);
                    if (newdir == Math.Abs(lastMod - 2)) //check to make sure it is not going backwards
                    {
                        newdir++;
                        newdir %= 4;
                    }
                    int arrayX = 0;
                    int arrayY = 0;
                    switch (newdir) //get the x and y of new position
                    {
                        case 0:
                            arrayX = currentX;
                            arrayY = currentY - 1;
                            break;
                        case 1:
                            arrayX = currentX + 1;
                            arrayY = currentY;
                            break;
                        case 2:
                            arrayX = currentX;
                            arrayY = currentY - 1;
                            break;
                        case 3:
                            arrayX = currentX - 1;
                            arrayY = currentY;
                            break;
                    }
                    if (!(arrayX < 0 || arrayY < 0 || arrayX > Rooms-1 || arrayY > Rooms-1)) //check to make sure it is not out of bounds
                    {
                        if(DungeonArray[arrayX, arrayY] == null)//add if null. if its not, the position moves into the already existing space, but does not write to it.
                        {
                            DungeonRoom newRoom = new DungeonRoom(); 
                            newRoom.GenerateRoom();
                            DungeonArray[arrayX, arrayY] = newRoom;
                            roomsCreated++;
                        }
                        currentX = arrayX; //move
                        currentY = arrayY;
                        placed = true;
                    }
                } while (!placed);
            }
        }

        public class DungeonRoom
        {
            public Tile[,] RoomArray { get; set; }

            public DungeonRoom()
            {
                RoomArray = new Tile[6,12]; //generate floor tiles
            }

            public void GenerateRoom() //create walls along the edges (change this to actual room generation eventually)
            {
                for (int i = 0; i < Math.Max(RoomArray.GetUpperBound(0)+1, RoomArray.GetUpperBound(1)+1); i++)
                {
                    RoomArray[0, Math.Min(i, RoomArray.GetUpperBound(1))] = Tile.Wall; //fill left
                    RoomArray[RoomArray.GetUpperBound(0), Math.Min(i, RoomArray.GetUpperBound(1))] = Tile.Wall; //fill right
                    RoomArray[Math.Min(i, RoomArray.GetUpperBound(0)), 0] = Tile.Wall; //fill top
                    RoomArray[Math.Min(i, RoomArray.GetUpperBound(0)), RoomArray.GetUpperBound(1)] = Tile.Wall; //fill bottom
                }
            }
        }
    }
}
