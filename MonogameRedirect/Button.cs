using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonogameRedirect
{
    class Button
    {
        public Vector2 Position { get; set; }
        public Texture2D Image { get; set; }
        public float Scale { get; set; }
        public Color Color { get; set; }
        public bool IsActive { get; set; }

        public Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)(Image.Width * Scale), (int)(Image.Height * Scale));
            }
        }

        public Button(Vector2 position, Texture2D image, float scale, Color color, bool isActive)
        {
            Position = position;
            Image = image;
            Scale = scale;
            Color = color;
            IsActive = isActive;
        }

        public virtual void Draw(SpriteBatch sb)
        {
            if (IsActive)
            {
                sb.Draw(Image, Hitbox, Color);
            }
        }
    }
}
