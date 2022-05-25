using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonogameRedirect
{
    class Brick : Sprite
    {
        public string PowerUp;
        public int Row;
        public int Sprite;
        public int Value;
        public int Durability;

        public bool IsVisible = true;

        public Brick(Vector2 position, Texture2D image, float scale, Color color, string powerup, int row, int value, int durability, bool visible, int sprite)
            : base(position, image, scale, color)
        {
            Position = position;
            Image = image;
            Scale = scale;
            Color = color;
            PowerUp = powerup;
            Row = row;
            IsVisible = visible;
            Sprite = sprite;
            Value = value;
            Durability = durability;
        }

        public Brick Clone()
        {
            return new Brick(Position, Image, Scale, Color, PowerUp, Row, Value, Durability, IsVisible, Sprite);
        }

        public override void Draw(SpriteBatch sb)
        {
            if (IsVisible==true)
            {
                base.Draw(sb);
            }
        }
    }
}
