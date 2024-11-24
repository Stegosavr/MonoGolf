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
    public class RectangleObstacle : IDrawable ,IObstacle
    {
        public IShapeF Bounds { get; }
        public static Texture2D Texture;

        public RectangleObstacle(IShapeF rectangleF)
        {
            Bounds = rectangleF;
        }

        public static RectangleObstacle CreateRandomRectangle(IEnumerable<IShapeF> obstacles)
        {
            var obstacle = new RectangleObstacle(new RectangleF(Vector2.Zero, new Vector2(100, 100)));
            obstacle.Bounds.Position = Obstacles.GetRandomPositionNotIntersecting(obstacle.Bounds, obstacles);
            Globals.RectanglesDrawable.Add(obstacle);
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

        public void Draw(SpriteBatch spriteBatch) => Drawable.Draw(spriteBatch, Functions.GetTextureCenter((RectangleF)Bounds)+Bounds.Position, Texture, ((RectangleF)Bounds).Width,Color.White);

    }
}
