﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;


namespace Snakedy
{
    public class Character : IObstacle,IEntity
    {
        public Texture2D Texture;
        public SoundEffect HitSound;

        public Vector2 Position;
        public float HitForce;

        public double angle = 0;
        public double Angle 
        { 
            get { return angle;} 
            set { angle = value; }
            //set { angle = value / Math.Abs(value) * (Math.Abs(value) % Math.PI); }
        }

        float DeltaSize = 32;

        public float Velocity = 0;
        public Vector2 Force = Vector2.Zero;
        Vector2 HitDirection;
        float MinSpeed = 0.01f;

        bool Moving = false;
        bool Collided = false;
        public bool Holding = false;

        public IShapeF Bounds { get; }
        public ICollisionActor CollidedWith { get; private set; }
        public double CollidedTo { get; private set; }

        public Character(Vector2 position, float hitForce = 0.008f,float collisionSize = 20f)//20
        {
            Position = position;
            HitForce = hitForce;
            Bounds = new CircleF(position, collisionSize);
            DeltaSize = DeltaSize / collisionSize;
            Globals.Entities.Add(this);
        }

        public void Update(GameTime gameTime)
        {
            PreparingHit(Globals.Control.HandleMouseHold());

            var RayInfo = Globals.RayCast.GetRayCastInfo();
            if (RayInfo != null)
            {
                Position = RayInfo.Item1;
                CalculateCollision(RayInfo.Item2, Globals.RayCast.PrevAngle);
                Globals.RayCast.Position = Vector2.Zero;
            }
            Globals.RayCast.PrevAngle = Angle;

            Move(gameTime);
            Position = Globals.Control.StayInBounds(Position, new Vector2(Globals.ScreenWidth, Globals.ScreenHeight));

            Globals.RayCast.Update(Position);
        }

        private void Move(GameTime gameTime)
        {
            Vector2 dir = new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle));
            Force = dir * Velocity;
            Position += Force * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Velocity > MinSpeed)
                Velocity -= MinSpeed;
            else if (Velocity > 0)
            {
                Velocity = 0;
                Moving = false;
            }

            Bounds.Position = Position;

            //var kState = Keyboard.GetState();
            //if (kState.IsKeyDown(Keys.Space))
            //    Collided = false;
        }

        public void PreparingHit(bool pressed) 
        {
            if (Moving) return;
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
                HitDirection = Functions.LimitHitVector(mousePos,Position,300);
                //HitDirection = Position - mousePos;
            }
            else if (HitDirection != Vector2.Zero)
            {
                Angle = Math.Atan2(HitDirection.Y, HitDirection.X);
                Velocity = HitDirection.Length() * HitForce;
                HitDirection = Vector2.Zero;
                CollidedWith = null;

                Moving = true;
                HitEffect();
            }
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (collisionInfo.Other is Collider) return;
            if (CollidedWith != collisionInfo.Other)
            {
                CalculateCollision(collisionInfo.PenetrationVector,Angle);
                CollidedWith = collisionInfo.Other;

                Velocity *= 0.95f;
                SoundEffects.CollisionSound(Velocity);
            }
            else
            {
                //Vector2 dir = new Vector2((float)Math.Cos(CollidedTo), (float)Math.Sin(CollidedTo));
                //Position += dir*10;
                //Console.WriteLine("Same collision");
            }
        }

        public void CalculateCollision(Vector2 penetrVec,double ballAngle)
        {
            var surface = new Vector2(penetrVec.Y, -penetrVec.X);
            //Console.WriteLine("ANGLE:"+Angle.ToString() + " ");
            //Console.WriteLine("SURFACE:"+surface.ToString() + " ");
            var surfAngle = (Math.Atan2(surface.Y, surface.X));// + Math.PI) % Math.PI;
            Angle = surfAngle + surfAngle - ballAngle;
            CollidedTo = Angle;
        }

        public void OnDeath()
        {
            Globals.Entities.Remove(this);
            Globals.CollisionComponent.Remove(this);
            Globals.RayCast.DeleteCollisions();
            Globals.Game.GameOver();
        }

        public void Reset()
        {
            Position = new Vector2(Globals.ScreenWidth/2,Globals.ScreenHeight/2);
            Velocity = 0f;
            Moving = false;
            Globals.Entities.Add(this);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var offset = Functions.OffsetTexture(new RectangleF(Position.X,Position.Y,Texture.Width/DeltaSize,Texture.Height/DeltaSize), Position);
            var destination = new Rectangle((int)offset.X , (int)offset.Y, (int)(Texture.Width / DeltaSize), (int)(Texture.Height / DeltaSize));
            spriteBatch.Draw(Texture, destination, Color.White);

            if (Holding)
                VisualEffects.DrawArrow(HitDirection, Position);
        }

        private void HitEffect()
        {
            var a = HitSound.CreateInstance();
            a.Pitch =(float)Globals.Random.NextDouble()*0.4f-0.2f;
            a.Play();
        }

        public void DrawCollision(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawCircle((CircleF)Bounds, 16, Color.Black, 3f);
        }
    }
}
