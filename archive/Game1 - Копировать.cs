using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snakedy
{
    public class Game2 : Game
    {
        Texture2D ballTexture;
        Vector2 ballPos;
        float ballSpeed;
        Control ballControl;

        Texture2D Wallpaper;
        Texture2D Wallpaper2;

        int screenWidth;
        int screenHeight;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game2()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            screenWidth = _graphics.PreferredBackBufferWidth;
            screenHeight = _graphics.PreferredBackBufferHeight;

            ballPos = new Vector2(screenWidth / 2, screenWidth / 2);
            ballSpeed = 0.5f;
            ballControl = new Control();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ballTexture = Content.Load<Texture2D>("Sprites/ball");
            Wallpaper = Content.Load<Texture2D>("Sprites/green");
            Wallpaper2 = Content.Load<Texture2D>("Sprites/xp");


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            ballPos += ballControl.MoveOnInput(gameTime) * ballSpeed;
            ballPos = ballControl.StayInBounds(ballPos, new Vector2(screenWidth, screenHeight));
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.Draw(Wallpaper2, new Rectangle(0,0,screenWidth,screenHeight), Color.White);

            _spriteBatch.Draw(ballTexture, OffsetTexture(ballTexture, ballPos), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected Vector2 OffsetTexture(Texture2D texture, Vector2 position) =>
            new Vector2(position.X-texture.Width/2,position.Y-texture.Height/2);
        protected Vector2 GetTextureCenter(Texture2D texture, Vector2 position) =>
            new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2);
    }
}