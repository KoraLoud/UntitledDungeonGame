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
    public class Tile : Entity
    {
        public DTypes.TileVersions TileVersion { get; set; }

        public Tile(Texture2D tex, Vector2 position)
        {
            Transform.X = position.X;
            Transform.Y = position.Y;
            Render.ChangeTexture(tex);
        }

    }
}
