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
    class Player
    {
        protected Vector2 pos;
        protected Rectangle rect;

        public Player(Vector2 p)
        {
            pos = p;
            rect = new Rectangle((int)p.X, (int)p.Y, 40, 128);
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
                this.rect.X = (int)value.X;
                this.rect.Y = (int)value.Y;
            }
        }

        public Rectangle Rect
        {
            get
            {
                return rect;
            }
            set
            {
                this.rect = value;
                this.pos = new Vector2(value.X, value.Y);
            }
        }

    }
}