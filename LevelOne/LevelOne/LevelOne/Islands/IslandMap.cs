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
        public Vector2 Postion { get; set; }
        public Dictionary<Vector2, Island> Islands { get; set; }

        public IslandMap(Texture2D islandTexture)
        {
            var islands = new List<Island>();
            var random = new Random();

            Parallel.For(1, 5, x => Parallel.For(1, 5, y => islands.Add(new Island(new Vector2(x, y), random, islandTexture))));

            Islands = islands.ToDictionary(island => island.Location);
        }

        public void Update(GameTime gameTime, Hero hero)
        {
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Space))
            {
                Parallel.ForEach(Islands.Values, island =>
                {
                    if (hero.Rect.Intersects(island.Rect))
                    {
                        island.Status = Status.Guiding;
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
            }

        }
    }
}
