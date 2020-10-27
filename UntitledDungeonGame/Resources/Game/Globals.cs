using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntitledDungeonGame.Resources.Game
{
    public static class Globals
    {
        //textures
        public static Dictionary<String, Texture2D> Textures = new Dictionary<String, Texture2D>();
        public static readonly int TILE_HEIGHT = 128;
        public static readonly int TILE_WIDTH = 128;
        public static readonly int FLOORS_COUNT = 4;
        public static readonly int WALLS_COUNT = 2;

        public static Vector2 GridToWorld(Vector2 GridSpace)
        {
            return new Vector2(GridSpace.X * TILE_WIDTH, GridSpace.Y * TILE_HEIGHT);
        }

        public static int GridXToWorld(int GridX)
        {
            return GridX * TILE_WIDTH;
        }

        public static int GridYToWorld(int GridY)
        {
            return GridY * TILE_HEIGHT;
        }

    }
}
