using System;
using LevelOne.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelOne.Rules
{
    public enum Status { Normal, Warding, Hexxing }

    public class Island : Sprite
    {
        public static readonly Vector2 IslandSpace = new Vector2(200, 155);

        public Vector2 Location { get; set; }
        public Status Status { get; set; }

        public Island(Vector2 location, Random random)
        {
            Location = location;

            Effects = random.NextBool() ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Texture = IslandsCurses.Textures["island"];
            Ratio = new Vector2(random.NextRatio(60) * (IslandSpace.X / Texture.Width), random.NextRatio(60) * (IslandSpace.Y / Texture.Width));

            Status = Status.Normal;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            if (Status == Status.Warding)
            {
                var ward = new Sprite
                               {
                                   Ratio = new Vector2(0.75f),
                                   Texture = IslandsCurses.Textures["ward"],
                                   Postion = Center + new Vector2(0.0f, -50.0f)
                               };
                ward.Draw(spriteBatch, gameTime);
            }
        }
    }
}
