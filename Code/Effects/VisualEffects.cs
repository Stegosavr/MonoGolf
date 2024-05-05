using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;

namespace Snakedy
{
    public static class VisualEffects
    {
        public static Texture2D ArrowTexture;

        public static void DrawArrow(Vector2 force, Vector2 ballPosition)
        {
            var from = ballPosition - force;
            var dist = force.Length();
            Color color = new Color(dist / 120, 100 / dist, 0);
            //_spriteBatch.DrawLine(from, ballPosition, color, 10);

            int count = (int)dist / 53 + 1;
            var center = Functions.GetTextureCenter(ArrowTexture.Bounds);
            var angle = Math.Atan2(force.Y, force.X) + Math.PI / 2;
            for (int i = 1; i <= count; i++)
            {
                var destination = new Rectangle((int)(ballPosition.X - force.X * i / count), (int)(ballPosition.Y - force.Y * i / count), ArrowTexture.Width / 10 * count + 15, ArrowTexture.Height * count / 6 + 5);
                Globals.SpriteBatch.Draw(ArrowTexture, destination, null, color, (float)angle, center, SpriteEffects.None, 0);
            }
        }

        public static void EmitWaterDrops(Vector2 force, Vector2 position)
        {
            int dropCount = (int)force.Length();/// 50;
            for (int i = 0; i < dropCount; i++)
            {
                var direction = Obstacles.RandomOffset(6)/20f;
                var size = Globals.Random.NextInt64(5, 15);
                var drop = new WaterDrop(position, 50, direction, size);
            }

        }

    }
}
