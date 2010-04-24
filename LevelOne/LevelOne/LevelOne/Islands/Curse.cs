using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LevelOne.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelOne.Islands
{
    public enum CurseType { Red = 0, Yellow = 300, Purple = 600 }

    public class Curse : Sprite
    {
        public static readonly Vector2 CurseDimensions = new Vector2(300.0f, 225.0f);

        public CurseType Type { get; private set; }

        public Curse(Texture2D texture, CurseType type)
        {
            Type = type;
            Texture = texture;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Texture, Postion - (Dimensions / 2),
                new Rectangle(0, 0, (int)CurseDimensions.X, (int)CurseDimensions.Y),
                Color.White, 0, Vector2.Zero, Ratio, Effects, 0);
        }
    }
}
