using Microsoft.Xna.Framework;
using System;

using PrincessPheverAvenue.Models;

namespace PrincessPheverAvenue.Components
{
    public class AvenueBuilding
    {
        // Private
        CollisionObstacleBody collisionBody;
        Vector2 buildingPosition;

        public AvenueBuilding() { }

        // Public
        // NOTE(Daniel): Don't use: C# Syntactic-Sugar: Get, Set.
        public Int32[] PriorityIds;
        public Boolean[] RenderedBuildings;
    }
}
