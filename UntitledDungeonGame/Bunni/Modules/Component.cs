using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunni.Resources.Modules
{
    public class Component
    {
        public Entity Parent { get; set; }
        public virtual void ComponentAdded(){}

        public virtual void PreUpdate(GameTime gameTime, Scene scene){}

        public virtual void Update(GameTime gameTime, Scene scene){}

        public virtual void PostUpdate(GameTime gameTime, Scene scene){}

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch){}

    }
}
