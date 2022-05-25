using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonogameRedirect
{
    class Sprite
    {
        private Vector2 _position;
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        public Texture2D Image { get; set; }
        public float Scale { get; set; }
        public Color Color { get; set; }

        public Rectangle Hitbox 
        { 
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)(Image.Width * Scale), (int)(Image.Height * Scale));
            }
            set
            {
            }
        }

        public Sprite(Vector2 position, Texture2D image, float scale, Color color)
        {
            Position = position;
            Image = image;
            Scale = scale;
            Color = color;
        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(Image, Hitbox, Color);
        }

    }
}
