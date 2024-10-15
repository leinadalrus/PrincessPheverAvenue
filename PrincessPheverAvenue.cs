#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public Model    model;
        public Texture2D texture;

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

    class RacecarAnatomy
    {
        public Vector3  position,
                        velocity,
                        force;
        public Single   angularVelocity,
                        torque,
                        orient,
                        inertia,
                        inverseInertia,
                        mass,
                        inverseMass,
                        staticFriction,
                        dyanmicFriction,
                        restitution;
        public Boolean  useGravity,
                        isGrounded,
                        freezeOrient;
        public CollisionShape collisionShape;

        public RacecarAnatomy() { }
    }

    class ThermodynamicsComponent
    {
        public Single   angularVelocity,
                        torque,
                        orient,
                        inertia,
                        inverseInertia,
                        mass,
                        inverseMass;
    }

    class AerodynamicsComponent
    {
        Single          staticFriction,
                        dyanmicFriction,
                        restitution;
    }

    class PowertrainComponent
    {
        public Vector3  position,
                        velocity,
                        force;
    }

    class ThermodynamicsRecord
    {
        public Int32 ID;
        public String Code;
    }

    class AerodynamicsRecord
    {
        public Int32 ID;
        public String Code;
    }

    class PowertrainRecord
    {
        public Int32 ID;
        public String Code;
    }

    class VehicleContext
    {
        public Model Model { get; set; }
        public BoundingSphere BoundingSphere { get; set; }
        public Vector3 Position { get; set; }
        public Boolean IsDirty { get; set; }

        public VehicleContext()
        {
            Model = null;
            BoundingSphere = new();
            Position = Vector3.Zero;
            IsDirty = false;
        }
    }

    class Camera3D
    {
        public Vector3 HeadOffset { get; set; }
        public Vector3 TargetOffset { get; set; }
        public Matrix ViewMatrix { get; set; }
        public Matrix ProjectionMatrix { get; set; }

        public Camera3D()
        {
            HeadOffset = new();
            TargetOffset = new();
            ViewMatrix = Matrix.Identity;
            ProjectionMatrix = Matrix.Identity;
        }

        public void Update(Vector3 position, float yaw, float aspectRatio)
        {
            Matrix rotationMatrix = Matrix.CreateRotationY(yaw);

            Vector3 transformedOffsetHead = Vector3.Transform(HeadOffset, rotationMatrix);
            Vector3 transformedReference = Vector3.Transform(TargetOffset, rotationMatrix);

            Vector3 cameraPosition = position + transformedOffsetHead;
            Vector3 cameraTarget = position + transformedReference;

            ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(100.0f), aspectRatio,
                1.0f, 1000.0f);
        }
    }

    class LinkNode
    {   // Cyclic LRU Cache (Circle Buffer.)
        public Int32 Key { get; set; }
        public Int32 Index { get; set; }

        public LinkNode Next { get; set; }
        public LinkNode Previous { get; set; }

        public LinkNode(int key = 0, int index = 0)
        {
            Key = key;
            Index = index;

            Next = null;
            Previous = null;
        }
    }

    class LfuCache
    {
        Int32 _size = 0;
        Dictionary<Int32, LinkNode> _map = null;
        LinkedList<Int32> _linke;
        LinkNode _node;

        public LfuCache(Int32 size)
        {
            _size = size;
            _map = new(); // : "new Dictionary();"
            _linke = new(); // : "new LinkedList();"
            _node = null;
        }

        void remove(LinkNode node)
        {
            var tail = node.Previous;
            var head = node.Next;

            tail.Next = head;
            head.Previous = tail;
        }

        void insert(LinkNode node)
        {
            var tail = _node.Previous;
            tail.Next = node;
            node.Previous = tail;
            node.Next = _node;
            _node.Previous = node;
        }

        public Int32 Get(int key)
        {
            if (_map.ContainsKey(key))
            {
                LinkNode node = _map[key];

                remove(node);
                insert(node);

                return node.Index;
            }

            return -1;
        }

        public void Put(int key, int index)
        {
            if (_map.ContainsKey(key))
            {
                remove(_map[key]);
            }

            var nu = new LinkNode(key, index);
            _map[key] = nu;

            insert(nu);

            if (_map.Count > _size)
            {
                var lfu = _node.Next;
                remove(lfu);

                _map.Remove(lfu.Key);
            }
        }

        public Boolean HadCycled(LinkNode head)
        {
            LinkNode slowIndexed = head, fastIndexing = head;

            while (fastIndexing != null && fastIndexing.Next != null)
            {
                fastIndexing = fastIndexing.Next.Next;
                slowIndexed = slowIndexed.Next;

                if (slowIndexed.Equals(fastIndexing))
                {
                    return true;
                }
            }

            // Else:
            return false;
        }
    }

    class BreadthFirstSearch
    {
        static Queue<Int32> _graphQueue = new();
        public static Int32 SearchBreadthFirst(Int32 originId, Int32 distancedId)
        {
            _graphQueue.Append(originId);
            _graphQueue.Append(distancedId);

            for (var i = 0; i < originId; i++)
            {
                for (var j = 0; j < distancedId; j++)
                {
                    if (i == j)
                    {
                        _graphQueue.Dequeue();
                    }

                    if (i > originId || j > distancedId)
                    {
                        _graphQueue.Clear();
                    }

                    SearchBreadthFirst(i, j);
                }
            }

            return RestartGraphQueue();
        }

        public void SetGraphQueue(Queue<Int32> nuQueue, Int32 predecessorId, Int32 successorId)
        {
            _graphQueue = nuQueue;
            _graphQueue.Append(predecessorId);
            _graphQueue.Append(successorId);
        }

        public static Int32 RestartGraphQueue()
        {
            _graphQueue.Dequeue();
            return _graphQueue.FirstOrDefault<Int32>();
        }
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
        readonly GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        Texture2D mirageSprite;

        Rectangle mirageCollision;
        Vector2 miragePosition;

        Single /* float */ mirageSpeed;
        int deadZone = 4096;

        VehicleContext groundFloor;
        Camera3D primaryCamera;

        public PrincessPheverAvenue()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        Single _averageSpeed(float y1, float y0, float x1, float x0)
        {
            var totalDistance = y1 - y0;
            var totalTime = x1 - x0;
            return totalDistance / totalTime;
        }

        // Derivatives
        Single _slopeSteepness(float rise, float run)
        {
            return rise / run;
        }

        // Limits
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

        void DrawTerrain(Model model)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = Matrix.Identity;

                    // Use already available matrices
                    effect.View = primaryCamera.ViewMatrix;
                    effect.Projection = primaryCamera.ProjectionMatrix;
                }

                mesh.Draw();
            }
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            miragePosition = new Vector2(_graphics.PreferredBackBufferWidth / 2,
                _graphics.PreferredBackBufferHeight / 2);
            mirageSpeed = 100.1f; // 100.0f as-is: 100% in my context.

            groundFloor = new();
            primaryCamera = new();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            mirageSprite = Content.Load<Texture2D>("Mirage-Rear");
            groundFloor.Model = Content.Load<Model>("Models/TarmacBase");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var keyboardState = Keyboard.GetState();

            Single rotation = 0.0f;
            Vector3 position = Vector3.Zero;
            primaryCamera.Update(position, rotation, _graphics.GraphicsDevice.Viewport.AspectRatio);

            Vector2 zeroVectorDirection = Vector2.Zero;
            zeroVectorDirection.Normalize();

            Single nuSpeed = mirageSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 nuPosition = +nuSpeed * zeroVectorDirection;

            if (keyboardState.IsKeyDown(Keys.W)
                || keyboardState.IsKeyDown(Keys.Up))
            {
                miragePosition.Y -= _easeInCosine(nuSpeed);
                zeroVectorDirection.Y -= nuSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.A)
                || keyboardState.IsKeyDown(Keys.Left))
            {
                miragePosition.X -= _easeOutSine(nuSpeed);
                zeroVectorDirection.X -= nuSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.S)
                || keyboardState.IsKeyDown(Keys.Down))
            {
                miragePosition.Y += _easeOutElastic(nuSpeed);
                zeroVectorDirection.Y += nuSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.D)
                || keyboardState.IsKeyDown(Keys.Right))
            {
                miragePosition.X += _easeOutSine(nuSpeed);
                zeroVectorDirection.X += nuSpeed;
            }

            // NOTE(Gamepad): Gamepad logic
            if (Joystick.LastConnectedIndex == 0)
            {
                var joystickState = Joystick.GetState((int)PlayerIndex.One);

                if (joystickState.Axes[1] < -deadZone)
                {
                    miragePosition.Y -= _easeInCosine(nuSpeed);
                }

                if (joystickState.Axes[1] > deadZone)
                {
                    miragePosition.Y += _easeOutElastic(nuSpeed);
                }

                if (joystickState.Axes[0] < -deadZone)
                {
                    miragePosition.X -= _easeOutSine(nuSpeed);
                }

                if (joystickState.Axes[0] > deadZone)
                {
                    miragePosition.X += _easeOutSine(nuSpeed);
                }
            }

            if (mirageCollision.X > _graphics.PreferredBackBufferWidth - mirageSprite.Width / 2)
            {
                mirageCollision.X = _graphics.PreferredBackBufferWidth - mirageSprite.Width / 2;
            }
            else if (mirageCollision.X < mirageSprite.Width / 2)
            {
                mirageCollision.X = mirageSprite.Width / 2;
            }

            if (mirageCollision.Y > _graphics.PreferredBackBufferHeight - mirageSprite.Height / 2)
            {
                mirageCollision.Y = _graphics.PreferredBackBufferHeight - mirageSprite.Height / 2;
            }
            else if (mirageCollision.Y < mirageSprite.Height / 2)
            {
                mirageCollision.Y = mirageSprite.Height / 2;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            DrawTerrain(groundFloor.Model);

            _spriteBatch.Begin();
            _spriteBatch.Draw(mirageSprite,
                miragePosition,
                null,
                Color.GhostWhite,
                0.0f,
                new Vector2(mirageSprite.Width / 5,
                            mirageSprite.Height / 4),
                Vector2.One,
                SpriteEffects.None,
                0.0f);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
