using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntitledDungeonGame.Bunni.Components
{
    public class AnimationAtlas
    {
        /// <summary>
        /// The sprite atlas that has all the sprite's animation frames on it
        /// </summary>
        public Texture2D Texture { get; set; }
        /// <summary>
        /// The amount of frames in the animation
        /// </summary>
        public int Frames { get; set; }

        /// <summary>
        /// Rectangles for each of the sprites in the atlas
        /// </summary>
        public Rectangle[] Rectangles;

        public AnimationAtlas(Texture2D atlas, int frames)
        {
            Rectangles = new Rectangle[frames];
            int width = atlas.Width / frames;
            for (int i = 0; i < frames; i++)
            {
                Rectangles[i] = new Rectangle((i * width), 0, width, atlas.Height);
            }
        }
    }
}
