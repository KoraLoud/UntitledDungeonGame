using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunni.Resources.Modules
{
    public class BniTypes
    {
        /// <summary>
        /// Collision layer the entity is on
        /// </summary>
        public enum CollisionLayer
        {
            Background,
            Midground,
            Foreground
        }

        /// <summary>
        /// Type of tag for an entity
        /// </summary>
        public enum Tag
        {
            None,
            Player,
            Enemy,
            Floor,
            Wall,
            Air
        }
    }
}
