using Bunni.Resources.Components;
using Bunni.Resources.Modules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
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

        public Color BackgroundColor = Color.DarkSlateGray;

        public Scene MainMenuScene;
        public Scene MainGameScene;

        public Dictionary<Tile, Texture2D> Textures = new Dictionary<Tile, Texture2D>();

        //public GameState CurrentGameState;
        private SpriteFont ArielFont;

        public Dungeon MainDungeon;

        public Engine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Camera.Init(new Vector2(400*2, 240*2), graphics, 800*2, 480*2);
            Camera.UpdateWindow(800*2, 480*2);
            Camera.Zoom = 0.8f;

            IsMouseVisible = true;
            //CurrentGameState = GameState.MainMenu;

            //scene constructors
            MainMenuScene = new Scene();
            MainGameScene = new Scene();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            CameraDude = new Entity(); //entity that exists just to hold our logic for moving camera
            Input cameraDudeInput = new Input();
            int cameraSpeed = 5;
            cameraDudeInput.BindKey(Keys.W, (pressed, held) =>
            {
                if (pressed || held)
                {
                    Camera.Position = new Vector2(Camera.Position.X, Camera.Position.Y - cameraSpeed/Camera.Zoom);
                }
            });

            cameraDudeInput.BindKey(Keys.A, (pressed, held) =>
            {
                if (pressed || held)
                {
                    Camera.Position = new Vector2(Camera.Position.X-cameraSpeed/Camera.Zoom, Camera.Position.Y);
                }
            });

            cameraDudeInput.BindKey(Keys.S, (pressed, held) =>
            {
                if (pressed || held)
                {
                    Camera.Position = new Vector2(Camera.Position.X, Camera.Position.Y + cameraSpeed/Camera.Zoom);
                }
            });

            cameraDudeInput.BindKey(Keys.D, (pressed, held) =>
            {
                if (pressed || held)
                {
                    Camera.Position = new Vector2(Camera.Position.X+cameraSpeed/Camera.Zoom, Camera.Position.Y);
                }
            });

            cameraDudeInput.BindKey(Keys.Q, (pressed, held) =>
            {
                if(pressed||held)
                {
                    Camera.Zoom -= 0.001f;
                }
            });

            cameraDudeInput.BindKey(Keys.E, (pressed, held) =>
            {
                if (pressed || held)
                {
                    Camera.Zoom += 0.001f;
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
            ArielFont = Content.Load<SpriteFont>("Ariel");

            //load stuff
            Texture2D stoneTexture = Content.Load<Texture2D>("Stone");
            Textures.Add(Tile.Floor, stoneTexture);
            Textures.Add(Tile.Wall, stoneTexture);
            Textures.Add(Tile.Doorway, stoneTexture);


            //main menu
            {
                Texture2D PlayTexture = Content.Load<Texture2D>("Play");
                Render PlayRender = new Render(PlayTexture);
                PositionVector PlayPosition = new PositionVector
                {
                    X = (Camera.VirtualWidth / 2) - PlayTexture.Width / 2,
                    Y = (Camera.VirtualHeight / 4)
                };

                Button playButton = new Button();
                playButton.AddComponent(PlayPosition);
                playButton.AddComponent(PlayRender);
                playButton.OnClick(() =>
                    {
                        SceneManager.ChangeScene(MainGameScene);
                    });
                MainMenuScene.AddEntity(playButton);
            }

            //game
            int rooms = 100;

            MainGameScene.SetOnLoad(() =>
            {
                BackgroundColor = Color.Black;
                //generate dungeon
                MainDungeon = new Dungeon(Textures);
                MainDungeon.GenerateDungeon(rooms);
                MainDungeon.CurrentRoom = MainDungeon.DungeonArray[0, 0];
                /*
                Entity[,] roomEntities = MainDungeon.BuildRoom(MainDungeon.CurrentRoom);

                for (int i = 0; i < roomEntities.GetUpperBound(0)+1; i++)
                {
                    for (int j = 0; j < roomEntities.GetUpperBound(1)+1; j++)
                    {
                        if (roomEntities[i,j] != null)
                        {
                            MainGameScene.AddEntity(roomEntities[i, j]);
                        }
                    }
                }*/

                for(int i=0;i<MainDungeon.DungeonArray.GetUpperBound(0)+1;i++)
                {
                    for(int j=0;j<MainDungeon.DungeonArray.GetUpperBound(1)+1;j++)
                    {
                        if(MainDungeon.DungeonArray[i,j] != null)
                        {
                            Entity[,] roomEntities = MainDungeon.BuildRoom(MainDungeon.DungeonArray[i, j]);

                            for (int ia = 0; ia < roomEntities.GetUpperBound(0) + 1; ia++)
                            {
                                for (int ja = 0; ja < roomEntities.GetUpperBound(1) + 1; ja++)
                                {
                                    if (roomEntities[ia, ja] != null)
                                    {
                                        PositionVector p = roomEntities[ia, ja].GetComponent<PositionVector>();
                                        p.X += (((roomEntities.GetUpperBound(0) + 1) * Textures[Tile.Floor].Width) * i);
                                        p.Y += (((roomEntities.GetUpperBound(0) + 1) * Textures[Tile.Floor].Height) * j);

                                        Camera.Position = new Vector2(p.X, p.Y);
                                        MainGameScene.AddEntity(roomEntities[ia, ja]);
                                    }
                                }
                            }
                        }
                    }
                }

                MainGameScene.AddEntity(CameraDude);
            });

            SceneManager.ChangeScene(MainMenuScene);

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



            SceneManager.CurrentScene.PostUpdate(gameTime);



            /*FpsCounter++;
            MilisecondsElapsed += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(MilisecondsElapsed >= 1000)
            {
                FPS = FpsCounter;
                FpsCounter = 0;
                MilisecondsElapsed = 0;
            }
            Window.Title = Title + " - FPS: " + FPS; */
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
            Window.Title = Title + " - FPS: " + FPS;

            //UI spritebatch
            spriteBatch.Begin();

            spriteBatch.End();

            //game world sprite batch
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Camera.TransformMatrix());
            SceneManager.CurrentScene.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
