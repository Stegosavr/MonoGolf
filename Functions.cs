using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snakedy
{
    public static class Functions
    {
        public static Vector2 LimitHitVector(Vector2 from, Vector2 to,int limit)
        {
            var vector = to - from;
            var len = vector.Length();
            if (len > limit)
            {
                var newFrom = vector / len * (len - limit)+from;
                vector = to - newFrom;
            }
            return vector;
        }

        public static Vector2 GetTextureCenter(Texture2D texture) =>
            new Vector2(texture.Width / 2, texture.Height / 2);

        public static Vector2 OffsetTexture(Texture2D texture, Vector2 position) =>
            position - GetTextureCenter(texture);
        //public static Vector2 GetTextureCenter(Texture2D texture, Vector2 position) =>
        //new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2);

    }
}
