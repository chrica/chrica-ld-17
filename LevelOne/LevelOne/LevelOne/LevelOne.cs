using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class LevelOne : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Dictionary<Vector2, Island> _islands;
        private Texture2D _islandTexture;
        private bool _newGame = true;

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

            if(_newGame)
            {
                var islands = new List<Island>();
                var random = new Random();

                Parallel.For(1, 5, x => {
                    Parallel.For(1, 5, y => {
                        var island = new Island { Location = new Vector2(x, y) };
                        island.Postion = island.Location * new Vector2(150.0f, 100.0f);
                        island.Effects = random.Next(10) % 2 == 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                        island.Ratio = new Vector2(random.Next(30, 50) / 100.0f, random.Next(40, 50) / 100.0f);
                        island.Texture = _islandTexture;
                        islands.Add(island);
                    });
                });

                _islands = islands.ToDictionary(island => island.Location);

                _newGame = false;
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

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
