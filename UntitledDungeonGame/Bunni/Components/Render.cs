using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunni.Resources.Modules;

namespace Bunni.Resources.Components

{
    public class Render : Component
    {
        public Texture2D Texture { get; set; }
        public Color Color { get; set; }
        public Rectangle RenderRectangle { get; set; }
        public bool Visible { get; set; } = true;
        public int ZLayer { get; set; } = 1;
        public Vector2 DrawOffset { get; set; } = Vector2.Zero;

        private PositionVector PositionVec { get; set; }

        public Render(Texture2D texture)
        {
            Texture = texture;
            Color = Color.White;
            RenderRectangle = new Rectangle(0, 0, texture.Width, Texture.Height);
        }

        public override void ComponentAdded()
        {
            PositionVector pos = Parent.GetComponent<PositionVector>();
            PositionVec = pos;
        }

        public override void Update(GameTime gameTime, Scene scene)
        {
            PositionVec = Parent.GetComponent<PositionVector>();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                spriteBatch.Draw(Texture, PositionVec.Position+DrawOffset, RenderRectangle, Color);
            }
            
        }

    }
}
