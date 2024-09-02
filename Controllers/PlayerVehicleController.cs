using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace PrincessPheverAvenue.Controllers
{
    public class PlayerVehicleController
    {
        public static void UpdateController(
            GameTime gameTime,
            Vector2 playerVehiclePosition,
            Single playerVehicleSpeed
        ) {
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
        }
    }
}
