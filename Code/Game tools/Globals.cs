using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Collisions;
using MonoGolf;

namespace Snakedy
{
    public static class Globals
    {
        public static MonoGolfGame Game;
        public static Character Ball;
        public static RayCast RayCast;
        public static Timer Timer;
        public static Control Control;
        public static UI UI;

        public static int ScreenWidth;
        public static int ScreenHeight;

        public static readonly Random Random = new Random(Guid.NewGuid().GetHashCode());

        public static GameTime GameTime;
        public static CollisionComponent CollisionComponent;
        public static SpriteBatch SpriteBatch;
        public static GraphicsDeviceManager GraphicsDeviceManager;

        public static string PlayerName = "Player";
        public static bool GameOver = false;
        public static int Score;
        public static List<Score> Scores;


        public static List<IObstacle> Obstacles = new List<IObstacle>();
        public static List<IEntity> Entities = new List<IEntity>();
        public static List<IDrawable> RectanglesDrawable = new List<IDrawable>();
        public static List<IDrawable> PitsDrawable = new List<IDrawable>();
        public static List<IEntity> VisualEffects = new List<IEntity>();
    }
}
