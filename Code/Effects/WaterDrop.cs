using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Snakedy
{
    public class WaterDrop : IDrawable
    {
        private Vector2 _position;
        private float _velocity;
        private Vector2 _direction;
        float _size;
        private double _lifeTime;

        public WaterDrop(Vector2 position, float velocity, Vector2 direction, float size)
        {
            _position = position;
            _velocity = velocity;
            _direction = direction;
            _size = size;
            _lifeTime = Globals.Random.NextDouble();
            Globals.VisualEffects.Add(this);
        }

        public void Update(GameTime gameTime)
        {
            _position += _direction * _velocity;
            _velocity *= 0.98f;
            _lifeTime -= gameTime.ElapsedGameTime.TotalSeconds;
            if (_lifeTime > 0.5)
                _size += 1;
            else
                _size *= 0.96f;

            if (_size < 0.01f)
                Globals.VisualEffects.Remove(this);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle(new CircleF(_position, _size), 12, Color.Blue,3);
        }
    }
}
