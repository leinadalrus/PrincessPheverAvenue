#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

// My used packages
#endregion

namespace PrincessPheverAvenue
{
    public class PrincessPheverAvenue : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D gftoSprite;
        Rectangle gftoCollision;
        Vector2 gftoPosition;
        Single /* float */ gftoSpeed;

        int deadZone = 4096;

        public PrincessPheverAvenue()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            gftoPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2,
                _graphics.PreferredBackBufferHeight / 2);
            gftoSpeed = 100.1f; // 100.0f as-is: 100% in my context.

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            gftoSprite = Content.Load<Texture2D>("Mirage-Idle");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            float nuSpeed = gftoSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.W))
            {
                gftoPosition.Y -= nuSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                gftoPosition.X -= nuSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                gftoPosition.Y += nuSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                gftoPosition.X += nuSpeed;
            }

            // NOTE(Gamepad): Gamepad logic
            if (Joystick.LastConnectedIndex == 0)
            {
                var joystickState = Joystick.GetState((int)PlayerIndex.One);

                if (joystickState.Axes[1] < -deadZone)
                {
                    gftoPosition.Y -= nuSpeed;
                }

                if (joystickState.Axes[1] > deadZone)
                {
                    gftoPosition.Y += nuSpeed;
                }

                if (joystickState.Axes[0] < -deadZone)
                {
                    gftoPosition.X -= nuSpeed;
                }

                if (joystickState.Axes[0] > deadZone)
                {
                    gftoPosition.X += nuSpeed;
                }
            }

            if (gftoCollision.X > _graphics.PreferredBackBufferWidth - gftoSprite.Width / 2)
            {
                gftoCollision.X = _graphics.PreferredBackBufferWidth - gftoSprite.Width / 2;
            }
            else if (gftoCollision.X < gftoSprite.Width / 2)
            {
                gftoCollision.X = gftoSprite.Width / 2;
            }

            if (gftoCollision.Y > _graphics.PreferredBackBufferHeight - gftoSprite.Height / 2)
            {
                gftoCollision.Y = _graphics.PreferredBackBufferHeight - gftoSprite.Height / 2;
            }
            else if (gftoCollision.Y < gftoSprite.Height / 2)
            {
                gftoCollision.Y = gftoSprite.Height / 2;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(gftoSprite,
                gftoPosition,
                null,
                Color.GhostWhite,
                0.0f,
                new Vector2(gftoSprite.Width / 5,
                            gftoSprite.Height / 4),
                Vector2.One,
                SpriteEffects.None,
                0.0f);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
