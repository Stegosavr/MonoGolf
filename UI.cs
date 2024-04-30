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
        public SpriteFont BigFont;

        static Vector2 TimerPosition = new Vector2(Globals.ScreenWidth / 2, 50);
        public UI()
        {
        }

        public void Update()
        {

        }

        public void DrawTimer(string str,bool isTime = true)
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
            Draw(text,TimerPosition, Color.White);
        }

        public void Draw(string text,Vector2 position,Color color,float size = 1)
        {
            Vector2 strDims = Font.MeasureString(text);
            //strDims.
            position.X -= (strDims.X/2) * size;
            Globals.SpriteBatch.DrawString(Font, text, position, color,0,Vector2.Zero,size,SpriteEffects.None,0);
        }

        public async void DrawGameOver()
        {
            //W
        }
    }
}
