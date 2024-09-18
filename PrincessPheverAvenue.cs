#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

// My used packages
#endregion

namespace PrincessPheverAvenue
{
    class CollisionShape
    {
        public Int32    id;
        public Vector3  currentPosition,
                        lastPosition,
                        velocityFromDistance;

        public Single   size;
        public Int32    sides;

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

        static void NoclipCheck(ref CollisionShape a0, ref CollisionShape b1)
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
        public CollisionShape shape;

        public PhysicsBody() { }
    }

    class AvenueBuilding : CollisionShape
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

            size = 1.0f;
            sides = 4;
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

        Single _easeInElastic(float fn)
        {
            const float d = (float)((2 * Math.PI) / 3);

            return (float)(fn == 0
                ? 0
                : fn == 1
                ? 1
                : -Math.Pow(2, 10 * fn - 10) * Math.Sin((fn * 10 - 10.75) * d));
        }

        Single _easeOutElastic(float fn)
        {
            const float d = (float)(2 * Math.PI) / 3;

            return (float)(fn == 0
                ? 0
                : fn == 1
                ? 1
                : Math.Pow(2, -10 * fn) * Math.Sin((fn * 10 - 0.75) * d) + 1);
        }

        readonly GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        Texture2D ftoSprite;
        Rectangle ftoCollision;
        Vector2 ftoPosition;
        Single /* float */ ftoSpeed;

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
            ftoPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2,
                _graphics.PreferredBackBufferHeight / 2);
            ftoSpeed = 100.1f; // 100.0f as-is: 100% in my context.

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ftoSprite = Content.Load<Texture2D>("Mirage-Idle");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var keyboardState = Keyboard.GetState();

            Vector2 zeroVectorDirection = Vector2.Zero;
            zeroVectorDirection.Normalize();

            Single nuSpeed = ftoSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 nuPosition = +nuSpeed * zeroVectorDirection;

            if (keyboardState.IsKeyDown(Keys.W)
                || keyboardState.IsKeyDown(Keys.Up))
            {
                ftoPosition.Y -= _easeInCosine(nuSpeed);
                zeroVectorDirection.Y -= nuSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.A)
                || keyboardState.IsKeyDown(Keys.Left))
            {
                ftoPosition.X -= _easeOutSine(nuSpeed);
                zeroVectorDirection.X -= nuSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.S)
                || keyboardState.IsKeyDown(Keys.Down))
            {
                ftoPosition.Y += _easeOutElastic(nuSpeed);
                zeroVectorDirection.Y += nuSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.D)
                || keyboardState.IsKeyDown(Keys.Right))
            {
                ftoPosition.X += _easeOutSine(nuSpeed);
                zeroVectorDirection.X += nuSpeed;
            }

            // NOTE(Gamepad): Gamepad logic
            if (Joystick.LastConnectedIndex == 0)
            {
                var joystickState = Joystick.GetState((int)PlayerIndex.One);

                if (joystickState.Axes[1] < -deadZone)
                {
                    ftoPosition.Y -= _easeInCosine(nuSpeed);
                }

                if (joystickState.Axes[1] > deadZone)
                {
                    ftoPosition.Y += _easeOutElastic(nuSpeed);
                }

                if (joystickState.Axes[0] < -deadZone)
                {
                    ftoPosition.X -= _easeOutSine(nuSpeed);
                }

                if (joystickState.Axes[0] > deadZone)
                {
                    ftoPosition.X += _easeOutSine(nuSpeed);
                }
            }

            if (ftoCollision.X > _graphics.PreferredBackBufferWidth - ftoSprite.Width / 2)
            {
                ftoCollision.X = _graphics.PreferredBackBufferWidth - ftoSprite.Width / 2;
            }
            else if (ftoCollision.X < ftoSprite.Width / 2)
            {
                ftoCollision.X = ftoSprite.Width / 2;
            }

            if (ftoCollision.Y > _graphics.PreferredBackBufferHeight - ftoSprite.Height / 2)
            {
                ftoCollision.Y = _graphics.PreferredBackBufferHeight - ftoSprite.Height / 2;
            }
            else if (ftoCollision.Y < ftoSprite.Height / 2)
            {
                ftoCollision.Y = ftoSprite.Height / 2;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(ftoSprite,
                ftoPosition,
                null,
                Color.GhostWhite,
                0.0f,
                new Vector2(ftoSprite.Width / 5,
                            ftoSprite.Height / 4),
                Vector2.One,
                SpriteEffects.None,
                0.0f);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
