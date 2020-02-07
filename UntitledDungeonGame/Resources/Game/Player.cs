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

        public int MoveSpeed { get; set; } = 100; //in miliseconds

        public bool Sprinting { get; set; } = false;

        public Player()
        {
            PositionVector playerPosition = new PositionVector();
            playerPosition.Position = Globals.GridToWorld(SceneManager.CurrentDungeon.DungeonSpawn);
            Vector2 playerGridPosition = SceneManager.CurrentDungeon.DungeonSpawn;
            AddComponent(playerPosition);
            Render playerRender = new Render(Globals.Textures["Char1"]);
            playerRender.DrawOffset = new Vector2(22.5f, -76.5f);
            playerRender.ZLayer = -1;
            AddComponent(playerRender);
            Input playerInput = new Input();

            //AnimationAtlas playerAnimationAtlas = new AnimationAtlas(Globals.Textures["Char1"], 10);
            //AddComponent(playerAnimationAtlas);

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
             * Frame 1: looking at camera
             * Frame 2: looking right
             * Frame 3: looking back
             * Frame 4: looking left
             * Frame 5 & 6: Running forward
             * Frame 7: walking right
             * Frame 8: walking left
             * Frame 9 & 10: Running up
             * 
            */
            //up
            playerInput.BindKey(Keys.W, (pressed, held) =>
            {
                if(pressed || !pressed && held && Sprinting && !playerPosition.Lerping)
                {
                    if (SceneManager.CurrentDungeon.DungeonTileGrid[(int)playerGridPosition.X, (int)playerGridPosition.Y - 1] == DTypes.TileType.Floor)
                    {
                        playerGridPosition.Y -= 1;
                        playerPosition.Lerp(Globals.GridToWorld(playerGridPosition), MoveSpeed);
                        
                    }
                }
            });

            //left
            playerInput.BindKey(Keys.A, (pressed, held) =>
            {
                if (pressed || !pressed && held && Sprinting && !playerPosition.Lerping)
                {
                    if (SceneManager.CurrentDungeon.DungeonTileGrid[(int)playerGridPosition.X - 1, (int)playerGridPosition.Y] == DTypes.TileType.Floor)
                    {
                        playerGridPosition.X -= 1;
                        playerPosition.Lerp(Globals.GridToWorld(playerGridPosition), MoveSpeed);
                    }
                }
            });

            //down
            playerInput.BindKey(Keys.S, (pressed, held) =>
            {
                if (pressed || !pressed && held && Sprinting && !playerPosition.Lerping)
                {
                    if (SceneManager.CurrentDungeon.DungeonTileGrid[(int)playerGridPosition.X, (int)playerGridPosition.Y + 1] == DTypes.TileType.Floor)
                    {
                        playerGridPosition.Y += 1;
                        playerPosition.Lerp(Globals.GridToWorld(playerGridPosition), MoveSpeed);
                    }
                }
            });

            //right
            playerInput.BindKey(Keys.D, (pressed, held) =>
            {
                if (pressed|| !pressed && held && Sprinting && !playerPosition.Lerping)
                {
                    if (SceneManager.CurrentDungeon.DungeonTileGrid[(int)playerGridPosition.X + 1, (int)playerGridPosition.Y] == DTypes.TileType.Floor)
                    {
                        playerGridPosition.X += 1;
                        playerPosition.Lerp(Globals.GridToWorld(playerGridPosition), MoveSpeed);
                    }
                }
            });

            playerInput.BindKey(Keys.LeftShift, (pressed, held) =>
            {
                if (pressed || held)
                {
                    Sprinting = true;
                    MoveSpeed = 50;
                }
                else
                {
                    Sprinting = false;
                    MoveSpeed = 100;
                }
            });

            AddComponent(playerInput);
        }

    }
}
