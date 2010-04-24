using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LevelOne.Islands;
using LevelOne.Core;
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
    public class LevelOne : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Dictionary<Vector2, Island> _islands;
        private Hero _hero;

        private Texture2D _islandTexture;
        private Texture2D _heroTexture;
        private bool _newGame = true;
        private bool _gameActive;

        public LevelOne()
        {
            _graphics = new GraphicsDeviceManager(this);
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

            _islandTexture = Content.Load<Texture2D>(@"island");
            _heroTexture = Content.Load<Texture2D>(@"hero");
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            if(_gameActive)
            {
                _hero.Velocity = new Vector2(0.0f);
                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Up))
                {
                    _hero.Velocity += new Vector2(0.0f, -Hero.Speed);
                } else if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Down))
                {
                    _hero.Velocity += new Vector2(0.0f, Hero.Speed);
                }

                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Left))
                {
                    _hero.Velocity += new Vector2(-Hero.Speed, 0.0f);
                }
                else if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Right))
                {
                    _hero.Velocity += new Vector2(Hero.Speed, 0.0f);
                }

                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Space))
                {
                    Parallel.ForEach(_islands.Values, island => {
                          if(_hero.Rect.Intersects(island.Rect))
                          {
                              island.Status = Status.Guiding;
                          }
                      });
                }

                _hero.Update(gameTime);
                Parallel.ForEach(_islands.Values, island => island.Update(gameTime));
            }


            if(_newGame)
            {
                var islands = new List<Island>();
                var random = new Random();

                Parallel.For(1, 5, x => Parallel.For(1, 5, y => islands.Add(new Island(new Vector2(x, y), random, _islandTexture))));

                _islands = islands.ToDictionary(island => island.Location);
                _hero = new Hero
                            {
                                Postion = _islands.TakeRandom(random).Value.Postion,
                                Texture = _heroTexture,
                                Ratio = new Vector2(0.6f)
                            };

                _newGame = false;
                _gameActive = true;
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();

            foreach (var island in _islands.Values)
            {
                island.Draw(_spriteBatch, gameTime);
            }

            _hero.Draw(_spriteBatch, gameTime);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
