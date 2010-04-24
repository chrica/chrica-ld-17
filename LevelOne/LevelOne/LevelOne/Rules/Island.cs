using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LevelOne.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LevelOne.Rules
{
    public enum Status { Normal, Guiding, Hexxed }
    public enum Curse { None, Red, Yellow, Purple }

    public class Island : Sprite
    {
        public Vector2 Location { get; set; }
        public Status Status { get; set; }
        public IList<Curse> Curses { get; set; }

        public Island()
        {

            Location = Vector2.Zero;
            Status = Status.Normal;
            Curses = new List<Curse>();
        }

        public override void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>(@"island");
        }
    }
}
