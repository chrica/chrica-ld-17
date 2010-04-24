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
        public const float Speed = 7.0f;

        public Hero(Texture2D texture)
        {
            Texture = texture;
            Ratio = new Vector2(0.1225f);
        }

        public void Update(GameTime gameTime, IslandMap islandMap)
        {
            Velocity = new Vector2(0.0f);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed &&
                (islandMap.Rect.Contains(Mouse.GetState().X, Mouse.GetState().Y) ||
                islandMap.Rect.Contains((int)Postion.X, (int)Postion.Y)))
            {
                Vector2 mouse = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                float distance = Vector2.Distance(Postion, mouse);
                if (distance > Speed)
                {
                    Vector2 rightDistance = mouse - Postion;
                    Velocity = new Vector2(rightDistance.X / distance * Speed, rightDistance.Y / distance * Speed);
                }
            }

            base.Update(gameTime);
        }
    }
}
