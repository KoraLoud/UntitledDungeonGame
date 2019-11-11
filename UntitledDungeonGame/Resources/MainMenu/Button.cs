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

        private Action onClick;
        private Action onHover;
        private bool Clicked = false;

        public void OnClick(Action callback)
        {
            onClick = callback;
        }

        public void OnHover(Action callback)
        {
            onHover = callback;
        }

        public override void Update(GameTime gameTime, Scene scene)
        {
            Vector2 mousePos = Camera.GetMouseWorldPosition();
            MouseState mouseState = Mouse.GetState();
            PositionVector pos = GetComponent<PositionVector>();
            Render rendComp = GetComponent<Render>();
            if (!(pos.X > mousePos.X
                    || pos.X + rendComp.Texture.Width < mousePos.X
                    || pos.Y > mousePos.Y
                    || pos.Y + rendComp.Texture.Height < mousePos.Y
                    ))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (!Clicked)
                    {
                        onClick?.Invoke();
                        Clicked = true;
                    }
                }
                else
                {
                    Clicked = false;
                }
            }
            else
            {
                onHover?.Invoke();
            }
        }
    }
}
