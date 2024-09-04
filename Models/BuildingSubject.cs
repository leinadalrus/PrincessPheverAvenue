using PrincessPheverAvenue.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrincessPheverAvenue.Models
{
    public class BuildingSubject
    {
        // NOTE(Daniel): Max observers of 4 pool groupings-
        // -equating to 16/32 acceptable objects rendered.
        BuildingObserver[] buildingObservers = new BuildingObserver[0];
        Int32 buildingObserverId = 0;

        public BuildingSubject() { }

        public void AddBuildingObserver(AvenueBuilding building) { }

        public void RemoveBuildingObserver(AvenueBuilding building) { }
    }
}
