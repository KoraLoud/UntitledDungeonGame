using Bunni.Resources.Components;
using Bunni.Resources.Modules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UntitledDungeonGame.Bunni.Components;

namespace UntitledDungeonGame.Resources.Game
{
    class Player : Entity
    {

        public int MoveSpeed { get; set; } = 200; //in miliseconds

        public bool Sprinting { get; set; } = false;

        public Player()
        {
            string charTexName = "Char1_3";

            Transform.Position = Globals.GridToWorld(SceneManager.CurrentDungeon.DungeonSpawn);
            Vector2 playerGridPosition = SceneManager.CurrentDungeon.DungeonSpawn;
            Render.DrawOffset = new Vector2(22.5f, -76.5f);
            Render.ZLayer = -1;
            Render.ChangeTexture(Globals.Textures[charTexName]);
            Input playerInput = new Input();

            //AnimationAtlas playerAnimationAtlas = new AnimationAtlas(Globals.Textures["Char1"], 10);
            //AddComponent(playerAnimationAtlas);
            Animation playerAnimation = new Animation();
            AddComponent(playerAnimation);

            playerAnimation.AddAtlas("StdPlayer1", Globals.Textures[charTexName], 10);
            playerAnimation.SetDefaultAtlus("StdPlayer1");
            playerAnimation.AddAnimation("RunningUp", 8, 9, 2, null, MoveSpeed/2);
            playerAnimation.AddAnimation("RunningForward", 4, 5, 0, null, MoveSpeed/2);
            playerAnimation.AddAnimation("RunningRight", 6, 6, 1, null, MoveSpeed);
            playerAnimation.AddAnimation("RunningLeft", 7, 7, 3, null, MoveSpeed);

            /*
             * Animation atlas rework
             * 
             * animation atlas class is filled w info about the texture, frames, rectangles
             * animation class holds all a player's animation tracks. this is a component.
             *      This class creates the atlases and tracks and gives them the info they need about the entity that controls them
             * animation track class sets the frames that get animated in an atlas, the speed, if it loops, play, stop, resume, pause controls
             */

            /*
             * PLAYER ANIMATIONS:
             * Frame 0: looking at camera
             * Frame 1: looking right
             * Frame 2: looking back
             * Frame 3: looking left
             * Frame 4 & 5: Running forward
             * Frame 6: walking right
             * Frame 7: walking left
             * Frame 8 & 9: Running up
             * 
            */
            //up
            playerInput.BindKey(Keys.W, (pressed, held) =>
            {
                if(pressed || !pressed && held && Sprinting && !Transform.Lerping)
                {
                    if (SceneManager.CurrentDungeon.DungeonTileGrid[(int)playerGridPosition.X, (int)playerGridPosition.Y - 1] == DTypes.TileType.Floor)
                    {
                        playerGridPosition.Y -= 1;
                        Transform.Lerp(Globals.GridToWorld(playerGridPosition), MoveSpeed);
                        playerAnimation.PlayAnimation("RunningUp");
                        
                    }
                }
            });

            //left
            playerInput.BindKey(Keys.A, (pressed, held) =>
            {
                if (pressed || !pressed && held && Sprinting && !Transform.Lerping)
                {
                    if (SceneManager.CurrentDungeon.DungeonTileGrid[(int)playerGridPosition.X - 1, (int)playerGridPosition.Y] == DTypes.TileType.Floor)
                    {
                        playerGridPosition.X -= 1;
                        Transform.Lerp(Globals.GridToWorld(playerGridPosition), MoveSpeed);
                        playerAnimation.PlayAnimation("RunningLeft");
                    }
                }
            });

            //down
            playerInput.BindKey(Keys.S, (pressed, held) =>
            {
                if (pressed || !pressed && held && Sprinting && !Transform.Lerping)
                {
                    if (SceneManager.CurrentDungeon.DungeonTileGrid[(int)playerGridPosition.X, (int)playerGridPosition.Y + 1] == DTypes.TileType.Floor)
                    {
                        playerGridPosition.Y += 1;
                        Transform.Lerp(Globals.GridToWorld(playerGridPosition), MoveSpeed);
                        playerAnimation.PlayAnimation("RunningForward");
                    }
                }
            });

            //right
            playerInput.BindKey(Keys.D, (pressed, held) =>
            {
                if (pressed|| !pressed && held && Sprinting && !Transform.Lerping)
                {
                    if (SceneManager.CurrentDungeon.DungeonTileGrid[(int)playerGridPosition.X + 1, (int)playerGridPosition.Y] == DTypes.TileType.Floor)
                    {
                        playerGridPosition.X += 1;
                        Transform.Lerp(Globals.GridToWorld(playerGridPosition), MoveSpeed);
                        playerAnimation.PlayAnimation("RunningRight");
                    }
                }
            });

            playerInput.BindKey(Keys.LeftShift, (pressed, held) =>
            {
                if (pressed || held)
                {
                    Sprinting = true;
                    MoveSpeed = 100;
                }
                else
                {
                    Sprinting = false;
                    MoveSpeed = 200;
                }
            });

            AddComponent(playerInput);
        }

    }
}
