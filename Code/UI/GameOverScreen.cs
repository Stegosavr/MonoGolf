using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snakedy
{
    public static class GameOverScreen
    {
        public static void Update(GameTime gameTime)
        {
            if (Globals.Control.HandleMouseClick())
                Globals.Game.ResetGame();
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            DrawScore();
            DrawScoreList();
        }

        private static void DrawScoreList()
        {
            var scoreList = Globals.Scores.Select(s=>s.PlayerName+" :"+s.Value+"\n");
            Globals.UI.Draw(String.Join("",scoreList), new Vector2(Globals.ScreenWidth * 0.8f, Globals.ScreenHeight/2), Color.White, 1);
        }

        public static void DrawScore()
        {
            Globals.UI.Draw("GAME OVER", new Vector2(Globals.ScreenWidth / 2, 200), Color.Red, 2);
            Globals.UI.Draw("Score - " + Globals.Score, new Vector2(Globals.ScreenWidth / 2, 250), Color.White, 2);
        }
    }
}
