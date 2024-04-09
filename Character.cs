using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;


namespace Snakedy
{
    internal class Character : ICollisionActor
    {
        public Texture2D Texture;
        public Vector2 Position;
        public float HitForce;

        public double Direction = 0;
        public float Velocity = 0;
        public Vector2 Force = Vector2.Zero;
        Vector2 HitDirection;
        float MinSpeed = 0.01f;

        public bool Holding = false;

        public IShapeF Bounds { get; }

        public Character(Vector2 position, float hitForce = 0.01f,float collisionSize = 20f)
        {
            Position = position;
            HitForce = hitForce;
            Bounds = new CircleF(position, collisionSize);
        }

        public void Move(GameTime gameTime)
        {
            Vector2 dir = new Vector2((float)Math.Cos(Direction), (float)Math.Sin(Direction));
            dir = dir * Velocity;
            Position += dir * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Velocity > MinSpeed)
                Velocity -= MinSpeed;
            else if (Velocity > 0)
                Velocity = 0;

            Bounds.Position = Position;
        }

        public void PreparingHit(bool pressed) 
        {
            if (pressed)
                HitCalculating(true);
            if (Holding = true && pressed == false)
                HitCalculating(false);
            Holding = pressed;
        }

        public void HitCalculating(bool prepare)
        {
            if (prepare)
            {
                var mousePos = Mouse.GetState().Position.ToVector2();
                HitDirection = Position - mousePos;
                //Console.WriteLine(HitDirection);
            }
            else if (HitDirection != Vector2.Zero)
            {
                Direction = Math.Atan2(HitDirection.Y, HitDirection.X);
                Velocity = HitDirection.Length() * HitForce;
                HitDirection = Vector2.Zero;
            }
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            Velocity = 0;
            Console.WriteLine("Collided with "+collisionInfo.ToString());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle((CircleF)Bounds, 16, Color.Red, 3f);
        }
    }
}
