using Bunni.Resources.Components;
using Bunni.Resources.Modules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntitledDungeonGame.Bunni.Components
{
    public class Animation : Component
    {

        public AnimationAtlas CurrentDefaultAtlas { get; set; }
        private Dictionary<string, AnimationAtlas> EntityAtlases = new Dictionary<string, AnimationAtlas>();
        private Dictionary<string, AnimationTrack> EntityAnimationTracks = new Dictionary<string, AnimationTrack>();

        public AnimationTrack CurrentAnimation { get; set; }

        public void AddAtlas(string atlasName, Texture2D atlasTexture, int frames)
        {
            AnimationAtlas nAtlas = new AnimationAtlas(atlasTexture, frames, this);
            EntityAtlases.Add(atlasName, nAtlas);
        }

        public void SetDefaultAtlus(string atlasName)
        {
            if(EntityAtlases.ContainsKey(atlasName))
            {
                CurrentDefaultAtlas = EntityAtlases[atlasName];
                Parent.GetComponent<Render>().RenderRectangle = CurrentDefaultAtlas.Rectangles[0];
            }
        }

        public AnimationAtlas GetAtlas(string atlasName)
        {
            return EntityAtlases[atlasName];
        }

        public void AddAnimation(string animName, int startFrame, int endFrame, int idle, string atlasName=null, int animationSpeed=250)
        {
            AnimationAtlas cAtlas = CurrentDefaultAtlas;
            bool specificAtlas = false;
            if(atlasName != null)
            {
                cAtlas = EntityAtlases[atlasName];
                specificAtlas = true;
            }
            AnimationTrack nAnimation = new AnimationTrack(this, cAtlas, startFrame, endFrame, idle);
            nAnimation.specificAtlas = specificAtlas;
            nAnimation.AnimationSpeed = animationSpeed;
            EntityAnimationTracks.Add(animName, nAnimation);
        }

        public AnimationTrack GetAnimation(string animName)
        {
            return EntityAnimationTracks[animName];
        }

        public void PlayAnimation(string animName)
        {
            if(CurrentAnimation != null)
            {
                CurrentAnimation.Stop();
            }
            EntityAnimationTracks[animName].Play();
        }
        
        public void StopAnimations()
        {
            if(CurrentAnimation != null)
            {
                CurrentAnimation.Stop();
            }
            CurrentAnimation = null;
        }

        public override void Update(GameTime gameTime, Scene scene)
        {
            if(CurrentAnimation != null)
            {
                CurrentAnimation.Update(gameTime, scene);
            }
        }


    }
}
