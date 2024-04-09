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

    public class Border : ICollisionActor
    {
        private readonly Game1 _game;
        public IShapeF Bounds { get; }

        public Border(Game1 game, RectangleF rectangleF)
        {
            _game = game;
            Bounds = rectangleF;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle((RectangleF)Bounds, Color.Red, 3);
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            Console.WriteLine("Collided");
        }

    }
}
