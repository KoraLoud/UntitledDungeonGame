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
            PositionVector posEnt = new PositionVector
            {
                X = position.X,
                Y = position.Y
            };

            Render rendEnt = new Render(tex);
            AddComponent(posEnt);
            AddComponent(rendEnt);
        }

    }
}
