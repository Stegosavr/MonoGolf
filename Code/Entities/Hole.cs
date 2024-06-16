using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace Snakedy
{
    public class Hole : IEntity
    {
        public Vector2 Position
        {
            get { return Bounds.Position; }
            set { Bounds.Position = value; AreaBounds.Position = value; }
        }
        public IShapeF Bounds { get; }
        public IShapeF AreaBounds { get; }

        public static Texture2D Texture;
        public static SoundEffect WinSound;

        List<IObstacle> Spawned;

        public Hole(Vector2 position = new Vector2())
        {
            Bounds = new CircleF(position, 37);
            AreaBounds = new CircleF(position, 57);
            SpawnHole();
            if (Position != Vector2.Zero)
            {
                Bounds.Position = position;
                Position = position;
            }
            Spawned = new List<IObstacle>();
            Globals.Entities.Add(this);
        }

        public void SpawnHole()
        {
            Position = Obstacles.GetRandomPositionNotIntersecting(Bounds, Globals.Obstacles.Select(o=>o.Bounds));
        }

        public bool Check(Vector2 ballPosition)
        {
            var distance = (Position - ballPosition).Length();
            if (distance < 30)
                return true;
            return false;
        }

        public void DespawnObstacles()
        {
            Obstacles.RemoveObstacles(Spawned);
            Spawned.Clear();
        }

        public void Update(GameTime gameTime)
        {
            if (Check(Globals.Ball.Position))
            {
                Globals.Ball.Velocity = 0.001f;

                Obstacles.RemoveObstacles(Spawned);
                Spawned = Obstacles.CreateRandomObstacles(3,(obs)=>RectangleObstacle.CreateRandomRectangle(obs),new List<IShapeF>() { AreaBounds});
                Spawned = Spawned.Concat(Obstacles.CreateRandomPools(2,7, new List<IShapeF>() { AreaBounds })).ToList();

                WinEffect();

                SpawnHole();
                Globals.Timer.AddTime();
                Console.WriteLine("Score: " + ++Globals.Score);
            }
        }

        private void WinEffect()
        {
            var a = WinSound.CreateInstance();
            a.Volume -= 0.3f;
            a.Pitch = (float)Globals.Random.NextDouble() * 0.4f - 0.2f;
            a.Play();
            VisualEffects.EmitParticles(Vector2.Zero, Position, 15, 20, 10, 15, Color.White, 10);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawCircle((CircleF)Bounds, 16, Color.Black, 37f);
            spriteBatch.Draw(Texture, Functions.OffsetTexture( Texture.Bounds,Position), Color.White);
            if (Globals.Random.NextDouble() > 0.9)
            VisualEffects.EmitParticles(Vector2.Zero, Position, 1, 1, 1, 2,Color.Yellow,260);
        }

    }
}
