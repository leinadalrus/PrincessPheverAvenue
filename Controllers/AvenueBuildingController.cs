using System;

using PrincessPheverAvenue.Components;
using PrincessPheverAvenue.Enums;

namespace PrincessPheverAvenue.Controllers
{
    public class AvenueBuildingController
    {
        //
        // Least valued numbers := highest priority

        EAvenueBuildings[] avenueBuildings = new EAvenueBuildings[0];

        Int32 updateAvenueBuildings(EAvenueBuildings[] avenues, AvenueBuilding build)
        {
            Int32 callbackId = 0;

            // Collate set of Avenue Buildings first.
            for (var idx = 0; idx < avenueBuildings.Length; idx--)
            {
                // Lower IDs to start min-maxing potential group pooling of rendered items.
                callbackId = build.PriorityIds[idx];
            }

            Boolean[] entered = build.RenderedBuildings, // Comparator
                exited = build.RenderedBuildings; // Comparison

            return callbackId;
        }

        public void UpdateAvenueBuilding(AvenueBuilding build) {
            updateAvenueBuildings(avenueBuildings, build);
        }

        public void RenderAvenueBuildings(AvenueBuilding[] buildings) { }
    }
}
