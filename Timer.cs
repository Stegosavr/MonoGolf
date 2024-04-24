using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Snakedy
{
    public class Timer
    {
        public double TickAt = 0.0;
        public double DelayTime = 0.0;

        public double TimeLeft 
        { 
            get { return TickAt/1000 - Globals.GameTime.TotalGameTime.TotalSeconds; }
        }

        public Timer(double DelayTime)
        {
            this.DelayTime = DelayTime;
        }

        public void Wait(GameTime gameTime, Action Action)
        {
            if (TickAt <= gameTime.TotalGameTime.TotalMilliseconds)
            {
                TickAt = gameTime.TotalGameTime.TotalMilliseconds + DelayTime;
                Action.Invoke();
            }
        }

        public void AddTime(double milliseconds)
        {
            TickAt += milliseconds;
        }
    }
}
