using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace Snakedy
{
    public class Game1 : Game
    {
        Character Ball;
        Control BallControl;
        Border Block;

        Hole Hole;

        int Score;

        public readonly Random Random = new Random(Guid.NewGuid().GetHashCode());

        Texture2D Wallpaper;
        Texture2D Wallpaper2;

        SoundEffect soundEffect;

        int screenWidth;
        int screenHeight;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly CollisionComponent _collisionComponent;

        public Border[] Borders { get; private set; }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            screenWidth = _graphics.PreferredBackBufferWidth;
            screenHeight = _graphics.PreferredBackBufferHeight;

            //_collisionComponent = new CollisionComponent(new RectangleF(0, 0, screenWidth, screenHeight));
            _collisionComponent = new CollisionComponent(new RectangleF(0-100, 0-100, screenWidth+100, screenHeight+100));


            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Ball = new Character(new Vector2(screenWidth / 2, screenHeight / 2));

            BallControl = new Control();

            Block = new Border(this, new RectangleF(new Point2(40, 40), new Size2(200, 100)));

            _collisionComponent.Insert(Block);
            _collisionComponent.Insert(Ball);
            SetBoundsCollisions(_collisionComponent);

            SpawnHole();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Effects.Setup(_spriteBatch);

            // TODO: use this.Content to load your game content here
            Ball.Texture = Content.Load<Texture2D>("Sprites/ball");
            Wallpaper = Content.Load<Texture2D>("Sprites/green");
            Wallpaper2 = Content.Load<Texture2D>("Sprites/xp");
            Effects.ArrowTexture = Content.Load<Texture2D>("Sprites/arrow");

            soundEffect = Content.Load<SoundEffect>("Sounds/hit");
            Ball.HitSound = soundEffect;


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (!this.IsActive) return;

            // TODO: Add your update logic here

            //ball.Position += ballControl.MoveOnArrowsInput(gameTime) * ball.Speed;
            Ball.PreparingHit(BallControl.HandleMousePress());
            Ball.Move(gameTime);

            Ball.Position = BallControl.StayInBounds(Ball.Position, new Vector2(screenWidth, screenHeight));



            if (Hole.Check(Ball.Position))
            {
                SpawnHole();
                Console.WriteLine("Score: " + ++Score);
            }

            _collisionComponent.Update(gameTime);

            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            if (!this.IsActive) return;
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.Draw(Wallpaper, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            _spriteBatch.Draw(Ball.Texture, Functions.OffsetTexture(Ball.Texture, Ball.Position), Color.White);

            Ball.Draw(_spriteBatch);
            Hole.Draw(_spriteBatch);
            Block.Draw(_spriteBatch);

            foreach (var b in Borders)
                b.Draw(_spriteBatch);


            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void SetBoundsCollisions(CollisionComponent comp)
        {
            int size = 100;
            int shift = 5;
            Borders = new Border[4];
            Borders[0] = (new Border(this, new RectangleF(0, -size+ shift, screenWidth, size)));
            Borders[1] = (new Border(this, new RectangleF(0, screenHeight- shift, screenWidth, size)));
            Borders[2] = (new Border(this, new RectangleF(-size+ shift, 0, size, screenHeight)));
            Borders[3] = (new Border(this, new RectangleF(screenWidth- shift, 0, size, screenHeight)));
            foreach (var b in Borders)
                comp.Insert(b);
        }

        public void SpawnHole()
        {
            var hol = new Hole(new Vector2(Random.Next(0, screenWidth), Random.Next(0, screenHeight)));
            Hole = hol;
        }
    }
}