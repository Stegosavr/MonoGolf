using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;

namespace Snakedy
{
    public static class Effects
    {
        static SpriteBatch _spriteBatch;
        public static Texture2D ArrowTexture;

        public static void Setup(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }
        public static void DrawArrow(Vector2 force, Vector2 ballPosition)
        {
            var from = ballPosition - force;
            var dist = force.Length();
            Color color = new Color(dist / 120, 100 / dist, 0);
            //_spriteBatch.DrawLine(from, ballPosition, color, 10);

            int count = (int)dist / 53 + 1;
            var center = Functions.GetTextureCenter(ArrowTexture);
            var angle = Math.Atan2(force.Y, force.X) + Math.PI / 2;
            for (int i = 1; i <= count; i++)
            {
                var destination = new Rectangle((int)(ballPosition.X-force.X*i/ count), (int)(ballPosition.Y - force.Y * i / count), ArrowTexture.Width / 10 * count + 15, ArrowTexture.Height*count/6 + 5);
                _spriteBatch.Draw(ArrowTexture, destination, null, color, (float)angle, center, SpriteEffects.None, 0);
            }
        }
    }
}
