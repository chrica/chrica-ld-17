using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LevelOne.Core;
using LevelOne.Rules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace LevelOne
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class IslandsCurses : Microsoft.Xna.Framework.Game
    {
        public static Dictionary<string, Texture2D> Textures { get; private set; }
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private IslandMap _islandMap;

        private Sprite _cursor;

        private bool _newGame = true;
        private bool _gameActive;

        public IslandsCurses()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 1024;
            #if !DEBUG
            _graphics.IsFullScreen = true;
            #endif
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Textures = new Dictionary<string, Texture2D>
                            {
                                { "island", Content.Load<Texture2D>("island") },
                                { "hero", Content.Load<Texture2D>("hero") },
                                { "curses", Content.Load<Texture2D>("curses") },
                                { "cursor", Content.Load<Texture2D>("cursor") },
                                { "ward", Content.Load<Texture2D>("ward") },
                            };
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape))
                Exit();

            if(_cursor == null)
            {
                _cursor = new Sprite { Texture = Textures["cursor"], Ratio = new Vector2(0.75f)};
            }
            _cursor.Postion = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            if(_newGame)
            {
                var boardSize = new Vector2(6.0f, 5.0f);

                _islandMap = new IslandMap(boardSize, Window.ClientBounds);

                _newGame = false;
                _gameActive = true;
            }


            if(_gameActive)
            {
                _islandMap.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(155, 230, 255));

            _spriteBatch.Begin();

            _islandMap.Draw(_spriteBatch, gameTime);
            _cursor.Draw(_spriteBatch, gameTime);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
