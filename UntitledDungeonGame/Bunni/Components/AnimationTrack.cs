﻿using Bunni.Resources.Components.Collision;
using Bunni.Resources.Modules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UntitledDungeonGame.Bunni.Components;

namespace Bunni.Resources.Components
{
    public class AnimationTrack
    {

        public AnimationAtlas Atlas;
        /// <summary>
        /// The amount of miliseconds in between each frame
        /// </summary>
        public int AnimationSpeed { get; set; } = 250;
        /// <summary>
        /// Returns whether the animation is playing or not
        /// </summary>
        public bool IsPlaying { get; private set; } = false;
        /// <summary>
        /// Whether the animation should loop or not
        /// </summary>
        public bool Loop { get; set; } = false;
        /// <summary>
        /// The current frame in the animation
        /// </summary>
        public int CurrentFrame { get; set; } = 0;

        /// <summary>
        /// This variable holds the amount of frames that have passed
        /// The animation will only move to the next frame when this equals AnimationSpeed
        /// </summary>
        private int AnimationHeartbeat;


        public AnimationTrack(AnimationAtlas atlas)
        {
            Atlas = atlas;
        }

        public void Update(GameTime gameTime, Scene scene)
        {
            if(IsPlaying)
            {
                AnimationHeartbeat+=gameTime.ElapsedGameTime.Milliseconds;
                if(AnimationHeartbeat>= AnimationSpeed)
                {
                    AnimationHeartbeat = 0;
                    CurrentFrame++;
                }
                if(CurrentFrame>=Atlas.Frames)
                {
                    if(Loop)
                    {
                        CurrentFrame = 0;
                    }
                    else
                    {
                        Stop();
                    }
                }
                /*Render EntityRenderComp = Parent.GetComponent<Render>();
                EntityRenderComp.RenderRectangle = Rectangles[CurrentFrame];
                Collider EntityCollider = Parent.GetComponent<Collider>();
                if(EntityCollider != null)
                {
                    EntityCollider.Hitbox.Width = Atlas.Texture.Width / Atlas.Frames;
                }*/
            }
        }

        /// <summary>
        /// Stops the animation and puts the animation frame back at 0
        /// </summary>
        public void Stop()
        {
            CurrentFrame = 0;
            IsPlaying = false;
        }

        /// <summary>
        /// Starts the animation from the begining
        /// </summary>
        /*public void Play()
        {
            CurrentFrame = 0;
            IsPlaying = true;
            Render EntityRenderComp = Parent.GetComponent<Render>();
            EntityRenderComp.Texture = Atlas;
            EntityRenderComp.RenderRectangle = Rectangles[0];
        }

        /// <summary>
        /// Pause the animation on the frame that it is on
        /// </summary>
        public void Pause()
        {
            IsPlaying = false;
        }

        /// <summary>
        /// Starts the animation from the frame that it is on
        /// </summary>
        public void Resume()
        {
            IsPlaying = true;
            Render EntityRenderComp = Parent.GetComponent<Render>();
            EntityRenderComp.Texture = Atlas;
            EntityRenderComp.RenderRectangle = Rectangles[CurrentFrame];
        }

        public void SetFrame(int frame)
        {
            Parent.GetComponent<Render>().RenderRectangle = Rectangles[frame];
        }*/
    }
}
