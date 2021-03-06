﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LevelOne.Core
{
    public class Sprite
    {
        public Texture2D Texture { get; set; }
        public Vector2 Postion { get; set; }


        public Vector2 Ratio { get; set; }
        public SpriteEffects Effects { get; set; }
        public Vector2 Velocity { get; set; }

        public Vector2 Dimensions
        {
            get
            {
                return new Vector2(Texture.Width * Ratio.X, Texture.Height * Ratio.Y);
            }
        }

        public Vector2 Center
        {
            get
            {
                return Postion + (Dimensions/2);
            }
        }

        public virtual Rectangle Rect
        {
            get
            {

                return new Rectangle((int)Postion.X, (int)Postion.Y, (int)Dimensions.X, (int)Dimensions.Y); 
            }
        }


        public Sprite()
        {
            Effects = SpriteEffects.None;
            Ratio = Vector2.One;
            Postion = Vector2.Zero;
            Velocity = Vector2.Zero;
        }

        public virtual void LoadContent(ContentManager content)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            Postion += Velocity;
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Texture, Postion, null, Color.White, 0, Vector2.Zero, Ratio, Effects, 0);
        }
    }
}
