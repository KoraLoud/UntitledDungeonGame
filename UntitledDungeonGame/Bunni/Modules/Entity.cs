﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunni.Resources.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UntitledDungeonGame.Resources.Game;

namespace Bunni.Resources.Modules
{
    public class Entity
    {
        private List<Component> Components = new List<Component>();
        public bool Active { get; set; } = true;
        public BniTypes.Tag Tag { get; set; }
        public DTypes.TileType TileType { get; set; }
        public Render Render;

        public Render.TransformC Transform
        {
            get
            {
                return Render.Transform;
            }
        }

        public Entity()
        {
            Render = new Render();
            AddComponent(Render);
        }

        Entity(Texture2D tex)
        {
            Render = new Render(tex);
            AddComponent(Render);
        }

        public void AddTexture(Texture2D tex)
        {
            Render.ChangeTexture(tex);
        }

        /// <summary>
        /// Used to add a component to an entity
        /// </summary>
        /// <param name="component">The component to add</param>
        public void AddComponent(Component component)
        {
            component.Parent = this;
            Components.Add(component);
            component.ComponentAdded();
        }

        /// <summary>
        /// Used to remove a component from the entity. This will delete the component.
        /// </summary>
        /// <param name="component">The component to remove</param>
        public void RemoveComponent(Component component)
        {
            Components.Remove(component);
        }

        /// <summary>
        /// Returns component that belongs to entity of given type
        /// </summary>
        /// <typeparam name="T">The type of component to get</typeparam>
        /// <returns>The component if it exists, else it returns false</returns>
        public T GetComponent<T>() where T : Component
        {
            foreach (var c in Components)
            {
                if (c is T)
                {
                    return c as T;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns if an entity has a given component or not
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool HasComponent<T>() where T : Component
        {
            foreach (var c in Components)
            {
                if (c is T)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Fired before Update
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void PreUpdate(GameTime gameTime, Scene scene)
        {
            foreach (Component component in Components)
            {
                component.PreUpdate(gameTime, scene);
            }
        }

        /// <summary>
        /// Fire after PreUpdate but before PostUpdate. Do all main logic here
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime, Scene scene)
        {
            foreach (Component component in Components)
            {
                component.Update(gameTime, scene);
            }
        }

        /// <summary>
        /// Fired after Update
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void PostUpdate(GameTime gameTime, Scene scene)
        {
            foreach (Component component in Components)
            {
                component.PostUpdate(gameTime, scene);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Component component in Components)
            {
                component.Draw(gameTime, spriteBatch);
            }
        }

    }
}
