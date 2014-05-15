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

        Platform[] platforms;

        Player player;

        bool jumping;
        bool falling;
        float startY;
        float jumpHeight;
        float maxJump;
        int jumpTimer;

        public RunnerGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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


            //platforms init creation
            platforms = new Platform[15];
            for (int i = 0; i < 15; i++)
            {
                platforms[i] = new Platform(Size.medium, new Vector2(90 + (i * 200), 250));
            }


            jumping = false;
            falling = false;
            maxJump = 25;
            jumpHeight = 0;
            jumpTimer = 60;
            startY = 0;

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

            

            #region Jumping
            //Jump!
            if (currentKeyboard.IsKeyDown(Keys.Space) && !prevKeyboard.IsKeyDown(Keys.Space) && !jumping)
            {
                jumping = true;
                startY = player.Pos.Y;
            }

            player.IsColliding = false;
            foreach (Platform p in platforms)
            {
                if (p.getCollisionRect().Contains(player.Rect.Center.X, player.Rect.Bottom)) 
                {
                    player.IsColliding = true;
                    startY = p.Bounds.Y - player.Rect.Height;
                }
            }

            if (jumping)
            {
                jumpTimer--;
                if (jumpTimer == 0)
                {
                    jumping = false;
                    falling = true;
                    jumpTimer = 60;
                }
            }
            if (currentKeyboard.IsKeyDown(Keys.Space) && jumping)
            {
                jumpHeight++;
                if(jumpHeight < maxJump)
                    player.Pos = new Vector2(player.Pos.X, player.Pos.Y - 3);
            }
            if (falling)
            {
                player.Pos = new Vector2(player.Pos.X, player.Pos.Y + 5);

                if (player.Pos.Y >= startY)
                {
                    falling = false;
                    jumpHeight = 0;
                    player.Pos = new Vector2(player.Pos.X, startY);
                }
            }
            #endregion

            foreach (Platform p in platforms)
            {
                p.Pos = new Vector2(p.Pos.X - 5, p.Pos.Y);
            }


            if (!jumping && !player.IsColliding)
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
            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}