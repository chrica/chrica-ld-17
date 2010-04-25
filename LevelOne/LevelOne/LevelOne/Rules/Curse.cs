using System;
using System.Collections.Generic;
using System.Linq;
using LevelOne.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelOne.Rules
{
    public static class CurseHelper
    {
        public static List<Vector2> GetDirections(this CurseType curse)
        {
            switch (curse)
            {
                case CurseType.Red:
                    return new List<Vector2> { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(0, 1) };
                case CurseType.Green:
                    return new List<Vector2> { new Vector2(1), new Vector2(-1), new Vector2(1, -1), new Vector2(-1, 1) };
                case CurseType.Purple:
                    return new List<Vector2>
                                {
                                    new Vector2(1, 0),
                                    new Vector2(-1, 0),
                                    new Vector2(0, -1),
                                    new Vector2(0, 1),
                                    new Vector2(1),
                                    new Vector2(-1),
                                    new Vector2(1, -1),
                                    new Vector2(-1, 1)
                                };
                default:
                    return new List<Vector2>();
            }
        }

        public static int GetOffset(this CurseType curse)
        {
            switch (curse)
            {
                case CurseType.Green:
                    return 60;
                case CurseType.Purple:
                    return 120;
                default:
                    return 0;
            }
        }

        public static int GetMoves(this CurseType curse)
        {
            switch (curse)
            {
                case CurseType.Red:
                    return 3;
                case CurseType.Green:
                    return 3;
                default:
                    return 1;
            }
        }
    }

    public enum CurseType { Red, Green, Purple }

    public class Curse : Sprite
    {
        public int Id { get; private set; }
        public const float Speed = 10.0f;
        public static readonly Vector2 CurseDimensions = new Vector2(60.0f, 45.0f);
        private const double _flyTime = 1000.0d;
        private bool _flying;
        private double _flyStart;

        public Island Haunt { get; set; }

        public CurseType Type { get; private set; }

        public Curse(int id, Random random) : this(id, Enum.GetValues(typeof(CurseType)).Cast<CurseType>().TakeRandom(random)) { }

        public Curse(int id, CurseType type)
        {
            Id = id;
            Type = type;
            Texture = IslandsCurses.Textures["curses"];
        }

        public override Rectangle Rect
        {
            get
            {

                return new Rectangle((int)Postion.X, (int)Postion.Y, (int)CurseDimensions.X, (int)CurseDimensions.Y);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Haunt != null)
            {
                if (!Rect.Contains(Haunt.Rect.Center.X, Haunt.Rect.Center.Y))
                {
                    if (_flying)
                    {
                        double ratio = Math.Min(gameTime.TotalGameTime.TotalMilliseconds - _flyStart, _flyTime) / _flyTime;
                        Postion = Vector2.Lerp(Postion, new Vector2(Haunt.Rect.Center.X, Haunt.Rect.Center.Y), Convert.ToSingle(ratio));
                    }
                    else
                    {
                        _flying = true;
                        _flyStart = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
                else
                {
                    _flying = false;
                }
            }
        }


        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Texture, Postion,
                new Rectangle(Type.GetOffset(), 0, (int)CurseDimensions.X, (int)CurseDimensions.Y),
                Color.White, 0, Vector2.Zero, Ratio, Effects, 0);
        }
    }
}
