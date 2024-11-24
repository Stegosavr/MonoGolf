using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace Snakedy
{
    public class Collider : ICollisionActor
    {
        public IShapeF Bounds { get; }
        public Vector2 Position
        {
            get { return Bounds.Position; }
            set { Bounds.Position = value; }
        }
        //Vector2 PrevPos;
        public Vector2 PenetrationVector;
        public bool Collided = false;
        ICollisionActor CollidedWith = null;

        public Collider(Vector2 position, IShapeF bounds)
        {
            Bounds = bounds;
            Position = position;
        }

        public void Update()
        {
            if (PenetrationVector != Vector2.Zero)
                PenetrationVector = Vector2.Zero;
            Collided = false;
        }

        public void ResetCollision()
        {
            CollidedWith = null;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle((CircleF)Bounds, 16, Color.Red, 3f);
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (collisionInfo.Other is Character || collisionInfo.Other is Collider)
                return;
            if (collisionInfo.Other == CollidedWith) return;
            PenetrationVector = collisionInfo.PenetrationVector;
            Collided = true;
           //Console.WriteLine("Ray collided");
            CollidedWith = collisionInfo.Other;
        }
    }




    public class RayCast //: ICollisionActor
    {
        public Character Ball;
        public double PrevAngle;
        public IShapeF Bounds { get; }
        public Vector2 Position
        {
            get { return Bounds.Position; }
            set { Bounds.Position = value; }
        }
        int UpdateCount;
        bool Collided;
        //CollisionData[] Collisions;
        Collider[] Colliders;

        public RayCast(Character ball,int updateCount = 2)
        {
            Ball = ball;
            var circle = (CircleF)ball.Bounds;
            Bounds = new CircleF(ball.Position, circle.Radius);
            UpdateCount = updateCount;
            Colliders = new Collider[updateCount];

            for (int i=0; i < updateCount; i++)
            {
                Colliders[i] = new Collider(Position, new CircleF(Bounds.Position, ((CircleF)Bounds).Radius));
            }
        }

        public Tuple<Vector2,Vector2> GetRayCastInfo()
        {
            if (Collided)
                Collided = false;
            foreach (var col in Colliders)
                if (col.Collided)
                {
                    Collided = true;
                    return Tuple.Create((Vector2)col.Position, col.PenetrationVector);
                }
            return null;
        }

        public void Update(Vector2 currentPos)
        {
            if (Position == Vector2.Zero) 
            {
                foreach (var col in Colliders)
                    col.ResetCollision();
                Position = currentPos; 
            }
            Vector2 step = ((Vector2)currentPos - Position) /(UpdateCount+1);
            for (int i = 1; i < UpdateCount+1; i++)
            {
                Colliders[i-1].Position = Position + (Vector2)step * i;
                Colliders[i - 1].Update();
            }
            Position = currentPos;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < UpdateCount; i++)
                Colliders[i].Draw(spriteBatch);
        }

        public void SetCollisions(CollisionComponent comp)
        {
            foreach (var col in Colliders)
                comp.Insert(col);
        }

        public void DeleteCollisions()
        {
            foreach (var col in Colliders)
                Globals.CollisionComponent.Remove(col);
        }

        //public void OnCollision(CollisionEventArgs collisionInfo)
        //{
        //    if (Bounds.Intersects)
        //        throw new NotImplementedException();
        //}
    }
}
