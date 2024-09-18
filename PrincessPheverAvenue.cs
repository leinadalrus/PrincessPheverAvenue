#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

// My used packages
#endregion

namespace PrincessPheverAvenue
{
    class ZigEncounter
    {
        public Int32    id;
        public Vector3  currentPosition,
                        lastPosition,
                        velocityFromDistance;

        public Model        model;
        public Texture2D    texture;

        public void MoveOntowards()
        {
            lastPosition = currentPosition;
            currentPosition += velocityFromDistance;
        }

        public void CollisionRebound()
        {
            currentPosition -= velocityFromDistance;
        }

        public void InverseVelocity()
        {
            velocityFromDistance.X = -velocityFromDistance.X;
            velocityFromDistance.Y = +velocityFromDistance.Y;
        }

        static void NoclipCheck(ref ZigEncounter a0, ref ZigEncounter b1)
        {
            for (int i = 0; i < a0.model.Meshes.Count; i++)
            {
                BoundingSphere boundingSphereA0 = a0.model.Meshes[i].BoundingSphere;
                boundingSphereA0.Center += a0.currentPosition;

                for (int j = 0; j < b1.model.Meshes.Count; j++)
                {
                    BoundingSphere boundingSphereB1 = b1.model.Meshes[j].BoundingSphere;
                    boundingSphereB1.Center += b1.currentPosition;

                    if (boundingSphereA0.Intersects(boundingSphereB1))
                    {
                        b1.InverseVelocity();
                        a0.CollisionRebound();
                        a0.InverseVelocity();
                    }
                }
            }
        }
    }

    class PhysicsShape
    {
        public Single   size;
        public Int32    sides;
    }

    class PhysicsBody
    {
        public UInt32   id;
        public Boolean  enabled;
        public Vector2  position,
                        velocity,
                        force;
        public Single   angularVelocity,
                        torque,
                        orient,
                        staticFriction,
                        dyanmicFriction,
                        restitution;
        public Boolean  useGravity,
                        isGrounded,
                        freezeOrient;
        public PhysicsShape shape;

        public PhysicsBody() { }
    }

    class AvenueBuilding : ZigEncounter
    {
        public AvenueBuilding() : base()
        {
            id = 0;
            currentPosition.X = 0;
            currentPosition.Y = 0;

            lastPosition.X = currentPosition.X;
            lastPosition.Y = currentPosition.Y;

            velocityFromDistance.X = 0;
            velocityFromDistance.Y = 0;

            model = null;
            texture = null;
        }
    }

    public class PrincessPheverAvenue : Game
    {
        Single _easeInCosine(float fn)
        {
            return (float)(1 - Math.Cos((fn * Math.PI) / 2));
        }

        Single _easeOutSine(float fn)
        {
            return (float)Math.Sin((fn * Math.PI) / 2);
        }

        readonly GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

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
            var keyboardState = Keyboard.GetState();

            Vector2 zeroVectorDirection = Vector2.Zero;
            zeroVectorDirection.Normalize();

            Single nuSpeed = gftoSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 nuPosition = +nuSpeed * zeroVectorDirection;

            if (keyboardState.IsKeyDown(Keys.W)
                || keyboardState.IsKeyDown(Keys.Up))
            {
                gftoPosition.Y -= _easeInCosine(nuSpeed);
                zeroVectorDirection.Y -= nuSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.A)
                || keyboardState.IsKeyDown(Keys.Left))
            {
                gftoPosition.X -= _easeOutSine(nuSpeed);
                zeroVectorDirection.X -= nuSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.S)
                || keyboardState.IsKeyDown(Keys.Down))
            {
                gftoPosition.Y += _easeOutSine(nuSpeed);
                zeroVectorDirection.Y += nuSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.D)
                || keyboardState.IsKeyDown(Keys.Right))
            {
                gftoPosition.X += _easeOutSine(nuSpeed);
                zeroVectorDirection.X += nuSpeed;
            }

            // NOTE(Gamepad): Gamepad logic
            if (Joystick.LastConnectedIndex == 0)
            {
                var joystickState = Joystick.GetState((int)PlayerIndex.One);

                if (joystickState.Axes[1] < -deadZone)
                {
                    gftoPosition.Y -= _easeInCosine(nuSpeed);
                }

                if (joystickState.Axes[1] > deadZone)
                {
                    gftoPosition.Y += _easeInCosine(nuSpeed);
                }

                if (joystickState.Axes[0] < -deadZone)
                {
                    gftoPosition.X -= _easeOutSine(nuSpeed);
                }

                if (joystickState.Axes[0] > deadZone)
                {
                    gftoPosition.X += _easeOutSine(nuSpeed);
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
