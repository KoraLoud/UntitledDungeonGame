using Bunni.Resources.Components;
using Bunni.Resources.Modules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntitledDungeonGame.Resources.MainMenu
{
    public class Button : Entity
    {

        private Action onWorldClick;
        private Action onScreenClick;
        private Action onWorldHover;
        private Action onScreenHover;
        private bool onWorldClickBool;
        private bool onScreenClickBool;
        private bool onWorldHoverBool;
        private bool onScreenHoverBool;
        private bool WorldClicked = false;
        private bool ScreenClicked = false;


        public void OnWorldClick(Action callback)
        {
            onWorldClick = callback;
            onWorldClickBool = true;
        }

        public void OnScreenClick(Action callback)
        {
            onScreenClick = callback;
            onScreenClickBool = true;
        }

        public void OnWorldHover(Action callback)
        {
            onWorldHover = callback;
            onWorldHoverBool = true;
        }

        public void OnScreenHover(Action callback)
        {
            onScreenHover = callback;
            onScreenHoverBool = true;
        }

        public override void Update(GameTime gameTime, Scene scene)
        {
            if (onWorldClickBool || onWorldHoverBool && Render.Visible)
            {
                Vector2 mousePos = Input.GetGlobalMousePosition();
                MouseState mouseState = Mouse.GetState();
                Render.TransformC pos = Render.Transform;
                if (!(pos.X > mousePos.X
                        || pos.X + Render.Texture.Width < mousePos.X
                        || pos.Y > mousePos.Y
                        || pos.Y + Render.Texture.Height < mousePos.Y
                        ))
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (!WorldClicked)
                        {
                            onWorldClick?.Invoke();
                            WorldClicked = true;
                        }
                    }
                    else
                    {
                        WorldClicked = false;
                    }
                }
                else
                {
                    onWorldHover?.Invoke();
                }
            } else if (onScreenClickBool || onScreenHoverBool && Render.Visible)
            {
                MouseState mouseState = Mouse.GetState();
                Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y);
                //Console.WriteLine("X: " + mouseState.X + " Y: " + mouseState.Y);
                Render.TransformC pos = Render.Transform;
                if (!(pos.X > mousePos.X
                        || pos.X + Render.Texture.Width < mousePos.X
                        || pos.Y > mousePos.Y
                        || pos.Y + Render.Texture.Height < mousePos.Y
                        ))
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (!ScreenClicked)
                        {
                            onScreenClick?.Invoke();
                            ScreenClicked = true;
                        }
                    }
                    else
                    {
                        ScreenClicked = false;
                    }
                }
                else
                {
                    onScreenHover?.Invoke();
                }
            }
        }
    }
}
