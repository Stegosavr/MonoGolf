using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace Snakedy
{
    public class PitObstacle : IDrawable, IObstacle
    {
        public IShapeF Bounds { get; }
        public static Texture2D Texture;
        public static SoundEffect DrownSound;

        public PitObstacle(IShapeF circleF)
        {
            Bounds = circleF;
        }

        public static List<IObstacle> CreateRandomPool(int count, IEnumerable<IShapeF> obstacles)
        {
            var main = CreateRandomPit(obstacles);
            var list = new List<IObstacle>() { main };

            var positioner = () => (Vector2)main.Bounds.Position + Obstacles.RandomOffset(60);
            for (int i = 1; i < count; i++)
            {
                list.Add(CreateRandomPit(obstacles, positioner));
            }
            foreach (var shape in list)
            {
                Globals.CollisionComponent.Insert(shape);
                Globals.Obstacles.Add(shape);
            }
            return list;
        }


        public static PitObstacle CreateRandomPit(IEnumerable<IShapeF> obstacles, Func<Vector2> positioner = null)
        {
            var obstacle = new PitObstacle(new CircleF(Vector2.Zero, Globals.Random.Next(20, 50)));
            obstacle.Bounds.Position = Obstacles.GetRandomPositionNotIntersecting(obstacle.Bounds, obstacles, positioner);
            Globals.PitsDrawable.Add(obstacle);
            return obstacle;
        }

        public void DrawCollision(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle((CircleF)Bounds, 50, Color.Blue, 50);
        }

        public void Draw(SpriteBatch spriteBatch) => Drawable.Draw(spriteBatch, Bounds.Position, Texture, ((CircleF)Bounds).Diameter+35);

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (collisionInfo.Other is Character)
            {
                //Globals.CollisionComponent.Remove(collisionInfo.Other);
                DrownSound.Play();
                VisualEffects.EmitWaterDrops(new Vector2(10, 10), collisionInfo.Other.Bounds.Position);
                //Globals.Game.GameOver();
            }
        }
    }
}
