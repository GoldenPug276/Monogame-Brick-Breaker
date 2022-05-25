using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonogameRedirect
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        Texture2D ball;
        Texture2D paddle;
        SpriteFont font;
        Vector2 ballVector = new Vector2(200, 50);
        Vector2 paddle1Vector = new Vector2(20, 150);
        Vector2 paddle2Vector = new Vector2(350, 150);

        Rectangle ballHitbox;
        Rectangle paddle1HitBox;
        Rectangle paddle2HitBox;

        float xVelocity = -2;
        float yVelocity = -2;
        float paddle1Velocity = 5;
        float paddle2Height = 150;
        int PScore = 0;
        int EScore = 0;

        MouseState mouseState;
        bool clicked = false;

        //Sprite test;

        Random rand = new Random();

        public void EnemyAI(float ballLocatiom, float paddleLocation, float height)
        {
            if (ballLocatiom>paddleLocation)
            {
                if (paddle2Vector.Y + paddle.Height <= height)
                {
                    paddle2Height += 2;
                }
            }
            else if (ballLocatiom<paddleLocation)
            {
                if (paddle2Height >= 0)
                {
                    paddle2Height -= 2;
                }
            }
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            ball = Content.Load<Texture2D>("ball");
            paddle = Content.Load<Texture2D>("paddle");
            font = Content.Load<SpriteFont>("font");

            //test = new Sprite(new Vector2(100, 100), Content.Load<Texture2D>("smiley"), 1, Color.Red);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            int width = GraphicsDevice.Viewport.Width;
            int height = GraphicsDevice.Viewport.Height;

            mouseState = Mouse.GetState();
            if (mouseState.LeftButton==ButtonState.Pressed)
            {
                clicked = true;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.W)||keyboardState.IsKeyDown(Keys.Up))
            {
                if (paddle1Vector.Y>=0)
                {
                    paddle1Vector = new Vector2(20, paddle1Vector.Y - paddle1Velocity);
                }
            }
            if (keyboardState.IsKeyDown(Keys.S)||keyboardState.IsKeyDown(Keys.Down))
            {
                if (paddle1Vector.Y+paddle.Height<=height)
                {
                    paddle1Vector = new Vector2(20, paddle1Vector.Y + paddle1Velocity);
                }
            }

            ballVector = new Vector2(ballVector.X + xVelocity, ballVector.Y + yVelocity);
            paddle2Vector = new Vector2(width - 50, paddle2Height);
            ballHitbox = new Rectangle((int)ballVector.X, (int)ballVector.Y, ball.Width, ball.Height);
            paddle1HitBox = new Rectangle((int)paddle1Vector.X+(paddle.Width-3), (int)paddle1Vector.Y, 3, paddle.Height);
            paddle2HitBox = new Rectangle((int)paddle2Vector.X, (int)paddle2Vector.Y, 3, paddle.Height);
            if (ballVector.X<=0)
            {
                EScore++;
                xVelocity = rand.Next(2, 6);
                yVelocity = rand.Next(2, 6);
                ballVector = new Vector2(width/2, height/2);
            }
            else if (ballVector.Y<=0)
            {
                yVelocity*=-1;
            }
            if (ballVector.X+ball.Width>=width)
            {
                PScore++;
                xVelocity = rand.Next(-6, -1);
                yVelocity = rand.Next(-6, -1);
                ballVector = new Vector2(width/2, height/2);
            }
            else if (ballVector.Y+ball.Height>=height)
            {
                yVelocity*=-1;
            }
            if (ballHitbox.Intersects(paddle1HitBox))
            {
                xVelocity = rand.Next(2, 6);
                yVelocity = rand.Next(-4, 6);
            }
            else if (ballHitbox.Intersects(paddle2HitBox))
            {
                xVelocity = rand.Next(-6, -2);
                yVelocity = rand.Next(-4, 6);
            }

            EnemyAI(ballVector.Y+(ball.Height/2), paddle2Vector.Y+(paddle.Height/2), height);

            if (PScore==7||EScore==7)
            {
                ballVector = new Vector2(width / 2, height / 2);
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            int width = GraphicsDevice.Viewport.Width;
            int height = GraphicsDevice.Viewport.Height;

            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            if (clicked==false)
            {
                _spriteBatch.DrawString(font, "PONG", new Vector2((width / 2) - 60, 50), Color.White);
                _spriteBatch.DrawString(font, "CLICK TO PLAY", new Vector2((width / 2) - 125, 200), Color.White);
            }

            if (clicked==true)
            {
                //test.Draw(_spriteBatch);
                if (PScore!=7 && EScore!=7)
                {
                    _spriteBatch.Draw(ball, ballVector, null, Color.White, 0, new Vector2(0, 0), 1 / 1f, SpriteEffects.None, 0);
                }
                _spriteBatch.Draw(paddle, paddle1Vector, null, Color.White, 0, new Vector2(0, 0), 1 / 1f, SpriteEffects.None, 0);
                _spriteBatch.Draw(paddle, paddle2Vector, null, Color.White, 0, new Vector2(0, 0), 1 / 1f, SpriteEffects.None, 0);

                _spriteBatch.DrawString(font, $"{PScore}", new Vector2(75, 50), Color.White);
                _spriteBatch.DrawString(font, $"{EScore}", new Vector2(width-110, 50), Color.White);
                if (PScore==7)
                {
                    _spriteBatch.DrawString(font, "YOU WIN!!!", new Vector2((width/2)-75, 50), Color.White);
                }
                else if (EScore==7)
                {
                    _spriteBatch.DrawString(font, "YOU LOSE", new Vector2((width/2)-57, 50), Color.White);
                }
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
