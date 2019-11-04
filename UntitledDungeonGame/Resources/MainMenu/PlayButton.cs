using Bunni.Resources.Components;
using Bunni.Resources.Modules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntitledDungeonGame.Resources.MainMenu
{
    public class PlayButton : Entity
    {

        public PlayButton(Texture2D tex)
        {
            //add position component
            PositionVector PlayPositionVector = new PositionVector();
            PlayPositionVector.X = (Camera.VirtualWidth/2)-tex.Width/2;
            PlayPositionVector.Y = (Camera.VirtualHeight/4);
            AddComponent(PlayPositionVector);

            //add render component
            Render PlayRenderComponent = new Render(tex);
            AddComponent(PlayRenderComponent);


        }
    }
}
