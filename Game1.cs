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
        RayCast RayCast;
        Control BallControl;

        Obstacle Block;
        Obstacle Block2;

        Hole Hole;

        int Score;

        public Timer Timer;
        public UI UI;

        Texture2D Wallpaper;
        Texture2D Wallpaper2;

        SoundEffect soundEffect;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly CollisionComponent _collisionComponent;

        public Obstacle[] Borders { get; private set; }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            Globals.ScreenWidth = _graphics.PreferredBackBufferWidth;
            Globals.ScreenHeight = _graphics.PreferredBackBufferHeight;

            //_collisionComponent = new CollisionComponent(new RectangleF(0, 0, screenWidth, screenHeight));
            _collisionComponent = new CollisionComponent(new RectangleF(0-100, 0-100, Globals.ScreenWidth + 100, Globals.ScreenHeight + 100));
            Globals.CollisionComponent = _collisionComponent;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Globals.Obstacles = new List<IObstacle>();

            Ball = new Character(new Vector2(Globals.ScreenWidth / 2, Globals.ScreenHeight / 2));
            RayCast = new RayCast(Ball, 10);

            Hole = new Hole();

            BallControl = new Control();



            //Block = new Obstacle(new RectangleF(new Point2(40, 40), new Size2(200, 100)));
            //_collisionComponent.Insert(Block);
            //Block2 = new Obstacle(new RectangleF(new Point2(40, 40), new Size2(100, 200)));
            //_collisionComponent.Insert(Block2);

            _collisionComponent.Insert(Ball);
            SetBoundsCollisions(_collisionComponent);
            RayCast.SetCollisions(_collisionComponent);

            Globals.Ball = Ball;
            Globals.Obstacles.Add(Ball);
            Globals.Game = this;

            UI = new UI();

            Timer = new Timer(3000);
            Timer.TickAt = 3000;
            Hole.SpawnHole();


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

            UI.Font = Content.Load<SpriteFont>("Fonts/Arial16");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (!this.IsActive)
                return;

            // TODO: Add your update logic here
            Globals.GameTime = gameTime;

            Timer.Wait(gameTime,GameOver);

            //ball.Position += ballControl.MoveOnArrowsInput(gameTime) * ball.Speed;
            Ball.PreparingHit(BallControl.HandleMousePress());

            var RayInfo = RayCast.GetRayCastInfo();
            if (RayInfo != null)
            {
                Ball.Position = RayInfo.Item1;
                Ball.CalculateCollision(RayInfo.Item2,RayCast.PrevAngle);
                RayCast.Position = Point2.Zero;
            }
            RayCast.PrevAngle = Ball.Angle;

            Ball.Move(gameTime);
            Ball.Position = BallControl.StayInBounds(Ball.Position, new Vector2(Globals.ScreenWidth, Globals.ScreenHeight));

            RayCast.Update(Ball.Position);


            Hole.Update(Ball.Position, Timer);

            _collisionComponent.Update(gameTime);

            UI.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (!this.IsActive) return;
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.Draw(Wallpaper, new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight), Color.White);
            _spriteBatch.Draw(Ball.Texture, Functions.OffsetTexture(Ball.Texture, Ball.Position), Color.White);

            Ball.Draw(_spriteBatch);
            RayCast.Draw(_spriteBatch);
            Hole.Draw(_spriteBatch);



            //Block.Draw(_spriteBatch);
            //Block2.Draw(_spriteBatch);



            foreach (var b in Globals.Obstacles)
                b.Draw(_spriteBatch);

            UI.Draw(_spriteBatch,Timer.TimeLeft.ToString());

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void GameOver()
        {
            Console.WriteLine("GAME OVER --- SCORE:"+Globals.Score);
            Globals.Score = 0;
            Timer.TickAt = Globals.GameTime.TotalGameTime.TotalMilliseconds + 3000;

            Hole.SpawnHole();

            //using var game = new Snakedy.Game1();
            //game.Run();
        }

        void SetBoundsCollisions(CollisionComponent comp)
        {
            int size = 100;
            int shift = 5;
            Borders = new Obstacle[4];
            Borders[0] = (new Obstacle(new RectangleF(0, -size + shift, Globals.ScreenWidth, size)));
            Borders[1] = (new Obstacle(new RectangleF(0, Globals.ScreenHeight - shift, Globals.ScreenWidth, size)));
            Borders[2] = (new Obstacle(new RectangleF(-size + shift, 0, size, Globals.ScreenHeight)));
            Borders[3] = (new Obstacle(new RectangleF(Globals.ScreenWidth - shift, 0, size, Globals.ScreenHeight)));
            foreach (var b in Borders)
            {
                comp.Insert(b);
                Globals.Obstacles.Add(b);
            }
        }
    }
}