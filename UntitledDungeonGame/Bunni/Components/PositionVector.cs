using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunni.Resources.Modules;

namespace Bunni.Resources.Components
{
    public class PositionVector : Component
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }

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


        public PositionVector()
        {
            Position = Vector2.Zero;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
        }
        private int ElapsedTime = 0;
        public override void Update(GameTime gameTime, Scene scene)
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

                float percentage = (ElapsedTime - startTime) / ((float)stopTime - startTime);
                if (percentage >= 1)
                {
                    Lerping = false;
                }
                else
                {
                    Position = new Vector2(StartPosition.X + (LerpPosition.X - StartPosition.X) * percentage, StartPosition.Y + (LerpPosition.Y - StartPosition.Y) * percentage);
                }

            }

            ElapsedTime %= 100000;
            startTime %= 100000;
            stopTime %= 100000;

        }

        private Vector2 StartPosition;
        private Vector2 LerpPosition;
        private bool Lerping = false;
        private bool initialFrame = false;
        private int startTime;
        private int stopTime;
        public void Lerp(Vector2 lerpPosition, int duration)
        {
            Lerping = true;
            initialFrame = true;
            stopTime = duration;
            LerpPosition = lerpPosition;
        }

    }
}
