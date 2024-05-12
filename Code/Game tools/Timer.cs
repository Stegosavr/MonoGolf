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
        private double _delayTime;
        private double _timeLeft;
        public event Action OnTimeout;
        public string TimeLeftSeconds { get { return (_timeLeft/1000).ToString(); } }

        public Timer(double delayTimeMilliseconds)
        {
            _delayTime = delayTimeMilliseconds;
            _timeLeft = delayTimeMilliseconds;
        }

        public void Reset()
        {
            _timeLeft = _delayTime;
        }

        public void Update(GameTime gameTime)
        {
            if (Globals.GameOver)
            {
                return;
            }
            if (_timeLeft <= 0)
            {
                //_timeLeft = _delayTime;
                OnTimeout.Invoke();
            }
            _timeLeft -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public void AddTime()
        {
            _timeLeft += _delayTime;
        }
    }
}
