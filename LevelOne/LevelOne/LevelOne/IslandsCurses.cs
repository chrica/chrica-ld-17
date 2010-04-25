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
        private SpriteFont _font;
        private SpriteFont _titleFont;

        private bool _loadscreenActive = true;
        private bool _newGame;
        private bool _gameActive;
        private bool _showTutorial;
        private bool _showWin;

        public IslandsCurses()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 1024;
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

            _font = Content.Load<SpriteFont>("papyrus");
            _titleFont = Content.Load<SpriteFont>("papyrus-title");
            Textures = new Dictionary<string, Texture2D>
                            {
                                { "island", Content.Load<Texture2D>("island") },
                                { "hero", Content.Load<Texture2D>("hero") },
                                { "curses", Content.Load<Texture2D>("curses") },
                                { "cursor", Content.Load<Texture2D>("cursor") },
                                { "ward", Content.Load<Texture2D>("ward") },
                                { "island-full", Content.Load<Texture2D>("island-full") },
                                { "hero-full", Content.Load<Texture2D>("hero-full") },
                                { "curse-full", Content.Load<Texture2D>("curse-full") },
                                { "ward-full", Content.Load<Texture2D>("ward-full") },
                                { "tute", Content.Load<Texture2D>("tute") },
                                { "dialog", Content.Load<Texture2D>("dialog") },
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

            if (_cursor == null)
            {
                _cursor = new Sprite { Texture = Textures["cursor"], Ratio = new Vector2(0.75f) };
            }
            _cursor.Postion = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                _showTutorial = false;
                if (_showWin && (gameTime.TotalGameTime.TotalMilliseconds - _islandMap.WinTime) > 5000.0f)
                {
                    _showWin = false;
                    _loadscreenActive = true;
                }
            }

            if (_loadscreenActive)
            {
                if(Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    if(new Rectangle((Window.ClientBounds.Width / 2) - 30, (Window.ClientBounds.Height / 2) + 165, 60, 50).Contains(Mouse.GetState().X, Mouse.GetState().Y))
                    {
                        _loadscreenActive = false;
                        _newGame = true;
                    }

                    if (new Rectangle((Window.ClientBounds.Width / 2) - 30, (Window.ClientBounds.Height / 2) + 225, 60, 50).Contains(Mouse.GetState().X, Mouse.GetState().Y))
                    {
                        _showTutorial = true;
                    }
                }
            }

            if (_newGame)
            {
                var boardSize = new Vector2(6.0f, 5.0f);

                _islandMap = new IslandMap(boardSize, Window.ClientBounds);

                _newGame = false;
                _gameActive = true;
            }


            if (_gameActive)
            {
                if (_islandMap.Won)
                {
                    _gameActive = false;
                    _showWin = true;
                }
                else
                {
                    _islandMap.Update(gameTime);
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed &&
                        new Rectangle(Window.ClientBounds.Width - (int)60.0f, 0, 60, 60).Contains(Mouse.GetState().X, Mouse.GetState().Y))
                    {
                        _showTutorial = true;
                    }
                }
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

            //omg hack for your life!!!
            if (_loadscreenActive)
            {
                Vector2 center = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);

                _spriteBatch.DrawString(_titleFont, "Islands!?! Curses!?!", (center * new Vector2(1.0f, 0.0f)) + new Vector2(-350.0f, 25.0f), Color.Olive);
                _spriteBatch.DrawString(_font, "Play!", center + new Vector2(-30.0f, 165.0f), Color.Olive);
                _spriteBatch.DrawString(_font, "How to play!", center + new Vector2(-100.0f, 225.0f), Color.Olive);

                new Sprite
                    {
                        Texture = Textures["island-full"],
                        Ratio = new Vector2(1.0f),
                        Postion = center - new Vector2(Textures["island-full"].Width, Textures["island-full"].Width) / 2 * new Vector2(1.0f)
                    }.Draw(_spriteBatch, gameTime);
                new Sprite
                    {
                        Texture = Textures["hero-full"],
                        Ratio = new Vector2(0.4f),
                        Postion = (center - new Vector2(Textures["hero-full"].Width, Textures["hero-full"].Width) / 2 * new Vector2(0.4f)) + new Vector2(50, 0)
                    }.Draw(_spriteBatch, gameTime);
                new Sprite
                    {
                        Texture = Textures["curse-full"],
                        Ratio = new Vector2(0.4f),
                        Postion = (center - new Vector2(Textures["hero-full"].Width, Textures["hero-full"].Width) / 2 * new Vector2(0.4f)) + new Vector2(-125, -175)
                    }.Draw(_spriteBatch, gameTime);
            }

            if (_gameActive || _showWin)
            {
                //Lets draw some hud!!
                //curse status
                Vector2 cursesStatus = new Vector2(0.0f, 25.0f);
                foreach (var curse in _islandMap.Curses)
                {
                    new Curse(0, curse.Type)
                        {
                            Postion = cursesStatus += new Vector2(Curse.CurseDimensions.X, 0.0f)
                    }.Draw(_spriteBatch, gameTime);

                }

                double timeLeft = (_islandMap.WinTime > 0.0 ? _islandMap.WinTime : gameTime.TotalGameTime.TotalMilliseconds) - _islandMap.StartTime;
                _spriteBatch.DrawString(_font, string.Format("{0:00}:{1:00}",
                    (int)((timeLeft / 1000.0d) / 60.0d),
                    (int)(timeLeft / 1000.0d) % 60.0d),
                    new Vector2(450.0f, 15.0f), Color.Olive);

                //tutorial
                _spriteBatch.DrawString(_titleFont, "?", new Vector2(Window.ClientBounds.Width - 50.0f, -25.0f), Color.Olive);

                _islandMap.Draw(_spriteBatch, gameTime);
            }

            if (_showTutorial)
            {
                Vector2 center = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
                new Sprite
                {
                    Texture = Textures["tute"],
                    Ratio = new Vector2(1.0f),
                    Postion = center - new Vector2(400.0f, 300.0f)
                }.Draw(_spriteBatch, gameTime);
            }

            if (_showWin)
            {
                Vector2 center = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
                new Sprite
                {
                    Texture = Textures["dialog"],
                    Ratio = new Vector2(1.0f),
                    Postion = center - new Vector2(400.0f, 100.0f)
                }.Draw(_spriteBatch, gameTime);

                _spriteBatch.DrawString(_font, string.Format(
                    "You Win! way to capture those things...\n Masks: {0}, Time: {1:00}:{2:00}, Score: {3:0}",
                    _islandMap.Islands.Count(island => island.Value.Status == Status.Warding),
                    (int)(((_islandMap.WinTime - _islandMap.StartTime) / 1000.0d) / 60.0d),
                    (int)(((_islandMap.WinTime - _islandMap.StartTime) / 1000.0d) % 60.0d),
                     (_islandMap.Islands.Count(island => island.Value.Status != Status.Warding) * 15000) - (_islandMap.WinTime - _islandMap.StartTime)
                    ), center - new Vector2(330.0f, 75.0f), Color.Olive);
            }

            _cursor.Draw(_spriteBatch, gameTime);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
