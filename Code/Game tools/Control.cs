using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Snakedy
{
    public class Control
    {
        //Vector2 objectPos;
        //float objectSpeed;
        ButtonState PrevMouseState = ButtonState.Released;
        bool PrevHoldState = false;
        bool HoldingBefore = false;

        public void GetActualState()
        {
            HoldingBefore = HandleMouseHold();
        }

        public Vector2 MoveOnArrowsInput(GameTime gameTime)
        {
            var kState = Keyboard.GetState();
            var direction = Vector2.Zero;

            if (kState.IsKeyDown(Keys.Up))
            {
                direction.Y -= 1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            if (kState.IsKeyDown(Keys.Down))
            {
                direction.Y += 1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            if (kState.IsKeyDown(Keys.Left))
            {
                direction.X -= 1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            if (kState.IsKeyDown(Keys.Right))
            {
                direction.X += 1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            return direction;
        }

        public bool HandleMouseHold()
        {
            var mState = Mouse.GetState();
            var holding = false;

            //if (Mouse.)

            if (mState.LeftButton == ButtonState.Pressed)
            {
                holding = true;
            }
            if (mState.LeftButton == ButtonState.Released && PrevMouseState == ButtonState.Pressed)
            {
                holding = false;
            }

            PrevMouseState = mState.LeftButton;

            return holding;
        }

        public bool HandleMouseClick()
        {
            var mState = Mouse.GetState();
            var clicked = false;
            var holding = HandleMouseHold();


            //if (PrevHoldState == true && holding == false && !HoldingBefore)
            //{
            //    clicked = true;
            //}
            //else
            //{
            //    clicked = false;
            //}

            if (holding && !HoldingBefore)
            {
                clicked = true;
            }
            else
            {
                clicked = false;
            }

            if (HoldingBefore && !holding)
                HoldingBefore = false;

            return clicked;
        }

        public Vector2 StayInBounds(Vector2 objectPos, Vector2 bounds,Character ball = null)
        {
            //if (ball != null)
            //    ball.CalculateCollision;
            if (objectPos.X < 0)
            {
                objectPos.X = 0;
            }
            if (objectPos.Y < 0)
            {
                objectPos.Y = 0;
            }
            if (objectPos.X > bounds.X)
            {
                objectPos.X = bounds.X;
            }
            if (objectPos.Y > bounds.Y)
            {
                objectPos.Y = bounds.Y;
            }

            return objectPos;
        }
    }
}
