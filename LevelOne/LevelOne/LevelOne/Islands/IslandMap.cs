using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevelOne.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LevelOne.Islands
{
    public class IslandMap
    {
        private Vector2 _postion;
        public Vector2 Postion {
            get
            {
                return _postion;
            }
            set
            {
                _postion = value;
                Parallel.ForEach(Islands.Values, island => island.Postion = _postion + (island.Location - Vector2.One) * Island.IslandSpace);
            }
}
        public Vector2 BoardSize { get; private set; }
        public Dictionary<Vector2, Island> Islands { get; private set; }

        public Rectangle Rect
        {
            get
            {

                return new Rectangle((int)Postion.X, (int)Postion.Y, (int)Dimensions.X, (int)Dimensions.Y);
            }
        }

        public Vector2 Dimensions
        {
            get
            {
                return BoardSize * Island.IslandSpace;
            }
        }

        public IslandMap(Texture2D islandTexture, Vector2 boardSize)
        {
            BoardSize = boardSize;

            Islands = new Dictionary<Vector2, Island>();
            var random = new Random();
            for (int x = 1; x < (int)BoardSize.X + 1; x++) for (int y = 1; y < (int)BoardSize.Y + 1; y++)
            {
                Islands[new Vector2(x, y)] = new Island(new Vector2(x, y), random, islandTexture);
            }
        }

        public void Update(GameTime gameTime, Hero hero)
        {
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Space))
            {
                Parallel.ForEach(Islands.Values, island =>
                {
                    if (hero.Rect.Intersects(island.Rect))
                    {
                        island.Status = Status.Warding;
                    }
                });
            }

            Parallel.ForEach(Islands.Values, island => island.Update(gameTime));
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var island in Islands.Values)
            {
                island.Draw(spriteBatch, gameTime);

                foreach (var curse in island.Curses)
                {
                    curse.Draw(spriteBatch, gameTime);
                }
            }
        }
    }
}
