﻿using System;
using System.Collections.Generic;
using LevelOne.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelOne.Islands
{
    public enum Status { Normal, Warding, Hexxing }

    public class Island : Sprite
    {
        public static readonly Vector2 IslandSpace = new Vector2(200, 155);

        public Vector2 Location { get; set; }
        public Status Status { get; set; }
        public IList<Curse> Curses { get; set; }

        public Island(Vector2 location, Random random, Texture2D texture)
        {
            Location = location;

            Effects = random.NextBool() ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Ratio = new Vector2(random.NextRatio(60) * (IslandSpace.X / texture.Width), random.NextRatio(60) * (IslandSpace.Y / texture.Width));
            Texture = texture;

            Status = Status.Normal;
            Curses = new List<Curse>();
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Texture, Postion + (IslandSpace - Dimensions) / 2, null, Color.White, 0, Vector2.Zero, Ratio, Effects, 0);
        }
    }
}
