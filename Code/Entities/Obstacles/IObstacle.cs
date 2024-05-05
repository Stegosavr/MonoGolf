using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Collisions;

namespace Snakedy
{
    public interface IObstacle : ICollisionActor
    {
        public void DrawCollision(SpriteBatch spriteBatch);
    }
}
