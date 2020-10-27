using Bunni.Resources.Components;
using Bunni.Resources.Modules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using UntitledDungeonGame.Resources;
using UntitledDungeonGame.Resources.Game;
using UntitledDungeonGame.Resources.MainMenu;

namespace UntitledDungeonGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Engine : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Entity CameraDude;

        public string Title = "Untitled Dungeon Game";

        public int FPS;
        private int FpsCounter = 0;
        private int MilisecondsElapsed = 0;

        private bool Debugging = false;

        public Color BackgroundColor = Color.DarkSlateGray;

        public Scene MainMenuScene;
        public Scene MainGameScene;
        public Scene UILayerScene;



        public Dungeon MainDungeon;
        public Player player;

        public Engine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;

            //scene constructors
            MainMenuScene = new Scene();
            MainGameScene = new Scene();
            UILayerScene = new Scene();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            Camera.Init(new Vector2(400, 240), graphics, 800, 480);
            //Camera.UpdateWindow(800, 480);
            Camera.UpdateWindow(1600, 900);
            Camera.Zoom = 0.8f;

            CameraDude = new Entity(); //entity that exists just to hold our logic for moving camera
            Input cameraDudeInput = new Input();
            int cameraSpeed = 5;
            cameraDudeInput.BindKey(Keys.Up, (pressed, held) =>
            {
                if (pressed || held)
                {
                    Camera.Position = new Vector2(Camera.Position.X, Camera.Position.Y - cameraSpeed / Camera.Zoom);
                }
            });

            cameraDudeInput.BindKey(Keys.Left, (pressed, held) =>
            {
                if (pressed || held)
                {
                    Camera.Position = new Vector2(Camera.Position.X - cameraSpeed / Camera.Zoom, Camera.Position.Y);
                }
            });

            cameraDudeInput.BindKey(Keys.Down, (pressed, held) =>
            {
                if (pressed || held)
                {
                    Camera.Position = new Vector2(Camera.Position.X, Camera.Position.Y + cameraSpeed / Camera.Zoom);
                }
            });

            cameraDudeInput.BindKey(Keys.Right, (pressed, held) =>
            {
                if (pressed || held)
                {
                    Camera.Position = new Vector2(Camera.Position.X + cameraSpeed / Camera.Zoom, Camera.Position.Y);
                }
            });

            cameraDudeInput.BindKey(Keys.Q, (pressed, held) =>
            {
                if (pressed || held)
                {
                    Camera.Zoom -= 0.01f;
                }
            });

            cameraDudeInput.BindKey(Keys.E, (pressed, held) =>
            {
                if (pressed || held)
                {
                    Camera.Zoom += 0.01f;
                }
            });

            cameraDudeInput.BindKey(Keys.Space, (pressed, held) =>
            {
                if (pressed)
                {
                    MainGameScene.SceneEntities.RemoveAll((e) =>
                    {
                        return true;
                    });

                    SceneManager.ChangeScene(MainGameScene);
                }
            });

            CameraDude.AddComponent(cameraDudeInput);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load tiles
            DirectoryInfo dir = new DirectoryInfo(Content.RootDirectory + "/Tiles");
            if (dir.Exists)
            {
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    string fileName = file.Name.Split('.')[0];
                    Globals.Textures.Add(fileName, Content.Load<Texture2D>("Tiles/" + fileName));
                }
            }


            //load characters
            dir = new DirectoryInfo(Content.RootDirectory + "/Characters");
            if (dir.Exists)
            {
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    string fileName = file.Name.Split('.')[0];
                    Globals.Textures.Add(fileName, Content.Load<Texture2D>("Characters/" + fileName));
                }
            }

            Texture2D PlayTexture = Content.Load<Texture2D>("Play");


            Globals.Textures.Add("blank", Content.Load<Texture2D>("blank"));

            //main menu
            {
                Button playButton = new Button();
                playButton.AddTexture(PlayTexture);
                playButton.Render.Transform.X = (Camera.VirtualWidth / 2) - PlayTexture.Width / 2;
                playButton.Render.Transform.Y = (Camera.VirtualHeight / 4);

                playButton.OnWorldClick(() =>
                {
                    SceneManager.ChangeScene(MainGameScene);
                    SceneManager.ChangeUiScene(UILayerScene);
                });
                MainMenuScene.AddEntity(playButton);
            }

            //game

            MainGameScene.SetOnLoad(() =>
            {
                BackgroundColor = new Color(32, 32, 32);
                //generate dungeon
                //MainDungeon = new Dungeon(40, 6, 12, 50, 50);
                MainDungeon = new Dungeon(15, 3, 6, 25, 25);

                //add dungeon grid to scene entities
                for (int i = 0; i < MainDungeon.DungeonWidth; i++)
                {
                    for (int j = 0; j < MainDungeon.DungeonHeight; j++)
                    {
                        if (MainDungeon.DungeonEntityGrid[i, j] != null)
                        {
                            MainGameScene.AddEntity(MainDungeon.DungeonEntityGrid[i, j]);
                        }
                    }
                }
                foreach (Entity wall in MainDungeon.Walls)
                {
                    MainGameScene.AddEntity(wall);
                }
                Camera.Position = new Vector2((MainDungeon.DungeonWidth / 2) * Globals.TILE_WIDTH, (MainDungeon.DungeonHeight / 2) * Globals.TILE_HEIGHT);
                Camera.Zoom = 0.15f;
                MainGameScene.AddEntity(CameraDude);
                SceneManager.CurrentDungeon = MainDungeon;
                player = new Player();
                MainGameScene.AddEntity(player);
            });

            UILayerScene.SetOnLoad(() =>
            {
                Button AdminButton = new Button(); //temp texture
                AdminButton.AddTexture(PlayTexture);
                AdminButton.Transform.X = PlayTexture.Width / 2;
                AdminButton.Transform.Y = Camera.VirtualHeight / 4;
                AdminButton.OnScreenClick(() =>
                {
                    Debugging = !Debugging;
                });

                UILayerScene.AddEntity(AdminButton);

            });

            SceneManager.ChangeScene(MainMenuScene);

            //effect = Content.Load<Effect>("ShaderTest");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic
            SceneManager.CurrentScene.PreUpdate(gameTime);

            //Console.WriteLine(Camera.GetMouseWorldPosition());
            //Console.WriteLine(Camera.GetMouseWorldPosition());
            SceneManager.CurrentScene.Update(gameTime);
            if (SceneManager.CurrentUIScene != null)
            {
                SceneManager.CurrentUIScene.Update(gameTime);
            }




            SceneManager.CurrentScene.PostUpdate(gameTime);

            foreach (Entity tile in SceneManager.CurrentScene.SceneEntities)
            {
                if (tile.HasComponent<Render>())
                {
                    //tile.GetComponent<Render>().Color = Color.White;
                }
            }

            foreach (Entity tile in SceneManager.CurrentScene.SceneEntities)
            {
                if (tile.HasComponent<Render>() && Debugging)
                {
                    if (tile.TileType == DTypes.TileType.Wall)
                    {
                        tile.GetComponent<Render>().Color = Color.Purple;
                    }
                    else if (tile.TileType == DTypes.TileType.Floor)
                    {
                        tile.GetComponent<Render>().Color = Color.Green;
                    }
                    else if (tile.TileType == DTypes.TileType.Entrance)
                    {
                        tile.GetComponent<Render>().Color = Color.Yellow;
                    }
                    else if (tile.TileType == DTypes.TileType.Exit)
                    {
                        tile.GetComponent<Render>().Color = Color.Red;
                    }
                }
            }

            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {

                foreach (Entity tile in SceneManager.CurrentScene.SceneEntities)
                {
                    if (tile.HasComponent<Render>())
                    {
                        tile.GetComponent<Render>().Color = Color.White;
                    }
                }

                Vector2 MousePos = Input.GetGlobalMousePosition();
                int mouseX = (int)MousePos.X / Globals.TILE_WIDTH;
                int mouseY = (int)MousePos.Y / Globals.TILE_HEIGHT;
                if (SceneManager.DungeonExists() && mouseX >= 0 && mouseX <= SceneManager.CurrentDungeon.DungeonEntityGrid.GetLength(0) - 1 && mouseY >= 0 && mouseY <= SceneManager.CurrentDungeon.DungeonEntityGrid.GetLength(1) - 1 &&
                    SceneManager.CurrentDungeon.DungeonEntityGrid[mouseX, mouseY] != null)
                {
                    Tile End = SceneManager.CurrentDungeon.DungeonEntityGrid[mouseX, mouseY];
                    Tile Start = SceneManager.CurrentDungeon.DungeonEntityGrid[(int)player.playerGridPosition.X, (int)player.playerGridPosition.Y];
                    AStarSearch Path = new AStarSearch(Start, End);
                    Tile index = End;
                    while (index != Start)
                    {
                        index.Render.Color = Color.Yellow;
                        index = Path.cameFrom[index];
                    }
                }


                Console.WriteLine(mouseX + " " + mouseY);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackgroundColor);

            FpsCounter++;
            MilisecondsElapsed += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (MilisecondsElapsed >= 1000)
            {
                FPS = FpsCounter;
                FpsCounter = 0;
                MilisecondsElapsed = 0;
            }
            Window.Title = Title + " - FPS: " + FPS + " - " + SceneManager.CurrentScene.SceneEntities.Count + " entities in scene";

            //game world sprite batch
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Camera.TransformMatrix(gameTime));
            SceneManager.CurrentScene.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            //UI spritebatch
            spriteBatch.Begin();
            if (SceneManager.CurrentUIScene != null)
            {
                SceneManager.CurrentUIScene.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
