using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Runner
{
    public enum Size { small, medium, large };
    class Platform
    {
        

        private Size size;
        private Rectangle bounds;
        private Rectangle collisionRect;
        private Vector2 pos;

        public Platform(Size s, Vector2 p)
        {
            size = s;
            pos = p;

            if (size == Size.small)
            {
                bounds = new Rectangle((int)pos.X, (int)pos.Y, 60, 30);
                collisionRect = new Rectangle((int)pos.X, (int)pos.Y, 60, 5);
            }
            if (size == Size.medium)
            {
                bounds = new Rectangle((int)pos.X, (int)pos.Y, 90, 30);
                collisionRect = new Rectangle((int)pos.X, (int)pos.Y, 90, 5);
            }
            if (size == Size.large)
            {
                bounds = new Rectangle((int)pos.X, (int)pos.Y, 120, 30);
                collisionRect = new Rectangle((int)pos.X, (int)pos.Y, 120, 5);
            }

        }

        public Vector2 Pos
        {
            get
            {
                return pos;
            }
            set
            {
                this.pos = value;
                this.bounds.X = (int)value.X;
                this.bounds.Y = (int)value.Y;
                this.collisionRect.X = (int)value.X;
                this.collisionRect.Y = (int)value.Y;
            }
        }

        public Rectangle Bounds
        {
            get
            {
                return bounds;
            }
            set
            {
                this.bounds = value;
                this.collisionRect = new Rectangle(value.X, value.Y, collisionRect.Width, collisionRect.Height);
                this.pos = new Vector2(value.X, value.Y);
            }
        }

        public void setType(Size s)
        {
            size = s;
        }

        public Rectangle getCollisionRect()
        {
            return collisionRect;
        }
    }
}
