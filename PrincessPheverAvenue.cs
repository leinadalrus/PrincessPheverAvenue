using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace PrincessFeverAvenue
{
    public class IMiddleForwardLayout
    {
        Vector2 layoutPosition,
                layoutDirection;

        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
    }

    public class PlayerBox2D
    {
        public Vector2 Velocity;
        public Vector2 Force;

        public Matrix  Yaw;
        public Vector2 YawVelocityAxis,
                       YawForce;

        public Single MaxVelocity,
                      MaxYawVelocity;

        public Single DampingForce,
                      DampingYawForce;

        public Single InputForce,
                      InputYawForce;

        public PlayerBox2D()
        {
            Velocity = Vector2.Zero;
            Force = Vector2.Zero;

            Yaw = Matrix.Identity;
            YawVelocityAxis = Vector2.Zero;
            YawForce = Vector2.Zero;

            MaxVelocity = 100.0f;
            MaxYawVelocity = 1.1f;

            DampingForce = 100.0f * 2;
            DampingYawForce = 1.1f * 2;

            InputForce = 1000.0f;
            InputYawForce = 1.1f * 2;
        }
    }

    public class PlayerVehicle
    {
        Vector2 position;
        Rectangle hitbox;
    }

    public class PrincessPheverAvenue : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D playerVehicleSprite;
        Vector2 playerVehiclePosition;
        Single /* float */ playerVehicleSpeed;

        public PrincessPheverAvenue()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            playerVehiclePosition = new Vector2(_graphics.PreferredBackBufferWidth / 2,
                _graphics.PreferredBackBufferHeight / 2);
            playerVehicleSpeed = 100.0f; // 100.0f as-is: 100% in my context.

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            playerVehicleSprite = Content.Load<Texture2D>("player-vehicle-sprite-idle");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var keyboardState = Keyboard.GetState();

            Boolean wasUpperMovement = keyboardState.IsKeyDown(Keys.Up)
                    || keyboardState.IsKeyDown(Keys.W);
            Boolean wasLowerMovement = keyboardState.IsKeyDown(Keys.Down)
                    || keyboardState.IsKeyDown(Keys.S);
            Boolean wasLeftmostMovement = keyboardState.IsKeyDown(Keys.Left)
                    || keyboardState.IsKeyDown(Keys.A);
            Boolean wasRightmostMovement = keyboardState.IsKeyDown(Keys.Right)
                    || keyboardState.IsKeyDown(Keys.D);

            if (wasUpperMovement)
                playerVehiclePosition.Y +=
                        playerVehicleSpeed *
                        (Single)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (wasLowerMovement)
                playerVehiclePosition.Y -=
                        playerVehicleSpeed *
                        (Single)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (wasLeftmostMovement)
                playerVehiclePosition.X -=
                        playerVehicleSpeed *
                        (Single)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (wasRightmostMovement)
                playerVehiclePosition.X +=
                        playerVehicleSpeed *
                        (Single)gameTime.ElapsedGameTime.TotalMilliseconds;


            if (playerVehiclePosition.X > _graphics.PreferredBackBufferWidth - playerVehicleSprite.Width / 2)
            {
                playerVehiclePosition.X = _graphics.PreferredBackBufferWidth - playerVehicleSprite.Width / 2;
            }
            else if (playerVehiclePosition.X < playerVehicleSprite.Width / 2)
            {
                playerVehiclePosition.X = playerVehicleSprite.Width / 2;
            }

            if (playerVehiclePosition.Y > _graphics.PreferredBackBufferHeight - playerVehicleSprite.Height / 2)
            {
                playerVehiclePosition.Y = _graphics.PreferredBackBufferHeight - playerVehicleSprite.Height / 2;
            }
            else if (playerVehiclePosition.Y < playerVehicleSprite.Height / 2)
            {
                playerVehiclePosition.Y = playerVehicleSprite.Height / 2;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(playerVehicleSprite,
                playerVehiclePosition,
                null,
                Color.GhostWhite,
                0.0f,
                new Vector2(playerVehicleSprite.Width / 2,
                            playerVehicleSprite.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0.0f);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
