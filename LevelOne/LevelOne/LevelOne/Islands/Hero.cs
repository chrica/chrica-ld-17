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
    public class Hero : Sprite
    {
        public const float Speed = 5.0f;

        public Hero(Texture2D texture)
        {
            Texture = texture;
            Ratio = new Vector2(0.1225f);
        }

        public override void Update(GameTime gameTime)
        {
            Velocity = new Vector2(0.0f);
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Up))
            {
                Velocity += new Vector2(0.0f, -Speed);
            }
            else if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Down))
            {
                Velocity += new Vector2(0.0f, Speed);
            }

            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Left))
            {
                Velocity += new Vector2(-Speed, 0.0f);
            }
            else if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Right))
            {
                Velocity += new Vector2(Speed, 0.0f);
            }

            base.Update(gameTime);
        }
    }
}
