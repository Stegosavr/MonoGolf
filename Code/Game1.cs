using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace Snakedy
{
    public class Game1 : Game
    {
        Character Ball;
        RayCast RayCast;
        Control Control;

        RectangleObstacle Block;
        RectangleObstacle Block2;

        Hole Hole;

        int Score;

        public Timer Timer;
        public UI UI;

        Texture2D Wallpaper;
        Texture2D Wallpaper2;

        SoundEffect soundEffect;
        Song backgroundMusic;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private CollisionComponent _collisionComponent;
        private ScoreManager _scoreManager;

        public RectangleObstacle[] Borders { get; private set; }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Globals.GraphicsDeviceManager = _graphics;


            //_graphics.IsFullScreen = true;
            //_graphics.PreferredBackBufferHeight = 1080;
            //_graphics.PreferredBackBufferWidth = 1920;

            Globals.ScreenWidth = _graphics.PreferredBackBufferWidth;
            Globals.ScreenHeight = _graphics.PreferredBackBufferHeight;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _collisionComponent = new CollisionComponent(new RectangleF(0 - 100, 0 - 100, Globals.ScreenWidth + 100, Globals.ScreenHeight + 100));
            Globals.CollisionComponent = _collisionComponent;


            Ball = new Character(new Vector2(Globals.ScreenWidth / 2, Globals.ScreenHeight / 2));
            RayCast = new RayCast(Ball, 25);

            Hole = new Hole();

            Control = new Control();


            SetBoundsCollisions(_collisionComponent);
            _collisionComponent.Insert(Ball);
            RayCast.SetCollisions(_collisionComponent);

            Globals.Ball = Ball;
            Globals.Control = Control;
            Globals.RayCast = RayCast;
            Globals.Obstacles.Add(Ball);
            Globals.Game = this;

            UI = new UI();

            Timer = new Timer(3000);
            Timer.OnTimeout += () => Ball.OnDeath();
            Hole.SpawnHole();

            Globals.Timer = Timer;
            Globals.UI = UI;


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.SpriteBatch = _spriteBatch;
            _scoreManager = ScoreManager.Load();
            //Globals.ScoreManager = _scoreManager;

            // TODO: use this.Content to load your game content here
            Ball.Texture = Content.Load<Texture2D>("Sprites/ball");
            Hole.Texture = Content.Load<Texture2D>("Sprites/flag");
            Wallpaper = Content.Load<Texture2D>("Sprites/green");
            Wallpaper2 = Content.Load<Texture2D>("Sprites/xp");
            VisualEffects.ArrowTexture = Content.Load<Texture2D>("Sprites/arrow");

            PitObstacle.Texture = Content.Load<Texture2D>("Sprites/pound");
            RectangleObstacle.Texture = Content.Load<Texture2D>("Sprites/crate");
            Particle.Texture = Content.Load<Texture2D>("Sprites/particle");


            soundEffect = Content.Load<SoundEffect>("Sounds/hit");
            PitObstacle.DrownSound = Content.Load<SoundEffect>("Sounds/water");
            backgroundMusic = Content.Load<Song>("Sounds/Arc_De_Soleil_-_Swirly_Bird");
            Ball.HitSound = soundEffect;

            SoundEffects.soundCollision = Content.Load<SoundEffect>("Sounds/collision");
            MediaPlayer.Volume = 0.025f;

            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.MediaStateChanged += MediaPlayerMediaStateChanged;



            UI.Font = Content.Load<SpriteFont>("Fonts/Arial16");
            //UI.BigFont = Content.Load<SpriteFont>("Fonts/Arial16");

        }

        void MediaPlayerMediaStateChanged(object sender, System.EventArgs e)
        {
            MediaPlayer.Volume = 0.025f;
            MediaPlayer.Play(backgroundMusic);
        }



        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (!this.IsActive)
                return;
            if (Globals.GameOver)
            {
                GameOverScreen.Update(gameTime);
            }

            
            var mState = Mouse.GetState();
            if (mState.RightButton == ButtonState.Pressed)
                Ball.OnDeath();

            Globals.GameTime = gameTime;

            // TODO: Add your update logic here
            Timer.Update(gameTime);

            //ball.Position += ballControl.MoveOnArrowsInput(gameTime) * ball.Speed;

            Update(Globals.Entities,gameTime);

            _collisionComponent.Update(gameTime);

            UI.Update();

            Update(Globals.VisualEffects,gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (!this.IsActive) return;
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.Draw(Wallpaper, new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight), Color.White);

            //DrawCollisions(Globals.Obstacles,_spriteBatch);

            DrawTextures(Globals.PitsDrawable, _spriteBatch);
            DrawTextures(Globals.RectanglesDrawable, _spriteBatch);
            DrawTextures(Globals.Entities,_spriteBatch);
            DrawTextures(Globals.VisualEffects,_spriteBatch);

            if (Globals.GameOver)
            {
                GameOverScreen.Draw(_spriteBatch);
            }

            if (!Globals.GameOver)
                UI.DrawTimer(Timer.TimeLeftSeconds);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void GameOver()
        {
            Console.WriteLine("GAME OVER --- SCORE:" + Globals.Score);
            Globals.GameOver = true;
            Globals.Control.GetActualState();

            _scoreManager.Add(new Score() { PlayerName = "You",Value = Globals.Score});
            ScoreManager.Save(_scoreManager);
            //_scoreManager.EditLatestScore("Editted");
            //ScoreManager.Save(_scoreManager);
        }
        public void ResetGame() 
        {
            Globals.GameOver = false;
            Globals.Score = 0;
            Timer.Reset();
            Hole.DespawnObstacles();
            Globals.VisualEffects.Clear();
            Hole.SpawnHole();

            _collisionComponent = new CollisionComponent(new RectangleF(0 - 100, 0 - 100, Globals.ScreenWidth + 100, Globals.ScreenHeight + 100));
            Globals.CollisionComponent = _collisionComponent;

            _collisionComponent.Insert(Ball);
            foreach (var e in Globals.Obstacles.Take(4))
                _collisionComponent.Insert(e);

            RayCast = new RayCast(Ball, 25);
            Globals.RayCast = RayCast;
            RayCast.SetCollisions(_collisionComponent);
            //RayCast.SetCollisions(_collisionComponent);

            Ball.Reset();

            //Globals.Obstacles = Globals.Obstacles;
            //Globals.CollisionComponent.GetType();
            //RayCast.DeleteCollisions();
            //RayCast = new RayCast(Ball, 20);
        }

        void SetBoundsCollisions(CollisionComponent comp)
        {
            int size = 100;
            int shift = 5;
            Borders = new RectangleObstacle[4];
            Borders[0] = (new RectangleObstacle(new RectangleF(0, -size + shift, Globals.ScreenWidth, size)) );
            Borders[1] = (new RectangleObstacle(new RectangleF(0, Globals.ScreenHeight - shift, Globals.ScreenWidth, size)) );
            Borders[2] = (new RectangleObstacle(new RectangleF(-size + shift, 0, size, Globals.ScreenHeight)) );
            Borders[3] = (new RectangleObstacle(new RectangleF(Globals.ScreenWidth - shift, 0, size, Globals.ScreenHeight)) );
            foreach (var b in Borders)
            {
                comp.Insert(b);
                Globals.Obstacles.Add(b);
            }
        }
        private void DrawCollisions(List<IObstacle> list,SpriteBatch spriteBatch)
        {
            for (int i = 0; i < list.Count; i++)
                list[i].DrawCollision(spriteBatch);
        }

        private void DrawTextures(List<IDrawable> list, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < list.Count; i++)
                list[i].Draw(spriteBatch);
        }

        private void DrawTextures(List<IEntity> list, SpriteBatch spriteBatch) => DrawTextures(list.Select(e => (IDrawable)e).ToList(), spriteBatch);
        
        private void Update(List<IEntity> list, GameTime gameTime)
        {
            for (int i = 0; i < list.Count; i++)
                list[i].Update(gameTime);
        }
    }
}