using Bunni.Resources.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunni.Resources.Modules
{
    public class Scene
    {
        public List<Entity> SceneEntities = new List<Entity>();

        private Action onLoad;

        public void SetOnLoad(Action callback)
        {
            onLoad = callback;
        }

        public void Load()
        {
            onLoad?.Invoke();
        }

        /// <summary>
        /// Adds an entity to the scene
        /// </summary>
        /// <param name="entity">Entity to be added</param>
        public void AddEntity(Entity entity)
        {
            SceneEntities.Add(entity);
            SortEntities();
        }

        /// <summary>
        /// Removes an entity from the scene
        /// </summary>
        /// <param name="entity">Entity to be removed</param>
        public void RemoveEntity(Entity entity)
        {
            SceneEntities.Remove(entity);
        }


        public virtual void PreUpdate(GameTime gameTime)
        {
            foreach (Entity e in SceneEntities.ToList())
            {
                e.PreUpdate(gameTime, this);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (Entity e in SceneEntities.ToList())
            {
                e.Update(gameTime, this);
            }
        }

        public virtual void PostUpdate(GameTime gameTime)
        {
            foreach (Entity e in SceneEntities.ToList())
            {
                e.PostUpdate(gameTime, this);
            }
        }

        private static int SortByZ(Entity x, Entity y)
        {
            if (x.HasComponent<Render>() && y.HasComponent<Render>())
            {
                Render xComponent = x.GetComponent<Render>();
                Render yComponent = y.GetComponent<Render>();
                if (xComponent.ZLayer > yComponent.ZLayer)
                {
                    return -1;
                }
                else if (xComponent.ZLayer < yComponent.ZLayer)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        private void SortEntities()
        {
            SceneEntities.Sort(SortByZ);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var e in SceneEntities)
            {
                e.Draw(gameTime, spriteBatch);
            }
        }
    }
}
