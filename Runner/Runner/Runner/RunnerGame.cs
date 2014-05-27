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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 

    public class RunnerGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D bubble;
        Texture2D waterBG;
        Texture2D skyBG;
        Texture2D playerTex;

        KeyboardState currentKeyboard;
        KeyboardState prevKeyboard;
        MouseState currentMouse;
        MouseState prevMouse;

        SpriteFont font;

        Random rand;

        Platform[] platforms;

        Player player;

        bool jumping;
        bool initJump;
        float jumpHeight;
        float maxJump;
        int jumpTimer;
        int SCORE;

        public RunnerGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        /// 

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            player = new Player(new Vector2(100, 100));

            prevKeyboard = Keyboard.GetState();
            prevMouse = Mouse.GetState();
            rand = new Random();

            jumping = false;
            initJump = false;
            maxJump = 25;
            jumpHeight = 0;
            jumpTimer = 40;

            SCORE = 0;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            skyBG = Content.Load<Texture2D>(@"Backgrounds//skyBG");
            waterBG = Content.Load<Texture2D>(@"Backgrounds//waterBG");
            bubble = Content.Load<Texture2D>(@"Misc//bubble");
            playerTex = Content.Load<Texture2D>(@"Player//player");
            font = Content.Load<SpriteFont>("font");

            //platforms init creation
            platforms = new Platform[8];
            for (int i = 0; i < 8; i++)
            {
                int pType = rand.Next(5);
                if(pType == 0)
                    platforms[i] = new Platform(Size.small, new Vector2(rand.Next(50) + (i * 150), rand.Next(300, 400)));
                if (pType >= 1 && pType < 3)
                    platforms[i] = new Platform(Size.medium, new Vector2(rand.Next(50) + (i * 150), rand.Next(300, 400)));
                if (pType >= 3)
                    platforms[i] = new Platform(Size.large, new Vector2(rand.Next(50) + (i * 150), rand.Next(300, 400)));
            }

            

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape))
                this.Exit();

            currentKeyboard = Keyboard.GetState();
            currentMouse = Mouse.GetState();

            if (currentKeyboard.IsKeyDown(Keys.R) && !prevKeyboard.IsKeyDown(Keys.R))
                Initialize();

            if (GraphicsDevice.Viewport.Bounds.Contains((int)player.Pos.X, (int)player.Pos.Y))
                SCORE++;

            #region Jumping

            //Jump!

            if (currentKeyboard.IsKeyDown(Keys.Space) && !prevKeyboard.IsKeyDown(Keys.Space) && !jumping)
            {
                jumping = true;
                initJump = true;
            }

            player.IsColliding = false;
            foreach (Platform p in platforms)
            {
                if (p.getCollisionRect().Contains(player.Rect.Center.X, player.Rect.Bottom)) 
                {
                    player.IsColliding = true;
                    jumpTimer = 40;
                    //startY = p.Bounds.Y - player.Rect.Height;
                }
            }

            if (jumping)
            {
                jumpTimer--;
                if (jumpTimer <= 0)
                {
                    jumping = false;
                }
            }

            if (!currentKeyboard.IsKeyDown(Keys.Space) && initJump)
            {
                jumping = false;
                initJump = false;
            }

            if (currentKeyboard.IsKeyDown(Keys.Space) && jumping)
            {
                jumpHeight++;
                if(jumpHeight < maxJump)
                    player.Pos = new Vector2(player.Pos.X, player.Pos.Y - 6);
            }
            if (!jumping && player.IsColliding)
                jumpHeight = 0;

            #endregion

            if (currentKeyboard.IsKeyDown(Keys.D))
                player.Pos = new Vector2(player.Pos.X + 3, player.Pos.Y);
            if (currentKeyboard.IsKeyDown(Keys.A))
                player.Pos = new Vector2(player.Pos.X - 3, player.Pos.Y);

            #region platform handling
            foreach (Platform p in platforms)
            {
               p.Pos = new Vector2(p.Pos.X - 3, p.Pos.Y);
               if (p.Bounds.Right < 0)
               {
                   int pType = rand.Next(5);
                   if (pType == 0)
                       p.setType(Size.small);
                   if (pType >= 1 && pType < 4)
                       p.setType(Size.medium);
                   if (pType >= 4)
                       p.setType(Size.large);


                   int i = 0;
                   int index = 0;
                   foreach (Platform p2 in platforms)
                   {
                       if (p == platforms[i])
                           index = i;
                       i++;
                   }

                   int difY = 100;
                   bool test = false;
                   p.Pos = new Vector2(1280 + rand.Next(50), rand.Next(200, 520));
                   if (i != 0)
                   {
                       while (!test)
                       {
                           p.Pos = new Vector2(1280 + rand.Next(50), rand.Next(200, 520));
                           if (difY > Math.Abs((p.Pos.Y - platforms[i - 1].Pos.Y)))
                           {
                               test = true;
                           }
                       }
                   }
                   else
                   {
                       while (!test)
                       {
                           p.Pos = new Vector2(1280, rand.Next(200, 520));
                           if (difY > Math.Abs((p.Pos.Y - platforms[i-1].Pos.Y)))
                           {
                               test = true;
                           }
                       }
                   }
               }
            }
            #endregion

            //gravity
            if (!player.IsColliding && !jumping)
                player.Pos = new Vector2(player.Pos.X, player.Pos.Y + 4);

            //end stuff
            prevKeyboard = Keyboard.GetState();
            prevMouse = Mouse.GetState();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            spriteBatch.Draw(waterBG, new Rectangle(0, 0, 1280, 720), Color.White);
            spriteBatch.Draw(playerTex, player.Rect, Color.White);
            foreach (Platform p in platforms)
            {
                spriteBatch.Draw(bubble, p.Bounds, Color.White);
                //spriteBatch.Draw(skyBG, p.getCollisionRect(), Color.Green);
            }
            //if (player.IsColliding)
              //  spriteBatch.DrawString(font, "IsColliding = true", new Vector2(0, 0), Color.Black);
            //if (!player.IsColliding)
              //  spriteBatch.DrawString(font, "IsColliding = false", new Vector2(0, 0), Color.Black);
            
                //spriteBatch.DrawString(font, "JumpTimer: " + jumpTimer, new Vector2(0, 30), Color.Black);

            spriteBatch.DrawString(font, "SCORE: " + SCORE, new Vector2(550, 0), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}