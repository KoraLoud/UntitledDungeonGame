using Bunni.Resources.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunni.Resources.Modules
{
    public static class Camera
    {
        public static int VirtualWidth { get; private set; }
        public static int VirtualHeight { get; private set; }
        public static int ActualHeight { get; private set; }
        public static int ActualWidth { get; private set; }
        public static bool Lerping { get; private set; }

        private static GraphicsDeviceManager Graphics;
        private static Vector2 _Position;
        public static Vector2 Position
        {
            get
            {
                return _Position;
            }
            set
            {
                _Position = value;
                Updated = true;
            }
        }
        private static float _Zoom = 0.5f;
        public static float Zoom
        {
            get
            {
                return _Zoom;
            }
            set
            {
                _Zoom = value;
                Updated = true;
            }
        }
        private static float _Rotation = 0;
        public static float Rotation
        {
            get
            {
                return _Rotation;
            }
            set
            {
                _Rotation = value;
                Updated = true;
            }
        }

        public static Vector2 Origin;
        public static Rectangle View { get; set; }

        public static Matrix Transform = Matrix.Identity;

        private static bool Updated = true;

        /// <summary>
        /// Called to initialize the camera
        /// </summary>
        /// <param name="position">The starting position of the camera</param>
        /// <param name="graphics">The GraphicsDeviceManager of the game</param>
        /// <param name="virtualWidth">The virtual width that is being drawn to before re-scaling</param>
        /// <param name="virtualHeight">The virtual height that is being drawn to before re-scaling</param>
        public static void Init(Vector2 position, GraphicsDeviceManager graphics, int virtualWidth, int virtualHeight)
        {
            Position = position;
            Graphics = graphics;
            ActualWidth = graphics.PreferredBackBufferWidth;
            ActualHeight = graphics.PreferredBackBufferHeight;
            VirtualWidth = virtualWidth;
            VirtualHeight = virtualHeight;
            Updated = true;
            View = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            Origin = new Vector2(View.Width / 2, View.Height / 2);
        }

        /// <summary>
        /// Called every time the resolution is needed to be updated
        /// </summary>
        /// <param name="width">New width of the window</param>
        /// <param name="height">New height of the window</param>
        public static void UpdateWindow(int width, int height)
        {
            ActualWidth = width;
            ActualHeight = height;
            Graphics.PreferredBackBufferWidth = width;
            Graphics.PreferredBackBufferHeight = height;
            Graphics.ApplyChanges();
            View = new Rectangle(0, 0, ActualWidth, ActualHeight);
            Origin = new Vector2(View.Width / 2, View.Height / 2);
            Updated = true;
        }

        /// <summary>
        /// Takes a world position, then returns where it is at on the screen
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static Vector2 WorldPosToScreenPos(Vector2 pos)
        {
            return Vector2.Transform(pos, Transform);
        }

        /// <summary>
        /// Returns where on the screen
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static Vector2 GetEntityScreenPosition(Entity e)
        {
            Vector2 newPos = WorldPosToScreenPos(e.Transform.Position);
            return newPos;
        }

        private static int ElapsedTime = 0;

        /// <summary>
        /// Gets the Matrix from the camera. Call this in the SpriteBatch.Begin() function before drawing.
        /// </summary>
        /// <returns></returns>
        public static Matrix TransformMatrix(GameTime gameTime)
        {
            ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (Updated)
            {
                Transform = Matrix.CreateTranslation(new Vector3(-Position, 0)) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(new Vector3(Zoom * ((float)ActualWidth / VirtualWidth), Zoom * ((float)ActualHeight / VirtualHeight), 1)) *
                    //Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                    Matrix.CreateTranslation(new Vector3(Origin, 0));

                Updated = false;
            }

            if (Lerping)
            {
                if (initialFrame)
                {
                    startTime = ElapsedTime;
                    stopTime += ElapsedTime;
                    StartPosition = Position;
                    initialFrame = false;
                }

                float percentage = Math.Min((ElapsedTime - startTime) / ((float)stopTime - startTime), 1);
                Position = new Vector2(StartPosition.X + (LerpPosition.X - StartPosition.X) * percentage, StartPosition.Y + (LerpPosition.Y - StartPosition.Y) * percentage);
                if(Finished)
                {
                    Lerping = false;
                }
                if(percentage == 1 && !Finished)
                {
                    Finished = true;
                }

            }

            ElapsedTime %= 100000;
            startTime %= 100000;
            stopTime %= 100000;
            return Transform;
        }

        private static Vector2 StartPosition;
        private static Vector2 LerpPosition;
        private static bool initialFrame = false;
        private static int startTime;
        private static int stopTime;
        private static bool Finished = true;

        /// <summary>
        /// Lerp camera to one position to another
        /// </summary>
        /// <param name="lerpPosition"></param>
        /// <param name="duration"></param>
        public static void Lerp(Vector2 lerpPosition, int duration)
        {
            Lerping = true;
            initialFrame = true;
            stopTime = duration;
            LerpPosition = lerpPosition;
            Finished = false;
        }

    }
}
