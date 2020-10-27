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
        public Color Color { get; set; } = Color.White;
        public Rectangle RenderRectangle { get; set; }
        public bool Visible { get; set; } = true;
        public int ZLayer { get; set; } = 1;
        public Vector2 DrawOffset { get; set; } = Vector2.Zero;

        public TransformC Transform { get; set; }

        public Render()
        {
            Transform = new TransformC();
            Visible = false;
        }

        public Render(Texture2D texture)
        {
            Texture = texture;
            RenderRectangle = new Rectangle(0, 0, texture.Width, Texture.Height);
            Transform = new TransformC();
        }

        public void ChangeTexture(Texture2D texture)
        {
            bool show = false;
            if (Texture == null)
            {
                show = true;
            }
            Texture = texture;
            RenderRectangle = new Rectangle(0, 0, texture.Width, Texture.Height);
            if (show) Visible = true;
        }

        public override void ComponentAdded()
        {
        }

        public override void Update(GameTime gameTime, Scene scene)
        {
            Transform.Update(gameTime, scene);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                spriteBatch.Draw(Texture, Transform.Position + DrawOffset, RenderRectangle, Color);
            }

        }

        public class TransformC
        {
            public Vector2 Position { get; set; }
            public Vector2 Velocity { get; set; }
            public Vector2 Acceleration { get; set; }
            public bool Lerping { get; private set; } = false;

            public float X
            {
                get
                {
                    return Position.X;
                }
                set
                {
                    Position = new Vector2(value, Position.Y);
                }
            }

            public float Y
            {
                get
                {
                    return Position.Y;
                }
                set
                {
                    Position = new Vector2(Position.X, value);
                }
            }

            public TransformC()
            {
                Position = Vector2.Zero;
                Velocity = Vector2.Zero;
                Acceleration = Vector2.Zero;
            }
            private int ElapsedTime = 0;
            public void Update(GameTime gameTime, Scene scene)
            {
                ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

                if (Lerping)
                {
                    if (initialFrame)
                    {
                        startTime = ElapsedTime;
                        stopTime += startTime;
                        StartPosition = Position;
                        initialFrame = false;
                    }

                    float percentage = Math.Min((ElapsedTime - startTime) / ((float)stopTime - startTime), 1);
                    Position = new Vector2(StartPosition.X + (LerpPosition.X - StartPosition.X) * percentage, StartPosition.Y + (LerpPosition.Y - StartPosition.Y) * percentage);
                    if (Finished)
                    {
                        Lerping = false;
                    }
                    if (percentage == 1 && !Finished)
                    {
                        Finished = true;
                    }

                }

                ElapsedTime %= 100000;
                startTime %= 100000;
                stopTime %= 100000;

            }

            private Vector2 StartPosition;
            private Vector2 LerpPosition;
            private bool initialFrame = false;
            private int startTime;
            private int stopTime;
            private bool Finished = true;
            public void Lerp(Vector2 lerpPosition, int duration)
            {
                Lerping = true;
                initialFrame = true;
                stopTime = duration;
                LerpPosition = lerpPosition;
                Finished = false;
            }

        }


    }
}
