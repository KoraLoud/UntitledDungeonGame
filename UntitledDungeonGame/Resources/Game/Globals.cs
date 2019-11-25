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

    }
}
