#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

// My used packages
using PrincessPheverAvenue.Controllers;
#endregion

namespace PrincessPheverAvenue
{
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
            playerVehicleSpeed = 100.1f; // 100.0f as-is: 100% in my context.

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
            PlayerController.UpdateController(gameTime, playerVehiclePosition, playerVehicleSpeed);

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
