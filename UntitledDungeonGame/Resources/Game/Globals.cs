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
        public static Dictionary<Tile, Texture2D> Textures = new Dictionary<Tile, Texture2D>();
        public static readonly int TILE_HEIGHT = 128;
        public static readonly int TILE_WIDTH = 128;
    }
}
