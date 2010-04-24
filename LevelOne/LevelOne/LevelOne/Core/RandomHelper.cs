using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelOne.Core
{
    public static class RandomHelper
    {
        public static bool NextBool(this Random random)
        {
            return random.Next(10)%2 == 0;
        }

        public static T TakeRandom<T>(this IEnumerable<T> enumerable, Random random)
        {
            return enumerable.Skip(random.Next(0, enumerable.Count())).First();
        }

        public static T TakeRandom<T>(this IEnumerable<T> enumerable)
        {
            return TakeRandom(enumerable, new Random());
        }
    }
}
