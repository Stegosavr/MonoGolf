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
    public static class Effects
    {
        static SpriteBatch _spriteBatch;

        public static void Setup(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }
        public static void DrawArrow(Vector2 from,Vector2 to)
        {
            var dist = (from - to).Length();
            Color color = new Color(dist/150, 150/dist, 0);
            _spriteBatch.DrawLine(from,to,color,10);
        }
    }
}
