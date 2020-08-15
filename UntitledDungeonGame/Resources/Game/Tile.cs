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
        public Vector2 GridPosition { get; set; } = Vector2.Zero;
        public int Cost { get; set; } = 1;

        public Tile(Texture2D tex, Vector2 position)
        {
            Transform.X = position.X;
            Transform.Y = position.Y;
            Render.ChangeTexture(tex);
        }

        public static readonly Vector2[] DIRS = new[]
        {
            new Vector2(1, 0),
            new Vector2(0, -1),
            new Vector2(-1, 0),
            new Vector2(0, 1)
        };

        private bool IsValid(Tile a)
        {
            if(a != null && a.TileVersion == DTypes.TileVersions.Floor)
            {
                return true;
            }
            return false;
        }

        public IEnumerable<Tile> Neighbors()
        {
            foreach(var dir in DIRS)
            {
                Tile next = SceneManager.CurrentDungeon.DungeonEntityGrid[(int)(GridPosition.X + dir.X), (int)(GridPosition.Y + dir.Y)];
                if (IsValid(next))
                {
                    yield return next;
                }
            }
        }

    }
}
