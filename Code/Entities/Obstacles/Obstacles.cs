using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace Snakedy
{
    public static class Obstacles 
    {
        public static List<IObstacle> CreateRandomObstacles(int count,Func<IEnumerable<IShapeF>,IObstacle> creator,List<IShapeF> obstacles = null)
        {
            var list = new List<IObstacle>();
            for (int i = 0; i < count; i++)
            {
                var shape = creator(obstacles);
                list.Add(shape);
                Globals.CollisionComponent.Insert(shape);
                Globals.Obstacles.Add(shape);
            }
            return list;
        }

        public static List<IObstacle> CreateRandomPools(int count,int size, List<IShapeF> obstacles = null)
        {
            var list = new List<IObstacle>();
            for (int i = 0; i < count; i++)
            {
                var shapes = PitObstacle.CreateRandomPool(size,obstacles);
                list = list.Concat(shapes).ToList();
            }
            return list;
        }


        public static void RemoveObstacles(List<IObstacle> list)
        {
            foreach (var e in list)
            {
                Globals.CollisionComponent.Remove(e);
                Globals.Obstacles.Remove(e);

                Globals.PitsDrawable.Clear();
                Globals.RectanglesDrawable.Clear();
            }
        }

        public static Vector2 GetRandomPositionNotIntersecting(IShapeF shape, IEnumerable<IShapeF> obstacles,Func<Vector2> positoner = null)
        {
            Vector2 randomPosition = Vector2.Zero;
            bool isIntersects = true;
            while (isIntersects)
            {
                if (positoner == null)
                    randomPosition = RandomPosition();
                else
                    randomPosition = positoner();

                shape.Position = randomPosition;
                isIntersects = IsIntersects(shape, obstacles);
            }
            return randomPosition;
        }

        public static bool IsIntersects(IShapeF shape, IEnumerable<IShapeF> obstacles)
        {
            var isIntersects = false;
            foreach (var obstacle in obstacles)
                if (shape.Intersects(obstacle))
                    isIntersects = true;
            return isIntersects;
        }

        public static Vector2 RandomPosition () => new Vector2(Globals.Random.Next(0, Globals.ScreenWidth), Globals.Random.Next(0, Globals.ScreenHeight));
        public static Vector2 RandomOffset(int offset) => new Vector2(Globals.Random.Next(-offset, offset), Globals.Random.Next(-offset, offset));

    }
}
