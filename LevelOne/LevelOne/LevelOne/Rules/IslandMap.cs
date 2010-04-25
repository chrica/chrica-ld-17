using System;
using System.Collections.Generic;
using System.Linq;
using LevelOne.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelOne.Rules
{
    public class IslandMap
    {
        public Vector2 Position { get; private set; }
        public Vector2 BoardSize { get; private set; }
        public Dictionary<Vector2, Island> Islands { get; private set; }
        public List<Curse> Curses { get; private set; }
        public Hero Hero { get; private set; }

        public Rectangle Rect
        {
            get
            {

                return new Rectangle((int)Position.X, (int)Position.Y, (int)Dimensions.X, (int)Dimensions.Y);
            }
        }

        public Vector2 Dimensions
        {
            get
            {
                return BoardSize * Island.IslandSpace;
            }
        }

        public IslandMap(Vector2 boardSize, Rectangle bounds)
        {
            BoardSize = boardSize;
            Position = new Vector2(bounds.Width, bounds.Height) / 2 - (boardSize * Island.IslandSpace) / 2;

            Islands = new Dictionary<Vector2, Island>();
            var random = new Random();
            for (int x = 1; x < (int)BoardSize.X + 1; x++) for (int y = 1; y < (int)BoardSize.Y + 1; y++)
                {
                    Islands.Add(new Vector2(x, y), new Island(new Vector2(x, y), random) { Postion = Position + (new Vector2(x, y) - Vector2.One) * Island.IslandSpace });
                }

            Curses = new List<Curse>();
            for (int curse = 0; curse < (BoardSize.X + BoardSize.Y) / 2; curse++)
            {
                var island = Islands.TakeRandom(random).Value;
                Curses.Add(new Curse(curse, random) { Postion = island.Postion, Haunt = island });
            }

            Hero = new Hero(Rect, bounds) { Postion = Islands.TakeRandom(random).Value.Postion };
        }

        public void Update(GameTime gameTime)
        {
            var random = new Random();
            Hero.Update(gameTime);

            var herosIsland = Islands.Values.SingleOrDefault(island => island.Rect.Contains(Hero.Rect.Center));



            foreach (var curse in Curses)
            {
                if (herosIsland != null && herosIsland.Location == curse.Haunt.Location)
                {
                    MoveCurse(curse, random);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var island in Islands)
            {
                island.Value.Draw(spriteBatch, gameTime);
            }

            Hero.Draw(spriteBatch, gameTime);

            foreach (var curse in Curses)
            {
                curse.Draw(spriteBatch, gameTime);
            }
        }

        public void MoveCurse(Curse curse, Random random)
        {
            var availableHaunts = curse.Type.GetDirections()
                .SelectMany(direction => AvailableIslandsInDirection(curse.Haunt.Location + direction, direction, curse.Type.GetMoves(), location =>
                {
                    if (!Islands.ContainsKey(location))
                        return false;

                    if (Islands[location].Status == Status.Warding)
                        return false;

                    return true;
                }).Select(location => Islands[location]));

            if (availableHaunts.Any())
            {
                curse.Haunt = availableHaunts.TakeRandom(random);
                curse.Postion = curse.Haunt.Postion;
            }
            else
            {
                Curses.Remove(curse);
            }
        }

        public List<Vector2> AvailableIslandsInDirection(Vector2 location, Vector2 direction, int movesLeft, Func<Vector2, bool> isValidIsland)
        {
            if (movesLeft > 0 && isValidIsland.Invoke(location))
            {
                var availableIslands = AvailableIslandsInDirection(location + direction, direction, --movesLeft, isValidIsland);
                availableIslands.Add(location);
                return availableIslands;
            }

            return new List<Vector2>();
        }
    }
}
