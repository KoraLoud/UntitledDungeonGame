using Bunni.Resources.Components;
using Bunni.Resources.Modules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Render playerRender = new Render(Globals.Textures["blank"]);
            playerRender.ZLayer = -1;
            AddComponent(playerRender);
            Input playerInput = new Input();
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
