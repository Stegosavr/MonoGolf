using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Snakedy
{
    public interface IEntity:IDrawable
    {
        public void Update(GameTime gameTime);
    }
}
