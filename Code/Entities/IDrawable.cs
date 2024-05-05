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
    public interface IDrawable
    {
        public void Draw(SpriteBatch spriteBatch);
    }
    public static class Drawable
    {
        public static void Draw(SpriteBatch spriteBatch, Vector2 position,Texture2D texture,float size)
        {
            float deltaSize = texture.Width / size;
            var offset = Functions.OffsetTexture(new RectangleF(position.X, position.Y, texture.Width / deltaSize, texture.Height / deltaSize), position);
            var destination = new Rectangle((int)offset.X, (int)offset.Y, (int)(texture.Width / deltaSize), (int)(texture.Height / deltaSize));
            spriteBatch.Draw(texture, destination, Color.White);
        }
    }
}
