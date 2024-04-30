using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;

namespace Snakedy
{
    public class SoundEffects
    {
        public static SoundEffect soundCollision;

        public static void CollisionSound(double velocity)
        {
            soundCollision.Play();
        }
    }
}
