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
    public class Particle : IDrawable,IEntity
    {
        private Vector2 _position;
        private float _velocity;
        private Vector2 _direction;
        float _size;
        private double _lifeTime;
        private Color _color;
        private bool _solid;
        public static Texture2D Texture;

        public Particle(Vector2 position, float velocity, Vector2 direction, float size,double lifeTime, Color color,bool solid = false)
        {
            _position = position;
            _velocity = velocity;
            _direction = direction;
            _size = size;
            _lifeTime = Globals.Random.NextDouble()*lifeTime;
            _color = color;
            _solid = solid;
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

            if (_solid)
                Drawable.Draw(spriteBatch, _position, Texture, _size*2,_color);
            else
                spriteBatch.DrawCircle(new CircleF(_position, _size/2), 12, _color,3);
        }
    }
}
