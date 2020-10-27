using Bunni.Resources.Modules;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunni.Resources.Components.Collision
{
    public interface ICollider
    {

        Collider Parent { get; set; }
        int Bottom { get; }
        int Left { get; }
        int Top { get; }
        int Right { get; }

        int Width { get; set; }
        int Height { get; set; }
        bool Intersects(ICollider c2);
        bool IntersectsOnLayer(ICollider c2);
        bool IntersectsWithTag(ICollider c2, BniTypes.Tag Tag);
        void ComponentAdded();
    }
}
