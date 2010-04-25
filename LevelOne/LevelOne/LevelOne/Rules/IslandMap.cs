using System;
using System.Collections.Generic;
using System.Linq;
using LevelOne.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LevelOne.Rules
{
    public class IslandMap
    {
        public double StartTime { get; private set; }
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
                bool unique = false;
                do
                {
                    var island = Islands.TakeRandom(random).Value;
                    if (!Curses.Any(otherCurse => otherCurse.Haunt.Location == island.Location))
                    {
                        Curses.Add(new Curse(curse, random) {Postion = island.Postion, Haunt = island});
                        unique = true;
                    }
                } while (!unique);
            }

            Hero = new Hero(Rect, bounds) { Postion = Islands.TakeRandom(random).Value.Postion };
        }

        public void Update(GameTime gameTime)
        {
            if (StartTime < 1.0f)
                StartTime = gameTime.TotalGameTime.Milliseconds;
                
            var random = new Random();
            var herosIsland = Islands.Values.SingleOrDefault(island => island.Rect.Contains(Hero.Rect.Center));
            if (herosIsland != null)
            {
                var localCurses = Curses.Where(c => c.Haunt.Location == herosIsland.Location).ToList();
                foreach (var localCurse in localCurses)
                {
                    var availableHaunts = GetAvailableHaunts(localCurse);
                    if (availableHaunts.Any())
                    {
                        localCurse.Haunt = availableHaunts.TakeRandom(random);
                    }
                    else
                    {
                        Curses.Remove(localCurse);
                    }
                }
                Hero.Swimming = false;
            }
            else
            {
                Hero.Swimming = true;
            }

            if (Mouse.GetState().RightButton == ButtonState.Pressed && herosIsland != null && herosIsland.Status != Status.Warding)
            {
                herosIsland.Status = Status.Warding;

                var nearbyCurses = CurseType.Purple.GetDirections()
                    .Where(direction => Islands.ContainsKey(herosIsland.Location + direction))
                    .SelectMany(direction => Curses.Where(curse => curse.Haunt.Location == herosIsland.Location + direction));

                foreach (var nearbyCurse in nearbyCurses)
                {
                    var availableHaunts = GetAvailableHaunts(nearbyCurse);
                    if (availableHaunts.Any())
                    {
                        nearbyCurse.Haunt = availableHaunts.TakeRandom(random);
                    }
                }
            }

            Hero.Update(gameTime);

            foreach (var curse in Curses)
            {
                curse.Update(gameTime);
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

        public List<Island> GetAvailableHaunts(Curse curse)
        {
            return curse.Type.GetDirections()
                .SelectMany(direction => GetAvailableHauntsInDirection(curse.Haunt.Location + direction, direction, curse.Type.GetMoves(), location =>
                {
                    if (!Islands.ContainsKey(location))
                        return false;

                    if (Islands[location].Status == Status.Warding)
                        return false;

                    if (Curses.Any(otherCurse => otherCurse.Haunt.Location == location))
                        return false;

                    return true;
                }).Select(location => Islands[location])).ToList();
        }

        public List<Vector2> GetAvailableHauntsInDirection(Vector2 location, Vector2 direction, int movesLeft, Func<Vector2, bool> isValidIsland)
        {
            if (movesLeft > 0 && isValidIsland.Invoke(location))
            {
                var availableIslands = GetAvailableHauntsInDirection(location + direction, direction, --movesLeft, isValidIsland);
                availableIslands.Add(location);
                return availableIslands;
            }

            return new List<Vector2>();
        }
    }
}
