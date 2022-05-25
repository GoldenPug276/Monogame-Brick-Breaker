using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonogameRedirect
{
    public class Game2 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        ParticleEngine trailParticles;

        Texture2D background;
        Texture2D brick;
        Sprite BlackRectangle;
        Sprite WhiteRectangle;
        Sprite paddle;
        Sprite ball;
        Sprite fateBall;
        Color fateColor = new Color(Color.White, 80);
        PowerUp bomb;
        PowerUp fateBomb;
        PowerUp saw;
        PowerUp fateSaw;
        PowerUp speedUp;
        PowerUp slowDown;
        Texture2D HUD;
        Texture2D PauseMenu;
        Texture2D Menu;
        Texture2D LoseScreen;
        Texture2D WinScreen;
        SpriteFont pixelFont;

        Rectangle paddleHitBox1;
        Rectangle paddleHitBox2;
        Rectangle paddleHitBox3;
        Rectangle leftEdge;
        Rectangle rightEdge;

        Button MenuButton;
        Button MenuBackButton;
        Button RetryButton;
        Button ContinueButton;
        Button QuitButton;
        Button MasterSlider;
        Button MusicSlider;
        Button SoundSlider;
        Button LoseRetry;
        Button LoseRestart;
        Button LoseQuit;
        Button WinNext;
        Button WinQuit;

        double xVelocity = -2;
        double yVelocity = -2;
        float paddleVelocity = 3.1f;
        double powerUpVelocity = 2;

        double savedXVelocity = -2;
        double savedYVelocity = -2;

        bool hasCollided = false;
        double speedMultiplier = 1;
        double savedSpeedMultiplier = 1;

        bool halfCleared = false;

        double sawLength = 2000;
        double sawLengthLeft;
        double fateSawLengthLeft;

        Brick[] bricks;
        Brick[] firstBricks;
        Brick[] fateBricks;
        int rowLength = 0;
        int bricksPerRow = 8;

        Random rand = new Random();

        bool isPaused = false;
        bool isInMenu = false;
        bool isBackPressed = false;
        float masterVolume = 0;
        bool isMasterSliderSelected = false;
        float musicVolume = 0;
        bool isMusicSliderSelected = false;
        float soundVolume = 0;
        bool isSoundSliderSelected = false;

        bool isInfLivesOn = false;
        int levelsCleared = 0;
        bool isMIHModeOn = false;
        double timeBeforeMIHSpeed = 1000;
        double MIHSpeedIncrease = 0.01;
        double fadeToWhiteTime = 15;
        bool universeHasReset = false;
        
        Sprite fatePosition2;
        Sprite fatePosition4;
        Sprite fatePositionNow;
        bool fateCalcHappening = false;
        Vector2[] fateVelocities = new Vector2[1];
        Vector2[] fateVelZero = new Vector2[1];
        double fateInterval = 2000;

        int lives = 3;
        int score = 0;
        int blockCount = 120;
        bool win = false;
        bool lose = false;
        bool retry = false;
        bool restart = false;
        /*
        TO-DO:

        *Sort of Done*
        Add Particle System: http://rbwhitaker.wikidot.com/2d-particle-engine-1
        Hold on to This: https://gamedev.stackexchange.com/questions/91942/c-xna-monogame-ghost-trail-effect
        Add Ribbon Trail
        *Sort of Done*

        Implement speed up mechanic *DONE*
        Fix brick bounces *Maybe Done*
        Fix bug that makes you break the speed barrier *DONE*
        Fix bug that makes you stuck above the screen *Maybe Done*
        Convert most important items from images to sprites *DONE*
        Bug Fixing

        Add background *DONE*
            Hold on to this: https://artemouse.itch.io/breakout-pixel-assets
            Maybe add immortal blocks that can only be broken with saw later *DONE*
        Add title (scrapped), win, and lose screen *DONE*
            *Title* *Scrapped*
            *Win* *DONE*
            *Lose* *DONE*
            *Put score below lose screen* *DONE*
        Add pause screen and menu *DONE*
        Add lives system *DONE*
        Make shuffling and "Next Level" mechanic *DONE*
        Rewrite Bomb Code *DONE* (Might take up more space now, but I can at least tell what I'm telling it to do.)
        Add actual powerup assets *DONE I think*
        Add powerups *Priority*
        Add particles *Scrapped*
        Add sounds and music (although not important) *Scrapped*

        *May want to condense powerup related code in the future* *Ah, whatever*

        Final Thing I want to add: After clearing 5 levels, basically MIH, and you have inf lives to get as much score as you can within a minute.
            Make the bounce speeds not change so  that it isn't 2*2 and really fast.

        Powerup Ideas:
        Speed Up *DONE*
        Slow Down *DONE*
        Saw *DONE*
        */
        public Game2()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        Brick[] Shuffle(int blockCount, bool powerUpsOn, bool areImmortals)
        {
            Brick[] bricks = new Brick[blockCount];
            int value = 3;
            int x = brick.Width * 8 + 25;
            int y = value;
            string power = "none";
            int row = 1;
            int color = 0;
            int score = 10;
            int durability = 1;
            int immortals = 0;
            Texture2D UsedBrick = Content.Load<Texture2D>("BrickBlue1");
            for (int i = 0; i < bricks.Length; i++)
            {
                score = 10;
                durability = 1;
                if (rand.Next(1, 1000) % 3 == 0)
                {
                    power = "speedUp";
                }
                if (rand.Next(1, 1000) % 4 == 0)
                {
                    power = "slowDown";
                }
                if (rand.Next(1, 1000) % 5 == 0)
                {
                    power = "bomb";
                }
                if (rand.Next(1, 1000) % 7 == 0)
                {
                    power = "saw";
                }
                if (rowLength == bricksPerRow)
                {
                    rightEdge = new Rectangle(x - 3, 0, 500, 750);
                    y += brick.Height + value;
                    x = brick.Width * 8 + 25;
                    row++;
                    rowLength = 0;
                }

                if (immortals<=8 && areImmortals==true)
                {
                    color = rand.Next(1, 12);
                }
                else
                {
                    color = rand.Next(1, 11);
                }
                switch (color)
                {
                    case 2:
                        UsedBrick = Content.Load<Texture2D>("BrickGreen2");
                        break;
                    case 3:
                        UsedBrick = Content.Load<Texture2D>("BrickPurple3");
                        break;
                    case 4:
                        UsedBrick = Content.Load<Texture2D>("BrickRed4");
                        break;
                    case 5:
                        UsedBrick = Content.Load<Texture2D>("BrickOrange5");
                        break;
                    case 6:
                        UsedBrick = Content.Load<Texture2D>("BrickBlack6");
                        break;
                    case 7:
                        UsedBrick = Content.Load<Texture2D>("BrickWhite7");
                        break;
                    case 8:
                        UsedBrick = Content.Load<Texture2D>("BrickPink8");
                        break;
                    case 9:
                        UsedBrick = Content.Load<Texture2D>("BrickSilver9-1");
                        durability = 2;
                        score = 30;
                        break;
                    case 10:
                        UsedBrick = Content.Load<Texture2D>("BrickGold10-1");
                        durability = 3;
                        score = 50;
                        break;
                    case 11:
                        UsedBrick = Content.Load<Texture2D>("Brick");
                        durability = int.MaxValue;
                        score = 100;
                        immortals += 1;
                        break;
                }
                if (powerUpsOn==false)
                {
                    power = "none";
                }
                bricks[i] = new Brick(new Vector2(x, y), UsedBrick, 1, Color.White, power, row, score, durability, true, color);
                color = 0;
                UsedBrick = Content.Load<Texture2D>("BrickBlue1");
                power = "none";
                rowLength++;

                x += brick.Width + value;
            }
            rowLength = 0;
            return bricks;
        }

        Brick[] FateBricksConverter(Brick[] bricks)
        {
            Brick[] fateBricks = new Brick[bricks.Length];

            for (int i = 0; i < fateBricks.Length; i++)
            {
                fateBricks[i] = bricks[i].Clone();
                if (bricks[i].Image!=Content.Load<Texture2D>("BrickSilver9-1") && bricks[i].Image!=Content.Load<Texture2D>("BrickGold10-1"))
                {
                    fateBricks[i].Color = new Color(Color.White, 50);
                    fateBricks[i].Image = Content.Load<Texture2D>("BrickBlack6");
                }
            }

            return fateBricks;
        }

        Brick[] BrickCopier(Brick[] original)
        {
            Brick[] copy = new Brick[original.Length];
            for (int i = 0; i < copy.Length; i++)
            {
                copy[i] = original[i].Clone();
            }

            return copy;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            int width = GraphicsDevice.Viewport.Width;
            int height = GraphicsDevice.Viewport.Height;

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            background = Content.Load<Texture2D>("background");
            BlackRectangle = new Sprite(new Vector2(0, 0), Content.Load<Texture2D>("BlackRectange"), 1 / 1f, new Color(Color.White, 75));
            WhiteRectangle = new Sprite(new Vector2(0, 0), Content.Load<Texture2D>("WhiteRectange"), 1 / 1f, new Color(0, 0, 0, 50));

            brick = Content.Load<Texture2D>("Brick");
            paddle = new Sprite(new Vector2(0,0), Content.Load<Texture2D>("BrickPaddle"), 1/1f, Color.White);
            ball = new Sprite(new Vector2(0,0), Content.Load<Texture2D>("ball"), 1 / 1f, Color.White);
            fateBall = new Sprite(new Vector2(paddle.Position.X + paddle.Image.Width / 2 - ball.Image.Width / 2, paddle.Position.Y + 20), Content.Load<Texture2D>("fateBall"), 1 / 1f, fateColor);

            fatePosition2 = new Sprite(new Vector2(0, 0), Content.Load<Texture2D>("fateBall"), 1 / 1f, fateColor);
            fatePosition4 = new Sprite(new Vector2(0, 0), Content.Load<Texture2D>("fateBall"), 1 / 1f, fateColor);
            fatePositionNow = new Sprite(new Vector2(0, 0), Content.Load<Texture2D>("fateBall"), 1 / 1f, fateColor);

            bomb = new PowerUp(new Vector2(0, 0), Content.Load<Texture2D>("bomb"), 1 / 1f, Color.White, false, false, Color.Red);
            saw = new PowerUp(new Vector2(0, 0), Content.Load<Texture2D>("Buzzsaw"), 1 / 1f, Color.White, false, false, Color.White);
            speedUp = new PowerUp(new Vector2(0, 0), Content.Load<Texture2D>("SpeedUp"), 1 / 1f, Color.White, false, false, Color.White);
            slowDown = new PowerUp(new Vector2(0, 0), Content.Load<Texture2D>("SlowDown"), 1 / 1f, Color.White, false, false, Color.White);

            fateBomb = new PowerUp(new Vector2(0, 0), Content.Load<Texture2D>("fateBomb"), 1 / 1f, Color.White, false, false, new Color(Color.Red, 135));
            fateSaw = new PowerUp(new Vector2(0, 0), Content.Load<Texture2D>("FateBuzzsaw"), 1 / 1f, Color.White, false, false, fateColor);

            HUD = Content.Load<Texture2D>("HUD");
            PauseMenu = Content.Load<Texture2D>("PauseMenu");
            Menu = Content.Load<Texture2D>("Menu");
            LoseScreen = Content.Load<Texture2D>("Lose Screen");
            WinScreen = Content.Load<Texture2D>("Win Screen");
            pixelFont = Content.Load<SpriteFont>("PixelFont");

            paddle.Position = new Vector2(width / 2 - paddle.Image.Width / 2, height - 75);
            ball.Position = new Vector2(paddle.Position.X + paddle.Image.Width / 2 - ball.Image.Width / 2, paddle.Position.Y - 50);
            fateBall.Position = new Vector2(paddle.Position.X + paddle.Image.Width / 2 - ball.Image.Width / 2, paddle.Position.Y - 50);

            MenuButton = new Button(new Vector2(330, 285), Content.Load<Texture2D>("ButtonRectange"), 1 / 1f, Color.Transparent, false);
            RetryButton = new Button(new Vector2(330, 255), Content.Load<Texture2D>("ButtonRectange"), 1 / 1f, Color.Transparent, false);
            MenuBackButton = new Button(new Vector2(330, 327), Content.Load<Texture2D>("ButtonRectange"), 1 / 1f, Color.Transparent, false);
            ContinueButton = new Button(new Vector2(330, 215), Content.Load<Texture2D>("ButtonRectange"), 1 / 1f, Color.Transparent, false);
            QuitButton = new Button(new Vector2(330, 320), Content.Load<Texture2D>("ButtonRectange"), 1 / 1f, Color.Transparent, false);
            MasterSlider = new Button(new Vector2(403, 232), Content.Load<Texture2D>("Volume Slider"), 1 / 1f, Color.White, false);
            MusicSlider = new Button(new Vector2(403, 267), Content.Load<Texture2D>("Volume Slider"), 1 / 1f, Color.White, false);
            SoundSlider = new Button(new Vector2(403, 302), Content.Load<Texture2D>("Volume Slider"), 1 / 1f, Color.White, false);
            //min volume x=403
            //max volume x=486
            //each pixel=about 1.2 audio points
            LoseRetry = new Button(new Vector2(335, 265), Content.Load<Texture2D>("ButtonRectange"), 1 / 1f, Color.White, false);
            LoseRestart = new Button(new Vector2(330, 315), Content.Load<Texture2D>("ButtonRectange"), 1 / 1f, Color.White, false);
            LoseQuit = new Button(new Vector2(335, 368), Content.Load<Texture2D>("ButtonRectange"), 1 / 1f, Color.White, false);
            WinNext = new Button(new Vector2(330, 240), Content.Load<Texture2D>("ButtonRectange"), 1 / 1f, Color.White, false);
            WinQuit = new Button(new Vector2(330, 305), Content.Load<Texture2D>("ButtonRectange"), 1 / 1f, Color.White, false);

            bricks = Shuffle(blockCount, true, true);
            fateBricks = FateBricksConverter(bricks);
            firstBricks = BrickCopier(bricks);

            List<Texture2D> texture2Ds = new List<Texture2D>();
            //texture2Ds.Add(Content.Load<Texture2D>("diamond"));
            texture2Ds.Add(Content.Load<Texture2D>("TrailBall"));
            //texture2Ds.Add(Content.Load<Texture2D>("star"));
            trailParticles = new ParticleEngine(rand, 5000, texture2Ds, new Vector2(0,0), 0, 0, 1, Color.White, 600, 01f);

            leftEdge = new Rectangle(0, 0, brick.Width * 8 + 21, 750);
            

            // TODO: use this.Content to load your game content here
        }
        KeyboardState lastKeyboardState;
        MouseState lastMouseState;
        protected override void Update(GameTime gameTime)
        {
            int width = GraphicsDevice.Viewport.Width;
            int height = GraphicsDevice.Viewport.Height;
            hasCollided=false;

            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            if (fateCalcHappening==false)
            {
                if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left) && isPaused==false)
                {
                    if (paddle.Hitbox.Intersects(leftEdge))
                    {
                    }
                    else
                    {
                        paddle.Position = new Vector2(paddle.Position.X-paddleVelocity, height-75);
                    }
                }
                if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right) && isPaused==false)
                {
                    if (paddle.Hitbox.Intersects(rightEdge))
                    {
                    }
                    else
                    {
                        paddle.Position = new Vector2(paddle.Position.X+paddleVelocity, height-75);
                    }
                }
            }
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                speedMultiplier = 2;
            }
            else if (keyboardState.IsKeyDown(Keys.RightShift))
            {
                speedMultiplier = 0.5f;
            }
            else if (keyboardState.IsKeyDown(Keys.OemOpenBrackets))
            {
                lose = true;
            }
            else if (keyboardState.IsKeyDown(Keys.OemCloseBrackets))
            {
                win = true;
            }
            else if (keyboardState.CapsLock==true||isMIHModeOn==true)
            {
                isInfLivesOn = true;
            }
            else if (keyboardState.CapsLock==false && isMIHModeOn==false)
            {
                isInfLivesOn = false;
            }
            else
            {
                speedMultiplier = savedSpeedMultiplier;
            }
            if (isPaused==false)
            {
                isPaused = false;
                isInMenu = false;
                MenuButton.IsActive = false;
                MenuBackButton.IsActive = false;
                RetryButton.IsActive = false;
                ContinueButton.IsActive = false;
                QuitButton.IsActive = false;
                MasterSlider.IsActive = false;
                isMasterSliderSelected = false;
                MusicSlider.IsActive = false;
                isMusicSliderSelected = false;
                SoundSlider.IsActive = false;
                isSoundSliderSelected = false;
                xVelocity = savedXVelocity;
                yVelocity = savedYVelocity;
            }

            if (MasterSlider.Hitbox.Contains(mouseState.Position) && MasterSlider.IsActive == true && mouseState.LeftButton == ButtonState.Pressed)
            {
                isMasterSliderSelected = true;
            }
            else if (isMasterSliderSelected == true && mouseState.LeftButton == ButtonState.Released)
            {
                isMasterSliderSelected = false;
            }
            else if (MusicSlider.Hitbox.Contains(mouseState.Position) && MusicSlider.IsActive == true && mouseState.LeftButton == ButtonState.Pressed)
            {
                isMusicSliderSelected = true;
            }
            else if (isMusicSliderSelected == true && mouseState.LeftButton == ButtonState.Released)
            {
                isMusicSliderSelected = false;
            }
            else if (SoundSlider.Hitbox.Contains(mouseState.Position) && SoundSlider.IsActive == true && mouseState.LeftButton == ButtonState.Pressed)
            {
                isSoundSliderSelected = true;
            }
            else if (isSoundSliderSelected == true && mouseState.LeftButton == ButtonState.Released)
            {
                isSoundSliderSelected = false;
            }
            else if (isBackPressed==true && mouseState.LeftButton==ButtonState.Released)
            {
                isBackPressed = false;
            }

            if (keyboardState.IsKeyUp(Keys.Escape) && lastKeyboardState.IsKeyDown(Keys.Escape))
            {
                if (isPaused==true)
                {
                    isPaused = false;
                    xVelocity = savedXVelocity;
                    yVelocity = savedYVelocity;
                }
                else if (isPaused==false)
                {
                    isPaused = true;
                    MenuButton.IsActive = true;
                    RetryButton.IsActive = true;
                    ContinueButton.IsActive = true;
                    QuitButton.IsActive = true;
                    xVelocity = 0;
                    yVelocity = 0;
                }
            }
            if (MenuButton.Hitbox.Contains(mouseState.Position) && MenuButton.IsActive==true)
            {
                if (mouseState.LeftButton==ButtonState.Pressed && lastMouseState.LeftButton==ButtonState.Released)
                {
                    MenuButton.IsActive = false;
                    RetryButton.IsActive = false;
                    ContinueButton.IsActive = false;
                    QuitButton.IsActive = false;
                    MenuBackButton.IsActive = true;
                    MasterSlider.IsActive = true;
                    MusicSlider.IsActive = true;
                    SoundSlider.IsActive = true;
                    isInMenu = true;
                }
            }
            else if (RetryButton.Hitbox.Contains(mouseState.Position) && RetryButton.IsActive == true)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                {
                    retry = true;
                    isPaused = false;
                }
            }
            else if (ContinueButton.Hitbox.Contains(mouseState.Position) && ContinueButton.IsActive == true)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                {
                    isPaused = false;
                    xVelocity = savedXVelocity;
                    yVelocity = savedYVelocity;
                }
            }
            else if (MenuBackButton.Hitbox.Contains(mouseState.Position) && MenuBackButton.IsActive==true)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                {
                    MenuButton.IsActive = true;
                    RetryButton.IsActive = true;
                    ContinueButton.IsActive = true;
                    QuitButton.IsActive = true;
                    MenuBackButton.IsActive = false;
                    MasterSlider.IsActive = false;
                    MusicSlider.IsActive = false;
                    SoundSlider.IsActive = false;
                    isInMenu = false;
                    isBackPressed = true;
                }
            }
            else if (QuitButton.Hitbox.Contains(mouseState.Position) && QuitButton.IsActive == true)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                {
                    if (isBackPressed==false)
                    {
                        Exit();
                    }
                }
            }
            else if (isMasterSliderSelected==true)
            {
                if (mouseState.X-MasterSlider.Image.Width/2>=403 && mouseState.X+MasterSlider.Image.Width/2<=486)
                {
                    MasterSlider.Position = new Vector2(mouseState.X - MasterSlider.Image.Width / 2, 232);
                }
                else if (mouseState.X-MasterSlider.Image.Width/2<403)
                {
                    MasterSlider.Position = new Vector2(403, 232);
                }
                else if (mouseState.X+MasterSlider.Image.Width/2>486)
                {
                    MasterSlider.Position = new Vector2(486, 232);
                }
                masterVolume = (mouseState.X - 403) * 1.2f;
            }
            else if (isMusicSliderSelected == true)
            {
                if (mouseState.X - MusicSlider.Image.Width / 2 >= 403 && mouseState.X + MusicSlider.Image.Width / 2 <= 486)
                {
                    MusicSlider.Position = new Vector2(mouseState.X - MusicSlider.Image.Width / 2, 267);
                }
                else if (mouseState.X - MusicSlider.Image.Width / 2 < 403)
                {
                    MusicSlider.Position = new Vector2(403, 267);
                }
                else if (mouseState.X + MusicSlider.Image.Width / 2 > 486)
                {
                    MusicSlider.Position = new Vector2(486, 267);
                }
                musicVolume = (mouseState.X - 403) * 1.2f;
            }
            else if (isSoundSliderSelected == true)
            {
                if (mouseState.X - SoundSlider.Image.Width / 2 >= 403 && mouseState.X + SoundSlider.Image.Width / 2 <= 486)
                {
                    SoundSlider.Position = new Vector2(mouseState.X - SoundSlider.Image.Width / 2, 302);
                }
                else if (mouseState.X - SoundSlider.Image.Width / 2 < 403)
                {
                    SoundSlider.Position = new Vector2(403, 302);
                }
                else if (mouseState.X + SoundSlider.Image.Width / 2 > 486)
                {
                    SoundSlider.Position = new Vector2(486, 302);
                }
                soundVolume = (mouseState.X - 403) * 1.2f;
            }

            if (LoseRetry.Hitbox.Contains(mouseState.Position) && LoseRetry.IsActive == true)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                {
                    retry = true;
                    lose = false;
                }
            }
            else if (LoseRestart.Hitbox.Contains(mouseState.Position) && LoseRestart.IsActive == true)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                {
                    retry = true;
                    restart = true;
                    lose = false;
                }
            }
            else if (LoseQuit.Hitbox.Contains(mouseState.Position) && LoseQuit.IsActive == true)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                {
                    Exit();
                }
            }

            if (WinNext.Hitbox.Contains(mouseState.Position) && WinNext.IsActive == true)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                {
                    retry = true;
                }
            }
            else if (WinQuit.Hitbox.Contains(mouseState.Position) && WinQuit.IsActive == true)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                {
                    Exit();
                }
            }

            if (isMIHModeOn==true)
            {
                if (isPaused == false)
                {
                    timeBeforeMIHSpeed -= gameTime.ElapsedGameTime.TotalMilliseconds;
                    fadeToWhiteTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                if (savedSpeedMultiplier>=10.50)
                {
                    //RESET
                    //make reset work, and then add voice lines and fading/animation
                    //once the stage is reached, pucci says somethign which starts the speed-up. then, up untill the reset, the little "pow..pow..pow" can be heard in the background
                    //pucci then says another voice line as the screen fades to white. Maybe throw in a uni reset animation if you feel like it
                    //maybe, after the reset, add a ghost ball in front of the ball untill you lose. what i mean is, try to make you see your fate.

                    //make the reset and fate-viewing first and add effects after

                    //viewing can be made by copying the ball and bricks to a seperate thing and "play another game" and once you know the events of that game, show and follow them on the original.

                    //don't care, screw animation and noises

                    retry = true;
                    levelsCleared=0;
                    universeHasReset = true;
                }
                else if (savedSpeedMultiplier >= 4.0)
                {
                    MIHSpeedIncrease = 1;
                }
                else if (savedSpeedMultiplier >= 3.0)
                {
                    MIHSpeedIncrease = 0.5;
                }
                else if (savedSpeedMultiplier >= 2.3)
                {
                    MIHSpeedIncrease = 0.25;
                }
                else if (savedSpeedMultiplier >= 1.6)
                {
                    MIHSpeedIncrease = 0.1;
                }
                else if (savedSpeedMultiplier >= 1.2)
                {
                    MIHSpeedIncrease = 0.05;
                }
                else if (savedSpeedMultiplier >= 1.1)
                {
                    MIHSpeedIncrease = 0.02;
                }
                else if (savedSpeedMultiplier >= 1)
                {
                    MIHSpeedIncrease = 0.01;
                }
                if (timeBeforeMIHSpeed<=0)
                {
                    timeBeforeMIHSpeed = 1000;
                    savedSpeedMultiplier += MIHSpeedIncrease;
                    speedMultiplier = savedSpeedMultiplier;
                }
                if (fadeToWhiteTime<=0)
                {
                    fadeToWhiteTime = 16.5;
                    if (MIHSpeedIncrease==1)
                    {
                        WhiteRectangle.Color = new Color(WhiteRectangle.Color.R + 1, WhiteRectangle.Color.G + 1, WhiteRectangle.Color.B + 1, WhiteRectangle.Color.A);
                    }
                }
            }

            trailParticles.EmitterLocation = new Vector2((ball.Position.X + ball.Image.Width / 2) - (trailParticles.ParticleImage[0].Width / 2), (ball.Position.Y + ball.Image.Height / 2) - (trailParticles.ParticleImage[0].Height / 2));
            trailParticles.Update(gameTime);

            if (fateCalcHappening==true && universeHasReset==false)
            {
                fateCalcHappening = false;
            }

            if (universeHasReset==false)
            {
                ball.Position = new Vector2((float)(ball.Position.X + xVelocity), (float)(ball.Position.Y + yVelocity));
                fateBall.Position = new Vector2(0, 0);
                fateCalcHappening = false;
                /*
                if (fateBall.Color==fateColor)
                {
                    ball.Color = Color.White;
                }
                else
                {
                    ball.Color = fateBall.Color;
                }
                if (fateBall.Image==Content.Load<Texture2D>("fateBall"))
                {
                    ball.Image = Content.Load<Texture2D>("ball");
                }
                else
                {
                    ball.Image = fateBall.Image;
                }
                */
            }
            else
            {
                if (isPaused == false)
                {
                    fateInterval -= gameTime.ElapsedGameTime.TotalMilliseconds;
                }

                //for now, just make fate and normal identical, then make it ahead
                //make sure that identical means identical. make fateBall get affected by everyhting, but have fateBall only be visible when uni has reset. if it hasn't, transfer effects onto ball
                //identicallity done. now the hard part

                //with the fate, you will be able to see where the ball will be 2, 4, and 6 seconds in the future. the fate ball will assume that there is no paddle, and the paddle is the only thing you'll be able to control.
                //after the ball bounces from the paddle, the calculations will go from the paddle

                //also, for now, fate is enabled by default for testing reasons

                if (fateBall.Position.Y<paddle.Position.Y+paddle.Image.Height)
                {
                    fateBall.Position = new Vector2((float)(fateBall.Position.X + xVelocity), (float)(fateBall.Position.Y + yVelocity));
                }
                Vector2[] fateVelAdd = new Vector2[fateVelocities.Length + 1];
                for (int i = 0; i < fateVelocities.Length; i++)
                {
                    fateVelAdd[i] = fateVelocities[i];
                    if (i + 1 == fateVelocities.Length)
                    {
                        fateVelAdd[i + 1] = new Vector2((float)xVelocity, (float)yVelocity);
                    }
                }
                fateVelocities = fateVelAdd;

                if (fatePosition2.Position != new Vector2(0, 0) && fatePosition4.Position != new Vector2(0, 0) && fatePositionNow.Position != new Vector2(0, 0) || fateBall.Position.Y > paddle.Position.Y + paddle.Image.Height)
                {
                    if (ball.Hitbox.Intersects(paddle.Hitbox))
                    {
                        fateCalcHappening = true;
                    }
                    else
                    {
                        fateCalcHappening = false;
                    }
                    ball.Position = new Vector2(ball.Position.X + fateVelocities[0].X, ball.Position.Y + fateVelocities[0].Y);
                    ball.Hitbox = new Rectangle((int)ball.Position.X, (int)ball.Position.Y, (int)(ball.Image.Width * ball.Scale), (int)(ball.Image.Height * ball.Scale));
                    Vector2[] fateVelSub = new Vector2[fateVelocities.Length - 1];
                    for (int i = 0; i < fateVelSub.Length; i++)
                    {
                        fateVelSub[i] = fateVelocities[i + 1];
                    }
                    fateVelocities = fateVelSub;
                }

                if (fateInterval<=0)
                {
                    //made fate display. now make normal ball do fate
                    //made bricks exist and fate bricks not
                    //make powerups carry over to real ball. (Nvm, I do NOT care.)
                    //fix bomb being too big
                    //fix bomb and saw being equipped at the same time

                    //also, there some kind of bug that makes both balls stuck on row 1? fix if can figure out what is happening

                    //screw it. after uni reset, make bomb destroy 3 rows completly. that's just easier to deal with, especially with fate
                    //test this next class

                    fateInterval = 2000;

                    fatePositionNow.Position = fateBall.Position;


                    fatePosition2.Position = fatePosition4.Position;


                    fatePosition4.Position = fatePositionNow.Position;

                }
            }

            paddle.Hitbox = new Rectangle((int)paddle.Position.X, (int)paddle.Position.Y, paddle.Image.Width, paddle.Image.Height);
            paddleHitBox1 = new Rectangle((int)paddle.Position.X, (int)paddle.Position.Y, paddle.Image.Width / 3, paddle.Image.Height);
            paddleHitBox2 = new Rectangle((int)paddle.Position.X + paddle.Image.Width / 3, (int)paddle.Position.Y, paddle.Image.Width / 3, paddle.Image.Height);
            paddleHitBox3 = new Rectangle((int)(paddle.Position.X+paddle.Image.Width) - (paddle.Image.Width / 3), (int)paddle.Position.Y, paddle.Image.Width / 3, paddle.Image.Height);

            if (bomb.IsOut==true && isPaused==false && fateCalcHappening==false)
            {
                bomb.Position = new Vector2(bomb.Position.X, (float)(bomb.Position.Y + powerUpVelocity));
            }
            if (saw.IsOut==true && isPaused==false && fateCalcHappening==false)
            {
                saw.Position = new Vector2(saw.Position.X, (float)(saw.Position.Y + powerUpVelocity));
            }
            if (speedUp.IsOut == true && isPaused == false && fateCalcHappening==false)
            {
                speedUp.Position = new Vector2(speedUp.Position.X, (float)(speedUp.Position.Y + powerUpVelocity));
            }
            if (slowDown.IsOut == true && isPaused == false && fateCalcHappening==false)
            {
                slowDown.Position = new Vector2(slowDown.Position.X, (float)(slowDown.Position.Y + powerUpVelocity));
            }
            switch (saw.IsCollected)
            {
                case true:
                    {
                        ball.Color = saw.PowerColor;
                        ball.Image = saw.Image;
                        saw.Position = new Vector2(0, 0);
                        bomb.IsCollected = false;
                        if (sawLengthLeft <= 0)
                        {
                            saw.IsCollected = false;
                        }
                        if (isPaused == false && fateCalcHappening == false)
                        {
                            sawLengthLeft -= gameTime.ElapsedGameTime.TotalMilliseconds;
                        }
                        break;
                    }
                case false:
                    {
                        ball.Color = Color.White;
                        ball.Image = Content.Load<Texture2D>("ball");
                        break;
                    }
            }
            if (bomb.IsCollected == true)
            {
                ball.Color = bomb.PowerColoring();
                bomb.Position = new Vector2(0, 0);
                saw.IsCollected = false;
            }
            else
            {
                ball.Color = Color.White;
            }

            if (fateBomb.IsOut == true && isPaused == false)
            {
                fateBomb.Position = new Vector2(fateBomb.Position.X, (float)(fateBomb.Position.Y + powerUpVelocity));
            }
            if (fateSaw.IsOut == true && isPaused == false)
            {
                fateSaw.Position = new Vector2(fateSaw.Position.X, (float)(fateSaw.Position.Y + powerUpVelocity));
            }
            if (fateBall.Color==Color.White)
            {
                fateBall.Color = fateColor;
            }
            switch (fateSaw.IsCollected)
            {
                case true:
                    {
                        fateBall.Color = fateSaw.PowerColor;
                        fateBall.Image = fateSaw.Image;
                        fateSaw.Position = new Vector2(0, 0);
                        fateBomb.IsCollected = false;
                        if (fateSawLengthLeft <= 0)
                        {
                            fateSaw.IsCollected = false;
                        }
                        if (isPaused == false)
                        {
                            fateSawLengthLeft -= gameTime.ElapsedGameTime.TotalMilliseconds;
                        }
                        break;
                    }
                case false:
                    {
                        fateBall.Color = fateColor;
                        fateBall.Image = Content.Load<Texture2D>("fateBall");
                        break;
                    }
            }
            if (fateBomb.IsCollected == true)
            {
                fateBall.Color = fateBomb.PowerColoring();
                fateBomb.Position = new Vector2(0, 0);
                fateSaw.IsCollected = false;
            }
            else
            {
                fateBall.Color = fateColor;
            }

            if (ball.Hitbox.Intersects(leftEdge))
            {
                ball.Position = new Vector2(leftEdge.Width, ball.Position.Y);
                xVelocity = 1.2f + rand.NextDouble();
                hasCollided = true;
            }
            else if (ball.Position.Y <= 0)
            {
                ball.Position = new Vector2(ball.Position.X, 0);
                yVelocity *= -1;
                hasCollided = true;
            }
            else if (ball.Hitbox.Intersects(rightEdge))
            {
                ball.Position = new Vector2(rightEdge.X - ball.Image.Width, ball.Position.Y);
                xVelocity = -1.0 - rand.NextDouble();
                yVelocity *= 1 + rand.NextDouble();
                hasCollided = true;
            }
            else if (ball.Position.Y + ball.Image.Height >= paddle.Position.Y+paddle.Image.Height)
            {
                ball.Position = new Vector2(paddle.Position.X + paddle.Image.Width / 2 - fateBall.Image.Width / 2, paddle.Position.Y - 50);

                if (universeHasReset==true)
                {
                    fateBall.Position = new Vector2(paddle.Position.X + paddle.Image.Width / 2 - fateBall.Image.Width / 2, paddle.Position.Y - 50);
                    fatePosition2.Position = new Vector2(0, 0);
                    fatePosition4.Position = new Vector2(0, 0);
                    fatePositionNow.Position = new Vector2(0, 0);
                    fateVelocities = fateVelZero;
                    if(fateBomb.IsCollected==true && bomb.IsCollected==false)
                    {
                        fateBomb.IsCollected = false;
                    }
                    if (fateSaw.IsCollected==true && saw.IsCollected==false)
                    {
                        fateSaw.IsCollected = false;
                    }
                    fateCalcHappening = true;
                    fateBricks = FateBricksConverter(bricks);
                }

                if (halfCleared==true && isMIHModeOn==false)
                {
                    savedSpeedMultiplier = 1.4f;
                    speedMultiplier = savedSpeedMultiplier;
                }
                else if (halfCleared==false && isMIHModeOn==false)
                {
                    savedSpeedMultiplier = 1.0f;
                    speedMultiplier = savedSpeedMultiplier;
                }
                if (isMIHModeOn==false)
                {
                    xVelocity = 0.1f;
                    yVelocity = 1;
                }
                if (lives==1 && isInfLivesOn==false)
                {
                    lose = true;
                    lives -= 1;
                }
                else if (lives!=0 && isInfLivesOn==false)
                {
                    lives -= 1;
                }
            }

            if (fateBall.Hitbox.Intersects(leftEdge) && universeHasReset==true)
            {
                fateBall.Position = new Vector2(leftEdge.Width, fateBall.Position.Y);
                xVelocity = 1.2f + rand.NextDouble();
                hasCollided = true;
            }
            else if (fateBall.Position.Y <= 0 && universeHasReset==true)
            {
                fateBall.Position = new Vector2(fateBall.Position.X, 0);
                yVelocity *= -1;
                hasCollided = true;
            }
            else if (fateBall.Hitbox.Intersects(rightEdge) && universeHasReset==true)
            {
                fateBall.Position = new Vector2(rightEdge.X - fateBall.Image.Width, fateBall.Position.Y);
                xVelocity = -1.5 - rand.NextDouble();
                yVelocity *= 1 + rand.NextDouble();
                hasCollided = true;
            }

            if (ball.Hitbox.Intersects(paddle.Hitbox) && fateCalcHappening == false)
            {
                if (ball.Hitbox.Intersects(paddleHitBox1))
                {
                    xVelocity = -1 - rand.NextDouble();
                    yVelocity = -1 - rand.NextDouble();
                }
                else if (ball.Hitbox.Intersects(paddleHitBox2))
                {
                    xVelocity = rand.Next(1, 2) + rand.NextDouble();
                    yVelocity = -1 - rand.NextDouble();
                }
                else if (ball.Hitbox.Intersects(paddleHitBox3))
                {
                    xVelocity = 1.5 + rand.NextDouble();
                    yVelocity = -1 - rand.NextDouble();
                }
                if (universeHasReset==true)
                {
                    fateBall.Position = ball.Position;
                    fatePosition2.Position = new Vector2(0, 0);
                    fatePosition4.Position = new Vector2(0, 0);
                    fatePositionNow.Position = new Vector2(0, 0);
                    fateVelocities = fateVelZero;
                    fateCalcHappening = true;
                    fateBricks = FateBricksConverter(bricks);
                }

                hasCollided = true;
            }
            else if (ball.Hitbox.Intersects(bomb.Hitbox) || paddle.Hitbox.Intersects(bomb.Hitbox) ||
                ball.Hitbox.Intersects(saw.Hitbox) || paddle.Hitbox.Intersects(saw.Hitbox) ||
                ball.Hitbox.Intersects(speedUp.Hitbox) || paddle.Hitbox.Intersects(speedUp.Hitbox) ||
                ball.Hitbox.Intersects(slowDown.Hitbox) || paddle.Hitbox.Intersects(slowDown.Hitbox))
            {
                if (ball.Hitbox.Intersects(bomb.Hitbox) || paddle.Hitbox.Intersects(bomb.Hitbox) && bomb.IsOut == true && saw.IsCollected == false)
                {
                    bomb.IsCollected = true;
                    bomb.IsOut = false;
                    ball.Color = bomb.PowerColor;
                }
                else if (ball.Hitbox.Intersects(saw.Hitbox) || paddle.Hitbox.Intersects(saw.Hitbox) && saw.IsOut == true && bomb.IsCollected == false)
                {
                    saw.IsCollected = true;
                    saw.IsOut = false;
                    ball.Color = saw.PowerColor;
                    ball.Image = saw.Image;
                    sawLengthLeft = sawLength;
                }
                else if (ball.Hitbox.Intersects(speedUp.Hitbox) || paddle.Hitbox.Intersects(speedUp.Hitbox) && speedUp.IsOut == true)
                {
                    speedUp.IsOut = false;
                    speedUp.Position = new Vector2(0, 0);
                    savedSpeedMultiplier += 1.2d;
                    speedMultiplier = savedSpeedMultiplier;
                }
                else if (ball.Hitbox.Intersects(slowDown.Hitbox) || paddle.Hitbox.Intersects(slowDown.Hitbox) && slowDown.IsOut == true)
                {
                    slowDown.IsOut = false;
                    slowDown.Position = new Vector2(0, 0);
                    savedSpeedMultiplier *= 0.8d;
                    speedMultiplier = savedSpeedMultiplier;
                }
            }

            if (fateBall.Hitbox.Intersects(fateBomb.Hitbox)||fateBall.Hitbox.Intersects(fateSaw.Hitbox))
            {
                if (fateBall.Hitbox.Intersects(fateBomb.Hitbox) && fateBomb.IsOut == true && fateSaw.IsCollected == false)
                {
                    fateBomb.IsCollected = true;
                    fateBomb.IsOut = false;
                    fateBall.Color = fateBomb.PowerColor;
                }
                else if (fateBall.Hitbox.Intersects(fateSaw.Hitbox) && fateSaw.IsOut == true && fateBomb.IsCollected == false)
                {
                    fateSaw.IsCollected = true;
                    fateSaw.IsOut = false;
                    fateBall.Color = fateSaw.PowerColor;
                    fateBall.Image = fateSaw.Image;
                    sawLengthLeft = sawLength;
                }
            }

            if (bomb.Position.Y>height)
            {
                bomb.IsOut = false;
            }
            if (saw.Position.Y > height)
            {
                saw.IsOut = false;
            }
            if (speedUp.Position.Y > height)
            {
                speedUp.IsOut = false;
            }
            if (slowDown.Position.Y > height)
            {
                slowDown.IsOut = false;
            }

            if (fateBomb.Position.Y > height)
            {
                fateBomb.IsOut = false;
            }
            if (fateSaw.Position.Y > height)
            {
                fateSaw.IsOut = false;
            }

            //HEAVILY optimize brick breaking code when you figure out how to
            //currently can't since i don't know how to get enough outputs, and i just need it to work for now
            bool brokenBlock = false;

            for (int i = 0; i < fateBricks.Length; i++)
            {
                if (fateBricks[i].IsVisible == true)
                {
                    if (fateBall.Hitbox.Intersects(fateBricks[i].Hitbox) && brokenBlock == false)
                    {
                        if (saw.IsCollected != true)
                        {
                            xVelocity *= (double)(-1 - rand.NextDouble());
                            yVelocity *= (double)(-1 - rand.NextDouble());
                            hasCollided = true;
                        }

                        if (fateBomb.IsCollected == true && fateSaw.IsCollected==false)
                        {
                            /*
                            bool bombed = false;
                            for (int j = 0; j < fateBricks.Length; j++)
                            {
                                if (j + bricksPerRow * 2 >= i - 2 && j + bricksPerRow * 2 <= i + 2)
                                {
                                    if (fateBricks[i].Row-2==fateBricks[j].Row)
                                    {
                                        bombed = true;
                                    }
                                }
                                else if (j + bricksPerRow >= i - 2 && j + bricksPerRow <= i + 2)
                                {
                                    if (fateBricks[i].Row - 1 == fateBricks[j].Row)
                                    {
                                        bombed = true;
                                    }
                                }
                                else if (j >= i - 2 && j <= i + 2)
                                {
                                    if (fateBricks[i].Row == fateBricks[j].Row)
                                    {
                                        bombed = true;
                                    }
                                }
                                else if (j - bricksPerRow >= i - 2 && j - bricksPerRow <= i + 2)
                                {
                                    if (fateBricks[i].Row + 1 == fateBricks[j].Row)
                                    {
                                        bombed = true;
                                    }
                                }
                                else if (j - bricksPerRow * 2 >= i - 2 && j - bricksPerRow * 2 <= i + 2)
                                {
                                    if (fateBricks[i].Row + 2 == fateBricks[j].Row)
                                    {
                                        bombed = true;
                                    }
                                }

                                if (bombed==true)
                                {
                                    score += fateBricks[j].Value;
                                    fateBricks[j].IsVisible = false;
                                }
                                bombed = false;
                            }
                            bomb.IsCollected = false;
                            fateBall.Color = fateColor;
                            */
                            fateBricks = fateBomb.Explosion(fateBricks, bricksPerRow, i, universeHasReset);
                            fateBomb.IsCollected = false;
                            fateBall.Color = fateColor;
                        }

                        fateBricks[i].Durability -= 1;
                        if (fateBricks[i].Durability <= 0 || saw.IsCollected == true
                            || bricks[i].Image != Content.Load<Texture2D>("BrickSilver9-1")
                            && bricks[i].Image != Content.Load<Texture2D>("BrickGold10-1")
                            && bricks[i].Image != Content.Load<Texture2D>("BrickGold10-2")
                            && bricks[i].Image != Content.Load<Texture2D>("Brick"))//change when multi-durability fate bricks made
                        {
                            if (fateBricks[i].PowerUp == "bomb" && fateBomb.IsOut == false && fateBomb.IsCollected == false)
                            {
                                fateBomb.Position = new Vector2(fateBricks[i].Position.X, fateBricks[i].Position.Y + 5);
                                fateBomb.IsOut = true;
                            }
                            if (fateBricks[i].PowerUp == "saw" && fateSaw.IsOut == false && fateSaw.IsCollected == false)
                            {
                                fateSaw.Position = new Vector2(fateBricks[i].Position.X, fateBricks[i].Position.Y + 5);
                                fateSaw.IsOut = true;
                            }
                            fateBricks[i].IsVisible = false;
                        }
                        else if (fateBricks[i].Durability == 1 && fateBricks[i].Image == Content.Load<Texture2D>("BrickSilver9-1"))
                        {
                            fateBricks[i].Image = Content.Load<Texture2D>("BrickSilver9-2");
                        }
                        else if (fateBricks[i].Image == Content.Load<Texture2D>("BrickGold10-1"))
                        {
                            fateBricks[i].Image = Content.Load<Texture2D>("BrickGold10-2");
                        }
                        else if (fateBricks[i].Image == Content.Load<Texture2D>("BrickGold10-2"))
                        {
                            fateBricks[i].Image = Content.Load<Texture2D>("BrickGold10-3");
                        }
                        brokenBlock = true;
                    }
                }
            }

            brokenBlock = false;

            for (int i = 0; i < bricks.Length; i++)
            {
                if (bricks[i].IsVisible == true)
                {
                    if (ball.Hitbox.Intersects(bricks[i].Hitbox) && brokenBlock == false)
                    {
                        if (saw.IsCollected != true)
                        {
                            xVelocity *= (double)(-1 - rand.NextDouble());
                            yVelocity *= (double)(-1 - rand.NextDouble());
                            hasCollided = true;
                        }

                        if (bomb.IsCollected == true && saw.IsCollected == false)
                        {
                            /*
                            bool bombed = false;
                            for (int j = 0; j < bricks.Length; j++)
                            {
                                if (j + bricksPerRow * 2 >= i - 2 && j + bricksPerRow * 2 <= i + 2)
                                {
                                    if (bricks[i].Row - 2 == bricks[j].Row)
                                    {
                                        bombed = true;
                                    }
                                }
                                else if (j + bricksPerRow >= i - 2 && j + bricksPerRow <= i + 2)
                                {
                                    if (bricks[i].Row - 1 == bricks[j].Row)
                                    {
                                        bombed = true;
                                    }
                                }
                                else if (j >= i - 2 && j <= i + 2)
                                {
                                    if (bricks[i].Row == bricks[j].Row)
                                    {
                                        bombed = true;
                                    }
                                }
                                else if (j - bricksPerRow >= i - 2 && j - bricksPerRow <= i + 2)
                                {
                                    if (bricks[i].Row + 1 == bricks[j].Row)
                                    {
                                        bombed = true;
                                    }
                                }
                                else if (j - bricksPerRow * 2 >= i - 2 && j - bricksPerRow * 2 <= i + 2)
                                {
                                    if (bricks[i].Row + 2 == bricks[j].Row)
                                    {
                                        bombed = true;
                                    }
                                }

                                if (bombed == true)
                                {
                                    score += bricks[j].Value;
                                    bricks[j].IsVisible = false;
                                }
                                bombed = false;
                            }
                            bomb.IsCollected = false;
                            ball.Color = fateColor;
                            */
                            bricks = bomb.Explosion(bricks, bricksPerRow, i, universeHasReset);
                            score += 150;
                            bomb.IsCollected = false;
                            fateBomb.IsCollected = false;
                            ball.Color = Color.White;
                            fateBall.Color = fateColor;
                        }

                        bricks[i].Durability -= 1;
                        if (bricks[i].Durability <= 0 || saw.IsCollected == true
                            || bricks[i].Image != Content.Load<Texture2D>("BrickSilver9-1")
                            && bricks[i].Image != Content.Load<Texture2D>("BrickGold10-1")
                            && bricks[i].Image != Content.Load<Texture2D>("BrickGold10-2")
                            && bricks[i].Image != Content.Load<Texture2D>("Brick"))
                        {
                            if (bricks[i].PowerUp == "bomb" && bomb.IsOut == false && bomb.IsCollected == false)
                            {
                                bomb.Position = new Vector2(bricks[i].Position.X, bricks[i].Position.Y + 5);
                                bomb.IsOut = true;
                            }
                            if (bricks[i].PowerUp == "saw" && saw.IsOut == false && saw.IsCollected == false)
                            {
                                saw.Position = new Vector2(bricks[i].Position.X, bricks[i].Position.Y + 5);
                                saw.IsOut = true;
                            }
                            if (bricks[i].PowerUp == "speedUp" && speedUp.IsOut == false)
                            {
                                speedUp.Position = new Vector2(bricks[i].Position.X, bricks[i].Position.Y + 5);
                                speedUp.IsOut = true;
                            }
                            if (bricks[i].PowerUp == "slowDown" && slowDown.IsOut == false)
                            {
                                slowDown.Position = new Vector2(bricks[i].Position.X, bricks[i].Position.Y + 5);
                                slowDown.IsOut = true;
                            }
                            score += bricks[i].Value;
                            bricks[i].IsVisible = false;
                        }
                        else if (bricks[i].Durability == 1 && bricks[i].Image == Content.Load<Texture2D>("BrickSilver9-1"))
                        {
                            bricks[i].Image = Content.Load<Texture2D>("BrickSilver9-2");
                        }
                        else if (bricks[i].Image == Content.Load<Texture2D>("BrickGold10-1"))
                        {
                            bricks[i].Image = Content.Load<Texture2D>("BrickGold10-2");
                        }
                        else if (bricks[i].Image == Content.Load<Texture2D>("BrickGold10-2"))
                        {
                            bricks[i].Image = Content.Load<Texture2D>("BrickGold10-3");
                        }
                        brokenBlock = true;

                        if (universeHasReset==false)
                        {
                            fateBricks = FateBricksConverter(bricks);
                        }
                    }
                }
            }

            int blocksR = 0;
            for (int i = 0; i < bricks.Length; i++)
            {
                if (bricks[i].IsVisible == true)
                {
                    blocksR++;
                }
                if (i + 1 == bricks.Length)
                {
                    if (blocksR <= bricks.Length / 2 && halfCleared == false && isMIHModeOn == false)
                    {
                        savedSpeedMultiplier *= 1.4f;
                        speedMultiplier = savedSpeedMultiplier;
                        halfCleared = true;
                    }
                }
            }

            if (blocksR==0 && isMIHModeOn==false)
            {
                win = true;
            }
            else if (blocksR==0 && isMIHModeOn==true)
            {
                for (int i = 0; i < bricks.Length; i++)
                {
                    bricks[i].IsVisible = true;
                    firstBricks[i].IsVisible = true;
                }
                bricks = Shuffle(blockCount, false, false);
                fateBricks = FateBricksConverter(bricks);
                paddle.Position = new Vector2(width / 2 - paddle.Image.Width / 2, height - 75);
                fateBall.Position = new Vector2(paddle.Position.X + paddle.Image.Width / 2 - fateBall.Image.Width / 2, paddle.Position.Y - 50);
            }

            if (retry==true)
            {
                for (int i = 0; i < bricks.Length; i++)
                {
                    bricks[i].IsVisible = true;
                    firstBricks[i].IsVisible = true;
                }
                paddle.Position = new Vector2(width / 2 - paddle.Image.Width / 2, height - 75);
                fateBall.Position = new Vector2(paddle.Position.X + paddle.Image.Width / 2 - fateBall.Image.Width / 2, paddle.Position.Y - 50);
                ball.Position = new Vector2(paddle.Position.X + paddle.Image.Width / 2 - ball.Image.Width / 2, paddle.Position.Y - 50);
                xVelocity = 0.1f;
                yVelocity = 1;
                savedSpeedMultiplier = 1;
                speedMultiplier = savedSpeedMultiplier;
                fateBall.Color = fateColor;
                fateBall.Image = Content.Load<Texture2D>("fateBall");
                ball.Color = Color.White;
                ball.Image = Content.Load<Texture2D>("ball");
                if (restart==true)
                {
                    bricks = firstBricks;
                    fateBricks = FateBricksConverter(bricks);
                }
                halfCleared = false;
                bomb.IsOut = false;
                bomb.IsCollected = false;
                saw.IsOut = false;
                saw.IsCollected = false;
                if (win==false)
                {
                    score = 0;
                    lives = 3;
                }
                else
                {
                    bricks = Shuffle(blockCount, true, true);
                    fateBricks = FateBricksConverter(bricks);
                    levelsCleared += 1;
                }
                retry = false;
                restart = false;
                win = false;
                lose = false;
                LoseRetry.IsActive = false;
                LoseRestart.IsActive = false;
                LoseQuit.IsActive = false;
                WinNext.IsActive = false;
                WinQuit.IsActive = false;
                if (levelsCleared>=4)
                {
                    isMIHModeOn = true;
                    score = 0;
                    bricks = Shuffle(blockCount, false, false);
                }
                if (isMIHModeOn==true && universeHasReset==true)
                {
                    isMIHModeOn = false;
                    bricks = Shuffle(blockCount, true, true);
                    fateBricks = FateBricksConverter(bricks);
                }
                if (universeHasReset==true)
                {
                    bricks = Shuffle(blockCount, true, true);
                    fateBricks = FateBricksConverter(bricks);
                    fateBall.Position = ball.Position;
                }
            }
            else if (win==true)
            {
                xVelocity = 0.1f;
                yVelocity = 0.1f;
                WinNext.IsActive = true;
                WinQuit.IsActive = true;
            }
            else if (lose==true)
            {
                xVelocity = 0.1f;
                yVelocity = 0.1f;
                LoseRetry.IsActive = true;
                LoseRestart.IsActive = true;
                LoseQuit.IsActive = true;
            }

            if (xVelocity>6)
            {
                xVelocity = 6;
            }
            else if (yVelocity>6)
            {
                yVelocity = 6;
            }
            if (isMIHModeOn==false)
            {
                if (speedMultiplier>2)
                {
                    speedMultiplier = 2;
                }
                if (speedMultiplier<0.25)
                {
                    speedMultiplier = 0.25;
                }
            }
            if (hasCollided==true)
            {
                xVelocity *= speedMultiplier;
                yVelocity *= speedMultiplier;
            }

            if (fateBall.Position.Y==paddle.Position.Y-50 && yVelocity==0)
            {
                yVelocity += 0.5f;
            }

            lastKeyboardState = keyboardState;
            if (xVelocity!=0 && yVelocity!=0)
            {
                savedXVelocity = xVelocity;
                savedYVelocity = yVelocity;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            int width = GraphicsDevice.Viewport.Width;
            int height = GraphicsDevice.Viewport.Height;

            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            _spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

            trailParticles.Color = ball.Color;
            trailParticles.Draw(_spriteBatch);

            for (int i = 0; i < bricks.Length; i++)
            {
                bricks[i].Draw(_spriteBatch);
            }

            bomb.Draw(_spriteBatch);
            saw.Draw(_spriteBatch); 
            speedUp.Draw(_spriteBatch);
            slowDown.Draw(_spriteBatch);

            fateBomb.Draw(_spriteBatch);
            fateSaw.Draw(_spriteBatch);

            paddle.Draw(_spriteBatch);

            ball.Draw(_spriteBatch);

            _spriteBatch.Draw(HUD, new Vector2(leftEdge.Width, height - 50), null, Color.White, 0, new Vector2(0, 0), 1 / 1f, SpriteEffects.None, 0);
            if (isMIHModeOn==false)
            {
                switch (lives)
                {
                    case 0:
                        {
                            break;
                        }

                    case 1:
                        {
                            _spriteBatch.Draw(Content.Load<Texture2D>("Heart1"), new Vector2(leftEdge.Width + 10, height - 35), null, Color.White, 0, new Vector2(0, 0), 1 / 1f, SpriteEffects.None, 0);
                            break;
                        }

                    case 2:
                        {
                            _spriteBatch.Draw(Content.Load<Texture2D>("Heart2"), new Vector2(leftEdge.Width + 10, height - 35), null, Color.White, 0, new Vector2(0, 0), 1 / 1f, SpriteEffects.None, 0);
                            break;
                        }

                    case 3:
                        {
                            _spriteBatch.Draw(Content.Load<Texture2D>("Heart3"), new Vector2(leftEdge.Width + 10, height - 35), null, Color.White, 0, new Vector2(0, 0), 1 / 1f, SpriteEffects.None, 0);
                            break;
                        }
                }
            }

            _spriteBatch.DrawString(pixelFont, "SCORE:", new Vector2(rightEdge.X - 100, height - 40), Color.Black);
            _spriteBatch.DrawString(pixelFont, $"{score}", new Vector2(rightEdge.X - 100, height - 25), Color.Black);
            if (isMIHModeOn==true)
            {
                _spriteBatch.DrawString(pixelFont, "Speed Multiplier", new Vector2(5, 90), Color.White);
                _spriteBatch.DrawString(pixelFont, string.Format("{0:0.00}", speedMultiplier), new Vector2(5, 110), Color.White);
                if (MIHSpeedIncrease==1)
                {
                    WhiteRectangle.Draw(_spriteBatch);
                }
            }

            if (universeHasReset==true)
            {
                for (int i = 0; i < fateBricks.Length; i++)
                {
                    fateBricks[i].Draw(_spriteBatch);
                }
                fateBall.Draw(_spriteBatch);
                fatePosition2.Draw(_spriteBatch);
                fatePosition4.Draw(_spriteBatch);
            }

            if (isPaused==true)
            {
                BlackRectangle.Color = new Color(Color.Black, 75);
                BlackRectangle.Draw(_spriteBatch);
                if (isInMenu==false) { _spriteBatch.Draw(PauseMenu, new Vector2((width / 2) - (PauseMenu.Width / 2), (height / 2) - (PauseMenu.Height / 2)), null, Color.White, 0, new Vector2(0, 0), 1 / 1f, SpriteEffects.None, 0); }
                if (isInMenu==true) { _spriteBatch.Draw(Menu, new Vector2((width / 2) - (Menu.Width / 2), (height / 2) - (Menu.Height / 2)), null, Color.White, 0, new Vector2(0, 0), 1 / 1f, SpriteEffects.None, 0); }

                if (MasterSlider.IsActive==true) { MasterSlider.Draw(_spriteBatch); }
                if (MusicSlider.IsActive==true) { MusicSlider.Draw(_spriteBatch); }
                if (SoundSlider.IsActive == true) { SoundSlider.Draw(_spriteBatch); }
                if (MenuBackButton.IsActive == true) { MenuBackButton.Draw(_spriteBatch); }
            }

            if(lose==true)
            {
                BlackRectangle.Color = new Color(Color.Black, 2555);
                BlackRectangle.Draw(_spriteBatch);
                _spriteBatch.Draw(LoseScreen, new Vector2(150, 100), Color.White);
                _spriteBatch.DrawString(pixelFont, "SCORE:", new Vector2(leftEdge.Width + 85, 200), Color.White);
                _spriteBatch.DrawString(pixelFont, $"{score}", new Vector2(leftEdge.Width + 90, 215), Color.White);
            }
            else if (win==true)
            {
                BlackRectangle.Color = new Color(Color.Black, 2555);
                BlackRectangle.Draw(_spriteBatch);
                _spriteBatch.Draw(WinScreen, new Vector2(300, 100), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
