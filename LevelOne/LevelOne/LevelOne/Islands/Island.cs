using System;
using System.Collections.Generic;
using LevelOne.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelOne.Islands
{
    public enum Status { Normal, Guiding, Hexxed }
    public enum Curse { None, Red, Yellow, Purple }

    public class Island : Sprite
    {
        public Vector2 Location { get; set; }
        public Status Status { get; set; }
        public IList<Curse> Curses { get; set; }

        public Island(Vector2 location, Random random, Texture2D texture)
        {
            Location = location;

            Postion = Location * new Vector2(150.0f, 100.0f);
            Effects = random.NextBool() ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Ratio = new Vector2(random.Next(30, 50) / 100.0f, random.Next(40, 50) / 100.0f);
            Texture = texture;

            Status = Status.Normal;
            Curses = new List<Curse>();
        }

        public override void Update(GameTime gameTime)
        {
            if(Status == Status.Guiding)
            {
                Ratio = new Vector2(0.25f);
            }
        }
    }
}
