using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Snakedy
{
    internal class Hole
    {
        public Vector2 Position;

        public Hole(Vector2 position)
        {
            Position = position;
        }

        public bool Check(Vector2 ballPosition)
        {
            var distance = (Position - ballPosition).Length();
            if (distance < 30)
                return true;
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle(new CircleF(Position,37), 16, Color.Black, 37f);
        }

    }
}
