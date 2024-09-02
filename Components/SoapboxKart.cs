using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PrincessPheverAvenue.Models;
using System;

namespace PrincessPheverAvenue.Components
{
    public class SoapboxKart : MiddleForwardVehicle
    {
        public SoapboxKart() : base() { }

        void processInput(KeyboardState keyboardState)
        {
            bool wasUpperMovement = keyboardState.IsKeyDown(Keys.Up)
                    || keyboardState.IsKeyDown(Keys.W);
            bool wasLowerMovement = keyboardState.IsKeyDown(Keys.Down)
                    || keyboardState.IsKeyDown(Keys.S);
            bool wasLeftmostMovement = keyboardState.IsKeyDown(Keys.Left)
                    || keyboardState.IsKeyDown(Keys.A);
            bool wasRightmostMovement = keyboardState.IsKeyDown(Keys.Right)
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

        void updatePhysics(float deltaTime)
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

            float yawVelocityLength = YawVelocityAxis.Length();
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
}
