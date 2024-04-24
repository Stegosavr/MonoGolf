using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Snakedy
{
    public class UI
    {
        public SpriteFont Font;
        public UI()
        {
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch,string str,bool isTime = true)
        {
            string text = "";
            if (isTime)
            {
                var parts = str.Split(",");
                if (parts.Length > 1)
                    text = String.Join("", parts[0] + ',' + parts[1][0]);
            }
            else
                text = str;
            Vector2 strDims = Font.MeasureString(text);
            spriteBatch.DrawString(Font, text, new Vector2(100,30),Color.White);
        }
    }
}
