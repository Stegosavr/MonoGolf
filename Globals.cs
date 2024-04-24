using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Collisions;

namespace Snakedy
{
    public static class Globals
    {
        public static Game1 Game;
        public static Character Ball;

        public static int ScreenWidth;
        public static int ScreenHeight;

        public static readonly Random Random = new Random(Guid.NewGuid().GetHashCode());

        public static GameTime GameTime;
        public static CollisionComponent CollisionComponent;

        public static int Score;

        public static List<IObstacle> Obstacles;
    }
}
