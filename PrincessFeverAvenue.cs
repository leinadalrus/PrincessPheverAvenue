using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace PrincessFeverAvenue
{
    public class PrincessFeverAvenue : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D playerVehicleTexture;
        Vector2 playerVehiclePosition;
        Single /* float */ playerVehicleSpeed;

        public PrincessFeverAvenue()
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
            playerVehicleTexture = Content.Load<Texture2D>("player-vehicle-sprite-idle");
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


            if (playerVehiclePosition.X > _graphics.PreferredBackBufferWidth - playerVehicleTexture.Width / 2)
            {
                playerVehiclePosition.X = _graphics.PreferredBackBufferWidth - playerVehicleTexture.Width / 2;
            }
            else if (playerVehiclePosition.X < playerVehicleTexture.Width / 2)
            {
                playerVehiclePosition.X = playerVehicleTexture.Width / 2;
            }

            if (playerVehiclePosition.Y > _graphics.PreferredBackBufferHeight - playerVehicleTexture.Height / 2)
            {
                playerVehiclePosition.Y = _graphics.PreferredBackBufferHeight - playerVehicleTexture.Height / 2;
            }
            else if (playerVehiclePosition.Y < playerVehicleTexture.Height / 2)
            {
                playerVehiclePosition.Y = playerVehicleTexture.Height / 2;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(playerVehicleTexture,
                playerVehiclePosition,
                null,
                Color.GhostWhite,
                0.0f,
                new Vector2(playerVehicleTexture.Width / 2,
                            playerVehicleTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0.0f);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
