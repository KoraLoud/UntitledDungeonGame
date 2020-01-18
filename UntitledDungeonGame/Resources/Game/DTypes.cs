using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntitledDungeonGame.Resources.Game
{
    public class DTypes
    {
        public enum TileType
        {
            Floor,
            Wall,
            Air,
            Entrance,
            Exit
        }

        public enum TileVersions
        {
            Floor,
            Corner,
            BottomWall,
            InnerTopWall,
            RightWall,
            LeftWall,
            TopWall
        }
    }
}
