using Bunni.Resources.Components;
using Bunni.Resources.Modules;
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
            Entity[,] tempArray = new Entity[room.RoomArray.Length, room.RoomArray.Length];
            for(int i=0;i<room.RoomArray.GetUpperBound(0);i++)
            {
                for(int j=0;i<room.RoomArray.GetUpperBound(1);j++)
                {
                    PositionVector tempPos = new PositionVector();
                    Render tempRend = new Render(Textures[room.RoomArray[i,j]]);
                    tempPos.X = i * Textures[room.RoomArray[i, j]].Width;
                    tempPos.Y = j * Textures[room.RoomArray[i, j]].Height;
                    Entity tempEntity = new Entity();

                    tempEntity.AddComponent(tempPos);
                    tempEntity.AddComponent(tempRend);
                    tempArray[i, j] = tempEntity;
                }
            }
            return tempArray;
        }

        public void GenerateDungeon()
        {
            DungeonArray = new DungeonRoom[Rooms, Rooms];
            int lastMod = 0;
            int currentX = 0;
            int currentY = 0;
            Random randNum = new Random();
            for(int i=0;i<Rooms;i++)
            {
                int newdir = randNum.Next(1,5);
                if(newdir%2==lastMod)
                {
                    
                }
            }
        }

        public class DungeonRoom
        {
            public Tile[,] RoomArray { get; set; }

            public DungeonRoom()
            {
                RoomArray = new Tile[10, 10];
            }

            public void GenerateRoom()
            {
                for (int i = 0; i < RoomArray.Length; i++)
                {
                    RoomArray[0, i] = Tile.Wall;
                    RoomArray[RoomArray.Length - 1, i] = Tile.Wall;
                    RoomArray[i, 0] = Tile.Wall;
                    RoomArray[RoomArray.Length - 1, i] = Tile.Wall;
                }
            }
        }
    }
}
