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
    public interface IObstacle : ICollisionActor
    {
        public void Draw(SpriteBatch spriteBatch);
    }

    public class Obstacle : IObstacle
    {
        public IShapeF Bounds { get; }

        public Obstacle(IShapeF rectangleF)
        {
            Bounds = rectangleF;
        }

        public static List<Obstacle> CreateRandomRectangles(int count,List<IShapeF> obstacles = null)
        {
            var list = new List<Obstacle>();
            for (int i = 0; i < count; i++)
            {
                var rect = CreateRandomRectangle(obstacles);
                list.Add(rect);
                Globals.CollisionComponent.Insert(rect);
                Globals.Obstacles.Add(rect);
            }
            return list;
        }

        public static void RemoveObstacles(List<Obstacle> list)
        {
            foreach (var e in list)
            {
                Globals.CollisionComponent.Remove(e);
                Globals.Obstacles.Remove(e);
            }
        }

        public static Obstacle CreateRandomRectangle(List<IShapeF> obstacles)
        {
            var obstacle = new Obstacle(new RectangleF(Vector2.Zero,new Size2(100,100)));
            obstacle.Bounds.Position = GetRandomPositionNotIntersecting(obstacle.Bounds,obstacles);
            return obstacle;
        }

        public static Vector2 GetRandomPositionNotIntersecting(IShapeF shape, IEnumerable<IShapeF> obstacles)
        {
            Vector2 randomPosition = Vector2.Zero;
            bool isIntersects = true;
            while (isIntersects)
            {
                randomPosition = new Vector2(Globals.Random.Next(0, Globals.ScreenWidth), Globals.Random.Next(0, Globals.ScreenHeight));
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

    public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle((RectangleF)Bounds, Color.Red, 5);
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            //Console.WriteLine("Collided");
        }

    }
}
