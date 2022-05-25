using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonogameRedirect
{
    class PowerUp : Sprite
    {
        public bool IsOut;
        public bool IsCollected;
        public Color PowerColor;

        public PowerUp(Vector2 position, Texture2D image, float scale, Color color, bool sout, bool collect, Color pcolor)
            : base(position, image, scale, color)
        {
            Position = position;
            Image = image;
            Scale = scale;
            Color = color;
            IsOut = sout;
            IsCollected = collect;
            PowerColor = pcolor;
        }

        public Brick[] Explosion(Brick[] bricks, int bricksPerRow, int interval, bool hasUniReset)
        {
            Brick[] blownBricks = new Brick[bricks.Length];
            for (int i = 0; i < blownBricks.Length; i++)
            {
                blownBricks[i] = bricks[i].Clone();
            }
            bool bombed = false;
            if (hasUniReset==false)
            {
                for (int i = interval; i < blownBricks.Length; i++)
                {
                    for (int j = 0; j < bricks.Length; j++)
                    {
                        if (j + bricksPerRow * 2 >= i - 2 && j + bricksPerRow * 2 <= i + 2)
                        {
                            if (blownBricks[i].Row - 2 == blownBricks[j].Row)
                            {
                                bombed = true;
                            }
                        }
                        else if (j + bricksPerRow >= i - 2 && j + bricksPerRow <= i + 2)
                        {
                            if (blownBricks[i].Row - 1 == blownBricks[j].Row)
                            {
                                bombed = true;
                            }
                        }
                        else if (j >= i - 2 && j <= i + 2)
                        {
                            if (blownBricks[i].Row == blownBricks[j].Row)
                            {
                                bombed = true;
                            }
                        }
                        else if (j - bricksPerRow >= i - 2 && j - bricksPerRow <= i + 2)
                        {
                            if (blownBricks[i].Row + 1 == blownBricks[j].Row)
                            {
                                bombed = true;
                            }
                        }
                        else if (j - bricksPerRow * 2 >= i - 2 && j - bricksPerRow * 2 <= i + 2)
                        {
                            if (blownBricks[i].Row + 2 == blownBricks[j].Row)
                            {
                                bombed = true;
                            }
                        }

                        if (bombed == true)
                        {
                            blownBricks[j].IsVisible = false;
                        }
                        bombed = false;
                    }
                }
            }
            else
            {
                for (int i = interval; i < blownBricks.Length; i++)
                {
                    for (int j = 0; j < bricks.Length; j++)
                    {
                        if (blownBricks[i].Row==blownBricks[j].Row||blownBricks[i].Row==blownBricks[j].Row+1||blownBricks[i].Row==blownBricks[j].Row-1)
                        {
                            bombed = true;
                        }
                        if (bombed == true)
                        {
                            blownBricks[j].IsVisible = false;
                        }
                        bombed = false;
                    }
                }
            }

            return blownBricks;
        }

        public Color PowerColoring()
        {
            if (IsCollected==true)
            {
                return PowerColor;
            }
            else
            {
                return Color.White;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            if (IsOut == true)
            {
                base.Draw(sb);
            }
        }
    }
}
