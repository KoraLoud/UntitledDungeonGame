using Bunni.Resources.Components.Collision;
using Bunni.Resources.Modules;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunni.Resources.Components.Collision
{
    public class BoxCollider : ICollider
    {
        public Collider Parent { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int Bottom
        {
            get
            {
                return Parent.Y + Height;
            }
        }

        public int Left
        {
            get
            {
                return Parent.X;
            }
        }

        public int Top
        {
            get
            {
                return Parent.Y;
            }
        }

        public int Right
        {
            get
            {
                return Parent.X + Width;
            }
        }

        public void ComponentAdded()
        {
            Render rend = Parent.Parent.Render;
            if (rend.Texture != null)
            {
                Width = rend.Texture.Width;
                Height = rend.Texture.Height;
            }
        }

        public bool Intersects(ICollider c2)
        {
            return !(c2.Left > Right
                    || c2.Right < Left
                    || c2.Top > Bottom
                    || c2.Bottom < Top
                    );
        }

        public bool IntersectsOnLayer(ICollider c2)
        {
            if (Parent.CollisionLayer == c2.Parent.CollisionLayer)
            {
                return Intersects(c2);
            }
            else
            {
                return false;
            }
        }

        public bool IntersectsWithTag(ICollider c2, BniTypes.Tag Tag)
        {
            if (Tag == c2.Parent.Parent.Tag)
            {
                return Intersects(c2);
            }
            else
            {
                return false;
            }
        }
    }
}
