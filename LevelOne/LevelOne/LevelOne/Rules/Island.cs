using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LevelOne.Rules
{
    public enum Status { Normal, Guiding, Hexxed }
    public enum Curse { None, Red, Yellow, Purple }

    public class Island
    {
        public Vector2 Location { get; set; }
        public Status Status { get; set; }
        public IList<Curse> Curses { get; set; }
    }
}
