using LevelOne.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LevelOne.Rules
{
    public class Hero : Sprite
    {
        public const float Speed = 10.0f;
        public Rectangle PlayableArea { get; private set; }
        public Rectangle ClickableArea { get; private set; }
        public bool Swimming { get; set; }

        public Hero(Rectangle playableArea, Rectangle clickableArea)
        {
            Texture = IslandsCurses.Textures["hero"];
            PlayableArea = playableArea;
            ClickableArea = clickableArea;
        }

        public override void Update(GameTime gameTime)
        {
            Velocity = new Vector2(0.0f);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed &&
                (PlayableArea.Contains(Mouse.GetState().X, Mouse.GetState().Y) || PlayableArea.Contains((int)Postion.X, (int)Postion.Y)) &&
                new Rectangle(0, 0,ClickableArea.Width, ClickableArea.Height).Contains(Mouse.GetState().X, Mouse.GetState().Y))
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
