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
        public void DrawCollision(SpriteBatch spriteBatch);
    }

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

        public static void RemoveObstacles(List<IObstacle> list)
        {
            foreach (var e in list)
            {
                Globals.CollisionComponent.Remove(e);
                Globals.Obstacles.Remove(e);
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

    public class RectangleObstacle : IObstacle
    {
        public IShapeF Bounds { get; }

        public RectangleObstacle(IShapeF rectangleF)
        {
            Bounds = rectangleF;
        }

        public static RectangleObstacle CreateRandomRectangle(IEnumerable<IShapeF> obstacles)
        {
            var obstacle = new RectangleObstacle(new RectangleF(Vector2.Zero, new Size2(100, 100)));
            obstacle.Bounds.Position = Obstacles.GetRandomPositionNotIntersecting(obstacle.Bounds, obstacles);
            return obstacle;
        }

        public void DrawCollision(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle((RectangleF)Bounds, Color.Red, 5);
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            //throw new NotImplementedException();
        }
    }

    public class PitObstacle : IObstacle
    {
        public IShapeF Bounds { get; }

        public PitObstacle(IShapeF circleF)
        {
            Bounds = circleF;
        }

        public static List<IObstacle> CreateRandomPool(int count,IEnumerable<IShapeF> obstacles)
        {
            var main = CreateRandomPit(obstacles);
            var list = new List<IObstacle>() { main};

            var positioner = () => (Vector2)main.Bounds.Position+ Obstacles.RandomOffset(60);
            for (int i = 1; i < count; i++)
            {
                list.Add(CreateRandomPit(obstacles,positioner));
            }
            foreach (var shape in list)
            {
                Globals.CollisionComponent.Insert(shape);
                Globals.Obstacles.Add(shape);
            }
            return list;
        }


        public static PitObstacle CreateRandomPit(IEnumerable<IShapeF> obstacles,Func<Vector2> positioner = null)
        {
            var obstacle = new PitObstacle(new CircleF(Vector2.Zero, Globals.Random.Next(20,50)));
            obstacle.Bounds.Position = Obstacles.GetRandomPositionNotIntersecting(obstacle.Bounds, obstacles,positioner);
            return obstacle;
        }

        public void DrawCollision(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle((CircleF)Bounds, 50,Color.Blue, 50);
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (collisionInfo.Other is Character)
                Globals.Game.GameOver();
        }
    }
}
