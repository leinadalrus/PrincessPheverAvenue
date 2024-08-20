using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace PrincessPheverAvenue
{
    public class IMiddleForwardLayout
    {
        Vector2 layoutPosition,
                layoutDirection;

        public Vector2 Position
        {
            get { return layoutPosition; }
            set { }
        }

        public Vector2 Direction
        {
            get { return layoutDirection; }
            set { }
        }
    }

    // NOTE(Daniel): Class: Box2D.
    public class PlayerVehicle
    {
        public Vector2 Position,
                       Velocity,
                       Force;

        public Matrix  Yaw;
        public Vector2 YawVelocityAxis,
                       YawForce;

        public Single MaxVelocity,
                      MaxYawVelocity;

        public Single DampingForce,
                      DampingYawForce;

        public Single InputForce,
                      InputYawForce;

        // NOTE(Daniel): Collision obstacles can interface-soft-copy this-
        // -Box2D imitation class.
        public PlayerVehicle()
        {
            Position = Vector2.Zero;
            Velocity = Vector2.Zero;
            Force = Vector2.Zero;

            Yaw = Matrix.Identity;
            YawVelocityAxis = Vector2.Zero;
            YawForce = Vector2.Zero;

            MaxVelocity = 100.1f;
            MaxYawVelocity = 1.1f;

            DampingForce = 100.1f * 2;
            DampingYawForce = 1.1f * 2;

            InputForce = 1000.1f;
            InputYawForce = 1.1f * 2;
        }

        public Matrix Transform
        {
            // NOTE(Daniel): if we need Matrix, just make the Z-axis: "0" or "Null."
            get
            {
                var transform = Yaw;

                var deltaZ = new Vector3(Position.X, Position.Y, 0);
                transform.Translation = deltaZ;

                // Since "deltaZ" is within this lifetime's context-
                // -it should disappear from the heap in the future.
                return transform;
            }
        }

        public Single VelocityFactor
        {
            get { return Velocity.Length() / MaxVelocity; }
        }

        public Vector3 WorldVelocity
        {
            get
            {
                // TODO(Daniel): Make "deltaZ" into an Interface member.
                var deltaZ = new Vector3(Position.X, Position.Y, 0);
                return deltaZ.X * Yaw.Right + deltaZ.Y * Yaw.Up;
            }
            set
            {
                Velocity.X = Vector3.Dot(Yaw.Right, value);
                Velocity.Y = Vector3.Dot(Yaw.Up, value);
            }
        }
    }

    public class SoapboxKart : PlayerVehicle
    {
        public SoapboxKart() : base() { }

        void processInput(KeyboardState keyboardState)
        {
            Boolean wasUpperMovement = keyboardState.IsKeyDown(Keys.Up)
                    || keyboardState.IsKeyDown(Keys.W);
            Boolean wasLowerMovement = keyboardState.IsKeyDown(Keys.Down)
                    || keyboardState.IsKeyDown(Keys.S);
            Boolean wasLeftmostMovement = keyboardState.IsKeyDown(Keys.Left)
                    || keyboardState.IsKeyDown(Keys.A);
            Boolean wasRightmostMovement = keyboardState.IsKeyDown(Keys.Right)
                    || keyboardState.IsKeyDown(Keys.D);

            if (wasUpperMovement)
                Force.Y = InputForce;
            if (wasLowerMovement)
                Force.Y = -InputForce;
            if (wasLeftmostMovement)
                Force.X = -InputForce;
            if (wasRightmostMovement)
                Force.X = InputForce;
        }

        void updatePhysics(Single deltaTime)
        {
            Velocity += Force * deltaTime;

            if (Force.X > -0.1f && Force.X < 0.1f)
                if (Velocity.X > 0.0f)
                    Velocity.X = Math.Max(0.0f, Velocity.X - DampingForce * deltaTime);

            if (Force.Y > -0.1f && Force.Y < 0.1f)
                if (Velocity.Y > 0)
                    Velocity.Y = Math.Max(0.0f, Velocity.Y - DampingForce * deltaTime);

            Velocity.X = Math.Min(0.0f, Velocity.X + DampingForce * deltaTime);
            Velocity.Y = Math.Min(0.0f, Velocity.Y + DampingForce * deltaTime);

            var velocityLength = Velocity.Length();
            if (velocityLength > MaxVelocity)
                Velocity = Vector2.Normalize(Velocity) * MaxVelocity;

            var deltaZ = new Vector3(Velocity.X, Velocity.Y, 0);

            deltaZ += Yaw.Right * Velocity.X * deltaTime;
            deltaZ += Yaw.Up * Velocity.Y * deltaTime;

            YawVelocityAxis += YawForce * deltaTime;

            if (YawForce.X > -0.1 && YawForce.X < 0.1f)
                if (YawVelocityAxis.X > 0)
                    YawVelocityAxis.X = Math.Max(0.0f,
                        YawVelocityAxis.X
                        - DampingYawForce
                        * deltaTime);

            if (YawForce.Y > -0.1 && YawForce.Y < 0.1f)
                if (YawVelocityAxis.Y > 0)
                    YawVelocityAxis.Y = Math.Max(0.0f,
                        YawVelocityAxis.Y
                        - DampingYawForce
                        * deltaTime);

            YawVelocityAxis.X = Math.Min(0.0f,
                YawVelocityAxis.X
                + DampingYawForce
                * deltaTime);

            YawVelocityAxis.Y = Math.Min(0.0f,
                YawVelocityAxis.Y
                + DampingYawForce
                * deltaTime);

            Single yawVelocityLength = YawVelocityAxis.Length();
            if (yawVelocityLength > MaxYawVelocity)
                YawVelocityAxis = Vector2.Normalize(YawVelocityAxis) *
                    MaxYawVelocity;

            var yawVelocity = Matrix.Identity;

            if (YawVelocityAxis.X < -0.1f || YawVelocityAxis.X > 0.1f)
                yawVelocity = yawVelocity *
                    Matrix.CreateFromAxisAngle(Yaw.Right,
                    YawVelocityAxis.X * deltaTime);

            if (YawVelocityAxis.Y < -0.1f || YawVelocityAxis.Y > 0.1f)
                yawVelocity = yawVelocity *
                    Matrix.CreateFromAxisAngle(Yaw.Right,
                    YawVelocityAxis.Y * deltaTime);

            Yaw *= yawVelocity;
        }
    }

    public class SoapboxTrailer : PlayerVehicle
    {
        public SoapboxTrailer() : base() { }

        void updatePhysics(Single deltaTime)
        {
        }
    }

    // NOTE(Daniel): Remember, in an "M.F Layout" the body is in tow by-
    // -the driving wheels. (In this case: the "Forward-wheel Drive.)
    public class CollisionObstacleBody
    {
        // NOTE(Daniel): code ...
        // Override Velocity members and tangents to create a static object.
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
