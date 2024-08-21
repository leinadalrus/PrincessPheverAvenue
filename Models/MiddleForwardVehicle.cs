using Microsoft.Xna.Framework;
using System;

namespace PrincessPheverAvenue.Models
{
    // NOTE(Daniel): Class: Box2D.
    public class MiddleForwardVehicle
    {
        public Vector2 Position,
                       Velocity,
                       Force;

        public Matrix Yaw;
        public Vector2 YawVelocityAxis,
                       YawForce;

        public float MaxVelocity,
                      MaxYawVelocity;

        public float DampingForce,
                      DampingYawForce;

        public float InputForce,
                      InputYawForce;

        // NOTE(Daniel): Collision obstacles can interface-soft-copy this-
        // -Box2D imitation class.
        public MiddleForwardVehicle()
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

        public float VelocityFactor
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
}
